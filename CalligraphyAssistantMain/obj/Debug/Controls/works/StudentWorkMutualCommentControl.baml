   M S B A M L   `   `   `  ����S  OCalligraphyAssistantMain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulla5clr-namespace:CalligraphyAssistantMain.Controls.works'CalligraphyAssistantMain.Controls.works  5      M+clr-namespace:CalligraphyAssistantMain.CodeCalligraphyAssistantMain.Code  5      i9clr-namespace:CalligraphyAssistantMain.Controls.fivePoint+CalligraphyAssistantMain.Controls.fivePoint  5      U/clr-namespace:CalligraphyAssistantMain.Controls!CalligraphyAssistantMain.Controls  5	   
   M    GCalligraphyAssistantMain.Controls.works.StudentWorkMutualCommentControl   5      ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Glocal5clr-namespace:CalligraphyAssistantMain.Controls.works     5      <code+clr-namespace:CalligraphyAssistantMain.Code     5      O	fivePoint9clr-namespace:CalligraphyAssistantMain.Controls.fivePoint     5      Dcontrols/clr-namespace:CalligraphyAssistantMain.Controls     5	   
     � 	Resources  5       �� 
TargetType �� Property �� Value   TextBoxTemplatePlaceHolder �� 
TargetType ��Name J� SnapsToDevicePixels �� CornerRadius �� Padding	 ��Name
 ��Name
 � Tag J� Opacity �� Triggers P� Property P� Value �� 
TargetName3   -CalligraphyAssistantMain.Code.IsNullConverter  isNullConverter3   -CalligraphyAssistantMain.Code.NumberConverter  numberConverter3   -CalligraphyAssistantMain.Code.CutoffConverter  conutConverter6   0CalligraphyAssistantMain.Code.StringToVisibility  stringToVisibility5   /CalligraphyAssistantMain.Code.ScoreToVisibility  scoreToVisibilityA   ;CalligraphyAssistantMain.Code.StudentWorkCommentTypeToBrush ! studentWorkCommentTypeToBrushO   ICalligraphyAssistantMain.Code.StudentWorkCommentRoleToHorizontalAlignment / +studentWorkCommentRoleToHorizontalAlignment �� MergedDictionaries �� Source�� 5   
    5'      �� 5(      $P Jpack://application:,,,/CalligraphyAssistantMain;component/Style\Style.xaml?�6%   %s  '��       &	  �     &	 �    &	 �    &	 �    &	 �    &	 �    &	 �    &	     �� 5      " ��6   .�5      ��  � ��$ 黑体)�6/   �� 5       � ��$ Center=�68   �� 5       � ��$ Center=�66   5      �� 5      " ��6A   .R�5      �� $ border��6   #��z��6)   #��z��6S   #��z��6�    . $ 15��6�   $	 5,0q�6�   ._�5      � .+�5      �� $	 PART_ContentHost��6'   |�.  $��Hidden=�6S   $��Hidden=�6z   �� 5      $
 PlaceHolder��6$   #��z 69   $
��Left=�6V   $	��Top=�6q   $ 0Q�6�   
������ 6�   5      5       5      P� 5       � ��$  ��6/   . �5      �� $ PlaceHolder��6!    �@J�Opacity$	 0.3Q�6M   6[   5      5        5       6#     5!      6#     5"      6#     5#      6&     5$      6%     5%      61     5&      6?   5*      .��5,      � ���#FFCF4A � RowDefinitions 5-   
   �� 5.      $��82 �6   6*   �� 5/      6   �� 50      $��56 �6   6*   .+�53   
   � $��0��6   $
��0 20q�6   .+�54      �� $
��1778��6   $��32��6!   $��0,30,0,0q�6-   $	��Top=�6?   $=��7/CalligraphyAssistantMain;component/Images/TitleBar.png��6W   6�   �� 55       �� TextAlignment$ Center=�6   $��Center=�60   $��0,10,0,0q�6K   $��作品互评��6]   $��32(�6i   $
��Bold"�6w   6�   56      �� 58   
   -   5        $ workInfo��58      $��1��6#   $ 20��60   
�������$��30q�6U   ._�59      � $��50q�6    � ColumnDefinitions 5:      �� 5;      $	��780 �6'   65   �� 5<      6)   .+�5?      � $��0��6   $��2��6'   $��0,-24,955,50q�6;    5@      �� 5A      $��50 �6(   66   �� 5B      6*   .+�5E      �� $��0��6    $��Center=�6-   $��Center=�6J   �� 5F      $��18(�69   $
��Bold"�6G   ���#006400�6   �� *	ClassName+6p    �� 5G      $�--��6   6*    �� 5H      $��20(�6<   $
��Bold"�6J   
���  ���6   �� *name_student+6o   5I      �� 5K      $��1��6   $��Center=�6F   $��Center=�6c   $��Uniform=�6~   ��6)   �� *	LocalPath+6�   5L      �� 5N      $��1��6   $��
150,0,20,0q�6)   $ 20��6=   ���#eeeeee$��2q�6e   ���#fefefe._�5O      �  5P      �� 5R      $��30 �6,   6:   �� 5T      6.   �� 5V      $��12 �6,   6:   �� 5X      $	��132 �6,   6;   .+�5[      �� $��18(�6$   $
��Bold"�62   $��评价信息��6D   $
��Left=�6P   $��20,0,0,0q�6k   $��20��6}   $��73��6�   �� 5]      $��1��6'   
�������$��
10,10,10,3q�6G   .��5^      �� -   5         ��Name$ commentList��5^   +   $	��596��6@   ��5_   "   �� 5`   &   .R�5a   *   ��  �� Orientation$ Vertical=�65   5b   '   ��5d   "   �� 5e   &   .R�5f   *   �� $ Vertical=�65   .+�5g   .   �� $��Center=�6N   ���#3e3e3e$��	10 5 10 5q�6   ��68   �� *time+��6�   �� *self+ �� 	Converter# [ �� 5h   .   $ 
Horizontal=�69   $��	10 2 10 5q�6R   $��Center=�6e   ��6�   �� *self+# [ .+�5i   2   �� $��18(�6<   $
��Bold"�6�   $��Center=�6�   ��6J   �� *type+# [ ��6�   �� *name_comment+�� 5j   2   $��18(�6<   $
��Bold"�6�   $��Center=�6�   $��:��6�   ��6J   �� *type+# [ 6�   �� 5k   2   $��14(�6<   $��Center=�6�   $��5 0 0 0q�6�   ��6J   �� *title+y�6a   �� *title+# [ 6�   �� 5l   2   $��14(�6<   $��Center=�6�   $��5 0 0 0q�6�   ��6J   �� *	content+y�6c   �� *	content+# [ 6�   �� 5m   2   $��5 0 0 0q�6�   ��68   �� *image+y�6Q   �� *image+# [ 6�   �� 5n   2   $��14(�6<   $��Center=�6�   $��5 0 0 0q�6�   ��6J   �� *score+y�6a   �� *score+# [ 6�   �� 5o   2   $��14(�6<   $	��分��6J   $��Center=�6�   $��2 0 0 0q�6�   y�6S   �� *score+# [ 6�   �� 5p   2   -   5         � Cursor$
 Hand��5p   8   $=��7/CalligraphyAssistantMain;component/Resources/#iconfont)�6�   $	����6�   $��24(�6   ���#008000$��Center=�6#  $��5 0 0 0q�6>   6F   �� *audio+y�6\   �� *audio+# [ 6�  5q   /   5r   +   5s   '   5u      5v      �� 5�      $��3��6!   $ 4��6.   
������ $��0q�6Y   ���#dddddd$��
10,0,10,10q�6�   ._�5�      �� -   5         ��Name$ 	mycomment��5�   &   ���#008000$��18(�6O   $
��Bold"�6]   $�� ��6o   $��0,0,95,0q�6w   
������ $��0q�6�   $ 录入评语... ...��6�   #��[  6�   $	��118��6�   $��Center=�6  6)  5�      �� 5�      -   5        $��3��5�   !   $	��112��6.   
��� � �$��710,-10,0,0q�6N   $ 4��6c   $
 Hand��6t   $
��Left=�6�   $��Center=�6�   $��83��6�   ._�5�      �� -   5         ��Name$ tiger��5�   )   $ 
Horizontal=�68   $��Center=�6Q   $��0 0 0 0q�6n   $  ��6   .+�5�   "   �� $��2 0 0 0q�6,   $��24(�6=   
������ 
�������$��Center=�6w   $��Center=�6�    J� RenderTransformOrigin$ -0.052,0.354&�6�   发送6�   5�      5�      5�      5�      5�      5�      �� 5�   
   $��2��6   $ 
Horizontal=�6"   $��Right=�6;   .+�5�      �� -   5        $ 	returnBtn��5�      $ 8��6(   $
 Hand��69   $	��128��6G   $��48��6S   $��Center=�6_   $
��40 0q�6|   ���#34C58B._�5�      �� $��返回��6   $��16(�6&   ���#FFFFFF64   5�      5�      5�      5�      