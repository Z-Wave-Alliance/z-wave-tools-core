/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using Renci.SshNet;
using Utils;
using ZWave.Enums;
using ZWave.Layers;
using System.IO;
using System.Threading;

namespace ZWave.ZipApplication
{
    public class LibZwaveIpTransportClient
    {
        private bool _disposed = false;

        private string RpUsername = "pi";
        private string RpPassword = "raspberry";
        private bool isSshConnected = false;

        SshClient sshclient = null;
        public ShellStream shellStream = null;
        public StreamReader reader = null;
        public StreamWriter writer = null;
        public string outPut = "Output: ";
        public string zipHostAddress = Environment.GetEnvironmentVariable("RPI_IP_ADDRESS") ??
                                    Environment.GetEnvironmentVariable("RPI_IP_ADDRESS", EnvironmentVariableTarget.User);

        public bool IsOpen
        {
            get { return sshclient != null && sshclient.IsConnected; }
        }
        public CommunicationStatuses InnerConnect()
        {
            
            //Run SSH Client
            sshclient = new SshClient(zipHostAddress, RpUsername, RpPassword);
            try
            {
                sshclient.Connect();
                Console.WriteLine(outPut + "SSH Client connected.");
                isSshConnected = true;
                shellStream = sshclient.CreateShellStream("Commands", 240, 200, 132, 80, 1024);
                Console.WriteLine(outPut + "Create Client Shell Stream");
                reader = new StreamReader(shellStream);
                Console.WriteLine(outPut + "Create Stream Reader.");
                writer = new StreamWriter(shellStream);
                writer.AutoFlush = true;
                Console.WriteLine(outPut + "Create Stream Writer.");

                return CommunicationStatuses.Done;
            }
            catch (Exception e)
            {
                return CommunicationStatuses.Failed;
            }
        }

        public void InnerDisconnect()
        {
            //sshclient.Disconnect();
            //Console.WriteLine(outPut + "SSH Client Disconnected.");
        }

        public void InnerDispose()
        {
            InnerDisconnect();
            Console.WriteLine(outPut + "InnerDisconnect function disconnected.");
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            // Protect from being called multiple times.
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {

            }

            // Clean up all unmanaged resources.
            if (sshclient != null)
            {
                sshclient.Disconnect();
                Console.WriteLine(outPut + "SSH Client Disconnected.");
            }

            _disposed = true;
        }

        public void InnerWriteData(string data)
        {
            if (sshclient == null || writer == null || isSshConnected == false)
            {
                InnerConnect();
                Console.WriteLine(outPut + "Excute InnerConnect function.");
            }
            writer.WriteLine(data);
            Console.WriteLine("Writer: " + data);
        }
    }
}
