using Modbus.Device;
using ProductMonitor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Threading.Tasks;

namespace ProductMonitor.Services
{
    /// <summary>
    /// Modbus通信服务类
    /// </summary>
    public class ModbusService
    {
        private readonly string _portName;
        private readonly int _baudRate;
        private readonly byte _slaveId;
        private readonly byte _startAddress;
        private readonly byte _numberOfRegisters;
        SerialPort serialPort;
        IModbusSerialMaster master;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="portName">串口名称，默认COM3</param>
        /// <param name="baudRate">波特率，默认9600</param>
        /// <param name="slaveId">从设备地址，默认1</param>
        public ModbusService(string portName = "COM3", int baudRate = 9600)
        {
            _portName = portName;
            _baudRate = baudRate;
            _slaveId = 1;
            _startAddress = 0;
            _numberOfRegisters = 8;
            serialPort = new SerialPort(_portName, _baudRate, Parity.None, 8, StopBits.One);
            serialPort.Open();
            master = ModbusSerialMaster.CreateRtu(serialPort);

        }

        /// <summary>
        /// 读取设备监控数据
        /// </summary>
        /// <returns>设备数据列表</returns>
        public async Task<List<DeviceModel>> ReadDeviceDataAsync(byte slaveId , ushort startAddress ,ushort numberOfRegisters)
        {
            return await Task.Run(() =>
            {
                var deviceList = new List<DeviceModel>();
                
                try
                {
                        // 读取8个寄存器的数据（对应8个设备监控项）
                        // 假设设备数据存储在寄存器地址0-7
                        ushort[] values = master.ReadHoldingRegisters(slaveId, startAddress, numberOfRegisters);

                        // 根据实际设备配置映射数据
                        deviceList.Add(new DeviceModel { DeviceItem = "电能(Kw.h)", Value = values[0]  }); // 假设需要缩放
                        deviceList.Add(new DeviceModel { DeviceItem = "电压(V)", Value = values[1] });
                        deviceList.Add(new DeviceModel { DeviceItem = "电流(A)", Value = values[2]  });
                        deviceList.Add(new DeviceModel { DeviceItem = "压差(kpa)", Value = values[3] });
                        deviceList.Add(new DeviceModel { DeviceItem = "温度(℃)", Value = values[4] });
                        deviceList.Add(new DeviceModel { DeviceItem = "振动(mm/s)", Value = values[5] });
                        deviceList.Add(new DeviceModel { DeviceItem = "转速(r/min)", Value = values[6] });
                        deviceList.Add(new DeviceModel { DeviceItem = "气压(kpa)", Value = values[7]  });
                  
                }
                catch (Exception ex)
                {
                    // 如果读取失败，返回默认值（可以记录日志）
                    Console.WriteLine($"Modbus读取失败: {ex.Message}");
                    
                   
                }

                return deviceList;
            });
        }

        /// <summary>
        /// 读取环境监控数据
        /// </summary>
        /// <returns>环境数据列表</returns>
        public async Task<List<EnviromentModel>> ReadEnvironmentDataAsync(byte slaveId, ushort startAddress, ushort numberOfPoints)
        {
            
                return await Task.Run(() =>
                {
                    var environmentList = new List<EnviromentModel>();

                    try
                    {


                        // 读取7个寄存器的数据（对应7个环境监控项）
                        // 假设环境数据存储在寄存器地址8-14（与设备数据分开）
                        ushort[] values = master.ReadHoldingRegisters(slaveId, startAddress, numberOfPoints);

                        // 根据实际设备配置映射环境数据
                        environmentList.Add(new EnviromentModel { EnItemName = "光照(Lux)", EnItemValue = values[0] });
                        environmentList.Add(new EnviromentModel { EnItemName = "噪音(db)", EnItemValue = values[1] });
                        environmentList.Add(new EnviromentModel { EnItemName = "温度(℃)", EnItemValue = values[2] });
                        environmentList.Add(new EnviromentModel { EnItemName = "湿度(%)", EnItemValue = values[3] });
                        environmentList.Add(new EnviromentModel { EnItemName = "PM2.5(m?)", EnItemValue = values[4] });
                        environmentList.Add(new EnviromentModel { EnItemName = "硫化氢(PPM)", EnItemValue = values[5] });
                        environmentList.Add(new EnviromentModel { EnItemName = "氮气(PPM)", EnItemValue = values[6] });

                    }
                    catch (Exception ex)
                    {
                        // 如果读取失败，返回默认值（可以记录日志）
                        Console.WriteLine($"Modbus环境数据读取失败: {ex.Message}");


                    }

                    return environmentList;
                });
            
        }

        /// <summary>
        /// 读取雷达数据
        /// </summary>
        /// <returns>雷达数据列表</returns>
        public async Task<List<RaderModel>> ReadRadarDataAsync(byte slaveId, ushort startAddress, ushort numberOfRegisters)
        {
            return await Task.Run(() =>
            {
                var radarList = new List<RaderModel>();
                
                try
                {
                  
                  
                        // 读取5个寄存器的数据（对应5个雷达监控项）
                        // 假设雷达数据存储在寄存器地址15-19（与设备和环境数据分开）
                        ushort[] values = master.ReadHoldingRegisters(slaveId, startAddress, numberOfRegisters);

                        // 根据实际设备配置映射雷达数据
                        radarList.Add(new RaderModel { ItemName = "排烟风机", Value = values[0] });
                        radarList.Add(new RaderModel { ItemName = "客梯", Value = values[1]  }); // 假设需要缩放
                        radarList.Add(new RaderModel { ItemName = "供水机", Value = values[2]  }); // 假设需要缩放
                        radarList.Add(new RaderModel { ItemName = "喷淋水泵", Value = values[3]  }); // 假设需要缩放
                        radarList.Add(new RaderModel { ItemName = "稳压设备", Value = values[4] });
                    
                }
                catch (Exception ex)
                {
                    // 如果读取失败，返回默认值（可以记录日志）
                    System.Diagnostics.Debug.WriteLine($"Modbus雷达数据读取失败: {ex.Message}");
                    
                  
                }

                return radarList;
            });
        }

        /// <summary>
        /// 读取单个寄存器数据
        /// </summary>
        /// <param name="startAddress">起始地址</param>
        /// <param name="numberOfPoints">寄存器数量</param>
        /// <returns>寄存器值数组</returns>
        public async Task<ushort[]> ReadHoldingRegistersAsync(byte slaverId, ushort startAddress, ushort numberOfPoints)
        {
            return await Task.Run(() =>
            {
                try
                {
                   
                        return master.ReadHoldingRegisters(slaverId, startAddress, numberOfPoints);
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"读取寄存器失败: {ex.Message}");
                    return null; // 返回null表示读取失败
                }
            });
        }

       

        /// <summary>
        /// 读取饼图数据
        /// </summary>
        /// <returns>饼图数据集合</returns>
        public async Task<ObservableCollection<PieChartModel>> ReadPieChartDataAsync(byte slaveId, ushort startAddress, ushort numberOfPoints)
        {
            return await Task.Run(() =>
            {
                var pieChartData = new ObservableCollection<PieChartModel>();
                
                try
                {
                   
                       

                        // 假设饼图数据存储在寄存器地址40-43（4个寄存器）
                        ushort[] values = master.ReadHoldingRegisters(slaveId, startAddress, numberOfPoints);
            
                        System.Diagnostics.Debug.WriteLine($"Modbus读取成功: [{string.Join(", ", values)}]");

                        // 创建饼图数据对象并添加到集合
                        pieChartData.Add(new PieChartModel { Title = "压差", Value = values[0] });
                        pieChartData.Add(new PieChartModel { Title = "振动", Value = values[1] });
                        pieChartData.Add(new PieChartModel { Title = "设备温度", Value = values[2] });
                        pieChartData.Add(new PieChartModel { Title = "光照", Value = values[3] });
                    
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Modbus饼图数据读取失败: {ex.Message}");
                    
                
                    pieChartData.Add(new PieChartModel { Title = "压差", Value=25 });
                    pieChartData.Add(new PieChartModel { Title = "振动", Value = 25 });
                    pieChartData.Add(new PieChartModel { Title = "设备温度", Value = 25 });
                    pieChartData.Add(new PieChartModel { Title = "光照", Value = 25 } );
                    
                    System.Diagnostics.Debug.WriteLine($"使用模拟数据: [压差={pieChartData[0].Value}, 振动={pieChartData[1].Value}, 设备温度={pieChartData[2].Value}, 光照={pieChartData[3].Value}]");
                }

                return pieChartData;
            });
        }
    }
}