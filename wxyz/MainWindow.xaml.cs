using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using wxyz.ViewModel;

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
            this.DataContext = new UIViewModel();
            // MessageBox.Show(((dynamic)this.DataContext).UI.Channel);

            double x = SystemParameters.PrimaryScreenWidth;     //得到屏幕宽度
            double y = SystemParameters.PrimaryScreenHeight;    //得到屏幕高度

            this.Width =this.MinWidth = x / 2.8;             //设置窗体宽度
            this.Height =this.MinHeight =  y / 2;
            this.FontSize = this.FontSize * 1.25;
            
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
                    if(((dynamic)this.DataContext).UI.File1 == String.Empty || ((dynamic)this.DataContext).UI.File2 == String.Empty)
                    {
                        this.FunctionButton.IsEnabled = false;
                    }
                    else
                    {
                        this.FunctionButton.IsEnabled = true;
                    }
                }
                else
                { 
                    HideFile2Panel();
                    if (((dynamic)this.DataContext).UI.File1 == String.Empty)
                    {
                        this.FunctionButton.IsEnabled = false;
                    }
                    else
                    {
                        this.FunctionButton.IsEnabled = true;
                    }
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
            string channel = ((dynamic)this.DataContext).UI.Channel;
            string mode = ((dynamic)this.DataContext).UI.Mode;
            string file1 = ((dynamic)this.DataContext).UI.File1;
            string file2 = ((dynamic)this.DataContext).UI.File2;
            string game = ((dynamic)this.DataContext).UI.Game;
            string date = Convert.ToDateTime(((dynamic)this.DataContext).UI.Date).ToString("yyyy/MM/dd") ;
            if(file1 != string.Empty)
            {
                // MessageBox.Show(file1);
                Functions buttonClick = new Functions(mode, date, channel, game, file1, file2);
                Dictionary<string, string> ResultMessage = buttonClick.ButtonFunction();
                if (ResultMessage["code"] == "0")
                {
                    this.StatusText.Text = "Do nothing.";
                }
                else
                {
                    this.StatusText.Text = ResultMessage["message"];
                }
            }

        }
    }
}