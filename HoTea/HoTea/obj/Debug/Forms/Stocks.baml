   M S B A M L   `   `   `  ����?  ;lab9, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:lab9lab9  5   	       lab9.Stocks   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   $localclr-namespace:lab9     5   	   ���#fcf8f4  � Title$-  'Редактирование акции��5	   	   $	��252��6&   $	��350��63    � ShowInTaskbar .   � 
ResizeMode$ CanMinimize=�6U   $��SF Pro Display)�6n   $��14(�6�   .��5
      � $��0,0,0,-6q�6   .+�5   
   �� $��Код акции��6   $
��Left=�6$   $��	10,10,0,0q�6?   $	��Top=�6R   �� 5   
   $��Название��6   $
��Left=�6#   $��	10,41,0,0q�6>   $	��Top=�6Q   �� 5   
   $��Дата начала��6   $
��Left=�6&   $��	10,72,0,0q�6A   $	��Top=�6T   �� 5   
   $!��Дата окончания��6   $
��Left=�6)   $��
10,103,0,0q�6D   $	��Top=�6X   �� 5   
   $!��Процент скидки��6   $
��Left=�6)   $��	8,134,0,0q�6D   $	��Top=�6W   �� 5   
   -   5         ��Name$ tbStockName��5      $
��Left=�6'   $��
127,45,0,0q�6B    �� TextWrapping$
 Wrap=�6V   $	��Top=�6j   $	��189��6�   �� 5   
   -   5        $ tbStockPercent��5      $
��Left=�6*   $��127,138,0,0q�6E   $
 Wrap=�6Z   $	��Top=�6n   $	��189��6�   (  "System.Windows.Controls.DatePicker  5   
   -   5          Name$ dpStartDate��5      $
��Left=�6*   $��
127,73,0,0q�6E   $	��Top=�6Y   $	��189��6q     5   
   -   5        $ 	dpEndDate��5      $
��Left=�6(   $��127,104,0,0q�6C   $	��Top=�6X   $	��189��6p   �� 5   
   -   5         ��Name$ labelStockID��5      $��Авто��6&   $
��Left=�65   $��
127,12,0,0q�6P   $	��Top=�6d   $��23��6|   $	��189��6�   $��SF Pro Display)�6�   $��14(�6�   �� 5   
   -   5         ��Name$ btnSaveInStocks��5      $��42��6)   $��
10,161,0,0q�65   $	��Top=�6I   $��/Assets/Pics/save.png��6a    J� RenderTransformOrigin$ 0.5,0.5&�6�   $
��Left=�6�   $��40��6�   z�5      V� 5      .~�5      �� �� 5      �� 5      	 �� Angle$	 0Q�6&   T� 5      5      5      5      5       