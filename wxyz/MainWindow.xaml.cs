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

            this.Width =this.MinWidth = x / 2.2;             //设置窗体宽度
            this.Height =this.MinHeight =  y / 1.8;
            this.FontSize = this.FontSize * 1.2;
            
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
                }
                else
                { 
                    HideFile2Panel();
                }
            }
        }
        

        private void ShowFile2Panel()
        {
            this.ChannelDataText.Visibility = this.SelectButton2.Visibility = this.File2Name.Visibility = this.File2NameDisplay.Visibility = this.File2Time.Visibility = this.File2TimeDisplay.Visibility = this.File2Size.Visibility = this.File2SizeDisplay.Visibility = Visibility.Visible;
        }

        private void HideFile2Panel()
        {
            this.ChannelDataText.Visibility = this.SelectButton2.Visibility= this.File2Name.Visibility = this.File2NameDisplay.Visibility = this.File2Time.Visibility = this.File2TimeDisplay.Visibility = this.File2Size.Visibility = this.File2SizeDisplay.Visibility = Visibility.Hidden;
        }

        private void FunctionButton1_Click(string channel, string mode, params string[] files)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
          ModeSelectionChanged(this.ModeSelection, null);
        }

        private void FunctionButton_Click(object sender, RoutedEventArgs e)
        {
            string channel = ((dynamic)this.DataContext).UI.channel;
            string mode = ((dynamic)this.DataContext).UI.Mode;
            string file1 = ((dynamic)this.DataContext).UI.File1;
            string file2 = ((dynamic)this.DataContext).UI.File2;

            Functions.ButtonFunction(channel, mode, file1, file2);

        }
    }
}