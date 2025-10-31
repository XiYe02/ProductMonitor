using ProductMonitor.Models;
using System;
using System.IO;
using System.Text.Json;

namespace ProductMonitor.Services
{
    /// <summary>
    /// Modbus配置服务
    /// </summary>
    public class ConfigService
    {
        private static ConfigService _instance;
        private static readonly object _lock = new object();
        private ModbusConfig _config;
        private readonly string _configFilePath;

        /// <summary>
        /// 单例实例
        /// </summary>
        public static ConfigService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ConfigService();
                        }
                    }
                }
                return _instance;
            }
        }

        private ConfigService()
        {
            // 配置文件路径在程序根目录下
            _configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "modbusconfig.json");
            LoadConfig();
        }

        /// <summary>
        /// 加载配置文件
        /// </summary>
        private void LoadConfig()
        {
            try
            {
                if (!File.Exists(_configFilePath))
                {
                    throw new FileNotFoundException($"配置文件不存在: {_configFilePath}");
                }

                string json = File.ReadAllText(_configFilePath);
                _config = JsonSerializer.Deserialize<ModbusConfig>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                });

                if (_config == null)
                {
                    throw new Exception("配置文件解析失败");
                }

                Console.WriteLine($"成功加载Modbus配置文件: {_configFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载配置文件失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 重新加载配置文件
        /// </summary>
        public void ReloadConfig()
        {
            LoadConfig();
        }

        /// <summary>
        /// 获取完整配置
        /// </summary>
        public ModbusConfig GetConfig()
        {
            return _config;
        }

        /// <summary>
        /// 获取串口配置
        /// </summary>
        public SerialPortConfig GetSerialPortConfig()
        {
            return _config?.SerialPort;
        }

        /// <summary>
        /// 根据名称获取数据源配置
        /// </summary>
        /// <param name="name">数据源名称</param>
        /// <returns>数据源配置，未找到返回null</returns>
        public DataSourceConfig GetDataSourceConfig(string name)
        {
            return _config?.DataSources?.Find(ds => ds.Name == name);
        }

        /// <summary>
        /// 获取所有数据源配置
        /// </summary>
        public System.Collections.Generic.List<DataSourceConfig> GetAllDataSources()
        {
            return _config?.DataSources;
        }
    }
}
