   M S B A M L   `   `   `  ����S  OCalligraphyAssistantMain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:CalligraphyAssistantMain.Controls!CalligraphyAssistantMain.Controls  5      :    4CalligraphyAssistantMain.Controls.StudentListControl   5      ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Alocal/clr-namespace:CalligraphyAssistantMain.Controls     5      .��5	      � 
������ .+�5
   
   �� -   5          ��Name$  borderBd��5
      $
��1200��6#   $��	0,60,0,60q�60   
�������$��3q�6V   
���   � J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         Opacity$	 0.3Q�6#      ShadowDepth$ 8Q�61   5      � 5   
   -   5         �Name$ borderGd��5      $
��1200��6!   $��	0,60,0,60q�6.    � RowDefinitions 5      �� 5      $��40 �6    �� 5      $��* �6    �� 5      $	��100 �6    .+�5      � $��0��6   5      � 5      -   5        $��dragGd��5       $��1��6   .+�5      � -   5        $ backGd��5      
������ $��Center=�6@   58      �� 59      -   5         ��Name$ dragRect��59       J� Opacity$	 0.7Q�6.   $
��Left=�6<   $	��Top=�6W   $y�	Collapsed=�6o   5:      � 5;      $��2��6   .+�5<      � $��62��6   .+�5=      �� -   5         ��Name$ 
progressLb��5=       $��0,0,0,5q�64   $��Center=�6E   $��Center=�6b   ���#CC2B2B$��18(�6�   5>      � 5?      -   5        $ leftGd��5?      .+�5@      � -   5        $ imageBtn1Gd��5@      $	��140��60   $��62��6<   $��	40,0,0,25q�6H   $��Bottom=�6[   $
��Left=�6v   .+�5A      �� -	   5        	 ��Name$	 	imageBtn1��5A   .   
 � Cursor$

 Hand��6    $C��=/CalligraphyAssistantMain;component/Images/DefaultButton2.png��6A   6�   �� 5B      -
   5        $

 Hand��5B   $   $��一键拍照��62   $��Center=�6>   $��Center=�6[   $��18(�6v   
�������6�   5C      � 5D      -   5        $ imageBtn2Gd��5D      $y�	Collapsed=�60   $	��140��6G   $��62��6S   $��	40,0,0,25q�6_   $��Bottom=�6r   $
��Left=�6�   .+�5E      �� -   5        $	 	imageBtn2��5E   .   $

 Hand��6    $B��</CalligraphyAssistantMain;component/Images/DefaultButton.png��6A   6�   �� 5F      -   5        $

 Hand��5F   $   $��拍照��62   $��Center=�6<   $��Center=�6Y   $��18(�6t   
�������6�   5G      � 5H      -   5        $ imageBtn3Gd��5H      $y�	Collapsed=�60   $	��140��6G   $��62��6S   $��
200,0,0,25q�6_   $��Bottom=�6s   $
��Left=�6�   .+�5I      �� -   5        $	 	imageBtn3��5I   .   $

 Hand��6    $B��</CalligraphyAssistantMain;component/Images/DefaultButton.png��6A   6�   �� 5J      -   5        $

 Hand��5J   $   $��取消��62   $��Center=�6<   $��Center=�6Y   $��18(�6t   
�������6�   5K      5L      �� 5M      -   5        $ tipLb1��5M      $y�	Collapsed=�6,   ���#CC2B2B$��微软雅黑)�6X   $��18(�6j   $��
0,0,380,10q�6x   $��Right=�6�   $��Center=�6�   $0��*提示：拖动需要调整座位的学生��6�   � 5N      -   5        $ imageBtn4Gd��5N      $	��140��6,   $��62��68   $��
0,0,200,25q�6D   $��Bottom=�6X   $��Right=�6s   .+�5O      �� -   5        $	 	imageBtn4��5O   *   $

 Hand��6   $B��</CalligraphyAssistantMain;component/Images/DefaultButton.png��6=   6�   �� 5P      -   5        $ textLb1��5P       $

 Hand��61   $��调整座位��6?   $��Center=�6K   $��Center=�6h   $��18(�6�   
�������6�   5Q      � 5R      -   5        $ cancelGd��5R      $	��140��6)   $��62��65   $��	0,0,38,25q�6A   $��Bottom=�6T   $��Right=�6o   .+�5S      �� -   5        $	 	cancelBtn��5S   *   $

 Hand��6   $B��</CalligraphyAssistantMain;component/Images/DefaultButton.png��6=   6�   �� 5T      -   5        $ textLb2��5T       $

 Hand��61   $��返回��6?   $��Center=�6I   $��Center=�6f   $��18(�6�   
�������6�   5U      5V      � 5W      $��3��6   5X      �� 5Y      $��5q�6   $��40��6   $��40��6*   $
��Left=�66   $	��Top=�6Q   $��0��6i   $��0��6y   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   �� 5Z      $��5q�6   $��40��6   $��40��6*   $��Right=�67   $	��Top=�6S   $��2��6k   $��0��6{   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�    J� RenderTransformOrigin$ 0.5,0.5&�6�   z�5[      V� 5\      .~�5]      ��  �� ScaleY$ 1Q�6)    �� ScaleX$ -1Q�64   �� 5^       �� AngleY$ 0Q�6(    �� AngleX$ 0Q�63   �� 5_       �� Angle$ 0Q�6*   T� 5`      5a      5c      �� 5d      $��5q�6   $��40��6   $��40��6*   $��Right=�67   $��Bottom=�6S   $��2��6n   $��2��6~   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   $ 0.5,0.5&�6�   z�5e      V� 5f      .~�5g      �� $ -1Q�6)   $ -1Q�65   �� 5h      $ 0Q�6(   $ 0Q�63   �� 5i      $ 0Q�6*   T� 5j      5k      5m      �� 5n      $��5q�6   $��40��6   $��40��6*   $
��Left=�67   $��Bottom=�6R   $��0��6m   $��2��6}   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   $ 0.5,0.5&�6�   z�5o      V� 5p      .~�5q      �� $ -1Q�6)   $ -1Q�65   �� 5r      $ 0Q�6(   $ 0Q�63   �� 5s      $ 90Q�6*   T� 5t      5u      5w      5x      5y      5z      