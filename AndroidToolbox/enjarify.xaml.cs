﻿using System;
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
    /// Dex2Jar.xaml 的交互逻辑
    /// </summary>
    public partial class Enjarify : Window
    {
        public event DelReadStdOutput ReadStdOutput;
        public event DelReadErrOutput ReadErrOutput;
        public event DelProcessExit ProcessExit;

        public Enjarify()
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
            RealAction("enjarify\\enjarify.bat", this.tbSourceFile.Text + "   " + this.tbDstFile.Text);
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
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                this.tbSourceFile.Text = ofd.FileName;
                int index = ofd.FileName.LastIndexOf("\\");
                int index2 = ofd.FileName.LastIndexOf(".");
                int length = index2 - index;
                this.tbDstFile.Text = ofd.FileName.Substring(0, index) + "\\" + ofd.FileName.Substring(index + 1, length - 1) + ".jar";
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
                
                if (this.tbSourceFile.Text != null && !this.tbSourceFile.Text.Trim().Equals(""))
                {
                    String sourceFile = this.tbSourceFile.Text;
                    int index = sourceFile.LastIndexOf("\\");
                    int index2 = sourceFile.LastIndexOf(".");
                    int length = index2 - index;
                    this.tbDstFile.Text = d.FileName + "\\" + sourceFile.Substring(index + 1, length - 1) + ".jar";
                }
                else
                {
                    this.tbDstFile.Text = d.FileName + "classes.jar";
                }
            }
        }
    }
}
