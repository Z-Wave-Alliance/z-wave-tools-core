/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZWave.UicApplication.SocketTransport
{
    public class UicWebSocketClient : IDisposable
    {
        private ClientWebSocket _webSocket = null;
        public bool IsConnected { get; set; } = false;

        public Action<byte[]> DataReceivedHandler { get; set; }

        private BlockingCollection<byte[]> DataFromMqttCollection = new BlockingCollection<byte[]>();

        private Task _readFromServerTask;
        private Task _dataReceivedTask;

        public bool Connect(string uri)
        {
            try
            {
                ConnectInner($"ws://{uri}/ws");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            if (!IsConnected)
            {
                CloseClient();
                Console.WriteLine("Not connected");
            }
            return IsConnected;
        }

        public void ConnectInner(string uri)
        {
            try
            {
                _webSocket = new ClientWebSocket();
                _webSocket.ConnectAsync(new Uri(uri), CancellationToken.None).Wait();
                _readFromServerTask = Task.Run(() => ReadFromServer());
                _dataReceivedTask = Task.Run(() => DataReceivedTask());

                IsConnected = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex);
            }
            finally
            {
                if (!IsConnected && _webSocket != null)
                {
                    _webSocket.Dispose();
                }
            }
        }

        private void DataReceivedTask()
        {
            while (_webSocket.State == WebSocketState.Open)
            {
                byte[] message = DataFromMqttCollection.Take();
                DataReceivedHandler(message);
            }
        }

        private void ReadFromServer()
        {
            Console.WriteLine($"Started receiving responses from server.");
            try
            {
                while (_webSocket.State == WebSocketState.Open)
                {
                    var content = string.Empty;
                    byte[] bytes = new byte[8192];
                    // Read data from the client socket.

                    var receiveResult = _webSocket.ReceiveAsync(new ArraySegment<byte>(bytes), CancellationToken.None);

                    if (receiveResult.Result.Count > 0)
                    {
                        content = Encoding.ASCII.GetString(bytes, 0, receiveResult.Result.Count);

                        // All the data has been read from the
                        // client. Display it on the console.  
                        Console.WriteLine($"Read {content.Length} bytes from socket.\nData: {content}");
                        DataFromMqttCollection.Add(bytes);
                    }
                }
            }
            catch (WebSocketException wse)
            {
                Console.WriteLine($"Socket closed. With message: {wse.Message}");
            }
        }

        public void Send(string data)
        {
            try
            {
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes(data);
                var sendRes = _webSocket.SendAsync(new ArraySegment<byte>(byteData), WebSocketMessageType.Binary, true, CancellationToken.None);
                sendRes.Wait();
                Console.WriteLine($"Sent --- {data}  --- to server.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        public void CloseClient()
        {
            if (_webSocket != null)
            {
                _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                IsConnected = false;
                _readFromServerTask?.Wait();
                _dataReceivedTask?.Wait();
            }
        }

        public void Dispose()
        {
            CloseClient();
        }
    }
}
