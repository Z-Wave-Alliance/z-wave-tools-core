/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using Utils;
using ZWave.Enums;

namespace ZWave.UicApplication.SshTransport
{
    public class SshTerminalClient : IDisposable
    {
        private SshClient _sshclient = null;
        public IDataSource DataSource { get; set; }
        public int CommandTimeoutMs { get; set; } = 5000;
        private readonly string defaultUsername = "pi";
        private readonly string defaultPassword = "raspberry";
        public bool IsSshClientConnected { get { return _sshclient != null && _sshclient.IsConnected; } }
        public CommunicationStatuses Connect()
        {
            return Connect(null);
        }

        public CommunicationStatuses Connect(IDataSource dataSourceToConnect)
        {
            if (_sshclient != null && _sshclient.IsConnected)
            {
                return CommunicationStatuses.Done;
            }

            var result = CommunicationStatuses.Failed;
            ConnectionInfo connectionInfo = null;
            string host = string.Empty, username = defaultUsername, password = defaultPassword;

            dataSourceToConnect = dataSourceToConnect ?? DataSource;
            if (dataSourceToConnect != null)
            {
                host = dataSourceToConnect.SourceName;
                var splittedArgs = dataSourceToConnect.Args?.Length > 0 ? dataSourceToConnect.Args.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) : null;
                if (splittedArgs != null)
                {
                    var usernamePosition = Array.FindIndex(splittedArgs, x => x.Equals("-username", StringComparison.OrdinalIgnoreCase));
                    username = usernamePosition > 0 && usernamePosition + 1 < splittedArgs.Length ? splittedArgs[usernamePosition + 1] : username;

                    var passwordPosition = Array.FindIndex(splittedArgs, x => x.Equals("-password", StringComparison.OrdinalIgnoreCase));
                    password = passwordPosition > 0 && passwordPosition + 1 < splittedArgs.Length ? splittedArgs[passwordPosition + 1] : password;
                }
            }
            else
            {
                return result;
            }

            connectionInfo = new ConnectionInfo(host, username, new AuthenticationMethod[]
                {
                    new PasswordAuthenticationMethod(username, password)
                });
            _sshclient = _sshclient ?? new SshClient(connectionInfo);
            try
            {
                _sshclient.Connect();
                result = _sshclient.IsConnected ? CommunicationStatuses.Done : CommunicationStatuses.Failed;
            }
            catch (Exception e)
            {
                _sshclient.Dispose();
                _sshclient = null;
                Console.WriteLine(e.Message);
            }

            return result;
        }

        public string Send(string commandToRun)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(commandToRun))
            {
                if (!IsSshClientConnected)
                {
                    Connect();
                }

                try
                {
                    if (_sshclient != null && _sshclient.IsConnected)
                    {
                        var command = _sshclient.CreateCommand(commandToRun);
                        command.CommandTimeout = TimeSpan.FromMilliseconds(CommandTimeoutMs);
                        result = command.Execute();
                    }
                }
                catch (SshOperationTimeoutException)
                {
                    Console.WriteLine($"Command '{commandToRun}' timed out");
                }
                catch (Exception er)
                {
                    Console.WriteLine("An exception has been caught " + er.ToString());
                }
            }
            return result;
        }

        public string[] ExecuteBatch(params string[] batchToExecute)
        {
            string[] result = null;
            if (batchToExecute != null && batchToExecute.Length > 0)
            {
                List<string> results = new List<string>();
                foreach (var commandToRun in batchToExecute)
                {
                    results.Add(Send(commandToRun));
                }
            }
            return result;
        }

        public bool WriteFileStream(Stream fileStream, string path, bool forceWritePermissions)
        {
            bool result = false;
            using (SftpClient sftp = new SftpClient(_sshclient.ConnectionInfo))
            {
                try
                {
                    sftp.Connect();

                    if (forceWritePermissions)
                    {
                        var command = _sshclient.CreateCommand($"sudo chmod 777 {path}");
                        command.CommandTimeout = TimeSpan.FromMilliseconds(CommandTimeoutMs);
                        command.Execute();
                    }

                    sftp.UploadFile(fileStream, path);

                    sftp.Disconnect();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An exception has been caught " + ex.ToString());
                }
            }
            return result;
        }

        public void Disconnect()
        {
            _sshclient.Disconnect();
            _sshclient.Dispose();
            _sshclient = null;
        }

        public void Dispose()
        {
            Disconnect();
        }
    }
}
