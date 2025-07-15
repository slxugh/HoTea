using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace lab9.Forms
{
    /// <summary>
    /// Логика взаимодействия для TeaInOrder.xaml
    /// </summary>
    public partial class TeaInOrder : Window
    {
        public TeaInOrder(ТоварыВЗаказе teaInOrder, List<Чай> teaList)
        {   

            InitializeComponent();
            TeaInOrderForm.Title = "Редактирование товара в заказе";
            cbTea.ItemsSource  = teaList;
            cbTea.SelectedValuePath = "КодЧая";
            cbTea.DisplayMemberPath = "Название";
            cbTea.SelectedValue = teaInOrder.КодЧая;
            labelTeaInOrderID.Content = teaInOrder.КодТовараЗаказа;

            tbAmount.Text = teaInOrder.Количество.ToString();
            foreach (var item in teaList)
            {
                if (item.КодЧая == teaInOrder.КодЧая)
                {
                    labelPrice.Content = item.Цена;
                }
            }
            
        }

        public TeaInOrder(List<Чай> teaList)
        {
            InitializeComponent();
            TeaInOrderForm.Title = "Добавление нового товара в заказа";
            cbTea.ItemsSource = teaList;
            cbTea.SelectedValue = 1;
            cbTea.SelectedValuePath = "КодЧая";
            cbTea.DisplayMemberPath = "Название";
           

        }

        private void btnSave_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (tbAmount.Text != "")
            {
                DialogResult = true;
                Close();
            } else
            {
                MessageBox.Show("Поле количество не заполнено", "Ошибка");
            }
        }


        public ТоварыВЗаказе GetData()
        {
           
                ТоварыВЗаказе teaInOrder = new ТоварыВЗаказе();

                if (int.TryParse(labelTeaInOrderID.Content.ToString(), out int id))
                {
                    teaInOrder.КодТовараЗаказа = id;
                    teaInOrder.КодЧая = (int)cbTea.SelectedValue;
                    teaInOrder.Количество = int.Parse(tbAmount.Text);
                    teaInOrder.Цена = decimal.Parse(labelPrice.Content.ToString());
                    teaInOrder.Сумма = decimal.Parse(labelSum.Content.ToString());
                }
                else
                {   
                    teaInOrder.КодЧая = (int)cbTea.SelectedValue;
                    teaInOrder.Количество = int.Parse(tbAmount.Text);
                    teaInOrder.Цена = decimal.Parse(labelPrice.Content.ToString());
                    teaInOrder.Сумма = decimal.Parse(labelSum.Content.ToString());
                }
                return teaInOrder;
         


        }

        private void tbAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalcSum();
        }

       
        private void CalcSum()
        {
            try
            {
                labelSum.Content = decimal.Parse(labelPrice.Content.ToString(), NumberStyles.Any, CultureInfo.GetCultureInfo("ru-RU")) * int.Parse(tbAmount.Text);
            }
            catch (OverflowException) {
                MessageBox.Show("Слишком много", "Ошибка");
            }
            catch (FormatException)
            {

            }

        }


        
        private void tbAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void cbTea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var teaList = cbTea.ItemsSource as List<Чай>;
                foreach (var item in teaList)
                {
                    var tea = cbTea.SelectedItem as Чай;
                    if (tea.КодЧая == item.КодЧая)
                    {
                        labelPrice.Content = item.Цена;
                    }
                }
                CalcSum();
            }
            catch (FormatException) { }
        }
    }
}
