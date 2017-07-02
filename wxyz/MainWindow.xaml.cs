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

            this.Width = x / 3;             //设置窗体宽度
            this.Height = y / 2;
            this.FontSize = this.FontSize * 1.5;

            Thickness LineMargin = new Thickness();
            LineMargin.Bottom = this.Height * 0.02;
            this.ChannelText.Margin = LineMargin;
            this.ChannelGrid.Margin = LineMargin;
            this.SelectFile.Margin = LineMargin;

            Thickness Indent_Line = new Thickness();
            Indent_Line.Bottom = this.Height * 0.02;
            Indent_Line.Left = this.Width * 0.03; 
            this.channel_360.Margin = this.channel_shunfei.Margin = this.channle_xinshu.Margin =this.FilesName.Margin = this.FilesTime.Margin = this.FilesSize.Margin =  Indent_Line;
        }

        private void TextBlock_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }
    }
}
