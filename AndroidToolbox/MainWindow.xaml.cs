using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Diagnostics;
namespace AndroidToolbox
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Decompiler decompiler = new Decompiler();
            if (decompiler.ShowDialog() == true)
            {

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Compiler compiler = new Compiler();
            if (compiler.ShowDialog() == true)
            {

            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DecompileDEX dexd = new DecompileDEX();
            if (dexd.ShowDialog() == true)
            {

            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            CompileDex dexc = new CompileDex();
            if (dexc.ShowDialog() == true)
            {

            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Process.Start(".\\Windows_sign_tool\\Windows\\APKSign.exe");
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Process.Start(".\\jadx\\bin\\jadx-gui.bat");
        }
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            Process.Start("luyten.exe");
        }
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            Dex2Jar d = new Dex2Jar();
            if (d.ShowDialog() == true)
            {

            }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            Jar2Dex j = new Jar2Dex();
            if (j.ShowDialog() == true)
            {

            }
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("1:使用apk编译和反编译时请先安装必要组件，安装方法:\n\tjava -jar apktool.jar if framework-res.apk\n\tjava -jar apktool.jar if SystemUI.apk\n"+
                "2:升级组件时替换各个文件即可\n" +
                "3:使用Enjarify请安装Python，并加入环境变量");
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            Enjarify enjarify = new Enjarify();
            if (enjarify.ShowDialog() == true)
            {

            }
        }
    }
}
