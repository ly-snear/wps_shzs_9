   M S B A M L   `   `   `  ����S  OCalligraphyAssistantMain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:CalligraphyAssistantMain.Controls!CalligraphyAssistantMain.Controls  5      T PCefSharp.Wpf, Version=63.0.3.0, Culture=neutral, PublicKeyToken=40c4b6fc221f4138A0clr-namespace:CefSharp.Wpf;assembly=CefSharp.WpfCefSharp.Wpf 5      6    0CalligraphyAssistantMain.Controls.WebViewControl   5      ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Alocal/clr-namespace:CalligraphyAssistantMain.Controls     5      HcefSharpWPF0clr-namespace:CefSharp.Wpf;assembly=CefSharp.Wpf     5      .��5
      � 
������ .+�5   
   �� $
��1600��6   $	��900��6   
�������$��3q�6>   
���   �  J� Effect  5      3  -System.Windows.Media.Effects.DropShadowEffect  5         Opacity$	 0.3Q�6#      ShadowDepth$ 8Q�61   5      � 5   
   $
��1600��6   $	��900��6    � RowDefinitions 5      �� 5      $��40 �6    �� 5      $��* �6    �� 5      $	��100 �6    .+�5      � $��0��6   5      � 5      $��1��6   .+�5      � $
��20,0q�6   .+�5      <   6CalligraphyAssistantMain.Controls.ChromiumWebBrowserEx  -   5          Name$ chrome��5   1   <   6CalligraphyAssistantMain.Controls.ChromiumWebBrowserEx   DownloadProgressChanged$$ chrome_DownloadProgressChanged��6A   6|   5      5      � 5      $��2��6   .+�5      � $��	38,0,0,25q�6   $��Bottom=�6/   .+�5      � $	��140��6   $��62��6'   $��0,0,25,0q�63   $
��Left=�6E   .+�5       �� -   5         ��Name$ 	imageBtn1��5    .    � Cursor$
 Hand��6    $C��=/CalligraphyAssistantMain;component/Images/DefaultButton2.png��6A   6�   �� 5!      -   5        $
 Hand��5!       $��20��6.   $��20��69   $
��Left=�6E   $��20,0,0,0q�6`   $;��5/CalligraphyAssistantMain;component/Images/Icon19.png��6r   6�   �� 5"      -   5        $
 Hand��5"   $   $��后退��62   $��Center=�6<   $��Center=�6Y   $��18(�6t   
�������6�   5#      � 5$      $	��140��6   $��62��6'   $��0,0,25,0q�63   $
��Left=�6E   .+�5%      �� -   5        $ 	imageBtn2��5%   .   $
 Hand��6    $C��=/CalligraphyAssistantMain;component/Images/DefaultButton2.png��6A   6�   �� 5&      -   5        $
 Hand��5&       $��20��6.   $��20��69   $
��Left=�6E   $��20,0,0,0q�6`   $;��5/CalligraphyAssistantMain;component/Images/Icon19.png��6r    J� RenderTransformOrigin$ 0.5,0.5&�6�   z�5'      V� 5(   "   .~�5)   &   �� �� 5*   &   �� 5+   &   	 �� Angle$		 180Q�66   T� 5,   &   5-   #   5/      �� 50      -   5        $
 Hand��50   $   $��前进��62   $��Center=�6<   $��Center=�6Y   $��18(�6t   
�������6�   51      � 52      $	��140��6   $��62��6'   $��0,0,25,0q�63   $
��Left=�6E   .+�53      �� -   5        $ 	imageBtn3��53   .   $
 Hand��6    $C��=/CalligraphyAssistantMain;component/Images/DefaultButton2.png��6A   6�   �� 54      -	   5        $
 Hand��54   $   $��下载文件夹��62   $��Center=�6?   $��Center=�6\   $��18(�6w   
�������6�   55      � 56      $	��140��6   $��62��6'   $��0,0,25,0q�63   $
��Left=�6E   .+�57      �� -
   5        $ 	imageBtn4��57   .   $
 Hand��6    $C��=/CalligraphyAssistantMain;component/Images/DefaultButton2.png��6A   6�   �� 58      -   5        $
 Hand��58   $   $��下载列表��62   $��Center=�6>   $��Center=�6[   $��18(�6v   
�������6�   59      5:      � 5;      $	��140��6   $��62��6#   $��	0,0,38,25q�6/   $��Bottom=�6B   $��Right=�6]   .+�5<      �� -   5        $ 
cancelBtn2��5<   *   $
 Hand��6   $B��</CalligraphyAssistantMain;component/Images/DefaultButton.png��6>   6�   �� 5=      -   5        $
 Hand��5=       $��返回��6.   $��Center=�68   $��Center=�6U   $��18(�6p   
�������6�   5>      5?      � 5@      $��3��6   5A      �� 5B      $��5q�6   $��40��6   $��40��6*   $
��Left=�66   $	��Top=�6Q   $��0��6i   $��0��6y   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   �� 5C      $��5q�6   $��40��6   $��40��6*   $��Right=�67   $	��Top=�6S   $��2��6k   $��0��6{   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   $ 0.5,0.5&�6�   z�5D      V� 5E      .~�5F      �� 
 �� ScaleY$
 1Q�6)    �� ScaleX$ -1Q�64   �� 5G       �� AngleY$ 0Q�6(    �� AngleX$ 0Q�63   �� 5H      $	 0Q�6*   T� 5I      5J      5L      �� 5M      $��5q�6   $��40��6   $��40��6*   $��Right=�67   $��Bottom=�6S   $��2��6n   $��2��6~   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   $ 0.5,0.5&�6�   z�5N      V� 5O      .~�5P      �� $
 -1Q�6)   $ -1Q�65   �� 5Q      $ 0Q�6(   $ 0Q�63   �� 5R      $	 0Q�6*   T� 5S      5T      5V      �� 5W      $��5q�6   $��40��6   $��40��6*   $
��Left=�67   $��Bottom=�6R   $��0��6m   $��2��6}   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   $ 0.5,0.5&�6�   z�5X      V� 5Y      .~�5Z      �� $
 -1Q�6)   $ -1Q�65   �� 5[      $ 0Q�6(   $ 0Q�63   �� 5\      $	 90Q�6*   T� 5]      5^      5`      � 5b      -   5         �Name$ 
downloadGd��5b   $   $��3��6   
������ $y�	Collapsed=�6Q   .+�5c      � $
��1000��6   $	��580��6$    5d      �� 5e      $��40 �6(   �� 5f      $��* �6(   �� 5g      $	��100 �6(   .+�5i      �� $��3��6   
�������$��3q�6A   
���   �  5j        5k      $	 0.3Q�6/   $ 8Q�6=   5m      � 5n      $��2��6   .+�5o      � $	��140��6   $��62��6+   $��	0,0,38,25q�67   $��Bottom=�6J   $��Right=�6e   .+�5p      �� -   5        $ 	cancelBtn��5p   2   $
 Hand��6$   $B��</CalligraphyAssistantMain;component/Images/DefaultButton.png��6E   6�   �� 5q      -   5        $
 Hand��5q   (   $��返回��66   $��Center=�6@   $��Center=�6]   $��18(�6x   
�������6�   5r      5s      �� 5t      $��5q�6   $��40��6'   $��40��62   $
��Left=�6>   $	��Top=�6Y   $��0��6q   $��0��6�   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   �� 5u      $��5q�6   $��40��6'   $��40��62   $��Right=�6?   $	��Top=�6[   $��2��6s   $��0��6�   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   $ 0.5,0.5&�6�   z�5v      V� 5w      .~�5x   "   �� $
 1Q�61   $ -1Q�6<   �� 5y   "   $ 0Q�60   $ 0Q�6;   �� 5z   "   $	 0Q�62   T� 5{   "   5|      5~      �� 5      $��5q�6   $��40��6'   $��40��62   $��Right=�6?   $��Bottom=�6[   $��2��6v   $��2��6�   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   $ 0.5,0.5&�6�   z�5�      V� 5�      .~�5�   "   �� $
 -1Q�61   $ -1Q�6=   �� 5�   "   $ 0Q�60   $ 0Q�6;   �� 5�   "   $	 0Q�62   T� 5�   "   5�      5�      �� 5�      $��5q�6   $��40��6'   $��40��62   $
��Left=�6?   $��Bottom=�6Z   $��0��6u   $��2��6�   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   $ 0.5,0.5&�6�   z�5�      V� 5�      .~�5�   "   �� $
 -1Q�61   $ -1Q�6=   �� 5�   "   $ 0Q�60   $ 0Q�6;   �� 5�   "   $	 90Q�62   T� 5�   "   5�      5�      � 5�      $��1��6   .+�5�      �� $��60��6!   $��40,-10,40,0q�6-   $	��Top=�6B   $��0,0,0,2q�6Z   ���#555._�5�      �� $��下载列表��6(   $��Center=�64   $��20(�6O   $
��Bold"�6]   5�      � 5�      $��40,60,40,20q�6   .+�5�      �� $
��Auto=�6+   .��5�   "   �� -   5         ��Name$ downloadListSp��5�   -   5�   #   5�      5�      5�      5�      5�      5�      5�      5�      