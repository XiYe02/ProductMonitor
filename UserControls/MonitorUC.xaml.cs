using LiveCharts;
using LiveCharts.Wpf;
using ProductMonitor.Models;
using ProductMonitor.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ProductMonitor.UserControls
{
    /// <summary>
    /// MonitorUC.xaml 的交互逻辑
    /// </summary>
    public partial class MonitorUC : UserControl
    {
        private ModbusService _modbusService;
        private DispatcherTimer _timer;
        
        // 饼图数据集合（可选，当前不使用）
        public ObservableCollection<PieChartModel> PieChartData { get; set; }
        
        // 绑定到PieChart的Series集合
        public SeriesCollection PieSeriesCollection { get; set; }
        
        // 缓存上次数据，用于比较是否变化
        private double[] _lastPieValues;
        private string[] _lastPieTitles;
        
        public MonitorUC()
        {
            InitializeComponent();
            
            // 初始化ModbusService
            _modbusService = new ModbusService();
            
            // 初始化饼图数据（可选）
            PieChartData = new ObservableCollection<PieChartModel>();
            
            // 初始化PieChart的Series集合
            PieSeriesCollection = new SeriesCollection();
            
            // 加载数据
            this.Loaded += MonitorUC_Loaded;
        }

        private async void MonitorUC_Loaded(object sender, RoutedEventArgs e)
        {
            // 初始化定时器，每5秒更新一次数据
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            _timer.Tick += async (s, args) => await UpdatePieChartData();
            
            // 立即加载一次数据
            await UpdatePieChartData();
            
            // 启动定时器
            _timer.Start();

            // 保持其他绑定正常工作：使用父窗口的DataContext
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                this.DataContext = parentWindow.DataContext;
            }
        }
        
        /// <summary>
        /// 更新饼图数据
        /// </summary>
        private async Task UpdatePieChartData()
        {
            try
            {
                // 从PLC读取饼图数据
                var data = await _modbusService.ReadPieChartDataAsync();
                
                // 生成当前数据的快照
                var newValues = data.Select(d => d.Value).ToArray();
                var newTitles = data.Select(d => d.Title).ToArray();
                
                // 与上次数据比较，如果完全相同则不更新
                bool sameAsLast = _lastPieValues != null && _lastPieTitles != null
                                   && _lastPieValues.Length == newValues.Length
                                   && _lastPieTitles.Length == newTitles.Length;
                if (sameAsLast)
                {
                    for (int i = 0; i < newValues.Length; i++)
                    {
                        if (_lastPieValues[i] != newValues[i] || _lastPieTitles[i] != newTitles[i])
                        {
                            sameAsLast = false;
                            break;
                        }
                    }
                }
                
                if (sameAsLast)
                {
                    return; // 数据无变化，直接返回，不触发UI更新
                }
                
                // 重建饼图的Series集合
                PieSeriesCollection.Clear();
                for (int i = 0; i < data.Count; i++)
                {
                    PieSeriesCollection.Add(new PieSeries
                    {
                        Title = newTitles[i],
                        Values = new ChartValues<double> { newValues[i] },
                        StrokeThickness = 0,
                        DataLabels = true,
                        LabelPosition = PieLabelPosition.OutsideSlice
                    });
                }
                
                // 更新缓存
                _lastPieValues = newValues;
                _lastPieTitles = newTitles;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新饼图数据失败: {ex.Message}");
            }
        }
    }
}
