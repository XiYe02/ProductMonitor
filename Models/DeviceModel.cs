using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductMonitor.Models
{
    /// <summary>
    /// 设备数据模型
    /// </summary>
    public class DeviceModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 设备监控项名称
        /// </summary>
        public string DeviceItem { get; set; }

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
