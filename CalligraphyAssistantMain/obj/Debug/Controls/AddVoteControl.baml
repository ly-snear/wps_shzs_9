   M S B A M L   `   `   `  ����S  OCalligraphyAssistantMain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:CalligraphyAssistantMain.Controls!CalligraphyAssistantMain.Controls  5   	   6    0CalligraphyAssistantMain.Controls.AddVoteControl   5   	   ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Alocal/clr-namespace:CalligraphyAssistantMain.Controls     5   	     � WindowState$  	Maximized=�5   4    � AllowsTransparency .  � WindowStartupLocation$ CenterScreen=�6h    � ShowInTaskbar .   � 
ResizeMode$ NoResize=�6�    � WindowStyle$
 None=�6�    � 	Resources 5	       �� 
TargetType �� Property	 �� Value   RadioButtonStyle
 �� 
TargetType ��Name �� CornerRadius J� SnapsToDevicePixels ��Name �� Triggers ]� 
Conditions �� Property �� Value �� 
TargetName P� Property P� Value3  -System.Windows.Media.Effects.DropShadowEffect   ShadowDepth   	Direction�� 5
   
   %F  '��       &	  �     �� 5      " ��6   .�5      ��  � ��$	 黑体)�6/   �� 5       � ��$	 Center=�68   �� 5       � ��$	 Center=�66   5      �� 5      " 	�6   .�5      ��  � ��
	 ���� 6/   �� 5       � ��	 �#A2A2A26/   �� 5       � ��$	 1q�64   �� 5       � ��$	 0,0,15,0q�6+   �� 5       � ��$	 40��6+   �� 5       � ��$	 80��6*   �� 5       � ��	 5      �� 5      "
 	�6*   .R�5      �� $ radioButtonBorder��6%   #��z��6@   #��z��6j   $ 2��6�   #��z��5       . ._�5   "   � .+�5   &   �� $ Border��6-   $��0q�6=   $ 2��6Q   �� 5   &   $ contentPresenter��67   |�.  $��Center=�6c   #��z��6�   ��. # z 5   G   $��Center=�6�   5    #   5!       5"      ]� 5#   "    5$   &   �� 5%   *    �@J�IsMouseOver . 6K   �� 5&   *    �@d�	IsChecked$ FalseY�6I   ./�5(   &   �� $ radioButtonBorder��6C    � ��	 �#80006b   5)   #   P� 5*   "    �@d�	IsChecked$
 trueY�6?   . �5+   &   �� $ Border��6?    �@J�Effect	 5,   *     5-   .   $ 2Q�6?   $	 280Q�6O   5/   '   �� 50   &   $ radioButtonBorder��6D    � ��	 �#1890FF6c   �� 51   &   $ Border��6B    � {�	 .  6V   52   #   54      56      57      58      .��5:      � ��5;   
   �� 5<       �� ImageSource$9 3/CalligraphyAssistantMain;component/Images/Back.png��6   .+�5>   
   �� ���#FFFFFF$ 24��6&   $	��630��68   $	��322��6D   ._�5?      � $��12q�6    � RowDefinitions 5@      �� 5A      $��50 �6$   �� 5B      .+�5D      �� $��新建投票��6   $
��Left=�6(   $��16(�6C   ���#333333$
��20 0q�6f   � 5E      -   5         �Name$ closeBtn��5E      $��25��6)   $��25��65   $��Right=�6@   ��5F      �� 5G      $? 9/CalligraphyAssistantMain;component/Images\vote_close.png��6%    j� Stretch$ Uniform=�6m   5I      � 5J      $��1��6    5K      �� 5L      �� 5M      $	��100 �6(   .+�5O      �  5P      �� 5Q      �� 5R      .+�5T      � $
��45 0q�6    � ColumnDefinitions 5U      �� 5V   "   $��90 �63   �� 5W   "   .+�5Y      �� $��投票项目��6(   $
��Left=�64   $��14(�6O   ���#3333336]   �� 5Z      $��1��6)    �� Orientation$ 
Horizontal=�69   $
��Left=�6R   .+�5[   "   	� -   5        $
��2个��5[   /   #���   6<    d� 	IsChecked$
 TrueY�6g   6y   	� 5\   "   -   5        $
��3个��5\   /   #���   6<   6h   	� 5]   "   -   5        $
��4个��5]   /   #���   6<   6h   	� 5^   "   -   5        $
��5个��5^   /   #���   6<   6g   5_      5`      5b      �� 5f      $ 
Horizontal=�6!   $��1��6:   $��Center=�6G    5g      ��%T   '��       �� 5h      " ��6%   .�5i   "   ��  � ��$
	 20 0q�6;   5j      5k      .+�5l      �� -   5        $ 8��5l   !   $��1q�62   $	��128��6F   $��48��6R   ���#CCCCCC���#FFFFFF._�5m      �� $��取消��6(   $��16(�62   ���#3333336@   5n      �� 5p      -   5        $ btn_ok��5p   !   $ 8��61   $	��128��6C   $��48��6O   ���#34C58B._�5q      �� $��确认��6(   $��16(�62   ���#FFFFFF6@   5r      5s      5u      5w      5x      5y      5z      