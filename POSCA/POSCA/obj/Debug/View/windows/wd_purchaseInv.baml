   M S B A M L   `   `   `  ����@  <POSCA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:POSCA.View.windowsPOSCA.View.windows  5   	   '    !POSCA.View.windows.wd_purchaseInv   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:POSCA.View.windows     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   
   -   5          � WindowStyle$
  None=�5	   
    � WindowStartupLocation$ CenterScreen=�6    � 
ResizeMode$ NoResize=�5
   	   $��0q�6   $	��600��5   
   $	��800��6   
������  � AllowsTransparency . .��5      � -   5         �Name$ 	grid_main��5   +   $��1��6   $��1��6    � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��* �6   .+�5   
   � .+�5      �� $��0q�6   $��0��6*    �� CornerRadius$ 7,7,0,0��5         	MainColor#��[  6-   ._�5      � .+�5      �� -   5         ��Name$ 	btn_colse��5   Y    �� IsCancel . $��Right=�5   0   $��Center=�6L   
�������$��7.5,0q�5   I   $��SemiBold"�5      $��24��64   $��0q�6?   	 � ToolTip$	 Close��6K   $��14(�6\   ��5   g   Z� ��5      Z� .��5      '  !MaterialDesignThemes.Wpf.PackIcon  
�������
   Kind$
 Close=�6E   $��20��5   *   $��20��65   5      � 5      $��Center=�6    � ColumnDefinitions 5      �� 5      $��* �6/   �� 5       $
��1.2* �6/   .+�5"      F� 
������� �� Stretch$
 Fill=�6,   $	��5,0q�6;   $��LeftToRight=�6H   $��Right=�5#      $��0��6;   $��Center=�5$      $��20��66   $��20��6A     invoice#��[ 5%      �� 5&      -   5         ��Name$ 	txt_title��5&   4   $��invoices��6$   
�������$
��Left=�5'   $   $��Center=�6?   $��1��5(      $	��5,0q�64   5)      5*      5+      5,      �� 5-   
   $��1,0,1,1q�6   $��1��6,   $ 0,0,7,7��5.      ���#999999
�������._�5/      � $��5q�6    50      �� 51      $��* �6$    53      �� 54      .+�56      �� 
�������$��0��63   $
��Auto=�6B   .��57      �  58      �� 59      $
��auto �6,   �� 5:      $��* �6,   �� 5;      $
��auto �6,   .+�5=      �� $��2��6&   $
 22.5��6<   $��15 5 5 0q�6P   $��1q�6d     SecondColor#��[ 6x   
������� J� Effect 5>   "   3  -System.Windows.Media.Effects.DropShadowEffect  5?   &      ShadowDepth$ 1Q�5@         	Direction$	 360Q�5A         Color$
 Gray��5B         Opacity$	 0.3Q�5C         
BlurRadius$ 10Q�5D      ._�5F   "   � $��0 0 0 0q�6'    5G   &   �� 5H   *   $
��auto �6;   �� 5I   *   $��* �6;   �� 5J   *   $��* �6;   �� 5K   *   $��* �6;   �� 5L   *   $��* �6;   �� 5M   *   $
��auto �6<   �� 5N   *   $
��auto �6<   �� 5O   *   $
��auto �6<   $��75��6I   .+�5R   &   F� #��[ 6+   $
 Fill=�6O   $��20 0 0 0q�5S      $
��17.5��5T      $
��17.5��6'   $��RightToLeft=�65     
magnifying#��[ 5U      �� 5V   "   
�������$��1��6<   $��40��6L   $ 0��6X   $��15,0,5,0q�6i   $��0 0 0 1q�6}     Grey#��[ 6�   ._�5W   *   � .+�5Y   *   �� -   5         ��Name$ cb_LocationId��5Y   3   $��10,0,5,0q�5Z   +     	textColor#��[ 6?   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ LocationId...��5[   +    & "MaterialDesignFloatingHintComboBox#��[ 5\   ,   $��0��5]   +   $��40��6;   $	��0,0q�6H   ���#737373$��0q�5^   A    �� IsTextSearchEnabled .   �� 
IsEditable .  �� StaysOpenOnEdit .  �� SelectedValue 5Y   M   �� *
LocationId+ �� Mode$ TwoWay=�5b   +   5c   '   5d   '   �� 5e   "   
�������$��2��6<   $��40��6L   $ 0��6X   $	��5,0q�6i   $��0 0 0 1q�6x   #��[ 6�   ._�5f   &   � .+�5h   *   �� -   5         ��Name$ tb_searchInvNumber��5h   ?   $	��0,0q�62    % !MaterialDesignFloatingHintTextBox#��[ 5i   *   $��10,0,5,0q�5j   *   $��0q�6?   $ 	InvNumber��5k   *   #��[ 6W   $��40��5l   k   .  (MaterialDesignThemes.Wpf.TextFieldAssist   CharacterCounterStyle 6*   �� 5m   *   5o   '   5p   #   �� 5q   "   $��3��6*   $ 0��6:   $��5,5,5,5q�6K   $��0 0 0 1q�6^   #��[ 6x   ._�5r   &   � .+�5s   *   (  "System.Windows.Controls.DatePicker  -   5          Name$ dp_FromDate��5s   6    ( $MaterialDesignFloatingHintDatePicker#��[ 5t   )   ���#737373$��0q�5u   B   $��0 0 0 -1q�6V   #��[ 5v   )   $ FromDate...��5x   )   #��[ 5y   )   $��40��6S    5w   )   �� 5y   S   5�   '   5�   #   �� 5�   "   $��4��6)   $ 0��69   $��5,5,10,5q�6J   $��0 0 0 1q�6^   #��[ 6x   ._�5�   &   � .+�5�   *     -   5        $ 	dp_ToDate��5�   6   #��[ 5�   )   ���#737373$��0q�5�   B   $��0 0 0 -1q�6V   #��[ 5�   )   $ 	ToDate...��5�   )   #��[ 5�   )   $��40��6S      DisplayDateStart 5�   *   ��  �� ElementName$ dp_FromDate��  �� Path$  SelectedDate� 5�   )   �� 5�   S   5�   '   5�   #   �� 5�   "   $��7��6)   $��50��69   $ 0 22.5 22.5 0��5�      $��0q�5�      ! �� Padding$! 0q�6$   #��[ 60   ._�5�   *   �� -	   5        $ 
btn_search��5�      +  %MaterialDesignThemes.Wpf.ButtonAssist"   CornerRadius$" 15��5�      $��0q�5�      ��5�   2   Z� ��5�      Z� .��5�   .   �� # �� Orientation$# 
Horizontal=�69   $	��5 0q�6R   .+�5�   2   �� -
   5        $ 
txt_search��5�   <   $��search��6P   
�������$��5q�6r   5�   /   5�   +   5�   '   5�   #   5�      � 5�      $��1��6    .+�5�      �� -   5        $ ��Name$$ brd_roundDataGrid��5�   W   $��5,5,5,5q�6%   $��1��68   $ 10��6E   $��0q�6r   
�������5�      &   System.Windows.Controls.DataGrid  5�      -   5        %  Name$% dg_purchaseInvoice��5�   '   $��	15,15,5,5q�5�      &   CanUserAddRows& .  '   
IsReadOnly' .  (   CanUserDeleteRows( .  )   HeadersVisibility$) Column=�5�      
�������*   AutoGenerateColumns* .  +   ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$+ SizeToHeader 5�      ,   AlternatingRowBackground, �#f3f3f3- �� AlternationCount$- 2��5�   =   . J� OpacityMask. 5�   "   !� 5�   &   / !� Visual/ 62   �� $ brd_roundDataGrid��0 � 	Resources0 5�   "   =	  7System.Windows.Controls.Primitives.DataGridColumnHeader1 �� BasedOn2 �� 
TargetType3 �� Property4 �� Value 	 Font-cairo-regular��%>  (��       5�   6   * SystemColors.HighlightBrushKey+)'	       7	 7  7 	 �� 6&   6j   �� 5�   &   81   "2 	 6e   .�5�   *   �� 3 � ��4 �#BADAEF6G   �� 5�   *   3 � ��
4 �����6H   �� 5�   *   3 � ��$4 0 0 1 0q�6L   �� 5�   *   3 � ��84  6G   �� 5�   *   3 � ��84  6G   5�   '   5�   #   5   Columns5 5�   "   0
  *System.Windows.Controls.DataGridTextColumn
  5�   &   ,  &System.Windows.Controls.DataGridColumn6   Width$6 * 69   7   Header$7 	InvNumber��6D   1  +System.Windows.Controls.DataGridBoundColumn8   Binding8 6W   �� $  	InvNumber�$ OneWay=�
  5�   &   $6 * 69   $7 LocationName��6D   8 6Z   �� $  LocationName�$ OneWay=�
  5�   &   $6 * 69   $7 SupplierName��6D   8 6Z   �� $  SupplierName�$ OneWay=�
  5�   &   $6 * 69   $7 	OrderDate��6D   8 6W   �� $  	OrderDate�$ OneWay=� 
 dateFrameConverter9 �� 	Converter#9 [
 5�      5�      �� 5�      -   5        $ btn_save��5�   )   $��6��5�   #   $��2��60   $��select��6F   $" 7��6W   $��5,5,5,5q�5�      
�������#��[  5�      ��6C   Z� 5�      5�      5�      5�      5�      5�      