using System;
using System.Collections.Generic;

namespace ProductMonitor.Models
{
    /// <summary>
    /// Modbus配置根对象
    /// </summary>
    public class ModbusConfig
    {
        /// <summary>
        /// 串口配置
        /// </summary>
        public SerialPortConfig SerialPort { get; set; }

        /// <summary>
        /// 数据源配置列表
        /// </summary>
        public List<DataSourceConfig> DataSources { get; set; }
    }

    /// <summary>
    /// 串口配置
    /// </summary>
    public class SerialPortConfig
    {
        /// <summary>
        /// 串口名称 (如 COM3)
        /// </summary>
        public string PortName { get; set; }

        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate { get; set; }

        /// <summary>
        /// 校验位
        /// </summary>
        public string Parity { get; set; }

        /// <summary>
        /// 数据位
        /// </summary>
        public int DataBits { get; set; }

        /// <summary>
        /// 停止位
        /// </summary>
        public string StopBits { get; set; }
    }

    /// <summary>
    /// 数据源配置
    /// </summary>
    public class DataSourceConfig
    {
        /// <summary>
        /// 数据源名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 从站地址
        /// </summary>
        public byte SlaveId { get; set; }

        /// <summary>
        /// 起始地址
        /// </summary>
        public ushort StartAddress { get; set; }

        /// <summary>
        /// 寄存器数量
        /// </summary>
        public ushort NumberOfRegisters { get; set; }

        /// <summary>
        /// 更新间隔(毫秒)
        /// </summary>
        public int UpdateInterval { get; set; }

        /// <summary>
        /// 数据映射配置
        /// </summary>
        public List<DataMappingConfig> DataMapping { get; set; }

        /// <summary>
        /// 每台机器占用的寄存器数量(仅机台数据使用)
        /// </summary>
        public int? RegistersPerMachine { get; set; }

        /// <summary>
        /// 机台数量(仅机台数据使用)
        /// </summary>
        public int? MachineCount { get; set; }

        /// <summary>
        /// 状态映射(仅机台数据使用)
        /// </summary>
        public Dictionary<string, string> StatusMapping { get; set; }
    }

    /// <summary>
    /// 数据映射配置
    /// </summary>
    public class DataMappingConfig
    {
        /// <summary>
        /// 数据项名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 寄存器偏移量
        /// </summary>
        public int RegisterOffset { get; set; }

        /// <summary>
        /// 缩放系数
        /// </summary>
        public double Scale { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
