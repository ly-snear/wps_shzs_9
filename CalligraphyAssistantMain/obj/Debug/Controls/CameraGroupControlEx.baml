   M S B A M L   `   `   `  ����S  OCalligraphyAssistantMain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:CalligraphyAssistantMain.Controls!CalligraphyAssistantMain.Controls  5      <    6CalligraphyAssistantMain.Controls.CameraGroupControlEx   5      ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Alocal/clr-namespace:CalligraphyAssistantMain.Controls     5      .��5	      � 
������ .+�5
   
   �� -   5          ��Name$  borderBd��5
      $
��1200��6#   $��0,100,0,130q�60   
�������$��3q�6X   
���   � J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         Opacity$	 0.3Q�6#      ShadowDepth$ 8Q�61   5      � 5   
   -   5         �Name$ borderGd��5      $
��1200��6!   $��0,100,0,130q�6.    � RowDefinitions 5      �� 5      $��50 �6    �� 5      $��* �6    �� 5      $��80 �6    .+�5      � $��0��6   5      � 5      -   5        $��dragGd��5       $��1��6   .+�5      �  5      �� 5      $��* �6(   �� 5      $	��150 �6(   .+�5      � $��0��6   $��
30,0,30,10q�6(   
���   �.+�5      8   2CalligraphyAssistantMain.Controls.UDPPlayerControl  -   5          Name$ 
backPlayer��5   1   6G   5       � 5!      $��1��6   .+�5"      � $	��150��6$   $
��23,0q�61   .+�5#      �� -   5        $  
previewBd1��5#   %   
 � Tag$ 1��69   $	��7,0q�6A   ���#FEA356$��4q�6d   $	��240��6x   $	��150��6�   
���   �._�5$   "     -   5        $ player1��5$   9   8   2CalligraphyAssistantMain.Controls.UDPPlayerControl   Click$ Player_Click��6J   5%      �� 5&      -   5        $  
previewBd2��5&   %   $ 2��69   $	��7,0q�6A   
���   �$	��240��6b   $	��150��6n   $��4q�6{   
���   �._�5'   "     -   5        $ player2��5'   9   $ Player_Click��6J   5(      �� 5)      -	   5        $  
previewBd3��5)   %   $ 3��69   $	��7,0q�6A   
���   �$	��240��6b   $	��150��6n   $��4q�6{   
���   �._�5*   "     -
   5        $ player3��5*   9   $ Player_Click��6J   5+      5,      5-      5.      5/      � 50      $��2��6   .+�51      � $	��140��6   $��62��6#   $��	0,0,38,25q�6/   $��Bottom=�6B   $��Right=�6]   .+�52      �� -   5        	 ��Name$	 	cancelBtn��52   *   
 � Cursor$

 Hand��6   $B��</CalligraphyAssistantMain;component/Images/DefaultButton.png��6=   6�   �� 53      -   5         ��Name$ textLb2��53       $

 Hand��61   $��返回��6?   $��Center=�6I   $��Center=�6f   $��18(�6�   
�������6�   54      � 55      $y�	Collapsed=�6   $	��140��6.   $��62��6:   $��
0,0,200,25q�6F   $��Bottom=�6Z   $��Right=�6u   .+�56      �� -   5        $	 shareBtn��56   *   $

 Hand��6   $B��</CalligraphyAssistantMain;component/Images/DefaultButton.png��6<   6�   �� 57      -   5        $ textLb1��57       $

 Hand��61   $��分享��6?   $��Center=�6I   $��Center=�6f   $��18(�6�   
�������6�   58      59      �� 5:      $��5q�6   $��40��6   $��40��6*   $
��Left=�66   $	��Top=�6Q   $��0��6i   $��0��6y   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   �� 5;      $��5q�6   $��40��6   $��40��6*   $��Right=�67   $	��Top=�6S   $��2��6k   $��0��6{   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�    J� RenderTransformOrigin$ 0.5,0.5&�6�   z�5<      V� 5=      .~�5>      ��  �� ScaleY$ 1Q�6)    �� ScaleX$ -1Q�64   �� 5?       �� AngleY$ 0Q�6(    �� AngleX$ 0Q�63   �� 5@       �� Angle$ 0Q�6*   T� 5A      5B      5D      �� 5E      $��5q�6   $��40��6   $��40��6*   $��Right=�67   $��Bottom=�6S   $��2��6n   $��2��6~   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   $ 0.5,0.5&�6�   z�5F      V� 5G      .~�5H      �� $ -1Q�6)   $ -1Q�65   �� 5I      $ 0Q�6(   $ 0Q�63   �� 5J      $ 0Q�6*   T� 5K      5L      5N      �� 5O      $��5q�6   $��40��6   $��40��6*   $
��Left=�67   $��Bottom=�6R   $��0��6m   $��2��6}   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   $ 0.5,0.5&�6�   z�5P      V� 5Q      .~�5R      �� $ -1Q�6)   $ -1Q�65   �� 5S      $ 0Q�6(   $ 0Q�63   �� 5T      $ 90Q�6*   T� 5U      5V      5X      5Y      5Z      5[      