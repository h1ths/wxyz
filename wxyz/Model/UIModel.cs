using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wxyz.Model
{

    /// <summary>  
    /// UI实体类  
    /// </summary>  
    public class UIModel: INotifyPropertyChanged
    {
        private string _channel;
        /// <summary>  
        /// 渠道  
        /// </summary>  
        public string Channel
        {
            get { return _channel; }
            set
            {
                _channel = value;
                this.NotifyPropertyChanged("Channel");
            }
        }

        private string _file1;
        /// <summary>  
        /// 文件1  
        /// </summary>  
        public string File1
        {
            get { return _file1; }
            set
            {
                _file1 = value;
                this.NotifyPropertyChanged("File1");
            }
        }

        private string _file2;
        /// <summary>  
        /// 文件2  
        /// </summary>  
        public string File2
        {
            get { return _file2; }
            set
            {
                _file2 = value;
                this.NotifyPropertyChanged("File2");
            }
        }

        private string _mode;
        /// <summary>  
        /// 模式  
        /// </summary>  
        public string Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                this.NotifyPropertyChanged("Mode");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
