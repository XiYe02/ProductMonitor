   M S B A M L   `   `   `  ����I  EProductMonitor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:ProductMonitor.ViewsProductMonitor.Views  5   	   &     ProductMonitor.Views.SettingsWin   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:ProductMonitor.Views     5   	     � WindowStartupLocation$  CenterOwner=�5   	   
������ $��Microsoft YaHei)�6F   $��
ExtraLight"�6c    � 
ResizeMode$ NoResize=�6|    � Title$ 系统配置��5	   	   $	��450��6   $	��800��6#   '  !System.Windows.Shell.WindowChrome   WindowChrome 5
        5   
      GlassFrameThickness$ -1q�6   62    � 	Resources 5       �� 
TargetType �� Property �� Value	 �� 
TargetType
 �� Triggers P� Property P� Value��%�  '	�       �� 5   
   " 	�6   .�5      ��  � ��
 ���� 6A   �� 5       � �� 5      �� 5      "	 	�6&   .R�5      �� #��z��6!   $��30��6K   ._�5      �� $��Center=�6/   $
��Left=�6J   $
��10,0q�6f   6v   5      5      5      
 5      P� 5       �@d�	IsChecked$
 TrueY�6/   . �5      ��  � �� �#18aabd6E   �� 5       � ��
 �����6C   5       5"      5#      .��5$      �  � RowDefinitions 5%   
   �� 5&      $��30 �6   6*   �� 5'      6    � ColumnDefinitions 5)   
   �� 5*      $	��130 �6    6.   �� 5+      6!   .+�5.   
   �� $��Center=�6   $
��10,0q�6/   �� 5/      $	���6   $!��../Resource/Fonts/#iconfont)�6"   $��14(�6K   6[    �� 50      $�系统配置信息��6   6"   51      �� 54   
   $��0,0,1,0q�6   $��1��6+   ���#ddd6M   �� 55   
   $��1��6   $
��10,0q�6"   .+�56      	� -   5        $��设备清单��56      
 � Tag$ data1��6)    d� 	IsChecked$
 TrueY�65   6b   	� 57      -   5        $��环境监测��57      $ data2��6)   6Q   	� 58      -   5        $��设备监测��58      $ data3��6)   6Q   	� 59      -   5        $��	配置一��59      $ data4��6(   6P   	� 5:      -   5        $��	配置二��5:      $ data5��6(   6P   5;      �� 5>   
   $��1��6   $��1��6$   $��Hidden=�64   .��5@       � -   5          �Name$ frame��5@   /     � Source$ SettingsPage.xaml?�6     � NavigationUIVisibility$ Hidden=�6>   6`   5A      5B      5C      