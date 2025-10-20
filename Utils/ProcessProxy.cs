/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Text;

namespace Utils
{
    public class ProcessProxy
    {
        private readonly string _cmd;
        private readonly string _workingDirectory;
        private Process _runningProcess;
        private readonly bool _hasGui;
        private readonly string _argsTemplate;

        public bool IsReady { get; private set; }

#if NETCOREAPP
        private System.Threading.Tasks.Task<string> _readOutputTask;
        public string ProcessOutput => _readOutputTask?.Result;
#endif

 #if !NETCOREAPP
        public IntPtr MainWindowHandle
        {
            get
            {
                if (!IsReady || _runningProcess == null)
                {
                    return IntPtr.Zero;
                }

                return _runningProcess.MainWindowHandle;
            }
        }
#endif

        public ProcessProxy(string workingDirectory, string cmd, string argsTemplate)
            : this(true, workingDirectory, cmd, argsTemplate)
        {
        }

        public ProcessProxy(bool hasGui, string workingDirectory, string cmd, string argsTemplate)
        {
            _hasGui = hasGui;
            _argsTemplate = argsTemplate;
            _cmd = cmd;
            if (Directory.Exists(_workingDirectory))
            {
                IsReady = true;
                return;
            }

            try
            {
                _workingDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramW6432") ?? "", workingDirectory);
                if (!Directory.Exists(_workingDirectory))
                {
                    _workingDirectory = null;
                }
            }
            catch
            {
                // ignored
            }

            if (_workingDirectory == null)
            {
                try
                {
                    _workingDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)") ?? "", workingDirectory);
                    if (!Directory.Exists(_workingDirectory))
                    {
                        _workingDirectory = null;
                    }
                }
                catch
                {
                    // ignored
                }
            }
            if (_workingDirectory == null)
            {
                try
                {
                    _workingDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), workingDirectory);
                    if (!Directory.Exists(_workingDirectory))
                    {
                        _workingDirectory = null;
                    }
                }
                catch
                {
                    // ignored
                }
            }

            if (_workingDirectory != null)
            {
                IsReady = true;
            }
        }

#if NETCOREAPP
        public void Start(params object[] args)
        {
            if (!IsReady)
                return;

            var arguments = string.Format(_argsTemplate, args);
            _runningProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _cmd,
                    Arguments = arguments,
                    WorkingDirectory = _workingDirectory,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
            _runningProcess.Start();
            _readOutputTask = _runningProcess.StandardOutput.ReadToEndAsync();
        }
#else
        public void Start(params object[] args)
        {
            var arguments = string.Format(_argsTemplate, args);
            if (IsReady)
            {
                _runningProcess =
                    new Process { StartInfo = new ProcessStartInfo(_cmd, arguments) { WorkingDirectory = _workingDirectory } };
                _runningProcess.Start();
                if (_hasGui)
                {
                    _runningProcess.WaitForInputIdle();
                }
                Thread.Sleep(1000);
                while (_runningProcess.MainWindowHandle == IntPtr.Zero)
                {
                    Thread.Sleep(200);
                    _runningProcess.Refresh();
                }
                _runningProcess.PriorityClass = ProcessPriorityClass.RealTime;
                $"{_runningProcess.MainWindowTitle} PriorityClass = {_runningProcess.PriorityClass}"._DLOG();
            }
            else
            {
                throw new InvalidOperationException("IsReady == false");
            }

        }
#endif

        public void Close()
        {
#if NETCOREAPP
            if (!IsReady || _runningProcess == null)
                return;

            _runningProcess.WaitForExit();
#else
            if (IsReady)
            {
                if (_runningProcess != null)
                {
                    try
                    {
                        var pn = _runningProcess.ProcessName;
                        var closed = _runningProcess.CloseMainWindow();
                        if (closed)
                        {
                            $"Close PC Controller process {pn}"._DLOG();
                            _runningProcess.WaitForExit();
                            "Closed"._DLOG();
                        }
                        else
                        {
                            $"Urgent terminate PC Controller process {pn}"._DLOG();
                            _runningProcess.Kill();
                            "Terminated"._DLOG();
                        }
                    }
                    catch (InvalidOperationException)
                    {
                    }
                    _runningProcess = null;
                }
            }
#endif
        }
    }
}
