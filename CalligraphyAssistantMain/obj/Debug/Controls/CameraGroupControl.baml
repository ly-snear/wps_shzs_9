   M S B A M L   `   `   `  ����S  OCalligraphyAssistantMain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:CalligraphyAssistantMain.Controls!CalligraphyAssistantMain.Controls  5      F BWPFMediaKit, Version=2.2.0.0, Culture=neutral, PublicKeyToken=nullfBclr-namespace:WPFMediaKit.DirectShow.Controls;assembly=WPFMediaKitWPFMediaKit.DirectShow.Controls 5      :    4CalligraphyAssistantMain.Controls.CameraGroupControl   5      ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Alocal/clr-namespace:CalligraphyAssistantMain.Controls     5      ZWPFMediaKitBclr-namespace:WPFMediaKit.DirectShow.Controls;assembly=WPFMediaKit     5      .��5
      � 
������ .+�5   
   �� -   5          ��Name$  borderBd��5      $
��1200��6#   $��0,100,0,130q�60   
�������$��3q�6X   
���   � J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         Opacity$	 0.3Q�6#      ShadowDepth$ 8Q�61   5      � 5   
   -   5         �Name$ borderGd��5      $
��1200��6!   $��0,100,0,130q�6.    � RowDefinitions 5      �� 5      $��50 �6    �� 5      $��* �6    �� 5      $��80 �6    .+�5      � $��0��6   5      � 5      -   5        $��dragGd��5       $��1��6   .+�5      �  5      �� 5      $��* �6(   �� 5      $	��150 �6(   .+�5      � $��0��6   $��
30,0,30,10q�6(   
���   �.+�5       � -   5        $ playerBackGd��5       67   5!      � 5"      $��1��6   .+�5#      � $��Center=�6$   $	��150��6A   $
��23,0q�6N   .+�5$      �� -   5        $  
previewBd1��5$   %   
 � Tag$ 1��69   $	��7,0q�6A   ���#FEA356$��4q�6d   $	��240��6x   $	��150��6�   
���   �._�5%   "   � .+�5&   &   =   7CalligraphyAssistantMain.Controls.RtmpPlayerControl_WPF  -   5          Name$ player1��5&   B   =   7CalligraphyAssistantMain.Controls.RtmpPlayerControl_WPF   Playing$ Player_Playing��6S   8   2CalligraphyAssistantMain.Controls.USBPlayerControl  5'   &   -   5        	  Name$	 
usbPlayer1��5'   =   8   2CalligraphyAssistantMain.Controls.USBPlayerControl
   Playing$
 Player_Playing��6h   $y�	Collapsed=�6Q   6h   � 5(   &   -   5        $ titleGd1��5(   +   $��30��6=   $��Bottom=�6I   ���	#33000000.+�5)   *   �� -	   5         ��Name$ 	titleTxt1��5)   4   $��
摄像头1��6G   $��14(�6S   
�������$��Center=�6t   $��Center=�6�   5*   '   5+   #   5,      �� 5-      -
   5        $  
previewBd2��5-   %   $ 2��69   $	��7,0q�6A   
���   �$	��240��6b   $	��150��6n   $��4q�6{   
���   �._�5.   "   � .+�5/   &     -   5        $ player2��5/   B   $ Player_Playing��6S     50   &   -   5        $	 
usbPlayer2��50   =   $
 Player_Playing��6h   $y�	Collapsed=�6Q   6h   � 51   &   -   5        $ titleGd2��51   +   $��30��6=   $��Bottom=�6I   ���	#33000000.+�52   *   �� -   5        $ 	titleTxt2��52   4   $��
摄像头2��6G   $��14(�6S   
�������$��Center=�6t   $��Center=�6�   53   '   54   #   55      �� 56      -   5        $  
previewBd3��56   %   $ 3��69   $	��7,0q�6A   
���   �$	��240��6b   $	��150��6n   $��4q�6{   
���   �._�57   "   � .+�58   &     -   5        $ player3��58   B   $ Player_Playing��6S     59   &   -   5        $	 
usbPlayer3��59   =   $
 Player_Playing��6h   $y�	Collapsed=�6Q   6h   � 5:   &   -   5        $ titleGd3��5:   +   $��30��6=   $��Bottom=�6I   ���	#33000000.+�5;   *   �� -   5        $ 	titleTxt3��5;   4   $��
摄像头3��6G   $��14(�6S   
�������$��Center=�6t   $��Center=�6�   5<   '   5=   #   5>      5?      5@      5A      5B      � 5C      $��2��6   .+�5D      � $	��140��6   $��62��6#   $��	0,0,38,25q�6/   $��Bottom=�6B   $��Right=�6]   .+�5E      �� -   5         ��Name$ 	cancelBtn��5E   *    � Cursor$
 Hand��6   $B��</CalligraphyAssistantMain;component/Images/DefaultButton.png��6=   6�   �� 5F      -   5        $ textLb2��5F       $
 Hand��61   $��返回��6?   $��Center=�6I   $��Center=�6f   $��18(�6�   
�������6�   5G      � 5H      $y�	Collapsed=�6   $	��140��6.   $��62��6:   $��
0,0,200,25q�6F   $��Bottom=�6Z   $��Right=�6u   .+�5I      �� -   5        $ shareBtn��5I   *   $
 Hand��6   $B��</CalligraphyAssistantMain;component/Images/DefaultButton.png��6<   6�   �� 5J      -   5        $ textLb1��5J       $
 Hand��61   $��分享��6?   $��Center=�6I   $��Center=�6f   $��18(�6�   
�������6�   5K      5L      �� 5M      $��5q�6   $��40��6   $��40��6*   $
��Left=�66   $	��Top=�6Q   $��0��6i   $��0��6y   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   �� 5N      $��5q�6   $��40��6   $��40��6*   $��Right=�67   $	��Top=�6S   $��2��6k   $��0��6{   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�    J� RenderTransformOrigin$ 0.5,0.5&�6�   z�5O      V� 5P      .~�5Q      ��  �� ScaleY$ 1Q�6)    �� ScaleX$ -1Q�64   �� 5R       �� AngleY$ 0Q�6(    �� AngleX$ 0Q�63   �� 5S       �� Angle$ 0Q�6*   T� 5T      5U      5W      �� 5X      $��5q�6   $��40��6   $��40��6*   $��Right=�67   $��Bottom=�6S   $��2��6n   $��2��6~   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   $ 0.5,0.5&�6�   z�5Y      V� 5Z      .~�5[      �� $ -1Q�6)   $ -1Q�65   �� 5\      $ 0Q�6(   $ 0Q�63   �� 5]      $ 0Q�6*   T� 5^      5_      5a      �� 5b      $��5q�6   $��40��6   $��40��6*   $
��Left=�67   $��Bottom=�6R   $��0��6m   $��2��6}   $B��</CalligraphyAssistantMain;component/Images/LeftTopBorder.png��6�   $ 0.5,0.5&�6�   z�5c      V� 5d      .~�5e      �� $ -1Q�6)   $ -1Q�65   �� 5f      $ 0Q�6(   $ 0Q�63   �� 5g      $ 90Q�6*   T� 5h      5i      5k      5l      5m      5n      