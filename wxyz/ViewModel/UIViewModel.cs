using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using wxyz;
using wxyz.Model;
using wxyz.Model.Common;

namespace wxyz.ViewModel
{
    /// <summary>  
    /// UIViewModel类  
    /// </summary>  
    public class UIViewModel : INotifyPropertyChanged
    {
        public UIModel _ui = new UIModel();
        /// <summary>  
        /// UI  
        /// </summary>  
        public UIModel UI
        {
            get
            {
                return this._ui;
            }
            set
            {
                this._ui = value;
                this.RaisePropertyChanged("UI");
            }
        }

        public List<ValueKeyItem> _valueKeyList = new List<ValueKeyItem>();
        /// <summary>  
        /// Mode下拉框项  
        /// </summary>  
        public List<ValueKeyItem> ValueKeyList
        {
            get
            {
                return this._valueKeyList;
            }
            set
            {
                this._valueKeyList = value;
                this.RaisePropertyChanged("ValueKeyList");
            }
        }

        /// <summary>  
        /// 初始化  
        /// </summary>  
        public UIViewModel()
        {
            //初始化Mode下拉框项  
            InitValueKeyList();

            //初始化UI  
            UI.Channel = "360";
            UI.File1 = string.Empty;
            UI.File2 = string.Empty;
            UI.Mode = "拼表";
        }

        /// <summary>  
        /// 初始化Mode下拉框项  
        /// </summary>  
        private void InitValueKeyList()
        {
            ValueKeyList = new List<ValueKeyItem>()
        {
            new ValueKeyItem(){Key="拼表",Value="拼表"},
            new ValueKeyItem(){Key="花费",Value="花费"},
        };
        }


        //属性改变事件  
        public event PropertyChangedEventHandler PropertyChanged;

        //当属性改变的时候，调用该方法来发起一个消息，通知View中绑定了propertyName的元素做出调整  
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
            
        }
    }
}
