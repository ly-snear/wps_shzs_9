   M S B A M L   `   `   `  ����S  OCalligraphyAssistantMain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:CalligraphyAssistantMain.Controls!CalligraphyAssistantMain.Controls  5      U/clr-namespace:CalligraphyAssistantMain.Controls!CalligraphyAssistantMain.Controls  5      M+clr-namespace:CalligraphyAssistantMain.CodeCalligraphyAssistantMain.Code  5      3    -CalligraphyAssistantMain.Controls.MainControl   5      ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Alocal/clr-namespace:CalligraphyAssistantMain.Controls     5      DControls/clr-namespace:CalligraphyAssistantMain.Controls     5      <Code+clr-namespace:CalligraphyAssistantMain.Code     5      -   5          � 	Resources  5      8   2CalligraphyAssistantMain.Code.MainControlConverter   mainControlConverter�� 5   
   %   &	          5      6(   5      .��5      � .+�5   
   � $	��200��6   $	��Top=�6    � ColumnDefinitions 5      �� 5      $	��160 �6#   �� 5      $��* �6#   .+�5      �� $��Right=�6   $��80��60   $��80��6;   $9��3/CalligraphyAssistantMain;component/Images/Logo.png��6G   � 5      $��2��6   .+�5      0   *CalligraphyAssistantMain.Controls.TextPath  $��
20,74,0,80q�6!   ���#555���#555$��1��6O   $��Center=�6c      
FontWeight$
 Bold"�6~      
FontFamily$ 楷体)�6�      FontSize$ 50(�6�      Text$ 智慧美育教学系统��6�   6�   �� 5      -   5         ��Name$ tipLb��5      $��Right=�6+   $	��Top=�6G   ���#555$
��Bold"�6q   $��20(�6�   $��	0,90,30,0q�6�   � 5      -   5         �Name$ closeBtn��5      $y�	Collapsed=�6)    � ToolTip$ 退出艺学宝��6@   	 � Cursor$
	 Hand��6P   
������ $��	0,20,20,0q�6w   $��Right=�6�   $��32��6�   $��32��6�   $	��Top=�6�   .+�5      �� $A��;/CalligraphyAssistantMain;component/Images/CloseButton2.png��6   5      � 5      -   5        $ 
settingBtn��5      $y�	Collapsed=�6+   $ 登录设置��6B   $
	 Hand��6Q   
������ $��	0,20,70,0q�6x   $��Right=�6�   $��34��6�   $��34��6�   $	��Top=�6�   .+�5      �� $B��</CalligraphyAssistantMain;component/Images/SettingButton.png��6   5      � 5       -   5        $ exercisesBtn��5       $ 	练习题��6-   $
	 Hand��6;   
������ $��
0,20,115,0q�6b   $��Right=�6v   $��40��6�   $��40��6�   $	��Top=�6�   .+�5!      �� $��-1q�6   $;��5/CalligraphyAssistantMain;component/Images/Icon27.png��6(   $��Uniform=�6g   5"      � 5#      -   5        $ interactBtn��5#      $ 课堂互动��6,   $
	 Hand��6;   
������ $��	0,20,70,0q�6b   $��Right=�6u   $��34��6�   $��34��6�   $	��Top=�6�   .+�5$      �� $��-1q�6   $;��5/CalligraphyAssistantMain;component/Images/Icon28.png��6(   5%      � 5'      -   5        $ resourceDirectoryBtn��5'      $ 备课文件夹��65   $
	 Hand��6E   
������ $��	0,20,20,0q�6l   $��Right=�6   $��34��6�   $��34��6�   $	��Top=�6�   .+�5(      �� $��-1q�6   $L��F/CalligraphyAssistantMain;component/Images/ResourceDirectoryButton.png��6(   5)      � 5*      -   5        $ 	recordBtn��5*      $y�	Collapsed=�6*   
������ $
	 Hand��6Z   $��
0,20,170,0q�6h   $��Right=�6|   $��34��6�   $��34��6�   $	��Top=�6�   .+�5+      �� -	   5        
 ��Name$
 startRecordImage��5+      $ 开始录像��66    �� BitmapScalingMode$ HighQuality=�6E   $��28��6s   $��28��6~   $��-1q�6�   $@��:/CalligraphyAssistantMain;component/Images/RecordImage.png��6�   �� 5,      -
   5        $
 stopRecordImage��5,      $ 停止录像��65   $y�	Collapsed=�6D   $ HighQuality=�6[   $��28��6�   $��28��6�   $��-1q�6�   $D��>/CalligraphyAssistantMain;component/Images/StopRecordImage.png��6�   5-      � 5.      
������ $��
0,20,210,0q�60   $��Right=�6D   $	��234��6`   $��34��6l   $	��Top=�6x   .+�5/      �� -   5        $ 
cameraText��5/       $��16(�64   $��Center=�6B    �� TextAlignment$ Right=�6]   6u   50      51      52      � 53   
   $	��309��6   .+�54      � $��Center=�6   .+�55      � -   5        
 � Tag$ -1��55      $
	 Hand��6    $��	0,0,170,0q�6.   .+�56      �� $?��9/CalligraphyAssistantMain;component/Images/MainButton.png��6   6b     57      ���#BD9B4E���#BD9B4E$��1��6E   $ 楷体)�6Y   $ 46(�6i   $
 Bold"�6w   $��Center=�6�   $��Center=�6�   $ 	学生屏��6�   58      � 59      $
	 Hand��6   $��	0,0,170,0q�6%   .+�5:      �� $?��9/CalligraphyAssistantMain;component/Images/MainButton.png��6   6b     5;      $��1��6  $ 楷体)�6#  $ 46(�63  $
 Bold"�6A  $��Center=�6S  $��Center=�6n  $ 画面分享��6�  ��6%   �� *SelectedShareMenu+ �� 	Converter# [   �� ConverterParameter$ Menu1_Color_1����6�   �� *SelectedShareMenu+# [  $ Menu1_Color_1��6�  5<      � 5=      -   5        $ -2��5=      $
	 Hand��6    .+�5>      �� $?��9/CalligraphyAssistantMain;component/Images/MainButton.png��6   6b     5?      ���#BD9B4E���#BD9B4E$��1��6F   $ 楷体)�6Z   $ 46(�6j   $
 Bold"�6x   $��Center=�6�   $��Center=�6�   $ 书画软件��6�   5@      5A      5B      �� 5C   
   $��70��6   $��Center=�6   $��Right=�67    �� CornerRadius$ 5,0,0,5��6S   ��5D      �� 5E       �� ImageSource$B </CalligraphyAssistantMain;component/Images/RightMenuBack.png��6   ._�5G      �� -   5         ��Name$ rightToolBar��5G      $��	0,10,0,20q�6/   .+�5L      � $��80��6   $
	 Hand��6#   .+�5M      �� $��28��6   $��28��6'   $	��Top=�63   $��0,20,0,0q�6K   $:��4/CalligraphyAssistantMain;component/Images/Icon8.png��6]   6�   �� 5N      $��文件��6    $��Bottom=�6*   $��Center=�6E   $��16(�6b   
�������6�   5O      � 5P      $��80��6   $
	 Hand��6#   .+�5Q      �� $��28��6   $��28��6'   $	��Top=�63   $��0,20,0,0q�6K   $:��4/CalligraphyAssistantMain;component/Images/Icon9.png��6]   6�   �� 5R      $��检测��6    $��Bottom=�6*   $��Center=�6E   $��16(�6b   
�������6�   5S      � 5T      $��80��6   $
	 Hand��6#   .+�5U      �� $��28��6   $��28��6'   $	��Top=�63   $��0,20,0,0q�6K   $;��5/CalligraphyAssistantMain;component/Images/Icon10.png��6]   6�   �� 5V      $��反馈��6    $��Bottom=�6*   $��Center=�6E   $��16(�6b   
�������6�   5W      � 5X      $��80��6   $
	 Hand��6#   .+�5Y      �� $��28��6   $��28��6'   $	��Top=�63   $��0,20,0,0q�6K   $;��5/CalligraphyAssistantMain;component/Images/Icon11.png��6]   6�   �� 5Z      $��报告��6    $��Bottom=�6*   $��Center=�6E   $��16(�6b   
�������6�   5[      � 5\      $��80��6   $
	 Hand��6#   .+�5]      �� $��28��6   $��28��6'   $	��Top=�63   $��0,20,0,0q�6K   $;��5/CalligraphyAssistantMain;component/Images/Icon12.png��6]   6�   �� 5^      $��考勤��6    $��Bottom=�6*   $��Center=�6E   $��16(�6b   
�������6�   5_      � 5`      $��80��6   $
	 Hand��6#   .+�5a      �� $��28��6   $��28��6'   $	��Top=�63   $��0,20,0,0q�6K   $;��5/CalligraphyAssistantMain;component/Images/Icon13.png��6]   6�   �� 5b      $��教材��6    $��Bottom=�6*   $��Center=�6E   $��16(�6b   
�������6�   5c      5h      5i      � 5j   
   -   5        
������ $	��830��5j   (   $	��520��64   $��	0,200,0,0q�6A   .+�5k      � $	��Top=�6�   $��
50,-20,0,0q�6�   $	��409��6�   $	��422��6�   y�6   �� *ShareCameraMenuHovered+# [  $ Hover����5l      �� 5m       j� Stretch$ Uniform=�6!   $D >/CalligraphyAssistantMain;component/Images/MainButtonHover.png��63   5o      � 5q      
������ $��Bottom=�6�   $��Center=�6�   $	��120��6�   y�6,   �� *ShareCameraMenuHovered+# [  $ Hover��.+�5r      � $��Center=�6   .+�5s      � -   5        $ shareScreenBtn��5s      $
	 Hand��63   $ 0��6A   $	��154��6I   $��76��6U   ��5t      �� 5u       6)   �� *SelectedShareMenu+# [  $ Menu1_Back1_0��.+�5w      �� $��16(�6$   $��0,0,0,4q�62   $
��Wrap=�6�   $��Center=�6�   $ Center=�6�   ��6C   �� *SelectedShareMenu+# [  $ Menu1_Color1_0���� 6  -   5         ��Name$ titleShareDesk1��5w   	  $�分享��6"  �� 6.  �� 6:  -   5        $ titleShareDesk2��5w   >  $�桌面��6W  6d  5x      � 5y      -   5        $
	 Hand��5y      $ 1��6)   $��0,0,15,0q�61   $	��154��6C   $��76��6O   ��5z      �� 5{       6)   �� *SelectedShareMenu+# [  $ Menu1_Back1_1��.+�5}      �� $��16(�6$   $��0,0,0,4q�62   $
��Wrap=�6�   $��Center=�6�   $ Center=�6�   ��6C   �� *SelectedShareMenu+# [  $ Menu1_Color1_1���� 6  $�1号位��6	  �� 6  �� 6"  -   5        $ camera1Text��5}   &  $�桌子左下角演示��6;  6M  5~      � 5      -   5        $
	 Hand��5      $ 2��6)   $��0,0,15,0q�61   $	��154��6C   $��76��6O   ��5�      �� 5�       6)   �� *SelectedShareMenu+# [  $ Menu1_Back1_2��.+�5�      �� $��16(�6$   $��0,0,0,4q�62   $
��Wrap=�6�   $��Center=�6�   $ Center=�6�   ��6C   �� *SelectedShareMenu+# [  $ Menu1_Color1_2���� 6  $�2号位��6	  �� 6  �� 6"  -   5        $ camera2Text��5�   &  $�演示台操作��6;  6K  5�      � 5�      -   5        $
	 Hand��5�      $ 3��6)   $��0,0,15,0q�61   $	��154��6C   $��76��6O   ��5�      �� 5�       6)   �� *SelectedShareMenu+# [  $ Menu1_Back1_3��.+�5�      �� $��16(�6$   $��0,0,0,4q�62   $
��Wrap=�6�   $��Center=�6�   $ Center=�6�   ��6C   �� *SelectedShareMenu+# [  $ Menu1_Color1_3���� 6  $�3号位��6	  �� 6  �� 6"  -   5        $ camera3Text��5�   &  $�教室后方挂图��6;  6L  5�      � 5�      -   5        $ shareCopyBook��5�      $
	 Hand��62   $ 4��6@   $	��154��6H   $��76��6T   ��5�      �� 5�       6)   �� *SelectedShareMenu+# [  $ Menu1_Back1_4��.+�5�      �� $��16(�6$   $��0,0,0,4q�62   $
��Wrap=�6�   $��Center=�6�   $ Center=�6�   ��6C   �� *SelectedShareMenu+# [  $ Menu1_Color1_4���� 6  $�分享��6	  �� 6  �� 6!  -   5        $ shareImageText��5�   %  $�字帖��6=  6J  5�      5�      5�      5�      6   0CalligraphyAssistantMain.Controls.SettingControl  5�   
   -   5          Name$ settingControl��5�   "   $y�	Collapsed=�6:   @   :CalligraphyAssistantMain.Controls.ResourceDirectoryControl  5�   
   -   5          Name$ resourceDirectoryControl��5�   ,   $y�	Collapsed=�6N   :   4CalligraphyAssistantMain.Controls.CameraGroupControl  5�   
   -   5          Name$ cameraGroupControl��5�   &   $y�	Collapsed=�6B   5�      5�      