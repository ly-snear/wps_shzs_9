   M S B A M L   `   `   `  ����S  OCalligraphyAssistantMain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:CalligraphyAssistantMain.Controls!CalligraphyAssistantMain.Controls  5      9    3CalligraphyAssistantMain.Controls.FullScreenControl   5      ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Alocal/clr-namespace:CalligraphyAssistantMain.Controls     5      .��5	      � 
���   �.+�5   
   7   1CalligraphyAssistantMain.Controls.ImageRenderGrid  -   5           Name$  imageRenderGrid��5       7   1CalligraphyAssistantMain.Controls.ImageRenderGrid   MouseLeftButtonDown$&  fullScreenGd_MouseLeftButtonDown��6P   $y�	Collapsed=�69   6P   � 5   
   -   5         �Name$ 	loadingGd��5      
���   �.+�5      �� $��Center=�6   $��Center=�63   $��16(�6P   $
��Bold"�6^   
�������$��加载中...��6�   5      (� 5   
   -   5         (�Name$
 view��5   <    (� Stretch$ Uniform=�6   $	��Top=�6$   .��5      �� $
��1920��6h   $
��1080��6u   ��6   �� *$"studentInfo.FullPreviewImageSource+ �� IsAsync .  �� Mode$ OneWay=�6u   5      � 5   
   -   5        $��fullScreenGd��5      $��3��6#   $��4��64   .+�5      � -   5        $ 
controlBar��5      .+�5      �� $��0,0,0,0q�6    � UseLayoutRounding . $��1,0,1,1q�6C   ���	#FF171717$	��290��6u   $��45��6�   $	��Top=�6�    �� CornerRadius$ 	0,0,10,10��6�   ��5      �� 5      	 �� ImageSource$A	 ;/CalligraphyAssistantMain;component/Images/LeftMenuBack.png��6%   
 �� RelativeTransform
 5      V� 5   "   .~�5   &   ��  �� CenterY$	 0.5Q�65    �� CenterX$	 0.5Q�6C   �� 5   &    �� CenterY$	 0.5Q�64    �� CenterX$	 0.5Q�6B   �� 5   &    �� Angle$ 90Q�66    �� CenterY$	 0.5Q�6A    �� CenterX$	 0.5Q�6O   T� 5   &   5   #   5      ._�5!      � $��Center=�6    $	��0,0q�6>   $��45��6K   $��Center=�6W   .+�5"      �� $��1��6$   $��45��6.   6<   �� 5#      -   5        $��_tipLb2��5#   $   
�������$
��Left=�6t   $��微软雅黑)�6�   $
��12pt(�6�   $��Center=�6�   $
��Wrap=�6�   $	��190��6�   $��10,0,0,0q�6�   ��63   ��  �� Path$
 Text� �� ElementName$ _tipLb��6�   �� 5$      -   5         ��Name$ writeScreenBtn��5$       $��7,0,7,0q�68   $��Center=�6I    � Cursor$
 Hand��6d    � ToolTip$ 点评��6r   $;��5/CalligraphyAssistantMain;component/Images/Icon17.png��6   $��26��6�   $��26��6�   6  �� 5%      -   5        $ exitFullScreenBtn��5%       $��Center=�6;   $
 Hand��6V   $ 退出全屏��6d   $;��5/CalligraphyAssistantMain;component/Images/Icon18.png��6s   $��28��6�   $��28��6�   6  5&      5'      �� 5(      -	   5         ��Name$ previousBtn��5(      $
 Hand��6d   $��45��6r   $��70��6}   $
��Left=�6�   $ 	0,10,10,0��6�   ��5)      �� 5*      $A	 ;/CalligraphyAssistantMain;component/Images/LeftMenuBack.png��6%   ._�5,      �� $��32��6   $
��Left=�6'   $��3,0,0,0q�6B   $B��</CalligraphyAssistantMain;component/Images/PreviousImage.png��6S   5-      �� 5.      -
   5        $ nextBtn��5.      $
 Hand��6\   $��45��6j   $��70��6u   $��Right=�6�   $ 	10,0,0,10��6�   ��5/      �� 50      $A	 ;/CalligraphyAssistantMain;component/Images/LeftMenuBack.png��6%   
 51      V� 52   "   .~�53   &   �� $	 0.5Q�65   $	 0.5Q�6C   �� 54   &   $	 0.5Q�64   $	 0.5Q�6B   �� 55   &   $	 180Q�66   $	 0.5Q�6B   $	 0.5Q�6P   T� 56   &   57   #   59      ._�5;      �� $��32��6   $
��Left=�6'   $��7,0,0,0q�6B   $>��8/CalligraphyAssistantMain;component/Images/NextImage.png��6S   5<      5=      5>      :   4CalligraphyAssistantMain.Controls.WriteScreenControl  5?   
   -   5          Name$ writeScreenControl��5?   #   :   4CalligraphyAssistantMain.Controls.WriteScreenControl   Back$ writeScreenControl_Back��6V   $y�	Collapsed=�6?   6V   5@      5A      