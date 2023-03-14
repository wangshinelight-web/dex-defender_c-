using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DexDefender
{
    public class ApktoolRedirector
    {
        public event Action<string> OutputReceived;
        public event Action<string> ErrorReceived;
        ListBox outputListBox;
        public ApktoolRedirector(ListBox listBox)
        {
            outputListBox = listBox;
        }

        public int Execute(string cmd)
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "java.exe",
                    Arguments = cmd,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = System.Text.Encoding.UTF8,
                    StandardErrorEncoding = System.Text.Encoding.UTF8
                },
                EnableRaisingEvents = true
            };

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    outputListBox.Invoke((MethodInvoker)delegate {
                        outputListBox.Items.Add(e.Data);
                        outputListBox.TopIndex = outputListBox.Items.Count - 1;
                    });
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    outputListBox.Invoke((MethodInvoker)delegate {
                        outputListBox.Items.Add("[ERROR] " + e.Data);
                        outputListBox.TopIndex = outputListBox.Items.Count - 1;
                    });
                }
            };




            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            return process.ExitCode;
        }

        public int ExecuteW(string path, string cmd)
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = path + "zipalign.exe",
                    Arguments = cmd,
                    //WorkingDirectory = path,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = System.Text.Encoding.UTF8,
                    StandardErrorEncoding = System.Text.Encoding.UTF8
                },
                EnableRaisingEvents = true
            };

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    outputListBox.Invoke((MethodInvoker)delegate {
                        outputListBox.Items.Add(e.Data);
                        outputListBox.TopIndex = outputListBox.Items.Count - 1;
                    });
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    outputListBox.Invoke((MethodInvoker)delegate {
                        outputListBox.Items.Add("[ERROR] " + e.Data);
                        outputListBox.TopIndex = outputListBox.Items.Count - 1;
                    });
                }
            };




            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            return process.ExitCode;
        }

    }
}

/*

using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace DexDefender
{
    public class ApktoolRedirector :IDisposable
    {
        private Process process;
        private ListBox targetListBox;
        private bool isDisposed;

        public ApktoolRedirector(ListBox listBox)
        {
            targetListBox = listBox ?? throw new ArgumentNullException(nameof(listBox));
        }

        public int Execute(string apkPath, string outputDir)
        {
            try
            {
                process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "java.exe",
                        Arguments = $"-jar tools/apktool.jar d -f \"{apkPath}\" -o \"{outputDir}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        StandardOutputEncoding = System.Text.Encoding.UTF8,
                        StandardErrorEncoding = System.Text.Encoding.UTF8
                    },
                    EnableRaisingEvents = true
                };

                process.OutputDataReceived += ProcessOutputHandler;
                process.ErrorDataReceived += ProcessErrorHandler;

                if (!process.Start())
                    return process.ExitCode;

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();               
            }
            catch
            {
                
            }
            return process.ExitCode;
        }

        private void ProcessOutputHandler(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data) || targetListBox.IsDisposed)
                return;

            targetListBox.BeginInvoke((MethodInvoker)delegate
            {
                targetListBox.Items.Add(e.Data);
                targetListBox.TopIndex = targetListBox.Items.Count - 1;
                targetListBox.Refresh();
            });
        }

        private void ProcessErrorHandler(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data) || targetListBox.IsDisposed)
                return;

            targetListBox.BeginInvoke((MethodInvoker)delegate
            {
                targetListBox.Items.Add(e.Data);
                targetListBox.TopIndex = targetListBox.Items.Count - 1;
                targetListBox.Refresh();
            });
        }




        public void Dispose()
        {
            if (isDisposed) return;

            if (process != null)
            {
                if (!process.HasExited)
                    process.Kill();

                process.OutputDataReceived -= ProcessOutputHandler;
                process.ErrorDataReceived -= ProcessOutputHandler;
                process.Dispose();
            }

            isDisposed = true;
        }
    }
}*/