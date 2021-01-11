using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using NUnit.Framework;
using Debugger = SignalRMVCChat.Areas.sysAdmin.Service.MyGlobal;

namespace SignalRMVCChat.Controllers
{
    [TelegramBotsWebApplication.ActionFilters.MyControllerFilter]
    public class UpdateApplicationController:Controller
    {


        public ActionResult Index()
        {
            return View("Index");
        }
        
        public async   Task<ActionResult> Update(string command)
        {

            string directory = @"c:\\wwwroot\telegrambot\gapchat.ashpazerooz.ir\wwwroot\";// Environment.CurrentDirectory; // directory of the git repository

            if (string.IsNullOrEmpty(command)==false)
            {
                if (Debugger.IsAttached)
                {
                    directory = null;
                }
                /*var cmd = Cli.Wrap("git")
                    .WithWorkingDirectory(directory)
                    .WithArguments(command);

                var res=  await cmd.ExecuteBufferedAsync()
                    .Select(r => r.StandardOutput);;
                    
                    

                TempData["output"] = res;*/
                
                string strCmdText;
                strCmdText= "/c " + command + " & pause";
                System.Diagnostics.Process.Start("CMD.exe", strCmdText);
                
                TempData["output"] =  CommandOutput(command, directory);
            }
            /*using (Process process = new Process())
            {
                // set start info
                ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
                startInfo.WindowStyle = ProcessWindowStyle.Normal;
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardOutput =true;
                startInfo.UseShellExecute = false;
                startInfo.WorkingDirectory = directory;
                startInfo.RedirectStandardError = true;


                // Start with one argument.
                // Output of ArgsEcho:
                //  [0]=/a
                //startInfo.Arguments = "dir";
                process.StartInfo = startInfo;
                process.Start();
                


                if (string.IsNullOrEmpty(command))
                {
                    command = "git status";
                }
                
                // send command to its input
                process.StandardInput.Write(command + process.StandardInput.NewLine);
                //wait
                process.StandardInput.Flush();
                
                process.EnableRaisingEvents = true;
                process.Exited += new EventHandler((sender, args) =>
                {
                    
                });

                process.StandardInput.Close();
                
                
                string ds = process.StandardOutput.ReadToEnd();

                ds += process.StandardError.ReadToEnd();
                TempData["output"] = ds;

                process.Close();
                
            }*/
            return View("Index");
        }
        
        public static string CommandOutput(string command,
            string workingDirectory = null)
        {
            try
            {
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command );

                procStartInfo.RedirectStandardError = procStartInfo.RedirectStandardInput = procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                procStartInfo.CreateNoWindow = false;
                
                
                
                
                procStartInfo.RedirectStandardInput = true;
                if (null != workingDirectory)
                {
                    procStartInfo.WorkingDirectory = workingDirectory;
                }

                Process proc = new Process();
                proc.StartInfo = procStartInfo;
                proc.Start();

                StringBuilder sb = new StringBuilder();
                proc.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e)
                {
                    sb.AppendLine(e.Data);
                };
                proc.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs e)
                {
                    sb.AppendLine(e.Data);
                };

                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
                proc.WaitForExit();
                return sb.ToString();
            }
            catch (Exception objException)
            {
                return $"Error in command: {command}, {objException.Message}";
            }
        }
    }

    public class UpdateApplicationControllerTests
    {
        [Test]
        public void Test()
        {

            try
            {
                new UpdateApplicationController().Update(null);
            }
            catch (Exception e)
            {
                Test();
            }
        }
    }
}