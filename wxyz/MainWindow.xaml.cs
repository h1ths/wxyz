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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wxyz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            double x = SystemParameters.PrimaryScreenWidth;     //得到屏幕宽度
            double y = SystemParameters.PrimaryScreenHeight;    //得到屏幕高度

            this.Width =this.MinWidth = x / 2.5;             //设置窗体宽度
            this.Height =this.MinHeight =  y / 2;
            this.FontSize = this.FontSize * 1.5;

            Thickness Indent_Line = new Thickness();
            Indent_Line.Left = this.Width * 0.03; 
            //this.channel_360.Margin = this.channel_shunfei.Margin = this.channle_xinshu.Margin  =  Indent_Line;
            this.SelectButton1.Height = this.SelectButton2.Height = this.FontSize * 1.5;
        }

        private void SelectFile1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = ".csv";
            ofd.Filter = "csv file|*.csv";
            if (ofd.ShowDialog() == true)
            {
                string[] file = new string[3];
                file = Functions.GetFileInfo(ofd.FileName);
                this.File1NameDisplay.Text = file[0];
                this.File1TimeDisplay.Text = file[1];
                this.File1SizeDisplay.Text = file[2];
            }
        }

        private void SelectFile2(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = ".csv";
            ofd.Filter = "csv file|*.csv";
            if (ofd.ShowDialog() == true)
            {
                string[] file = new string[3];
                file = Functions.GetFileInfo(ofd.FileName);
                this.File2NameDisplay.Text = file[0];
                this.File2TimeDisplay.Text = file[1];
                this.File2SizeDisplay.Text = file[2];
            }
        }

        private void TextBlock_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }
    }
}
