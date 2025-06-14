﻿using ProductMonitor.OpCommand;
using ProductMonitor.UserControls;
using ProductMonitor.ViewModels;
using ProductMonitor.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProductMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 视图模型
        /// </summary>
        MainWindowVM mainWindowVM = new MainWindowVM();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = mainWindowVM;
        }

        /// <summary>
        /// 显示车间详情页
        /// </summary>
        private void ShowDetailUC()
        {
            WorkShopDetailUC workShopDetailUC = new WorkShopDetailUC();

            mainWindowVM.MonitorUC = workShopDetailUC;

            //动画效果(由下而上)
            //位移 移动时间                                                      初始位置                   结果位置                      经过的4秒时间 
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation(new Thickness(0, 50, 0, -50), new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 400));

            //透明度
            DoubleAnimation doubleAnimation = new DoubleAnimation(0, 1, new TimeSpan(0, 0, 0, 0, 400));

            // 设置厚度动画的目标为workShopDetailUC控件
            Storyboard.SetTarget(thicknessAnimation, workShopDetailUC);
            // 设置双精度动画的目标为workShopDetailUC控件
            Storyboard.SetTarget(doubleAnimation, workShopDetailUC);


            // 设置厚度动画的目标属性为 Margin
            Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath("Margin"));
            // 设置双精度动画的目标属性为 Opacity
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Opacity"));


            // 创建一个Storyboard对象，用于管理和播放动画序列
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(thicknessAnimation);
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
            // 启动Storyboard，开始播放其中的动画

        }

        /// <summary>
        /// 返回到监控
        /// </summary>
        private void GoBackMonitor()
        {
            MonitorUC monitorUC = new MonitorUC();

            mainWindowVM.MonitorUC = monitorUC;
        }

        /// <summary>
        /// 展示详情命令
        /// </summary>
        public Command ShowDetailCmm
        {
            get
            {
                return new Command(ShowDetailUC);
            }
        }

        /// <summary>
        /// 返回监控界面命令
        /// </summary>
        public Command GoBackCmm
        {
            get
            {
                return new Command(GoBackMonitor);
            }
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMin(object sender, RoutedEventArgs e)
        {
            //最小化
            this.WindowState = WindowState.Minimized;

            //this.WindowState = WindowState.Maximized;//最大化
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose(object sender, RoutedEventArgs e)
        {
            //this.Close();
            Environment.Exit(0);
        }

        #region 弹出配置窗口
        /// <summary>
        /// 弹出配置窗口
        /// </summary>
        private void ShowSettingWin()
        {
            //父子关系                               设置当前的主窗口是拥有者
            SettingsWin settingsWin = new SettingsWin() { Owner = this };
            settingsWin.ShowDialog();
        }

        /// <summary>
        /// 创建 弹出配置窗口 命令
        /// </summary>
        public Command ShowSettingaCmm
        {
            get
            {
                return new Command(ShowSettingWin);
            }
        }
        #endregion

    }
}
