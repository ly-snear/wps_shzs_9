   M S B A M L   `   `   `  ����S  OCalligraphyAssistantMain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:CalligraphyAssistantMainCalligraphyAssistantMain  5   	   *    $CalligraphyAssistantMain.FloatWindow   5   	   ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8local&clr-namespace:CalligraphyAssistantMain     5   	   -   5          � Title$  	艺学宝��5   	    � Topmost . $	��500��6$    � Left$ 0��61   $��64��6:    � WindowStyle$
 None=�6E    � AllowsTransparency . 
������  J� OpacityMask
 ����� � ShowInTaskbar .   � WindowStartupLocation$ Manual=�6�    � Icon$ Images/Icon.ico��6  .��5	      � -   5        	 �Name$	 backGd��5	      .+�5
   
   � -   5        $	 mainGd��5
      $	��500��6   $��64��6,   .+�5      �� -   5        
 ��Name$
 leftToolBar��5       �� CornerRadius$ 	0,10,10,0��6*   $��64��6C   $��Center=�6N   $
��Left=�6i   ��5      �� 5       �� ImageSource$ Images/LeftMenuBack.png��6!   ._�5      � .+�5      �� $��0,0,0,15q�6!   .+�5      � -   5        $��80��5      
 � Tag$ 屏写��6+    � ToolTip$ 屏写��64    � Cursor$
 Hand��6A   .+�5      �� $��32��6$   $��32��6/   $	��Top=�6;   $��0,20,0,0q�6S   $��Images/Icon1.png��6e   6�   �� 5      $��屏写��6(   $��Bottom=�62   $��Center=�6M   $��16(�6j   
�������6�   5      �� 5      -   5         ��Name$ classSp��5   %   .+�5      � -   5        $��80��5   #   $ 临摹��6/   $ 临摹��68   $
 Hand��6E   .+�5   "   �� $��32��6(   $��32��63   $	��Top=�6?   $��0,20,0,0q�6W   $!��Images/CalligraphyImage.png��6i   6�   �� 5   "   $��临摹��6,   $��Bottom=�66   $��Center=�6Q   $��16(�6n   
�������6�   5      � 5      -   5        $��80��5   #   $ 示范��6/   $ 示范��68   $
 Hand��6E   .+�5   "   �� $��32��6(   $��32��63   $	��Top=�6?   $��0,20,0,0q�6W   $��Images/Icon15.png��6i   6�   �� 5   "   $��示范��6,   $��Bottom=�66   $��Center=�6Q   $��16(�6n   
�������6�   5      � 5      -	   5        $��80��5   #   $ 拍照��6/   $ 拍照��68   $
 Hand��6E   .+�5   "   �� $��32��6(   $��32��63   $	��Top=�6?   $��0,20,0,0q�6W   $��Images/CameraImage.png��6i   6�   �� 5    "   $��拍照��6,   $��Bottom=�66   $��Center=�6Q   $��16(�6n   
�������6�   5!      � 5"      -
   5        $	 
classEndGd��5"   #   $��80��67   $ 下课��6C   $ 结束上课��6L   $
 Hand��6[   .+�5#   "   �� $��32��6(   $��32��63   $	��Top=�6?   $��0,20,0,0q�6W   $��Images/Icon3.png��6i   6�   �� 5$   "   $��下课��6,   $��Bottom=�66   $��Center=�6Q   $��16(�6n   
�������6�   5%      5&      � 5'      -   5        $��80��5'      $ 返回��6+   $ 返回艺学宝��64   $
 Hand��6D   .+�5(      �� $��28��6$   $��28��6/   $	��Top=�6;   $��0,20,0,0q�6S   $��Images/Icon6.png��6e   6�   �� 5)      $��返回��6(   $��Bottom=�62   $��Center=�6M   $��16(�6j   
�������6�   5*      5+      � 5,      -   5        $��minBtn��5,   )   $
 Hand��6   
������ $��12��6�   $��40��6�   $��Right=�6�   .+�5-      F� 
�������+����@KL  -1� � ��� � � ����@KL  -1	6L   5.      5/      50      �� 51      -   5        $
 leftMiniToolBar��51      $
 Hand��6.   $y�	Collapsed=�6<   $
��Left=�6S   $ 0,5,5,0��6n   $��20��6�   $��50��6�   $��Center=�6�   ��52      �� 53      $ Images/LeftMenuBack.png��6!   ._�55      � $
 Hand��6   
������ $��12��6>   $��40��6I   $��0,0,0,0q�6U   $��Right=�6f    J� RenderTransformOrigin$ 0.5,0.5&�6�   z�56      V� 57      .~�58      ��  �� ScaleY$ 1Q�6-    �� ScaleX$ -1Q�68   �� 59       �� AngleY$ 0Q�6,    �� AngleX$ 0Q�67   �� 5:       �� Angle$ 0Q�6.   T� 5;      5<      .+�5>      F� 
�������+����@KL  -1� � ��� � � ����@KL  -1	6H   5?      5@      5A      (� 5B   
   -   5         (�Name$ viewBox��5B       (� Stretch$ Uniform=�6#    (� StretchDirection$
 Both=�65   $y�	Collapsed=�6M   6f   5C      5D      