   M S B A M L   `   `   `  ����S  OCalligraphyAssistantMain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullu?clr-namespace:CalligraphyAssistantMain.Controls.studentGrouping1CalligraphyAssistantMain.Controls.studentGrouping  5      M+clr-namespace:CalligraphyAssistantMain.CodeCalligraphyAssistantMain.Code  5      U/clr-namespace:CalligraphyAssistantMain.Controls!CalligraphyAssistantMain.Controls  5      N    HCalligraphyAssistantMain.Controls.studentGrouping.StudentGroupingControl   5      ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Qlocal?clr-namespace:CalligraphyAssistantMain.Controls.studentGrouping     5      <code+clr-namespace:CalligraphyAssistantMain.Code     5      Dcontrols/clr-namespace:CalligraphyAssistantMain.Controls     5        � 	Resources  5       �� 
TargetType �� Property �� Value3   -CalligraphyAssistantMain.Code.IsNullConverter   isNullConverter �� MergedDictionaries �� Source�� 5   
    5      �� 5      $P Jpack://application:,,,/CalligraphyAssistantMain;component/Style\Style.xaml?�6%   %�   '��       &	  �     �� 5      " ��6   .�5      ��  � ��$ 黑体)�6/   �� 5       � ��$ Center=�68   �� 5       � ��$ Center=�66   5        5      6#   5      .��5      � ��5   
   �� 5       �� ImageSource$O Ipack://application:,,,/CalligraphyAssistantMain;component/Images/Back.png��6    � RowDefinitions 5   
   �� 5      $��50 �6   �� 5      �� 5      $��70 �6   .+�5!   
   � $��0��6   $	��0 0q�6   .+�5"      �� ���#A08466$��870px��6*   $
��50px��68    �� CornerRadius$ 	0 0 45 45��6F   6a   �� 5#      	 �� TextAlignment$	 Center=�6   $��Center=�60   $��讲台��6K   $��24(�6U   $
��Bold"�6c   ���#FFFFFF6�   5$      �� 5%   
   $��1��6   $ 4��6   $
��1490��60   $��0 30 0 0q�6=   $	��Top=�6O   ._�5&      �� $��1��6   $
��50 0q�6P   ��6(   �� *CameraItemInfos+��5'      �� 5(      .R�5)      � 
 � Orientation$
 
Horizontal=�6$   $
��Left=�6=   5*      ��5,      �� 5-      .R�5.      �� $��421px��6!   $��442px��6/   $��
20 0 20 20q�6>   ���#000000$��1q�6h    J� Effect 5/      3  -System.Windows.Media.Effects.DropShadowEffect  50   "      Opacity$	 0.2Q�63      ShadowDepth$ 8Q�6A   ._�53      �  54   "   �� 55   &   $��70 �64   �� 56   &   .+�58   "   � ���#ffffff$��2��6=    J� Opacity$	 0.5Q�6N   6^   �� 59   "    ��Name$ groupBtn��6)   -   5        ���#000000$��2q�59   Q   $	��155��6e   $
��Left=�6q   $��15 10q�6�   $ 5��6�   
 � Tag 6�   �� ._�5:   &   � $��3q�6+   ���#F1DA9E.+�5;   *   �� $	 Center=�65   $��Center=�6L   $��24(�6~   $
��Bold"�6�   ���#000000��6g   �� *Name+6�   5<   '   5=   #   �� 5>   "   $��1��6/   $
��24 0q�6`   ��6<   �� *StudentList+��5?   &   �� 5@   *   .R�5A   .   � $
 
Horizontal=�68   5B   +   ��5D   &   �� 5E   *   .R�5F   .   �� $ 
studentBtn��65   -   5        ���#000000$��2q�5F   _   $	��155��6s   $
��Left=�6   $��15 10q�6�   $ 5��6�    6�   �� ._�5G   2   � $��3q�67   ���#F1DA9E.+�5H   6   �� $	 Center=�6A   $��Center=�6X   $��24(�6�   $
��Bold"�6�   ���#000000��6s   �� *Name+6�   5I   3   5J   /   5K   +   5M   #   5N      5O      5Q      5S      5T      ;   5CalligraphyAssistantMain.Controls.RightToolBarControl  5U   
   $��1��6'   $��94��64   $��Center=�6?   $��Right=�6Z   6x   �� 5V   
   -   5        $ 	returnBtn��5V      $��2��6$   $ 8��61   $��Center=�6B   $	��228��6_   $��70��6k   ._�5W      �� $T��Npack://application:,,,/CalligraphyAssistantMain;component/Images/Button2_2.png��6   5X      5Y      5Z      