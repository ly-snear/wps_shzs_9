   M S B A M L   `   `   `  ����S  OCalligraphyAssistantMain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:CalligraphyAssistantMain.Controls!CalligraphyAssistantMain.Controls  5      =    7CalligraphyAssistantMain.Controls.SelectStudentsControl   5   	   ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Alocal/clr-namespace:CalligraphyAssistantMain.Controls     5        � WindowState$  	Maximized=�5   9    � AllowsTransparency .  � WindowStartupLocation$ CenterScreen=�6m    � ShowInTaskbar .   � 
ResizeMode$ NoResize=�6�    � WindowStyle$
 None=�6�    � 	Resources 5	       �� 
TargetType �� Property	 �� Value   CheckBoxStyle
 �� 
TargetType ��Name �� CornerRadius J� SnapsToDevicePixels ��Name �� Triggers ]� 
Conditions �� Property �� Value �� 
TargetName P� Property P� Value3  -System.Windows.Media.Effects.DropShadowEffect   ShadowDepth   	Direction �� MergedDictionaries �� Source�� 5
   
    58      �� 59      $P Jpack://application:,,,/CalligraphyAssistantMain;component/Style/Style.xaml?�6%   %>  '��       &	  �     �� 5      " ��6   .�5      ��  � ��$	 黑体)�6/   �� 5       � ��$	 Center=�68   �� 5       � ��$	 Center=�66   5      �� 5      " ��6   .�5      ��  � ��
	 ���� 6/   �� 5       � ��	 �#5555556/   �� 5       � ��$	 1q�64   �� 5       � ��$	 0,0,14,0q�6+   �� 5       � ��$	 34��6+   �� 5       � ��$		 100��6*   �� 5       � ��	 5      �� 5      "
 ��6*   .R�5      �� $ checkBoxBorder��6%   #��z��6=   #��z��6g   $ 2��6�   #��z��5   !    . ._�5   "   � .+�5   &   �� $ Border��6-   $��0q�6=   $ 2��6Q   �� 5   &   $ contentPresenter��67   |�.  $��Center=�6c   #��z��6�   ��. # z 5   J   $��Center=�6�   5    #   5!       5"      ]� 5#   "    5$   &   �� 5%   *    �@J�IsMouseOver . 6K   �� 5&   *    �@d�	IsChecked$ FalseY�6I   ./�5(   &   �� $ checkBoxBorder��6C    � ��	 �#80006_   5)   #   P� 5*   "    �@d�	IsChecked$
 trueY�6?   . �5+   &   �� $ Border��6?    �@J�Effect	 5,   *     5-   .   $ 2Q�6?   $	 280Q�6O   5/   '   �� 50   &   $ checkBoxBorder��6D    � ��	 �#1890FF6`   �� 51   &   $ Border��6B    � {�	 .  6V   52   #   54      56      57      5;      .��5=      � ��5>   
   �� 5?       �� ImageSource$9 3/CalligraphyAssistantMain;component/Images/Back.png��6   .+�5A   
   �� ���#FFFFFF$ 4��6&   $
��1063��67   $	��748��6D   $��3q�6Q   ���#191410 J� Effect 5B        5C         Opacity$	 0.3Q�6#   $ 8Q�61   ._�5E      � $��20q�6    � RowDefinitions 5F      �� 5G      $��40 �6$   �� 5H      .+�5J      �� $��选择学生��6   $
��Left=�6(   $��16(�6C   ���#000000$
��50 0q�6f   � 5K      -   5         �Name$ closeBtn��5K      $��25��6)   $��25��65   $��Right=�6@   ��5L      �� 5M      $? 9/CalligraphyAssistantMain;component/Images\vote_close.png��6%    j� Stretch$ Uniform=�6m   5O      � 5P      $��1��6    5Q      �� 5R      $��40 �6(   �� 5S      �� 5T      $	��100 �6(   .+�5V      ��   �� Orientation$  
Horizontal=�6!   $��Center=�6:   $
��50 0q�6U   .+�5W      	� -   5        ! 	�Name$! selectAllBtn��5W   &   $��全选��6<     RadioButtonStyle#��[ 6h   " d� 	IsChecked$
" TrueY�6�   ���#1890FF$��0 0 20 0q�6�   $��14(�6�   	� 5X      -   5        $! 	invertBtn��5X   &   $��反选��69   #��[ 6b   $
��20 0q�6�   $��14(�6�   	� 5Y      -   5        $! 
uncheckBtn��5Y   &   $��不选��6:   #��[ 6d   $
��20 0q�6�   $��14(�6�   5Z      �� 5[      $��1��6#   $
��50 0q�6X   ��60   �� *CameraItemInfos+��5\      �� 5]      .R�5^   "   �� $  Vertical=�6-   $
��Left=�6D   5_      ��5a      �� 5b      .R�5c   "   � # � Orientation$# 
Horizontal=�6,   $��Center=�6E   $	��0 0q�6`   .+�5d   &   � $��34��6,   $	��Top=�69   $
��0 10q�6Q   .+�5e   *   �� $ ��Name$$ groupCheckBox��63   $��Center=�6�   ���#000000$��14(�6�   ��6J   �� *Name+" 6c   �� *
IsSelected+
% � Tag% 6�   �� 6�   5f   '   �� 5g   &   $��1��63   $
��24 0q�6d   ��6@   �� *StudentList+��5h   *   �� 5i   .   .R�5j   2   � $# 
Horizontal=�6<   5k   /   ��5m   *   �� 5n   .   .R�5o   2   �� #���   6;   $��
0 10 15 10q�6�   " 6c   �� *
IsSelected+��6�   �� *Name+6�   5p   /   5r   '   5s   #   5t      5v      �� 5w      $��Right=�6!   $��Center=�6=   $��2��6X   $��100 0q�6e   $  
Horizontal=�6t   .+�5x      �� $��Center=�6%   $  
Horizontal=�6B   $��Center=�6[   .+�5y      �� $��
已选择 ��6(   $��16(�64   �� 5z      -   5        & ��Name$& selectCount��5z   (   $��0��6=   $��16(�6F   ���#34C58B6T   �� 5{      $
�� 人��6(   $��16(�62   5|      �� 5}      -   5        $ 
btn_cancel��5}   !   $ 8��65   $��1q�6F   $	��128��6Z   $��48��6f   ���#CCCCCC���#ffffff$
��24 0q�6�   ._�5~      �� $��取消��6(   $��16(�62   ���#3333336@   5      �� 5�      -   5        $ btn_ok��5�   !   $ 8��61   $��0q�6B   $	��128��6V   $��48��6b   ���#CCCCCC���#34C58B._�5�      �� $��确认��6(   $��16(�62   ���#FFFFFF6@   5�      5�      5�      5�      5�      5�      5�      