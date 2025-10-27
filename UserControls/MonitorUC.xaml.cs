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
        public MonitorUC()
        {
            InitializeComponent();
            
            // 加载数据
            this.Loaded += MonitorUC_Loaded;
        }

        private void MonitorUC_Loaded(object sender, RoutedEventArgs e)
        {
            // 使用父窗口的DataContext
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                this.DataContext = parentWindow.DataContext;
            }
        }
    }
}
