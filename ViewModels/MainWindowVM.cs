using ProductMonitor.Models;
using ProductMonitor.Services;
using ProductMonitor.UserControls;
using ProductMonitor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Wpf;

namespace ProductMonitor.ViewModels
{
    internal class MainWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly ModbusService _modbusService;
        private readonly ConfigService _configService;
      
        private Timer ReadModbusDataTimer;

        /// <summary>
        /// 视图模型构造函数
        /// </summary>
        public MainWindowVM()
        {
            // 初始化配置服务
            _configService = ConfigService.Instance;
            
            // 初始化Modbus服务
            _modbusService = new ModbusService();

            ReadModbusDataTimer = new Timer(1000);
            ReadModbusDataTimer.Elapsed += ReadModbusData;
            ReadModbusDataTimer.AutoReset = true;
          

            

            // 初始化饼图数据模型
            PieChartData = new ObservableCollection<PieChartModel>()
            {
                 new PieChartModel { Title = "压差", Value = 25 },
                 new PieChartModel { Title = "振动", Value = 25 },
                 new PieChartModel { Title = "设备温度", Value = 25 },
                 new PieChartModel { Title = "光照", Value = 25 }
            };

            // 初始化饼图Series集合 - 先创建SeriesCollection再填充数据
            InitializePieChartData();
       

            #region 初始化环境监控数据
            EnviromentList = new List<EnviromentModel>();

            // 初始化默认环境数据
            EnviromentList.Add(new EnviromentModel { EnItemName = "光照(Lux)", EnItemValue = 123 });
            EnviromentList.Add(new EnviromentModel { EnItemName = "噪音(db)", EnItemValue = 55 });
            EnviromentList.Add(new EnviromentModel { EnItemName = "温度(℃)", EnItemValue = 80 });
            EnviromentList.Add(new EnviromentModel { EnItemName = "湿度(%)", EnItemValue = 43 });
            EnviromentList.Add(new EnviromentModel { EnItemName = "PM2.5(m³)", EnItemValue = 20 });
            EnviromentList.Add(new EnviromentModel { EnItemName = "硫化氢(PPM)", EnItemValue = 15 });
            EnviromentList.Add(new EnviromentModel { EnItemName = "氮气(PPM)", EnItemValue = 18 });

  
            #endregion

            #region 初始化报警列表

            AlarmList = new List<AlarmModel>();
            AlarmList.Add(new AlarmModel { Num = "01", Msg = "设备温度过高", Time = "2023-11-23 18:34:56", Duration = 7 });
            AlarmList.Add(new AlarmModel { Num = "02", Msg = "车间温度过高", Time = "2023-12-08 20:40:59", Duration = 10 });
            AlarmList.Add(new AlarmModel { Num = "03", Msg = "设备转速过快", Time = "2024-01-05 12:24:34", Duration = 12 });
            AlarmList.Add(new AlarmModel { Num = "04", Msg = "设备气压偏低", Time = "2024-02-04 19:58:00", Duration = 90 });
            #endregion

            #region 初始化设备监控
            DeviceList = new List<DeviceModel>();
            
            // 初始化默认数据
            DeviceList.Add(new DeviceModel { DeviceItem = "电能(Kw.h)", Value = 60.8 });
            DeviceList.Add(new DeviceModel { DeviceItem = "电压(V)", Value = 390 });
            DeviceList.Add(new DeviceModel { DeviceItem = "电流(A)", Value = 5 });
            DeviceList.Add(new DeviceModel { DeviceItem = "压差(kpa)", Value = 13 });
            DeviceList.Add(new DeviceModel { DeviceItem = "温度(℃)", Value = 36 });
            DeviceList.Add(new DeviceModel { DeviceItem = "振动(mm/s)", Value = 4.1 });
            DeviceList.Add(new DeviceModel { DeviceItem = "转速(r/min)", Value = 2600 });
            DeviceList.Add(new DeviceModel { DeviceItem = "气压(kpa)", Value = 0.5 });

  
            #endregion

            #region 初始化雷达数据 
            RaderList = new List<RaderModel>();

            // 初始化默认雷达数据
            RaderList.Add(new RaderModel { ItemName = "排烟风机", Value = 90 });
            RaderList.Add(new RaderModel { ItemName = "客梯", Value = 30.00 });
            RaderList.Add(new RaderModel { ItemName = "供水机", Value = 34.89 });
            RaderList.Add(new RaderModel { ItemName = "喷淋水泵", Value = 69.59 });
            RaderList.Add(new RaderModel { ItemName = "稳压设备", Value = 20 });

            ReadModbusDataTimer.Start();

            #endregion

            #region 初始化人员缺岗信息
            StuffOutWorkList = new List<StuffOutWorkModel>();
            StuffOutWorkList.Add(new StuffOutWorkModel { StuffName = "张晓婷", Position = "技术员", OutWorkCount = 123 });
            StuffOutWorkList.Add(new StuffOutWorkModel { StuffName = "李晓", Position = "操作员", OutWorkCount = 23 });
            StuffOutWorkList.Add(new StuffOutWorkModel { StuffName = "王克俭", Position = "技术员", OutWorkCount = 134 });
            StuffOutWorkList.Add(new StuffOutWorkModel { StuffName = "陈家栋", Position = "统计员", OutWorkCount = 143 });
            StuffOutWorkList.Add(new StuffOutWorkModel { StuffName = "杨过", Position = "技术员", OutWorkCount = 12 });

            #endregion

            #region 初始化车间列表 
            WorkShopList = new List<WorkShopModel>();
            WorkShopList.Add(new WorkShopModel { WorkShopName = "贴片车间", WorkingCount = 32, WaitCount = 8, WrongCount = 4, StopCount = 0 });
            WorkShopList.Add(new WorkShopModel { WorkShopName = "封装车间", WorkingCount = 20, WaitCount = 8, WrongCount = 4, StopCount = 0 });
            WorkShopList.Add(new WorkShopModel { WorkShopName = "焊接车间", WorkingCount = 68, WaitCount = 8, WrongCount = 4, StopCount = 0 });
            WorkShopList.Add(new WorkShopModel { WorkShopName = "贴片车间", WorkingCount = 68, WaitCount = 8, WrongCount = 4, StopCount = 0 });

            #endregion

            #region 初始化机台列表
            MachineList = new List<MachineModel>();
            Random random = new Random();
            for (int i = 0; i < 20; i++)
            {
                int plan = random.Next(100, 1000);//计划量 随机数
                int finished = random.Next(0, plan);//已完成量
                MachineList.Add(new MachineModel
                {
                    MachineName = "焊接机-" + (i + 1),
                    FinishedCount = finished,
                    PlanCount = plan,
                    Status = "作业中",
                    OrderNo = "H202212345678"
                });
            }
            #endregion

        }



        #region 监控用户控件属性
        /// <summary>
        /// 监控用户控件
        /// </summary>
        private UserControl _MonitorUC;

        /// <summary>
        /// 监控用户控件
        /// </summary>
        public UserControl MonitorUC
        {
            get
            {
                if (_MonitorUC == null)
                {
                    _MonitorUC = new MonitorUC();
                }
                return _MonitorUC;
            }
            set
            {
                _MonitorUC = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MonitorUC"));
                }
            }
        } 
        #endregion

        #region 时间 日期 
        /// <summary>
        /// 时间 小时:分钟
        /// </summary>
        public string TimeStr
        {
            get
            {
                return DateTime.Now.ToString("HH:mm");
            }
        }

        /// <summary>
        /// 日期 年-月-日
        /// </summary>
        public string DateStr
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 星期
        /// </summary>
        public string WeekStr
        {
            get
            {
                int index = (int)DateTime.Now.DayOfWeek;

                string[] week = new string[7] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };

                return week[index];
            }
        }
        #endregion

        #region 计数
        /// <summary>
        /// 机台总数
        /// </summary>
        private string _MachineCount = "02981";

        /// <summary>
        /// 机台总数
        /// </summary>
        public string MachineCount
        {
            get { return _MachineCount; }
            set
            {
                _MachineCount = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MachineCount"));
                }
            }
        }

        /// <summary>
        /// 生产计数
        /// </summary>
        private string _ProductCount = "16403";

        /// <summary>
        /// 生产计数
        /// </summary>
        public string ProductCount
        {
            get { return _ProductCount; }
            set
            {
                _ProductCount = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ProductCount"));
                }
            }
        }

        /// <summary>
        /// 不良计数
        /// </summary>
        private string _BadCount = "0403";

        /// <summary>
        /// 不良计数
        /// </summary>
        public string BadCount
        {
            get { return _BadCount; }
            set
            {
                _BadCount = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("BadCount"));
                }
            }
        }
        #endregion

        #region 柱状图数据属性
        /// <summary>
        /// 生产计数柱状图数据
        /// </summary>
        private ChartValues<double> _ProductionChartData = new ChartValues<double> { };

        /// <summary>
        /// 生产计数柱状图数据
        /// </summary>
        public ChartValues<double> ProductionChartData
        {
            get { return _ProductionChartData; }
            set
            {
                _ProductionChartData = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ProductionChartData"));
                }
            }
        }

        /// <summary>
        /// 不良计数柱状图数据
        /// </summary>
        private ChartValues<double> _DefectChartData = new ChartValues<double> { };

        /// <summary>
        /// 不良计数柱状图数据
        /// </summary>
        public ChartValues<double> DefectChartData
        {
            get { return _DefectChartData; }
            set
            {
                _DefectChartData = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("DefectChartData"));
                }
            }
        }
        #endregion


        #region 折线图数据属性

        /// <summary>
        /// 质量数据
        /// </summary>
        private ChartValues<double> _QualityChartData = new ChartValues<double> { };

        /// <summary>
        /// 生产计数柱状图数据
        /// </summary>
        public ChartValues<double> QualityChartData
        {
            get { return _QualityChartData; }
            set
            {
                _QualityChartData = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("QualityChartData"));
                }
            }
        }
        #endregion

        #region 环境监控数据
        /// <summary>
        /// 环境监控数据
        /// </summary>
        private List<EnviromentModel> _EnviromentList;

        /// <summary>
        /// 环境监控数据
        /// </summary>
        public List<EnviromentModel> EnviromentList
        {
            get { return _EnviromentList; }
            set
            {
                _EnviromentList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("EnviromentList"));
                }
            }
        }

        #endregion

        #region 报警属性

        /// <summary>
        /// 报警集合
        /// </summary>
        private List<AlarmModel> _AlarmList;

        /// <summary>
        /// 报警集合
        /// </summary>
        public List<AlarmModel> AlarmList
        {
            get { return _AlarmList; }
            set
            {
                _AlarmList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AlarmList"));
                }
            }
        }
        #endregion

        #region 设备集合属性
        /// <summary>
        /// 设备集合
        /// </summary>
        private List<DeviceModel> _DeviceList;

        /// <summary>
        /// 设备集合
        /// </summary>
        public List<DeviceModel> DeviceList
        {
            get { return _DeviceList; }
            set
            {
                _DeviceList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("DeviceList"));
                }
            }
        }
        #endregion


        #region 雷达数据属性
        /// <summary>
        /// 雷达
        /// </summary>
        private List<RaderModel> _RaderList;

        /// <summary>
        /// 雷达
        /// </summary>
        public List<RaderModel> RaderList
        {
            get { return _RaderList; }
            set
            {
                _RaderList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("RaderList"));
                }
            }
        }

        #endregion

        #region 缺岗员工属性

        /// <summary>
        /// 缺岗员工
        /// </summary>
        private List<StuffOutWorkModel> _StuffOutWorkList;

        /// <summary>
        /// 缺岗员工
        /// </summary>
        public List<StuffOutWorkModel> StuffOutWorkList
        {
            get { return _StuffOutWorkList; }
            set
            {
                _StuffOutWorkList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("StuffOutWorkList"));
                }
            }
        }
        #endregion

        #region 车间属性
        /// <summary>
        /// 车间
        /// </summary>
        private List<WorkShopModel> _WorkShopList;

        /// <summary>
        /// 车间
        /// </summary>
        public List<WorkShopModel> WorkShopList
        {
            get { return _WorkShopList; }
            set { _WorkShopList = value; }
        }

        #endregion

        #region 机台集合属性

        /// <summary>
        /// 机台集合属性
        /// </summary>
        private List<MachineModel> _MachineList;

        /// <summary>
        /// 机台集合属性
        /// </summary>
        public List<MachineModel> MachineList
        {
            get { return _MachineList; }
            set
            {
                _MachineList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MachineList"));
                }
            }
        }
        #endregion

        #region 饼图数据属性
        /// <summary>
        /// 饼图数据集合（可选）
        /// </summary>
        private ObservableCollection<PieChartModel> _PieChartData;

        /// <summary>
        /// 饼图数据集合
        /// </summary>
        public ObservableCollection<PieChartModel> PieChartData
        {
            get { return _PieChartData; }
            set
            {
                _PieChartData = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PieChartData"));
                }
            }
        }

        /// <summary>
        /// 绑定到PieChart的Series集合
        /// </summary>
        private SeriesCollection _PieSeriesCollection;

        /// <summary>
        /// 绑定到PieChart的Series集合
        /// </summary>
        public SeriesCollection PieSeriesCollection
        {
            get { return _PieSeriesCollection; }
            set
            {
                _PieSeriesCollection = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("PieSeriesCollection"));
                }
            }
        }

        // 缓存上次数据，用于比较是否变化
        private double[] _lastPieValues;
        private string[] _lastPieTitles;
        
        // 缓存上次机台数据，用于比较是否变化
        private List<MachineModel> _lastMachineData;
        #endregion

        private async void ReadModbusData(object? sender, ElapsedEventArgs e)
        {
            // 从配置文件读取各数据源配置
            var deviceConfig = _configService.GetDataSourceConfig("DeviceData");
            var envConfig = _configService.GetDataSourceConfig("EnvironmentData");
            var radarConfig = _configService.GetDataSourceConfig("RadarData");
            var pieConfig = _configService.GetDataSourceConfig("PieChartData");
            var chartConfig = _configService.GetDataSourceConfig("ChartData");
            var qualityConfig = _configService.GetDataSourceConfig("QualityChartData");
            var machineConfig = _configService.GetDataSourceConfig("MachineData");
            
            // 根据配置更新各数据源
            if (deviceConfig != null)
                UpdateDeviceData(deviceConfig.SlaveId, deviceConfig.StartAddress, deviceConfig.NumberOfRegisters);
            
            if (envConfig != null)
                UpdateEnvironmentData(envConfig.SlaveId, envConfig.StartAddress, envConfig.NumberOfRegisters);
            
            if (radarConfig != null)
                UpdateRadarData(radarConfig.SlaveId, radarConfig.StartAddress, radarConfig.NumberOfRegisters);
            
            if (pieConfig != null)
                UpdatePieChartData(pieConfig.SlaveId, pieConfig.StartAddress, pieConfig.NumberOfRegisters);
            
            if (chartConfig != null)
                UpdateChartData(chartConfig.SlaveId, chartConfig.StartAddress, chartConfig.NumberOfRegisters);
            
            if (qualityConfig != null)
                UpdateQualityChartData(qualityConfig.SlaveId, qualityConfig.StartAddress, qualityConfig.NumberOfRegisters);
            
            if (machineConfig != null)
                UpdateMachineData(machineConfig.SlaveId, machineConfig.StartAddress, machineConfig.NumberOfRegisters);
        }

        #region 读取Modbus数据的方法

        /// <summary>
        /// 初始化饼图数据
        /// </summary>
        private void InitializePieChartData()
        {
            // 创建新的SeriesCollection
            var newSeries = new SeriesCollection();
            
            // 从 PieChartData 创建 Series
            if (PieChartData != null && PieChartData.Count > 0)
            {
                foreach (var data in PieChartData)
                {
                    newSeries.Add(new PieSeries
                    {
                        Title = data.Title,
                        Values = new ChartValues<double> { data.Value },
                        StrokeThickness = 0,
                        DataLabels = true,
                        LabelPosition = PieLabelPosition.OutsideSlice
                    });
                }
            }
            
            // 更新PieSeriesCollection，确保触发属性变更通知
            PieSeriesCollection = newSeries;
        }



       
        /// <summary>
        /// 设备数据定时器事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UpdateDeviceData(byte slaveId,ushort startAddress,ushort numberOfRegisters)
        {
            try
            {
               
                // 从Modbus读取设备数据
                var newDeviceData = await _modbusService.ReadDeviceDataAsync(slaveId, startAddress, numberOfRegisters);
                
                // 更新设备列表数据
                for (int i = 0; i < DeviceList.Count && i < newDeviceData.Count; i++)
                {
                    DeviceList[i].Value = newDeviceData[i].Value;
                }

               
            }
            catch (Exception ex)
            {
                // 记录错误（在实际应用中可以使用日志框架）
                Console.WriteLine($"读取设备数据时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 环境数据定时器事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UpdateEnvironmentData(byte slaveId, ushort startAddress, ushort numberOfRegisters)
        {
            try
            {
                

                // 从Modbus读取环境数据
                var newEnvironmentData = await _modbusService.ReadEnvironmentDataAsync(slaveId, startAddress, numberOfRegisters);
                
                // 更新环境列表数据
                for (int i = 0; i < EnviromentList.Count && i < newEnvironmentData.Count; i++)
                {
                    EnviromentList[i].EnItemValue = newEnvironmentData[i].EnItemValue;
                }
            }
            catch (Exception ex)
            {
                // 记录错误（在实际应用中可以使用日志框架）
                Console.WriteLine($"读取环境数据时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 雷达数据定时器事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UpdateRadarData(byte slaveId, ushort startAddress, ushort numberOfRegisters)
        {
            try
            {
            
                // 从Modbus读取雷达数据
                var newRadarData = await _modbusService.ReadRadarDataAsync(slaveId, startAddress, numberOfRegisters);
                
                // 重新设置整个雷达列表以触发UI更新
                RaderList = newRadarData;
            }
            catch (Exception ex)
            {
                // 记录错误（在实际应用中可以使用日志框架）
                Console.WriteLine($"读取雷达数据时发生错误: {ex.Message}");
            }
        }

       

        /// <summary>
        /// 饼图数据定时器事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UpdatePieChartData(byte slaveId, ushort startAddress, ushort numberOfRegisters)
        {
            try
            {
 
                // 从PLC读取饼图数据
                var newPieChartData = await _modbusService.ReadPieChartDataAsync(slaveId, startAddress, numberOfRegisters);
                
                // 检查是否成功读取到数据
                if (newPieChartData == null || newPieChartData.Count == 0)
                {
                   
                    return;
                }

                // 生成当前数据的快照
                var newValues = newPieChartData.Select(d => d.Value).ToArray();
                var newTitles = newPieChartData.Select(d => d.Title).ToArray();
                
                // 比较数据是否变化
                bool dataChanged = false;
                if (_lastPieValues == null || _lastPieTitles == null || 
                    _lastPieValues.Length != newValues.Length || 
                    _lastPieTitles.Length != newTitles.Length)
                {
                    dataChanged = true;
                }
                else
                {
                    // 检查数值是否变化
                    for (int i = 0; i < newValues.Length; i++)
                    {
                        if (Math.Abs(_lastPieValues[i] - newValues[i]) > 0.001 || _lastPieTitles[i] != newTitles[i])
                        {
                            dataChanged = true;
                            break;
                        }
                    }
                }
                
                // 如果数据没有变化，跳过更新
                if (!dataChanged)
                {
                   
                    return;
                }
                
               

                // 使用UI线程更新数据
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    // 更新PieChartData
                    PieChartData = newPieChartData;
                    
                    // 重建饼图的Series集合
                    var newSeries = new SeriesCollection();
                    for (int i = 0; i < newPieChartData.Count; i++)
                    {
                        newSeries.Add(new PieSeries
                        {
                            Title = newTitles[i],
                            Values = new ChartValues<double> { newValues[i] },
                            StrokeThickness = 0,
                            DataLabels = true,
                            LabelPosition = PieLabelPosition.OutsideSlice
                        });
                    }
                    
                    // 更新SeriesCollection
                    PieSeriesCollection = newSeries;
                    
                   
                });
                
                // 缓存本次数据，用于下次比较
                _lastPieValues = newValues;
                _lastPieTitles = newTitles;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"更新饼图数据失败: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"堆栈跟踪: {ex.StackTrace}");
            }
        }

        #endregion

        #region 柱状图数据定时器
        /// <summary>
        /// 图表数据定时器事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UpdateChartData(byte slaveId, ushort startAddress, ushort numberOfRegisters)
        {
            try
            {
             
                // 从Modbus读取生产计数数据
                var productionData = await _modbusService.ReadHoldingRegistersAsync(slaveId, startAddress, numberOfRegisters);
                if (productionData != null)
                {
                    var newProductionData = productionData.Select(v => (double)v);
                    if (!ProductionChartData.SequenceEqual(newProductionData))
                    {
                        ProductionChartData = new ChartValues<double>(newProductionData);
                    }
                }

                // 从Modbus读取不良计数数据
                var defectData = await _modbusService.ReadHoldingRegistersAsync(slaveId, (ushort)(startAddress + numberOfRegisters+1), numberOfRegisters);
                if (defectData != null)
                {
                    var newDefectData = defectData.Select(v => (double)v);
                    if (!DefectChartData.SequenceEqual(newDefectData))
                    {
                        DefectChartData = new ChartValues<double>(newDefectData);
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录错误
                Console.WriteLine($"读取图表数据时发生错误: {ex.Message}");
            }
        }
        #endregion

        #region 质量折线图
        private async void UpdateQualityChartData(byte slaveId, ushort startAddress, ushort numberOfRegisters)
        {
            try
            {

                // 从Modbus读取生产计数数据
                var qualityData = await _modbusService.ReadHoldingRegistersAsync(slaveId, startAddress, numberOfRegisters);
                if (qualityData != null)
                {
                    var newQualityData = qualityData.Select(v => (double)v);
                    if (!QualityChartData.SequenceEqual(newQualityData))
                    {
                        QualityChartData = new ChartValues<double>(newQualityData);
                    }
                }

             
            }
            catch (Exception ex)
            {
                // 记录错误
                Console.WriteLine($"读取图表数据时发生错误: {ex.Message}");
            }
        }
        #endregion

        #region 机台数据定时器
        /// <summary>
        /// 机台数据定时器事件处理
        /// </summary>
        /// <param name="slaveId">从站地址</param>
        /// <param name="startAddress">起始地址</param>
        /// <param name="machineCount">机台数量</param>
        private async void UpdateMachineData(byte slaveId, ushort startAddress, int machineCount)
        {
            try
            {
                // 从Modbus读取机台数据
                var newMachineData = await _modbusService.ReadMachineDataAsync(slaveId, startAddress, machineCount);
                
                // 如果读取失败或为空，保持当前数据
                if (newMachineData == null || newMachineData.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("机台数据读取失败或为空，保持当前数据");
                    return;
                }
                
                // 比较数据是否变化
                bool dataChanged = false;
                
                if (_lastMachineData == null || _lastMachineData.Count != newMachineData.Count)
                {
                    // 首次读取或数量变化
                    dataChanged = true;
                }
                else
                {
                    // 逐台机器比较数据
                    for (int i = 0; i < newMachineData.Count; i++)
                    {
                        var newMachine = newMachineData[i];
                        var oldMachine = _lastMachineData[i];
                        
                        if (newMachine.MachineName != oldMachine.MachineName ||
                            newMachine.Status != oldMachine.Status ||
                            newMachine.PlanCount != oldMachine.PlanCount ||
                            newMachine.FinishedCount != oldMachine.FinishedCount ||
                            newMachine.OrderNo != oldMachine.OrderNo)
                        {
                            dataChanged = true;
                            break;
                        }
                    }
                }
                
                // 如果数据没有变化，跳过更新
                if (!dataChanged)
                {
                    System.Diagnostics.Debug.WriteLine("机台数据无变化，跳过UI更新");
                    return;
                }
                
                System.Diagnostics.Debug.WriteLine($"机台数据发生变化，更新UI - 共{newMachineData.Count}台机器");
                
                // 使用UI线程更新数据
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    MachineList = newMachineData;
                });
                
                // 缓存本次数据，用于下次比较（深拷贝）
                _lastMachineData = newMachineData.Select(m => new MachineModel
                {
                    MachineName = m.MachineName,
                    Status = m.Status,
                    PlanCount = m.PlanCount,
                    FinishedCount = m.FinishedCount,
                    OrderNo = m.OrderNo
                }).ToList();
            }
            catch (Exception ex)
            {
                // 记录错误
                System.Diagnostics.Debug.WriteLine($"读取机台数据时发生错误: {ex.Message}");
            }
        }
        #endregion
        /// <summary>
        /// 停止定时器（在窗口关闭时调用）
        /// </summary>
        public void StopTimer()
        {

          ReadModbusDataTimer.Stop();
            ReadModbusDataTimer.Dispose();
        }
    }
}
