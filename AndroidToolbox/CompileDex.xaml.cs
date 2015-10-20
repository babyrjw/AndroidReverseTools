using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;
namespace AndroidToolbox
{
    /// <summary>
    /// CompileDex.xaml 的交互逻辑
    /// </summary>
    public partial class CompileDex : Window
    {public event DelReadStdOutput ReadStdOutput;
        public event DelReadErrOutput ReadErrOutput;
        public event DelProcessExit ProcessExit;

        public CompileDex()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            ReadStdOutput += new DelReadStdOutput(ReadStdOutputAction);
            ReadErrOutput += new DelReadErrOutput(ReadErrOutputAction);
            ProcessExit += new DelProcessExit(CmdProcess_Exited);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RealAction("compiledex.bat", this.tbSourceFile.Text +" "+this.tbDstFile.Text);
        }

        private void RealAction(string StartFileName, string StartFileArg)
        {
            Process CmdProcess = new Process();
            CmdProcess.StartInfo.FileName = StartFileName;      // 命令  
            CmdProcess.StartInfo.Arguments = StartFileArg;      // 参数  

            CmdProcess.StartInfo.CreateNoWindow = true;         // 不创建新窗口  
            CmdProcess.StartInfo.UseShellExecute = false;
            CmdProcess.StartInfo.RedirectStandardInput = true;  // 重定向输入  
            CmdProcess.StartInfo.RedirectStandardOutput = true; // 重定向标准输出  
            CmdProcess.StartInfo.RedirectStandardError = true;  // 重定向错误输出  
            //CmdProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;  

            CmdProcess.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            CmdProcess.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);

            CmdProcess.EnableRaisingEvents = true;                      // 启用Exited事件  
            CmdProcess.Exited += new EventHandler(p_ProcessExit);   // 注册进程结束事件  

            CmdProcess.Start();
            CmdProcess.BeginOutputReadLine();
            CmdProcess.BeginErrorReadLine();

            // 如果打开注释，则以同步方式执行命令，此例子中用Exited事件异步执行。  
            // CmdProcess.WaitForExit();       
        }

        private void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                // 4. 异步调用，需要invoke  
                if (!Dispatcher.CheckAccess())
                {
                    Dispatcher.Invoke(ReadStdOutput, new object[] { e.Data });
                }
            }
        }

        private void p_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                if (!Dispatcher.CheckAccess())
                {
                    Dispatcher.Invoke(ReadErrOutput, new object[] { e.Data });
                }

            }
        }

        private void p_ProcessExit(object sender, EventArgs e)
        {
            
                if (!Dispatcher.CheckAccess())
                {
                    Dispatcher.Invoke(ProcessExit, e);
                }

            
        }

        private void ReadStdOutputAction(string result)
        {
            this.textBoxShowStdRet.AppendText(result + "\r\n");
        }

        private void ReadErrOutputAction(string result)
        {
            this.textBoxShowStdRet.AppendText(result + "\r\n");
        }

        private void CmdProcess_Exited(EventArgs e)
        {
            this.textBoxShowStdRet.AppendText("执行结束" + "\r\n");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var d = new CommonOpenFileDialog();
            d.IsFolderPicker = true;	//set to false if need to select files
            d.Title = "选择文件夹位置:";
            var result = d.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                this.tbSourceFile.Text = d.FileName;
                int index = d.FileName.LastIndexOf("\\");
                int length  = d.FileName.Length - index;
                this.tbDstFile.Text = d.FileName + ".apk";
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var d = new CommonOpenFileDialog();
            d.IsFolderPicker = true;	//set to false if need to select files
            d.Title = "选择保存位置:";
            var result = d.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                //this.tbDstFile.Text = d.FileName;
                int index = d.FileName.LastIndexOf("\\");
                int length = d.FileName.Length - index;
                //this.tbDstFile.Text = d.FileName.Substring(0, index);
                this.tbDstFile.Text = d.FileName + ".dex";
            }
        }

    }
}
