using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMonitor.Models
{
    /// <summary>
    /// 雷达图数据模型
    /// </summary>
    public class RaderModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 项名称
        /// </summary>
        public string ItemName { get; set; }

        private double _value;
        /// <summary>
        /// 值
        /// </summary>
        public double Value 
        { 
            get { return _value; }
            set 
            { 
                _value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }
    }
}
