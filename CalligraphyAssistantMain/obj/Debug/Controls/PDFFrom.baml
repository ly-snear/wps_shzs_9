   M S B A M L   `   `   `  ����E  AMoonPdfLib, Version=0.3.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:MoonPdfLib;assembly=MoonPdfLib
MoonPdfLib  5   	    CalligraphyAssistantMain/   )CalligraphyAssistantMain.Controls.PDFFrom   5   	   ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   <mpp,clr-namespace:MoonPdfLib;assembly=MoonPdfLib     5   	   -   5          � Title$  
Pdf Viewer��5	   	    � WindowState$ 	Maximized=�5
   	    � 
ResizeMode$ NoResize=�5   	    � Topmost . .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� ���#f0f0f0._�5      �� $
��Left=�6    �� Orientation$ 
Horizontal=�64   .+�5      �� -   5        $��放大��5      $��50��6&   $	��0,5q�6L   �� 5      -   5        $��缩小��5      $��50��6&   $	��5,5q�6M   �� 5      -   5        $
��100%��5      $��50��6(   $	��0,5q�6N   �� 5      -   5        $
��200%��5      $��50��6(   $	��5,5q�6Q   �� 5       -   5        $��整页��5       $��50��6&   $	��0,5q�6Q   �� 5"      -   5        $��单页��5"      $��50��6&   $	��5,5q�6P   �� 5$      -   5        $��双页��5$      $��50��6&   $	��0,5q�6L   5%      5&      �� 5(   
   ���#d3d3d3$��1��6&   ._�5)         MoonPdfLib.MoonPdfPanel  -	   5          Name$ moonPdfPanel��5)      
�������   ViewType$ 
SinglePage=�6L      PageRowDisplay$ ContinuousPageRows=�6b   	   
PageMargin$	 0,2,4,2q�6�   
 J� 	AllowDrop
 . 6�   5*      5+      5,      