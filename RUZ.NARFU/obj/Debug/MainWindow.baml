   M S B A M L   `   `   `  ����D  @RUZ.NARFU, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:RUZ.NARFU	RUZ.NARFU  5   	       RUZ.NARFU.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:RUZ.NARFU     5   	   -   5        $	��Wnd��5   <     � Title$  Расписание��6	   $	��720��6   $
��1235��6,    � 	Resources 5	         RUZ.NARFU.TypeToColor   TypeConverter  RUZ.NARFU.TextToSign  SignsConverter  RUZ.NARFU.ResizeWidth  WidthConverter  RUZ.NARFU.ResizeHeight  HeightConverter   RUZ.NARFU.TextToVisibility  VisibilityConverter��%�   &	        &	      &	 &     &	 9     &	 L       5
   
   6     5   
   6     5   
   6     5   
   6     5   
   6!   5      .��5      � .+�5   
   ��  �� Orientation$ Vertical=�6   .+�5      o� 
�������.@�5      m� $��Загрузить��6    m� Command 6/   �� *
Settings+m� 5      $��Настройки��6   5      �� 5      $ 
Horizontal=�6   $��Center=�62   .+�5      �� 
����p��$��15(�6R   ��67   �� *	TableInfo+6R   5      �� 5      ��6   �� *Days+��5      �� 5      .R�5      �� ��6'   �� ��5      �� 5   "   .R�5   &   �� $ 
Horizontal=�61   5    #   ��5"      �� 5#   "   .R�5$   &   ��  �� CornerRadius$ 1��6.   $	��200��6?   $��20��6N   $��1q�6�   ���#e3e3e3��6Z   ��  �� ElementName$	 Wnd�� �� Path$ Width� �� 	Converter# [  �� Mode$ TwoWay=�._�5%   *   �� $��Center=�64   ��6Q   �� *	DayInfo+5&   '   5'   #   5)      5*      5,      5-      �� 5.   
   ��. $
��Auto=�60   $��2��6U   $��2��6i   $��0,65,0,0q�6v   .��5/      �� ��6   �� *Pairs+��50      �� 51      .R�52      �� ��6#   �� ��53      �� 54      .R�55   "   �� $ 
Horizontal=�6-   56      ��58      �� 59      .R�5:   "   �� $ 1��6)   $	��175��6:   $	��150��6J   $	��200��6Z   $��1q�6�  ���#e3e3e3��6i   �� $	 Wnd��$ Height�# [ $ TwoWay=���6�   �� $	 Wnd��$ Width�# [ $ TwoWay=���67  �� $
 type��$
 Text�# [  ._�5;   *   � .+�5<   .   �� .+�5=   .   �� $��	0.5,0,0,0q�68   $
��Left=�6K   	 �� TextAlignment$	 Center=�6g   ��6~   �� *Num+�� 5>   .   $
��type��6J   $��0,10,0,0q�68   ��6W   �� *Type+�� 5?   .   .+�5@   2   �� $
��Wrap=�6R   ��6<   �� *Name+6R   �� 5A   2   $	��lec��6<   ��6G   �� *
Lecturer+y�6a   �� $	 lec��$
 Text�# [ 5B   /   � 5C   .   
 � Orientation$
 
Horizontal=�68   .+�5D   2   �� $	��cab��6<   $��DemiBold"�6^   $
��Wrap=�6t   ��6G   �� *Class+6t   �� 5E   2   ��6<   �� $	 cab��$
 Text�# [ �� 5F   2   $
��Wrap=�6S   ��6<   �� *Place+6S   5G   /   �� 5H   .   $
��Wrap=�68   ��6M   �� *Group+5I   +   5J   '   5K   #   5L      5N      5O      5Q      5R      5S      5T      