   M S B A M L   `   `   `  ����U  QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf  5       MarketData.GUI   MarketData.GUI.FxView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Dwpf4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5      -   5          >�Name$  	FxControl��5   7    � 	Resources 5	          BooleanToVisibilityConverter��%   &	        �� 5
   
   6'   5      .��5   
   �  � ColumnDefinitions 5      �� 5      $��* �6#   5      �� 5      $
��Auto �6#    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $��* �6    .+�5      �� $��0��6   $��0��6&   $��2��66    �� Orientation$ 
Horizontal=�6J   .+�5      �� -   5         ��Name$ CcyComboBox��5      $
��Left=�6Y   $	��150��6t    �� DisplayMemberPath$
 Name��6�    �� SelectedValuePath$ Id��6�   $��Center=�6�   $��Center=�6  ��60   �� *CurrencyPairList+ �� SelectedItem 6�   �� *SelectedCurrencyPair+6  �� 5      -   5        	 ��Name$	 GraphButton��5      
 � ToolTip$
 Graph��6.   $
��Auto��6>   $��5,0,0,0q�6K   .��5      �� $��line-chart.ico��6   $��Center=�65   $��Center=�6R   $��20��6�   ��6m   ��  �� ElementName$ CcyComboBox�� �� Path$ Height�6�   5      5      � 5   
   $��1��6   $��0��6    5      �� 5       $��* �6#   �� 5!      $��* �6#   .+�5#      #   LiveCharts.Wpf.CartesianChart  -   5          Name$ FxCartesianChart��5#   !   $��Stretch=�5$   !   $
��Left=�5%   !   &    LiveCharts.Wpf.Charts.Base.Chart   LegendLocation$	 Top=�5&   !   $��0��5'   !   $��Stretch=�5(   !   $��Stretch=�5)   !   $
��Auto��5*   !   $
��Auto��5+   !      Zoom$ Xy=�6/      Series 69   �� *SeriesCollection+5,      &   System.Windows.Controls.DataGrid  5-      -   5        $��
FxDataGrid��5-      $��1��5.      $��Stretch=�5/      $��Stretch=�50      $
��Auto��51      $
��Auto��52      $
��Left=�53       �� AlternationCount$ 2��54         CanUserReorderColumns .    CanUserSortColumns .    GridLinesVisibility$ 
Horizontal=�57         AutoGenerateColumns .  ��59      �� *CurrencyPairPriceList+ � ContextMenu 5:      �� 5;      .@�5<      m� $��Menu1��6#   {�.  62   m� 5=      $��Menu2��6#   {�.  62   m� 5>      $��Menu3��6#   {�.  62   m� 5?      $��Menu4��6#   {�.  62   5@         Columns 5B      0  *System.Windows.Controls.DataGridTextColumn  5D      ,  &System.Windows.Controls.DataGridColumn   Header$ 	Ask Price��6)      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6<   �� *
AskPrice+6Y     5E      $ 	Bid Price��6)    .  6<   �� *
BidPrice+6Y     5F      $ 	Timestamp��6)    .  6<   �� *	TimeStamp+6Z   5H      5J      9� 5K   
   $��1��6   $��1��6    $��1q�60   
�����p� 9� 
IsExpanded .  $��Right=�6o    9� ExpandDirection$
 Left=�6�   .��5L      �� $
��Auto��6   .+�5M        -   5        $��ColumnChooserDataGrid��5M      "   VerticalScrollBarVisibility$
 Auto=�5N      $ 2��5O      $��0q�5P       .  . $ 
Horizontal=�5S       .  $��Center=�5V      $��Stretch=�5W      ��5U      �� $ 
FxDataGrid��$ Columns� 5X      4  .System.Windows.Controls.DataGridTemplateColumn  5Y      $ Select��65      Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6E      CellTemplate 5Z   "   5	  -System.Windows.Controls.ItemContainerTemplate�	  5[   &   .R�5\   *   ��   ��Name$  SomeCheckBox��63   $��Center=�5]   1   $��Center=�5^   1   ! d� 	IsChecked! 5_   1   �� " �� Mode$" TwoWay=�# �� UpdateSourceTrigger$# PropertyChanged=�$ Item.Visibility�$ �� 	Converter#$ [  % �� FallbackValue$
% True��5`   '   5b        5c       . $ Field��6C   $
 Auto 6n    6R   �� *Header+6n   5e      5f      5g      5h      5i      