   M S B A M L   `   `   `  ����S  OCalligraphyAssistantMain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:CalligraphyAssistantMain.Controls!CalligraphyAssistantMain.Controls  5      M+clr-namespace:CalligraphyAssistantMain.CodeCalligraphyAssistantMain.Code  5      <    6CalligraphyAssistantMain.Controls.MaterialCountControl   5      ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Alocal/clr-namespace:CalligraphyAssistantMain.Controls     5      <code+clr-namespace:CalligraphyAssistantMain.Code     5        � 	Resources  5
       �� 
TargetType �� Property �� Value   TextBoxTemplatePlaceHolder �� 
TargetType ��Name J� SnapsToDevicePixels �� CornerRadius �� Padding	 ��Name
 ��Name
 � Tag J� Opacity �� Triggers P� Property P� Value �� 
TargetName �� MergedDictionaries �� Source�� 5   
    5      �� 5      $P Jpack://application:,,,/CalligraphyAssistantMain;component/Style\Style.xaml?�6%   %�  '��       &	  �     �� 5      " ��6   .�5      ��  � ��$ 黑体)�6/   �� 5       � ��$ Center=�68   �� 5       � ��$ Center=�66   5      �� 5      " ��6A   .R�5      �� $ border��6   #��z��6)   #��z��6S   #��z��6�    . $ 15��6�   $	 5,0q�6�   ._�5      � .+�5      �� $	 PART_ContentHost��6'   |�.  $��Hidden=�6S   $��Hidden=�6z   �� 5      $
 PlaceHolder��6$   #��z 69   $
��Left=�6V   $	��Top=�6q   $ 0Q�6�   
������ 6�   5      5       5      P� 5       � ��$  ��6/   . �5      �� $ PlaceHolder��6!    �@J�Opacity$	 0.3Q�6M   6[   5      5      5!      .��5#      � ���#FFCF4A � RowDefinitions 5$   
   �� 5%      $��82 �6   6*   �� 5&      $��60 �6   6*   �� 5'      6   �� 5(      $��56 �6   6*   .+�5+   
   � $��0��6   $
��0 20q�6   .+�5,      �� $
��1778��6   $��32��6!   $��0,30,0,0q�6-   $	��Top=�6?   $=��7/CalligraphyAssistantMain;component/Images/TitleBar.png��6W   6�   �� 5-       �� TextAlignment$ Center=�6   $��Center=�60   $��0,10,0,0q�6K   $��拼图统计��6]   $��32(�6i   $
��Bold"�6w   6�   5.      �� 51   
   -   5        $
 mcount��51      $��1��6$   $
��Left=�61   $��10 0 0 0q�6L   �� 52      $�	共计：��6   $��18(�6   $
��Bold"�6+   6?    �� 53      $��20(�6)   $
��Bold"�67   
���  ���6   �� *Total+6\    �� 54      $-�'位同学参与拼图，已经完成：��6   $��18(�6'   $
��Bold"�65   6I    �� 55      $��20(�6,   $
��Bold"�6:   
���  ���6   �� *
Complete+6_    �� 56      $	�位��6   $��18(�6   $
��Bold"�6*   6>   57      8   2CalligraphyAssistantMain.Controls.countdownControl  5:   
   -   5          Name$	 cdc��5:   !   	   Id$ 1��6.   $��1��65   
���  �� K� ZIndex$
 1000��6S   $	��Top=�6g   $	��200��6   $��Right=�6�   $��10q�6�   6�   �� 5=   
   -   5        $
	 svgs��5=      $��2��6%   $
��Auto=�62   $
��Auto=�6U   $��20q�6z   .��5>      � -   5         �Name$ seats��5>       � ShowGridLines .  $��Center=�69   $��Center=�6V   6s   5?      �� 5B   
   $��3��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   .+�5C      �� -   5        $ 	returnBtn��5C      $ 8��6(    � Cursor$
 Hand��69   $	��128��6G   $��48��6S   $��Center=�6_   $
��40 0q�6|   ���#34C58B._�5D      �� $��返回��6   $��16(�6&   ���#FFFFFF64   5E      5F      5G      5H      