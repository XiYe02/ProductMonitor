using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMonitor.Models
{
    /// <summary>
    /// 环境信息
    /// </summary>
    public class EnviromentModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 环境项名称
        /// </summary>
        public string EnItemName { get; set; }

        private int _enItemValue;
        /// <summary>
        /// 环境项的值
        /// </summary>
        public int EnItemValue 
        { 
            get { return _enItemValue; }
            set 
            { 
                _enItemValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EnItemValue)));
            }
        }
    }
}
