   M S B A M L   `   `   `  ����H  DQMK Assistant, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:QMK_AssistantQMK_Assistant  5   	   !    QMK_Assistant.OverlayWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:QMK_Assistant     5   	     � Title$  OverlayWindow��5   	   $
��Auto��6   $
��Auto��6-    � 
ResizeMode$ NoResize=�6:    � ShowInTaskbar .   � WindowStyle$
 None=�6f   
������  J� Opacity$	 0.5Q�6�    � AllowsTransparency . .��5	      �  � RowDefinitions 5
   
   �� 5      -   5         ��Name$ IndicatorRow��5   *   $
��Auto �6   6*   �� 5      -   5        $ KeyboardRow��5   *   $
��Auto �6   6*   .+�5   
   (� -   5         (�Name$ IndicatorViewbox��5      $��Stretch=�6,   $��Stretch=�6J   $��0��6f   .��5      �� -   5        	 ��Name$	 IndicatorCanvas��5      $��Stretch=�6.   
������ 6L   5      (� 5   
   -   5        $ LayoutViewbox��5      $��Stretch=�6)   $��Stretch=�6G   $��1��6c   .��5      �� -   5        $	 LayoutCanvas��5      $��Stretch=�6+   
������ 6I   5      5      5      