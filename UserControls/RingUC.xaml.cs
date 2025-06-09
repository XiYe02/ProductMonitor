using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// RingUC.xaml 的交互逻辑
    /// </summary>
    public partial class RingUC : UserControl
    {
        public RingUC()
        {
            InitializeComponent();
            SizeChanged += OnSizeChanged;//界面大小改变 重新画
        }


        /// <summary>
        /// 界面大  小改变的时候重新绘画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Drug();
        }

        /// <summary>
        /// 百分比 比如70
        /// </summary>
        public double PercentValue
        {
            get { return (double)GetValue(PercentValueProperty); }
            set { SetValue(PercentValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PercentValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PercentValueProperty =
            DependencyProperty.Register("PercentValue", typeof(double), typeof(RingUC));

        /// <summary>
        /// 画圆环方法
        /// </summary>
        private void Drug()
        {
            // 设置宽度为存放控件的宽度
            LayOutGrid.Width = Math.Min(RenderSize.Width, RenderSize.Height);
            double raduis = LayOutGrid.Width / 2;

            // 计算根据百分比值确定的圆形路径上的点的坐标
            double x = raduis + (raduis - 3) * Math.Cos((PercentValue % 100 * 3.6 - 90) * Math.PI / 180);
            double y = raduis + (raduis - 3) * Math.Sin((PercentValue % 100 * 3.6 - 90) * Math.PI / 180);

            // 判断百分比值是否大于等于50，用于确定SVG路径数据中的大弧标志
            int Is50 = PercentValue < 50 ? 0 : 1;

            //M:移动  A:画弧
            string pathStr = $"M{raduis + 0.01} 3A{raduis - 3} {raduis - 3} 0 {Is50} 1 {x} {y}";//移动路径

            // 几何图形对象
            var converter = TypeDescriptor.GetConverter(typeof(Geometry));
            path.Data = (Geometry)converter.ConvertFrom(pathStr);//将路径数据给Path

        }
    }
}
