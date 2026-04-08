using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Text;
using System.Threading;
public class CMDToolMain
{
    public string WorkingDirectory { get; set; }

    public CMDToolMain(string workingDirectory)
    {
        //Debug.LogError(workingDirectory);
        WorkingDirectory = workingDirectory;
    }
    public string Run(string executablePath, string arguments = "")
    {
        ProcessStartInfo info = null;
        if (arguments != "")
        {
            info = new ProcessStartInfo(executablePath, arguments);
        }
        else
        {
            info = new ProcessStartInfo(executablePath);
        }

        info.CreateNoWindow = true;
        info.RedirectStandardOutput = true;
        info.UseShellExecute = false;
        info.WorkingDirectory = WorkingDirectory;
        info.ErrorDialog = true;
        var process = new Process
        {
            StartInfo = info,
        };
        process.Start();

        string log = process.StandardOutput.ReadToEnd();

        process.Dispose();
        process.Close();
        return log;
    }
    public void RunAnsic(string executablePath, string arguments, System.Action callback)
    {
        using (Process process = new Process())
        {
            StringBuilder processOutputBuilder = new StringBuilder();

            process.StartInfo = new ProcessStartInfo(executablePath, arguments);
            if (WorkingDirectory != null)
            {
                process.StartInfo.WorkingDirectory = WorkingDirectory;
            }

            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.EnableRaisingEvents = true;

            AutoResetEvent outputWaitHandle = new AutoResetEvent(false);
            AutoResetEvent errorWaitHandle = new AutoResetEvent(false);
            process.OutputDataReceived += (sender, eventArgs) =>
            {
                //if (eventArgs.Data != null)
                //{
                //    processOutputBuilder.AppendLine(eventArgs.Data);
                //}
                //else
                //{
                //    outputWaitHandle.Set();
                //}
            };

            process.ErrorDataReceived += (sender, eventArgs) =>
            {
                //if (eventArgs.Data != null)
                //{
                //    processOutputBuilder.AppendLine(eventArgs.Data);
                //}
                //else
                //{
                //    errorWaitHandle.Set();
                //}
            };
            process.Start();
            process.BeginOutputReadLine();
            callback();
        }
    }
}