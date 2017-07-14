using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using uvwxyz.ViewModel;

namespace uvwxyz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Functions ButtonClick;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new UIViewModel();
            this.ButtonClick = new Functions();
            double x = SystemParameters.PrimaryScreenWidth;     //得到屏幕宽度
            double y = SystemParameters.PrimaryScreenHeight;    //得到屏幕高度

            this.Width = this.MinWidth = x / 2.5;             //设置窗体宽度
            this.Height = this.MinHeight = y / 2;
            this.FontSize = this.FontSize * 1.25;      
        }

        /// <summary>Brings main window to foreground.</summary>
        public void BringToForeground()
        {
            if (this.WindowState == WindowState.Minimized || this.Visibility == Visibility.Hidden)
            {
                this.Show();
                this.WindowState = WindowState.Normal;
            }

            // According to some sources these steps gurantee that an app will be brought to foreground.
            this.Activate();
            this.Topmost = true;
            this.Topmost = false;
            this.Focus();
        }

        private void SelectFile1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            if (((dynamic)this.DataContext).UI.Channel== "新数")
            {
                ofd.DefaultExt = ".xls";
                ofd.Filter = "excel|*.xls";
            }
            else
            {
                ofd.DefaultExt = ".csv";
                ofd.Filter = "csv file|*.csv";
            }
           
            if (ofd.ShowDialog() == true)
            {
                string[] file = new string[3];
                file = Functions.GetFileInfo(ofd.FileName);
                this.File1NameDisplay.Text = file[0];
                this.File1TimeDisplay.Text = file[1];
                this.File1SizeDisplay.Text = file[2];
                ((dynamic)this.DataContext).UI.File1 = ofd.FileName;
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
                ((dynamic)this.DataContext).UI.File2 = ofd.FileName;
            }
        }
        
        private void ModeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            var value = combo.SelectedValue;
            if(value != null)
            {
                if((string)value == "拼表")
                {
                    ShowFile2Panel();
                }
                else
                { 
                    HideFile2Panel();
                }
            }
        }

        private void ShowFile2Panel()
        {
            this.SelectButton2.IsEnabled = true;
            this.SubsText.Opacity = this.File2Name.Opacity = this.File2NameDisplay.Opacity = this.File2Time.Opacity = this.File2TimeDisplay.Opacity = this.File2Size.Opacity = this.File2SizeDisplay.Opacity = 1;
            //this.ChannelDataText.Visibility = this.SelectButton2.Visibility = this.File2Name.Visibility = this.File2NameDisplay.Visibility = this.File2Time.Visibility = this.File2TimeDisplay.Visibility = this.File2Size.Visibility = this.File2SizeDisplay.Visibility = Visibility.Visible;
        }

        private void HideFile2Panel()
        {
            //this.ChannelDataText.Visibility = this.SelectButton2.Visibility= this.File2Name.Visibility = this.File2NameDisplay.Visibility = this.File2Time.Visibility = this.File2TimeDisplay.Visibility = this.File2Size.Visibility = this.File2SizeDisplay.Visibility = Visibility.Hidden;
            this.SelectButton2.IsEnabled = false;
            this.SubsText.Opacity = this.File2Name.Opacity = this.File2Size.Opacity  = this.File2Time.Opacity = 0.5;
            this.File2TimeDisplay.Opacity = this.File2NameDisplay.Opacity = this.File2SizeDisplay.Opacity = 0.9;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
          ModeSelectionChanged(this.ModeSelection, null);
        }

        private void FunctionButton_Click(object sender, RoutedEventArgs e)
        {     
            Message ResultMessage = this.ButtonClick.ButtonFunction(((dynamic)this.DataContext).UI.Mode, Convert.ToDateTime(((dynamic)this.DataContext).UI.Date).ToString("yyyy/MM/dd"), ((dynamic)this.DataContext).UI.Channel, ((dynamic)this.DataContext).UI.Game, ((dynamic)this.DataContext).UI.File1, ((dynamic)this.DataContext).UI.File2);
            if (ResultMessage.code == 0)
            {
                ((dynamic)this.DataContext).UI.Message = ResultMessage.text;
            }
            else
            {
                ((dynamic)this.DataContext).UI.Message = ResultMessage.text;
            }
        }
    }
}