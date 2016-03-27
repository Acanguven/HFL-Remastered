using System.Diagnostics;

namespace HFL_Remastered
{
    class CommandManager : Network
    {
        private Process cmd;
        private ProcessStartInfo startInfo;
        public CommandManager(){
            startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            cmd = new Process { StartInfo = startInfo };
            cmd.OutputDataReceived += cmd_DataReceived;
            cmd.Start();
            cmd.BeginOutputReadLine();
        }

        public void cmd_DataReceived(object sender, DataReceivedEventArgs e)
        {
            cmdNewLine(e.Data);
        }

        public void writeLine(string command)
        {
            if (cmd.HasExited)
            {
                cmd.Dispose();
                cmd = null;
                cmd = new Process { StartInfo = startInfo };
                cmd.OutputDataReceived += cmd_DataReceived;
                cmd.Start();
                cmd.BeginOutputReadLine();
                cmd.StandardInput.WriteLineAsync(command);
            }
            else
            {
                cmd.StandardInput.WriteLineAsync(command);
            }
        }
    }
}
