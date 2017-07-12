using System;
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



        public List<ValueKeyItem> _modeValueKeyList = new List<ValueKeyItem>();
        /// <summary>  
        /// mode下拉框项  
        /// </summary>  
        public List<ValueKeyItem> ModeValueKeyList
        {
            get
            {
                return this._modeValueKeyList;
            }
            set
            {
                this._modeValueKeyList = value;
                this.RaisePropertyChanged("ModeValueKeyList");
            }
        }


        public List<ValueKeyItem> _channelValueKeyList = new List<ValueKeyItem>();
        /// <summary>  
        /// Channel下拉框项  
        /// </summary>  
        public List<ValueKeyItem> ChannelValueKeyList
        {
            get
            {
                return this._channelValueKeyList;
            }
            set
            {
                this._channelValueKeyList = value;
                this.RaisePropertyChanged("ChannelValueKeyList");
            }
        }

        public List<ValueKeyItem> _gameValueKeyList = new List<ValueKeyItem>();
        public List<ValueKeyItem> GameValueKeyList
        {
            get
            {
                return this._gameValueKeyList;
            }
            set
            {
                this._gameValueKeyList = value;
                this.RaisePropertyChanged("GameValueKeyList");
            }
        }

        /// <summary>  
        /// 初始化  
        /// </summary>  
        public UIViewModel()
        {
            //初始化下拉框项 
            InitChannelValueKeyList();
            InitModeValueKeyList();
            InitGameValueKeyList();

            //初始化UI
            UI.Channel = "360";
            UI.File1 = string.Empty;
            UI.File2 = string.Empty;
            UI.Mode = "拼表";
            UI.Game = "三十六计2";
            UI.Date = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            UI.Button = true;
            UI.Message = "^o^";
        }

        /// <summary>  
        /// 初始化Channel下拉框项  
        /// </summary>  
        private void InitChannelValueKeyList()
        {
            ChannelValueKeyList = new List<ValueKeyItem>()
            {
                new ValueKeyItem(){Key="360",Value="360"},
                new ValueKeyItem(){Key="舜飞",Value="舜飞"},
                new ValueKeyItem(){Key="新数",Value="新数"}
            };
        }

        /// <summary>  
        /// 初始化Mode下拉框项  
        /// </summary>  
        private void InitModeValueKeyList()
        {
            ModeValueKeyList = new List<ValueKeyItem>()
        {
            new ValueKeyItem(){Key="拼表",Value="拼表"},
            new ValueKeyItem(){Key="花费",Value="花费"},
        };
        }

        /// <summary>  
        /// 初始化Game下拉框项  
        /// </summary>  
        private void InitGameValueKeyList()
        {
            GameValueKeyList = new List<ValueKeyItem>()
        {
            new ValueKeyItem(){Key="三十六计2",Value="三十六计2"},
            new ValueKeyItem(){Key="盗墓笔记",Value="盗墓笔记"},
        };
        }


        //属性改变事件  
        public event PropertyChangedEventHandler PropertyChanged;

        //当属性改变的时候，调用该方法来发起一个消息，通知View中绑定了propertyName的元素做出调整  
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
