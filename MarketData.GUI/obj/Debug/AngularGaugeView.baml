   M S B A M L   `   `   `  ����U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf  5       MarketData.GUI%   MarketData.GUI.AngularGaugeView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Dwpf4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5      -   5          >�Name$  AngularGaugeControl��5   7    � 	Resources 5	         BooleanToVisibility�� 5
   
   %   &	        �� 5      6+   5      .��5      � -   5         �Name$ 
MetricGrid��5       � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   9� $��1��6   $
��Auto��6#   $	��Top=�60   $	��Top=�6H   $��Center=�6g   $��Center=�6�    9� ExpandDirection$
 Left=�6�   .��5      �  5      �� 5      $
��Auto �6'   �� 5      $��* �6'   .+�5      �� $��Metric��6   $��0��6)   �� 5      -   5         ��Name$ ComboBox��5   +   $��1��6   $
��Left=�6`   $	��150��6{    �� DisplayMemberPath$
 Name��6�    �� SelectedValuePath$ Id��6�   $��Center=�6�   $��Center=�6	  ��6=   �� *
GaugeItems+ �� SelectedItem 6�   �� *SelectedGaugeItem+6	  5      5      � 5   
   -   5        $ CpuGrid��5      $��0��6    y�60   �� *CpuVisibility+	 �� 	Converter#	 [  
 J� Effect
 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         
BlurRadius$ 15Q�6#      	Direction$	 -90Q�63      RenderingBias$ Quality=�6C      Opacity$ .2Q�6[      ShadowDepth$ 1Q�6h    J� OpacityMask 5!      !� 5"       !� Visual 6   ��  �� ElementName$ Border�� � RowDefinitions 5$      �� 5%      $
��Auto �6    60   �� 5&      $
��Auto �6    60   �� 5'      $��* �6    6-   �� 5(      $	��.3* �6    6/   .+�5*      �� -   5         ��Name$ Border��5*      $��0��6%   $��4��62   
�������6C   �� 5+      $��0��6   $��3��6"   
�������6N   �� 5,      $��0��6    �� TextAlignment$ Center=�6%    �� Padding$ 10, 10, 0, 5q�6<   ���#303030$��25(�6h   	Cpu Usage5.      !   LiveCharts.Wpf.AngularGauge  5/      -   5          Name$ CpuGauge��5/      $��2��61      
LabelsStep$ 10Q�51         	TicksStep$ .5Q�52         Wedge$	 300Q�53         TicksForeground
 �����
�������$
��Bold"�56         SectionsInnerRadius$	 .75Q�57         Value 50      �� *
GaugeValue+   Sections 59      #   LiveCharts.Wpf.AngularSection     	FromValue$ 0Q�6)       ToValue$  35Q�67   !   Fill
! �2�2�6D     5:      $ 35Q�6)   $  70Q�68   
! � ���6E     5;      $ 70Q�6)   $	  100Q�68   
! �  ��6F   5=      �� 5>      $��3��6   $��Center=�6&   $��25, 0q�6A   .+�5?      �� " J� Opacity$" .4Q�6   $��13(�6)   Total CPU usage6H   �� 5@      # �� Orientation$# 
Horizontal=�6   .+�5A      �� ���#303030$��40(�65   ��6C   �� *
GaugeValue+$ �� StringFormat$$ {0:N1}���� 5B      ���#303030$��18(�65   $��Bottom=�6C   $
��8, 6q�6^   %6o   5C      5D      5E      � 5F   
   -   5        $ 
MemoryGrid��5F      $��0��6#   y�63   �� *MemoryVisibility+#	 [  
 5G        5H      $ 15Q�6#   $	 -90Q�63   $ Quality=�6C   $ .2Q�6[   $ 1Q�6h    5J      !� 5K       6   �� $ Border1�� 5M         LiveCharts.Wpf.LineSeries% �� 
TargetType   LiveCharts.Wpf.Series& �� Property' �� Value   LiveCharts.Wpf.Axis��%�  '        '  +    �� 5N      "%  6   .�5O      �� & �@ StrokeThickness$' 3Q�68   6D   �� 5P      & �@ Stroke
' �����6?   �� 5Q      & �@ Fill' �	#4EFFFFFF6A   �� 5R      & �@ PointGeometrySize$' 0Q�6:   6F   �� 5S      & �@ LineSmoothness$' 0Q�67   6C   5T      �� 5U      "%  6   .�5V      �� & �@ 
ShowLabels' .  6C   �� 5W      & � {�' .  6B   5X      5Y       5Z      �� 5[      $
��Auto �6    �� 5\      $
��Auto �6    �� 5]      $��* �6    �� 5^      $	��.3* �6    .+�5`      �� -   5        $ Border1��5`      $��0��6&   $��4��63   
�������6D   �� 5a      $��0��6   $��3��6"   
���"�"�6O   �� 5b      $��0��6   $ Center=�6%   $ 10, 10, 0, 5q�6<   ���#303030$��25(�6h   Memory Usage5d      #   LiveCharts.Wpf.CartesianChart  5e      $��2��6!   $��
0, 0, 0, 0q�6.   &    LiveCharts.Wpf.Charts.Base.Chart(   	Hoverable( .  )   Series) 6B   �� *SeriesCollection+*   DataTooltip* 6x   Z� +   AxisX+ 5h        ,   MinValue$, 2Q�6   6.   5j      �� 5k      $��3��6   $��Center=�6&   $��25, 0q�6A   .+�5l      �� $" .4Q�6   $��13(�6)   Total memory usage6K   �� 5m      $# 
Horizontal=�6   .+�5n      �� ���#303030$��40(�65   ��6C   �� *
GaugeValue+$$ {0:N1}���� 5o      ���#303030$��18(�65   $��Bottom=�6C   $
��8, 6q�6^   mb6p   5p      5q      5r      5s      5t      