   M S B A M L   `   `   `  ����S  OCalligraphyAssistantMain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:CalligraphyAssistantMainCalligraphyAssistantMain  5   	   U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 5   	   M+clr-namespace:CalligraphyAssistantMain.CodeCalligraphyAssistantMain.Code  5   	   /    )CalligraphyAssistantMain.StatisticControl   5   	   ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8local&clr-namespace:CalligraphyAssistantMain     5   	   Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     5   	   <code+clr-namespace:CalligraphyAssistantMain.Code     5   	     � WindowState$  	Maximized=�5
   4    � AllowsTransparency .  � WindowStartupLocation$ CenterScreen=�6h    � ShowInTaskbar .   � 
ResizeMode$ NoResize=�6�    � WindowStyle$
 None=�6�    � 	Resources 5       �� MergedDictionaries �� Source	 �� 
TargetType
 �� Property �� Value   RadioButtonStyle �� 
TargetType ��Name �� CornerRadius J� SnapsToDevicePixels ��Name �� Triggers ]� 
Conditions �� Property �� Value �� 
TargetName P� Property P� Value3  -System.Windows.Media.Effects.DropShadowEffect   ShadowDepth   	Direction3   -CalligraphyAssistantMain.Code.NumberConverter  numberConverter�� 5   
    5      �� 5      $P Jpack://application:,,,/CalligraphyAssistantMain;component/Style/Style.xaml?�6%   %c  '��       &	  �     &	 2    �� 5      "	 ��6   .�5      �� 
 � ��$ 黑体)�6/   �� 5      
 � ��$ Center=�68   �� 5      
 � ��$ Center=�66   5      �� 5      "	 	�6   .�5      �� 
 � ��
 ���� 6/   �� 5      
 � �� �#A2A2A26/   �� 5      
 � ��$ 1q�64   �� 5      
 � ��$ 0,0,15,0q�6+   �� 5      
 � ��$ 40��6+   �� 5      
 � ��$ 80��6*   �� 5      
 � �� 5      �� 5      " 	�6*   .R�5      �� $ radioButtonBorder��6%   #��z��6@   #��z��6j   $ 2��6�   #��z��5        . ._�5!   "   � .+�5"   &   �� $ Border��6-   $��0q�6=   $ 2��6Q   �� 5#   &   $ contentPresenter��67   |�.  $��Center=�6c   #��z��6�   ��. # z 5$   C   $��Center=�6   5%   #   5&       5'      ]� 5(   "    5)   &   �� 5*   *    �@J�IsMouseOver . 6K   �� 5+   *    �@d�	IsChecked$ FalseY�6I   ./�5-   &   �� $ radioButtonBorder��6C   
 � �� �#80006b   5.   #   P� 5/   "    �@d�	IsChecked$
 trueY�6?   . �50   &   �� $ Border��6?   
 �@J�Effect 51   *     52   .   $ 2Q�6?   $	 280Q�6O   54   '   �� 55   &   $ radioButtonBorder��6D   
 � �� �#1890FF6c   �� 56   &   $ Border��6B   
 � {� .  6V   57   #   59      5;      5<        5=      6#   5>      .��5@      � ��5A   
   �� 5B       �� ImageSource$9 3/CalligraphyAssistantMain;component/Images/Back.png��6   .+�5D   
   �� ���#FFFFFF$ 24��6&   $	��630��68   $	��530��6D   ._�5E      � $��12q�6    � RowDefinitions 5F      �� 5G      $��40 �6$   �� 5H      .+�5J      �� -   5         ��Name$ title��5J      $��答题统计��6+   $
��Left=�67   $��16(�6R   ���#333333$
��20 0q�6u   � 5K      -   5         �Name$ closeBtn��5K      $��22��6)   $��22��65   $��Right=�6@   ��5L      �� 5M      $? 9/CalligraphyAssistantMain;component/Images\vote_close.png��6%    j� Stretch$ Uniform=�6m   5O      � 5P      $��1��6    5Q      �� 5R      �� 5S      $��0��6(   $��60 �66   .+�5U      � $��
20 20 20 0q�6    5V      �� 5W      $��60 �6,   �� 5X      .+�5Z      � .+�5[      �� -   5        $ content��5[   (   $	��Top=�69   $
��Left=�6R   $
��Wrap=�6m   ���#555555$��CharacterEllipsis=�6�   $��14(�6�   5]      5^      � 5_      $��1��6    � ColumnDefinitions 5`      �� 5a   "   65   �� 5b   "   $	��140 �63   6A   .+�5d      #  LiveCharts.Wpf.CartesianChart  -   5           Name$  	cartesian��5d   1   $y�	Collapsed=�6D   &   LiveCharts.Wpf.Charts.Base.Chart!   LegendLocation$! Bottom=�6[   "   Zoom$
" None=�6�   #   Series# 6s   �� *Achievement+$ �� UpdateSourceTrigger$$ PropertyChanged=�%   AxisX% 5f   &     LiveCharts.Wpf.Axis  &   Position$& 
LeftBottom=�6I   '   Labels' 6/   �� *Labels+(   LabelFormatter( 6_   �� *	Formatter+)   	Separator) 5g   *     LiveCharts.Wpf.Separator  5h   .   *   Step$* 1Q�6<   5j   '   5u      � 5v      $��1��6$   $��15 0 0 0q�64    5w   "   �� 5x   &   $��23 �64   �� 5y   &   .+�5{   "   �� -   5        $	 txt��5{   ,   $��14(�69   ���#333333$
��Left=�6\   $	��Top=�6w   6�   �� 5|   "   $��1��6/   + %� IsVirtualizing+ . , %� VirtualizationMode$, 	Recycling=�6k   ��6�   �� *
ListData+- �� AlternationCount- 6�   �� . �� Path$. ListData.Count���5}   &   �� 5~   *   " ��6:   .R�5   .   �� $��Visible=�6<   $��Disabled=�6b   .��5�   2   �� 5�   /   5�   +   ��5�   &   �� 5�   *   .R�5�   .   �� $��35��66   $��0 0 0 0q�6B   ���#999999._�5�   2   �  5�   6   �� 5�   :   $��20 �6K   �� 5�   :   $��2* �6K   .+�5�   6   �� $	 bor��6=   $��Center=�6J   $��Center=�6g   $��20��6�   $��20��6�   ���#F0F2F5$ 12��6�   ._�5�   :   �� $	 num��6D   $��Center=�6Q   $��Center=�6n   $��14(�6�   ���#314659��5�   >   �� 5�   B   $%. (ItemsControl.AlternationIndex)�6�   / �� 	Converter#/ [ 6�   0 �� RelativeSource0 6J   �� *FindAncestor+1 �� AncestorType"1 ��6�   5�   ;   5�   7   �� 5�   6   $��1��6@   $
��Left=�6P   $��18 0 0 0q�6k   $��Center=�6}   $��14(�6�   ���#000000��6�   �� 6�   5�   3   5�   /   2 �� Triggers2 5�   .   �� 5�   2   3 �� Value$3 1��6h   4 �� Binding4 6>   �� *Text+5 �� ElementName$	5 num��.a�5�   6   �� $	 bor��6=   
 � �� �#3146596d   �� 5�   6   $	 num��6=   
 � �� �#FFFFFF6d   5�   3   �� 5�   2   $3 2��6h   4 6>   �� *Text+$	5 num��.a�5�   6   �� $	 bor��6=   
 � �� �#3146596d   �� 5�   6   $	 num��6=   
 � �� �#FFFFFF6d   5�   3   �� 5�   2   $3 3��6h   4 6>   �� *Text+$	5 num��.a�5�   6   �� $	 bor��6=   
 � �� �#3146596d   �� 5�   6   $	 num��6=   
 � �� �#FFFFFF6d   5�   3   5�   +   ��5�   &   �� 5�   *   .R�5�   .   $� 5�   +   5�   #   5�      5�      5�      �� 5�      6 �� Orientation$6 
Horizontal=�6!   $��1��6:   $��Center=�6G    5�      7 �� Triggers��%  '��       �� 5�      "	 ��6%   .�5�   "   �� 
 � ��$
 20 0q�6;   7 5�   "   P� 5�   &    � {� . . �5�   *   �� 
 � �� �#34C58B6G   5�   '   P� 5�   &    � {� .  . �5�   *   �� 
 � �� �#DDDDDD6G   5�   '   5�      5�      .+�5�      �� -   5        $ startBtn��5�   !   $y�	Collapsed=�63   $ 8��6J   $��0q�6[   $	��128��6o   $��48��6{   {�. ._�5�      �� $��公布答案��6(   $��16(�64   ���#FFFFFF6B   5�      5�      5�      5�      5�      5�      5�      