   M S B A M L   `   `   `  ����S  OCalligraphyAssistantMain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:CalligraphyAssistantMain.Controls!CalligraphyAssistantMain.Controls  5      6    0CalligraphyAssistantMain.Controls.SettingControl   5      ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Alocal/clr-namespace:CalligraphyAssistantMain.Controls     5      -   5          � 	Resources  5	       �� MergedDictionaries �� Source�� 5
   
    5      �� 5      $: 4/CalligraphyAssistantMain;component/Style/Style.xaml?�6%   %    5      .��5      � 
������ .+�5   
   �� $	��520��6   $	��250��6   
�������$��3q�6=   
���   � J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         Opacity$	 0.3Q�6#      ShadowDepth$ 8Q�61   5      � 5   
   $	��520��6   $	��250��6    � RowDefinitions 5      �� 5      $��50 �6    �� 5      $��* �6    �� 5      $��95 �6    .+�5      � $��1��6   .+�5      �� $��60��6   $��40,-10,40,0q�6%   $	��Top=�6:   $��0,0,0,2q�6R   ���#555._�5      �� $��登录设置��6    $��Center=�6,   $��20(�6G   $
��Bold"�6U   5      � 5       $��Bottom=�6   .+�5!      � $	��160��6   $��50,0,0,0q�6'   $��40��69   .+�5"      �� -   5         ��Name$ rememberPasswordCb��5"   c   
������    CheckBoxStyle#��[  6<    � Cursor$
 Hand��6   $��0��6�   $��18(�6�   ���#424242$
��Left=�6�   $��记住密码��6�   $��Center=�6�   $	��0,2q�6  6  5#      � 5$      $	��160��6   $��40��6'   .+�5%      �� -   5        $ autoLoginCb��5%   c   
������ #��[  6<   $
 Hand��6x   $��1��6�   $��18(�6�   ���#424242$
��Left=�6�   $��自动登录��6�   $��Center=�6�   $	��0,2q�6�   6  5&      5'      5(      � 5)      $��2��6   .+�5*      � $	��140��6   $��62��6#   $��
0,0,200,25q�6/   $��Bottom=�6C   $��Right=�6^   .+�5+      �� -   5        	 ��Name$	 okBtn��5+   *   $
 Hand��6   $B��</CalligraphyAssistantMain;component/Images/DefaultButton.png��69   6   �� 5,      -   5        $
 Hand��5,       $��保存��6.   $��Center=�68   $��Center=�6U   $��18(�6p   
�������6�   5-      � 5.      $	��140��6   $��62��6#   $��	0,0,38,25q�6/   $��Bottom=�6B   $��Right=�6]   .+�5/      �� -   5        $	 	cancelBtn��5/   *   $
 Hand��6   $B��</CalligraphyAssistantMain;component/Images/DefaultButton.png��6=   6�   �� 50      -   5        $
 Hand��50       $��返回��6.   $��Center=�68   $��Center=�6U   $��18(�6p   
�������6�   51      52      �� 53      $��5q�6   $��40��6   $��40��6*   $
��Left=�66   $	��Top=�6Q   $��0��6i   $��0��6y   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   �� 54      $��5q�6   $��40��6   $��40��6*   $��Right=�67   $	��Top=�6S   $��2��6k   $��0��6{   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   
 J� RenderTransformOrigin$
 0.5,0.5&�6�   z�55      V� 56      .~�57      ��  �� ScaleY$ 1Q�6)    �� ScaleX$ -1Q�64   �� 58       �� AngleY$ 0Q�6(    �� AngleX$ 0Q�63   �� 59       �� Angle$ 0Q�6*   T� 5:      5;      5=      �� 5>      $��5q�6   $��40��6   $��40��6*   $��Right=�67   $��Bottom=�6S   $��2��6n   $��2��6~   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   $
 0.5,0.5&�6�   z�5?      V� 5@      .~�5A      �� $ -1Q�6)   $ -1Q�65   �� 5B      $ 0Q�6(   $ 0Q�63   �� 5C      $ 0Q�6*   T� 5D      5E      5G      �� 5H      $��5q�6   $��40��6   $��40��6*   $
��Left=�67   $��Bottom=�6R   $��0��6m   $��2��6}   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   $
 0.5,0.5&�6�   z�5I      V� 5J      .~�5K      �� $ -1Q�6)   $ -1Q�65   �� 5L      $ 0Q�6(   $ 0Q�63   �� 5M      $ 90Q�6*   T� 5N      5O      5Q      5R      8   2CalligraphyAssistantMain.Controls.ImageEditControl  5S   
   -   5          Name$ imageEditControl��5S   !   $y�	Collapsed=�6;   $��	0,-50,0,0q�6R   5T      5U      