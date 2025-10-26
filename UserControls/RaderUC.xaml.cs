using ProductMonitor.Models;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProductMonitor.UserControls
{
    /// <summary>
    /// RaderUC.xaml 的交互逻辑
    /// </summary>
    public partial class RaderUC : UserControl
    {
        public RaderUC()
        {
            InitializeComponent();

            SizeChanged += OnSizeChanged;//Alt+Enter
        }

        /// <summary>
        /// 控件加载完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Drag(); // 控件加载完成后绘制雷达图
        }

        /// <summary>
        /// 窗体大小发生变化 重新画图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Drag();
        }

        /// <summary>
        /// 数据源。支持数据绑定 依赖属性
        /// </summary>
        public List<RaderModel> ItemSource
        {
            get { return (List<RaderModel>)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemSourceProperty =
            DependencyProperty.Register("ItemSource", typeof(List<RaderModel>), typeof(RaderUC), 
                new PropertyMetadata(null, OnItemSourceChanged));

        /// <summary>
        /// 当ItemSource属性变更时的回调方法
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnItemSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RaderUC raderUC && raderUC.IsLoaded)
            {
                // 使用Dispatcher确保在UI线程上执行
                raderUC.Dispatcher.BeginInvoke(new Action(() =>
                {
                    raderUC.Drag(); // 重新绘制雷达图
                }));
            }
        }

        /// <summary>
        /// 画图方法
        /// </summary>
        public void Drag()
        {
            //判断是否有数据
            if (ItemSource == null || ItemSource.Count == 0)
            {
                return;
            }

            // 确保控件已经完全加载并且有有效的尺寸
            if (RenderSize.Width <= 0 || RenderSize.Height <= 0)
            {
                return;
            }

            //清楚之前画的
            mainCanvas.Children.Clear();
            P1.Points.Clear();
            P2.Points.Clear();
            P3.Points.Clear();
            P4.Points.Clear();
            P5.Points.Clear();

            //调整大小(正方形)
            double size = Math.Min(RenderSize.Width, RenderSize.Height);
            if (size <= 0) size = 200; // 设置最小尺寸
            
            LayGrid.Height = size;
            LayGrid.Width = size;
            //半径
            double raduis = size / 2;

            //步子跨度
            double step = 360.0 / ItemSource.Count;

            for (int i = 0; i < ItemSource.Count; i++)
            {
                double x = (raduis - 20) * Math.Cos((step * i - 90) * Math.PI / 180);//x偏移量
                double y = (raduis - 20) * Math.Sin((step * i - 90) * Math.PI / 180);//y偏移量

                //X Y坐标
                P1.Points.Add(new Point(raduis + x, raduis + y));

                P2.Points.Add(new Point(raduis + x * 0.75, raduis + y * 0.75));

                P3.Points.Add(new Point(raduis + x * 0.5, raduis + y * 0.5));

                P4.Points.Add(new Point(raduis + x * 0.25, raduis + y * 0.25));

                //数据多边形
                P5.Points.Add(new Point(raduis + x * ItemSource[i].Value * 0.01, raduis + y * ItemSource[i].Value * 0.01));

                //文字处理
                TextBlock txt = new TextBlock();
                txt.Width = 60;
                txt.FontSize = 10;
                txt.TextAlignment = TextAlignment.Center;
                txt.Text = ItemSource[i].ItemName;
                txt.Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
                txt.SetValue(Canvas.LeftProperty, raduis + (raduis - 10) * Math.Cos((step * i - 90) * Math.PI / 180) - 30);//设置左边间距
                txt.SetValue(Canvas.TopProperty, raduis + (raduis - 10) * Math.Sin((step * i - 90) * Math.PI / 180) - 7);//设置上边间距

                mainCanvas.Children.Add(txt);
            }
        }
    }
}
