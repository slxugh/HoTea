   M S B A M L   `   `   `  ����?  ;lab9, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:lab9lab9  5   	       lab9.Connect   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   $localclr-namespace:lab9     5   	   -   5          �Name$  ConnectForm��5   	   ���#fcf8f4 � Title$5 /HoTea | Страница подключения��5	   	   $	��455��6.   $	��800��6;   $��Felix Titling)�6G    � 
ResizeMode$ CanMinimize=�6b    J� OpacityMask 5
      �� 5   
    �� EndPoint$ 0.5,1&�6    �� 
StartPoint$ 0.5,0&�6/   .��5      �  � Color$ Black��6   � 5      $ #fcf8f4��6    � Offset$ 1Q�6+   5      .��5      � $��0,0,0,-6q�6    � ColumnDefinitions 5   
   �� 5      .+�5   
   �� -   5        	 ��Name$	 	labelName��5      $��HoTea��6#   $��Center=�63   $��0,36,0,0q�6P   $	��Top=�6b   $	��172��6z   $��Felix Titling)�6�   $��48(�6�   $��63��6�   �� 5   
   -   5        $	 labelWelcome��5      $%��Добро пожаловать��6&   $��Center=�6A   $��0,99,0,0q�6^   $	��Top=�6p   $	��218��6�   $��Prata)�6�   $��22(�6�   $��41��6�   �� 5   
   -   5        $	 labelStatusConnect��5      $*��$Статус подключения:��6,   $
��Left=�6J   $��257,145,0,0q�6e   $	��Top=�6z   $��SF Pro Display)�6�   $��14(�6�   �� 5   
   -   5        $	 labelVersion��5      $��Релиз 1.0��6&   $
��Left=�6:   $��
10,389,0,0q�6U   $	��Top=�6i   $��SF Pro Display)�6�   $��77��6�   �� 5   
   -   5        $	 labelStatus��5      $��Отключено��6%   $
��Left=�69   $��429,145,0,0q�6T   $	��Top=�6i   $��SF Pro Display)�6�   $��14(�6�   
 J� RenderTransformOrigin$
 -0.019,0.586&�6�   �� 5   
   -   5        $��Center=�5      $��43��6-   $��	0,282,0,0q�69   $	��Top=�6L   $��44��6d   $��/Assets/Pics/login.png��6o   $
 0.5,0.5&�6�   z�5      �� 5      -   5        scaleTransform�� �� ScaleX$ 1Q�5   9    �� ScaleY$ 1Q�6D    � Triggers 5      :� 5       :� RoutedEvent$ 	MouseDown��6   .[�5       �� .��5!      �� .��5"      W�  �� 
TargetName$ scaleTransform��6.    �� TargetProperty$ ScaleX�5#   &   	 W� To$	 0.9X�5$   &    g� Duration$ 0:0:0.1D�6/    g� AutoReverse . 6B   W� 5%      $ scaleTransform��6.   $ ScaleY�5&   &   $	 0.9X�5'   &   $ 0:0:0.1D�6/    . 6B   5(      5)      5*      5,      �� 5-   
   -	   5        $	 
labelLogin��5-      $��
Логин��6$   $
��Left=�64   $��304,196,0,0q�6O   $	��Top=�6d   $��SF Pro Display)�6|   $��14(�6�   �� 5.   
   -
   5        $	 labelPassword��5.      $��Пароль��6'   $
��Left=�68   $��301,243,0,0q�6S   $	��Top=�6h   $��SF Pro Display)�6�   $��14(�6�   �� 5/   
   -   5         ��Name$ tbLogin��5/      $
��Left=�6#   $��392,200,0,0q�6>    �� TextWrapping$
 Wrap=�6S   $	��Top=�6g   $	��154��6   $��SF Pro Display)�6�   $��14(�6�   G� 50   
   -   5         G�Name$ 
pbPassword��50      $
��Left=�6*   $��392,247,0,0q�6E   $	��Top=�6Z   $	��154��6r   $��SF Pro Display)�6~   $��14(�6�   52      54      