   M S B A M L   `   `   `  ����S  OCalligraphyAssistantMain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:CalligraphyAssistantMain.Controls!CalligraphyAssistantMain.Controls  5      U/clr-namespace:CalligraphyAssistantMain.Controls!CalligraphyAssistantMain.Controls  5      M+clr-namespace:CalligraphyAssistantMain.CodeCalligraphyAssistantMain.Code  5      i9clr-namespace:CalligraphyAssistantMain.Controls.fivePoint+CalligraphyAssistantMain.Controls.fivePoint  5	      <    6CalligraphyAssistantMain.Controls.CommentAnswerControl   5      ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Alocal/clr-namespace:CalligraphyAssistantMain.Controls     5      DControls/clr-namespace:CalligraphyAssistantMain.Controls     5      <code+clr-namespace:CalligraphyAssistantMain.Code     5      O	fivePoint9clr-namespace:CalligraphyAssistantMain.Controls.fivePoint     5	        � 	Resources  5       �� MergedDictionaries �� Source<   6CalligraphyAssistantMain.Code.CommentBatchToVisibility   commentBatchToVisibility>   8CalligraphyAssistantMain.Code.CommentStudentToVisibility  commentStudentToVisibility�� 5   
    5      �� 5      $P Jpack://application:,,,/CalligraphyAssistantMain;component/Style/Style.xaml?�6%   %:   &	        &	        5      6,     5      6.   5      .��5      ��  �� Orientation$ Vertical=�6   .+�5   
   �� -   5         ��Name$	 snt��5       �� CornerRadius$ 10��6   $��30��60   ���#2F4F4Fy�6Q   �� *	StyleId+ �� 	Converter# [ ._�5      �� $��18(�6/   $
��Bold"�6=   
�������$��Center=�6b   $��Center=�6   $��5q�6�   ��6   �� *Title+6�   5      �� 5   
   -   5        $	 snb��5      $ 10��6   $��30��60   ���#2F4F4Fy�6Q   �� *	StyleId+# [  ._�5      �� $��批量点评��6   $��18(�6$   $
��Bold"�62   
�������$��Center=�6W   $��Center=�6t   $��5q�6�   5      �� 5   
   $ 10��6   $��40��6#   ._�5       �� -   5         ��Name$ myfive��5       ��5!      �� 5"      .R�5#      �� $ 
Horizontal=�6%   5$      ��5&      �� 5'      .R�5(      ?   9CalligraphyAssistantMain.Controls.fivePoint.FivePointStar  ?   9CalligraphyAssistantMain.Controls.fivePoint.FivePointStar   	MouseDown FivePointStar_MouseDown6  	 � Cursor$
	 Hand��62   
   Radius
 6@   �� *Radius+   CurrentPart 6Z   �� *CurrentValue+
 � Tag 6   �� *ID+   SelectBackground 6�   �� *SelectBackground+   UnSelectBackground 6�   �� *UnselectBackgroud+��6�   �� *	Margins+62  5)      5+      5,      �� 5.   
   $ 
Horizontal=�6   $��0 20 0 0q�6.   .+�5/      �� $��20(�6   $
��Bold"�6&   $��得分:��68   6E   �� 50      -   5         ��Name$ myscore��50      $	��100��6'   $��0 0 0 1q�63   
������ $��15 0 0 0q�6g   $��20(�6�   
���  ��$��Center=�6�   $��Center=�6�   ��6y   �� *Score+6U  51      �� 53   
   $ 3��6   $	��450��6"   $	��300��6/   
�������$��0 20 0 0q�6Q   ._�54      �� -   5        $ myremark��54      $��0q�6A   ���#eeeeee$��5q�6k   ��6(   �� *	Comment+6k   55      � 57   
    � ColumnDefinitions 58      �� 59      6%   �� 5:      6%   .+�5=      �� $��0��6   
��� � �$��10q�68   $ 10��6D   $
	 Hand��6V   $
��Left=�6d   ._�5>      �� -   5         ��Name$ 
selectFile��5>      $ 
Horizontal=�61   $��Center=�6J   $��	10 0 10 0q�6g   .+�5?      �� $=��7/CalligraphyAssistantMain;component/Resources/#iconfont)�6   $	����6a   $��24(�6t   
�������$��Center=�6�   $��Center=�6�   6�   �� 5@      $��5 0 0 0q�6    $��24(�61   
������ 
�������$��Center=�6k   $��Center=�6�   语音6�   5A      5B      �� 5D      -   5        $��1��5D      
��� � �$��10q�68   $ 10��6D   $
	 Hand��6V   $��Right=�6d   ._�5E      �� -   5        $ tiger��5E      $ 
Horizontal=�6,   $��Center=�6E   $��	10 0 10 0q�6b   $	  ��6u   .+�5F      �� $=��7/CalligraphyAssistantMain;component/Resources/#iconfont)�6   $	����6a   $��24(�6t   
�������$��Center=�6�   $��Center=�6�   6�   �� 5G      $��5 0 0 0q�6    $��24(�61   
������ 
�������$��Center=�6k   $��Center=�6�   完成6�   5H      5I      5J      5K      5L      