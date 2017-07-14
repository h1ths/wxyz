using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uvwxyz.Model.Common
{
    /// <summary>  
    /// 选择项类  
    /// </summary>  
    public class ValueKeyItem : INotifyPropertyChanged
    {
        private string _key;
        /// <summary>  
        /// 键  
        /// </summary>  
        public string Key
        {
            get { return _key; }
            set
            {
                _key = value;
                this.NotifyPropertyChanged("Key");
            }
        }

        private string _value;
        /// <summary>  
        /// 值  
        /// </summary>  
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                this.NotifyPropertyChanged("Value");
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
