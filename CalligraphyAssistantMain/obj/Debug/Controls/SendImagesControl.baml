   M S B A M L   `   `   `  ����S  OCalligraphyAssistantMain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:CalligraphyAssistantMain.Controls!CalligraphyAssistantMain.Controls  5      9    3CalligraphyAssistantMain.Controls.SendImagesControl   5      ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Alocal/clr-namespace:CalligraphyAssistantMain.Controls     5      .��5	      � 
������ .+�5
   
   �� $
��1280��6   $	��710��6   
�������$��3q�6>   
���   �  J� Effect  5      3  -System.Windows.Media.Effects.DropShadowEffect  5         Opacity$	 0.3Q�6#      ShadowDepth$ 8Q�61   5      � 5   
   $
��1280��6   $	��710��6    � RowDefinitions 5      �� 5      $��* �6    �� 5      $	��100 �6    .+�5      � $��0��6   .+�5      � -   5         �Name$ imageWp1��5      $��40q�6.   .+�5      �� $��15q�6   $	��268��6)   $	��230��65   ._�5      � .+�5      �� -   5         ��Name$ addImageBtn��5   (    � Cursor$
 Hand��6=   
������ ���#9E9E9E$��3��6o    �� StrokeDashArray$ 8R�6�   6�   �� 5      -   5        $
 Hand��5   (   $	��100��66   $��5��6B   ���#9E9E9E �� StrokeLineJoin$ Round=�6^   $��5��6u   6�   �� 5      -   5        $
 Hand��5   (   $��5��66   $	��100��6@   ���#9E9E9E$ Round=�6^   $��5��6u   6�   5      5      5      � 5      -   5        $ imageWp2��5      $y�	Collapsed=�6.   $��40q�6E   5      � 5       -   5        $ imageWp3��5       $y�	Collapsed=�6.   $��40q�6E   5!      5"      � 5#      $��1��6   .+�5$      � $	��140��6   $��62��6#   $��	40,0,0,25q�6/   $��Bottom=�6B   $
��Left=�6]   .+�5%      �� -   5        	 ��Name$	 	imageBtn2��5%   *   $
 Hand��6   $C��=/CalligraphyAssistantMain;component/Images/DefaultButton2.png��6=   6�   �� 5&      -   5        $
 Hand��5&      $��22��6*   $��22��65   $
��Left=�6A   $��18,0,0,0q�6\   $@��:/CalligraphyAssistantMain;component/Images/ButtonIcon2.png��6n   6�   �� 5'      -	   5        $
 Hand��5'       $��虎妞书画��6.   $��25,0,0,0q�6:   $��Center=�6L   $��Center=�6i   $��18(�6�   
�������6�   5(      � 5)      $y�	Collapsed=�6   $	��140��6.   $��62��6:   $��	40,0,0,25q�6F   $��Bottom=�6Y   $
��Left=�6t   .+�5*      �� -
   5        $	 	imageBtn1��5*   *   $
 Hand��6   $C��=/CalligraphyAssistantMain;component/Images/DefaultButton2.png��6=   6�   �� 5+      -   5        $
 Hand��5+      $��20��6*   $��20��65   $
��Left=�6A   $��20,0,0,0q�6\   $@��:/CalligraphyAssistantMain;component/Images/ButtonIcon1.png��6n   6�   �� 5,      -   5        $
 Hand��5,       $��临时分发��6.   $��25,0,0,0q�6:   $��Center=�6L   $��Center=�6i   $��18(�6�   
�������6�   5-      � 5.      $��Bottom=�6   $��Right=�67   .+�5/      � $	��140��6   $��62��6'   $��	0,0,20,25q�63   $��Bottom=�6F   $��Right=�6a   .+�50      �� -   5        $	 sendBtn��50   .   $
 Hand��6    $B��</CalligraphyAssistantMain;component/Images/DefaultButton.png��6?   6�   �� 51      -   5        $
 Hand��51   $   $��发送��62   $��Center=�6<   $��Center=�6Y   $��18(�6t   
�������6�   52      � 53      -   5        
 �Name$
 clearGd��53      $	��140��6,   $��62��68   $��	0,0,20,25q�6D   $��Bottom=�6W   $��Right=�6r   .+�54      �� -   5        $	 clearBtn��54   .   $
 Hand��6    $B��</CalligraphyAssistantMain;component/Images/DefaultButton.png��6@   6�   �� 55      -   5        $
 Hand��55   $   $��清除��62   $��Center=�6<   $��Center=�6Y   $��18(�6t   
�������6�   56      � 57      $	��140��6   $��62��6'   $��	0,0,40,25q�63   $��Bottom=�6F   $��Right=�6a   .+�58      �� -   5        $	 	cancelBtn��58   .   $
 Hand��6    $B��</CalligraphyAssistantMain;component/Images/DefaultButton.png��6A   6�   �� 59      -   5        $
 Hand��59   $   $��返回��62   $��Center=�6<   $��Center=�6Y   $��18(�6t   
�������6�   5:      5;      5=      �� 5>      $��5q�6   $��40��6   $��40��6*   $
��Left=�66   $	��Top=�6Q   $��0��6i   $��0��6y   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   �� 5?      $��5q�6   $��40��6   $��40��6*   $��Right=�67   $	��Top=�6S   $��2��6k   $��0��6{   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�    J� RenderTransformOrigin$ 0.5,0.5&�6�   z�5@      V� 5A      .~�5B      ��  �� ScaleY$ 1Q�6)    �� ScaleX$ -1Q�64   �� 5C       �� AngleY$ 0Q�6(    �� AngleX$ 0Q�63   �� 5D       �� Angle$ 0Q�6*   T� 5E      5F      5H      �� 5I      $��5q�6   $��40��6   $��40��6*   $��Right=�67   $��Bottom=�6S   $��2��6n   $��2��6~   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   $ 0.5,0.5&�6�   z�5J      V� 5K      .~�5L      �� $ -1Q�6)   $ -1Q�65   �� 5M      $ 0Q�6(   $ 0Q�63   �� 5N      $ 0Q�6*   T� 5O      5P      5R      �� 5S      $��5q�6   $��40��6   $��40��6*   $
��Left=�67   $��Bottom=�6R   $��0��6m   $��2��6}   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   $ 0.5,0.5&�6�   z�5T      V� 5U      .~�5V      �� $ -1Q�6)   $ -1Q�65   �� 5W      $ 0Q�6(   $ 0Q�63   �� 5X      $ 90Q�6*   T� 5Y      5Z      5\      5^      >   8CalligraphyAssistantMain.Controls.CalligraphyListControl  5_   
   -   5          Name$ calligraphyListControl��5_   '   >   8CalligraphyAssistantMain.Controls.CalligraphyListControl   CalligraphyListSelected$4 .calligraphyListControl_CalligraphyListSelected��6G   $y�	Collapsed=�6�   5`      5a      