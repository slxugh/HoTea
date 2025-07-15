using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using lab9.Forms;

namespace lab9
{
    /// <summary>
    /// Логика взаимодействия для Order.xaml
    /// </summary>
    public partial class Order : Window
    {

        public List<Чай> teaListForEditOrder;
        public int orderId;
        public bool changed = false;
        public bool applyPromo = false;
        public Order(Заказ order, List<Клиент> clientsList, List<Филиал> branchesList, List<Чай> teaList)
        {
            var teaInOrder = GetListTeaInOrder(order.КодЗаказа);

            teaListForEditOrder = teaList;
            orderId = order.КодЗаказа;
            InitializeComponent();
            
            labelOrderID.Content = order.КодЗаказа.ToString();
            cbClient.ItemsSource = clientsList;
            cbBranch.ItemsSource = branchesList;
            
            cbClient.DisplayMemberPath = "ПолноеИмя";
            cbClient.SelectedValuePath = "КодКлиента";
            cbClient.SelectedValue = order.КодКлиента;
            cbBranch.DisplayMemberPath = "Название";
            cbBranch.SelectedValuePath = "КодФилиала";
            cbBranch.SelectedValue = order.КодФилиала;

            using (var context = new AppDbContext())
            {
                var promo = context.Акции.ToList();
                cbPromo.ItemsSource = promo;
                cbPromo.DisplayMemberPath = "Название";
                cbPromo.SelectedValuePath = "КодАкции";
            }
            dgDataInOrder.ItemsSource = teaInOrder;
          
            dpOrderDate.Text = order.ДатаЗаказа.ToString();

            refreshDg();
        }
        public Order(List<Клиент> clientsList, List<Филиал> branchesList, List<Чай> teaList)
        {
            InitializeComponent();
            btnAdd.Visibility = Visibility.Collapsed;
            btnDelete.Visibility = Visibility.Collapsed;
            cbPromo.Visibility = Visibility.Collapsed;
            btnPromo.Visibility = Visibility.Collapsed;
            teaListForEditOrder = teaList;
            Title = "Добавление нового заказа";
            cbClient.ItemsSource = clientsList;
            cbBranch.ItemsSource = branchesList;

            
            cbClient.DisplayMemberPath = "ПолноеИмя";
            cbClient.SelectedValuePath = "КодКлиента";
            cbClient.SelectedIndex = 0;
    
            cbBranch.DisplayMemberPath = "Название";
            cbBranch.SelectedValuePath = "КодФилиала";
            cbBranch.SelectedIndex = 0;
           

        }

        public List<ViewTeaInOrderModel> GetListTeaInOrder(int orderNumber)
        {
            using (var context = new AppDbContext())
            {
                var teaInOrderList = context.ТоварыВЗаказе
                    .Where(tz => tz.КодЗаказа == orderNumber)
                    .Include(tz => tz.Чай) 
                    .Select(tz => new ViewTeaInOrderModel
                    {   
                        КодТовараВЗаказе = tz.КодТовараЗаказа,
                        Чай = tz.Чай.Название, 
                        Количество = tz.Количество,
                        Цена = tz.Цена,
                        Сумма = tz.Сумма
                    })
                    .ToList();

                return teaInOrderList;
            }
        }


        public Заказ GetData()
        {
            try {
                Заказ order = new Заказ();

                if (int.TryParse((string)labelOrderID.Content, out int id))
                {
                    order.КодЗаказа = id;
                    order.КодКлиента = (int)cbClient.SelectedValue;
                    order.КодФилиала = (int)cbBranch.SelectedValue;
                    order.ДатаЗаказа = DateTime.Parse(dpOrderDate.Text);
                    order.ОбщаяСумма = decimal.Parse(labelOrderSum.Content.ToString());
                } 
                else { 
                
                    order.КодКлиента = (int)cbClient.SelectedValue;
                    order.КодФилиала = (int)cbBranch.SelectedValue;
                    order.ДатаЗаказа = DateTime.Parse(dpOrderDate.Text);
                    order.ОбщаяСумма = decimal.Parse(labelOrderSum.Content.ToString());
                }
                return order;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Ошибка");
                return null;
            }


        }

       

        

        private void dgDataInOrder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var hit = e.OriginalSource as FrameworkElement;

            while (hit != null && !(hit is DataGridColumnHeader))
            {
                hit = VisualTreeHelper.GetParent(hit) as FrameworkElement;
            }

            if (hit is DataGridColumnHeader)
            {


            }
            else
            {
                var dg = sender as System.Windows.Controls.DataGrid;
                var objType = "";
                
                    objType = dg.SelectedItem.GetType().Name;
                    var selectedTeaInOrder = dg.SelectedItem as ViewTeaInOrderModel;
                    ТоварыВЗаказе teaInOrderToEdit = null;
                    using (var context = new AppDbContext())
                    {

                        teaInOrderToEdit = context.ТоварыВЗаказе
                        .Include(o => o.Чай)
                        .FirstOrDefault(o => o.КодТовараЗаказа == selectedTeaInOrder.КодТовараВЗаказе);


                        if (teaInOrderToEdit == null) return;

                        /*System.Windows.MessageBox.Show(objType);*/
                        TeaInOrder teaInOrder = new TeaInOrder(teaInOrderToEdit, teaListForEditOrder);
                        teaInOrder.Owner = this;
                        teaInOrder.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    if (teaInOrder.ShowDialog() == true)
                        {
                            var tea = teaInOrder.GetData();
                            if (tea != null)
                            {
                                var teaToUpdate = context.ТоварыВЗаказе.Find(tea.КодТовараЗаказа);
                                var teaStock = context.Чай.Find(tea.КодЧая);

                                if (teaStock == null || teaStock.Остаток + teaInOrderToEdit.Количество < tea.Количество)
                                {
                                    System.Windows.MessageBox.Show("Недостаточно товара на складе!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                                teaStock.Остаток += teaInOrderToEdit.Количество - tea.Количество;

                                if ( teaToUpdate != null )
                                {
                                    teaToUpdate.КодЧая = tea.КодЧая;
                                    teaToUpdate.КодЗаказа = orderId;
                                    teaToUpdate.Количество = tea.Количество;
                                    teaToUpdate.Сумма = tea.Сумма;
                                    teaToUpdate.Цена = tea.Цена;
                                    int affectedRows = context.SaveChanges();
                                    refreshDg();
                                    changed = true;
                                    System.Windows.MessageBox.Show($"Запись изменена!\nЗатронуто строк: {affectedRows}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                {
                                    refreshDg();

                                    System.Windows.MessageBox.Show("Товар не найден в бд", "Ошибка");
                                }
                            }
                        }
                    }
                

            }
        }

        
        private void btnSave_MouseDown(object sender, MouseButtonEventArgs e)
        {
                changed = false;
                DialogResult = true;
                this.Close();
        
        }

        private void refreshDg()
        {
            decimal sum = 0;
            var teaInOrderList = GetListTeaInOrder(orderId);
            foreach (var item in teaInOrderList)
            { 
                    sum += item.Сумма;
            }
            labelOrderSum.Content = sum;
            dgDataInOrder.ItemsSource = null;
            dgDataInOrder.ItemsSource = GetListTeaInOrder(orderId);
        }

        private void btnAdd_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TeaInOrder teaInOrder = new TeaInOrder(teaListForEditOrder);
            teaInOrder.Owner = this;
            teaInOrder.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (teaInOrder.ShowDialog() == true)
            {
                ТоварыВЗаказе newTea = teaInOrder.GetData();
                newTea.КодЗаказа = orderId;
                if (newTea != null)
                {
                    using (var context = new AppDbContext())
                    {
                        var teaStock = context.Чай.Find(newTea.КодЧая);
                        if (teaStock == null || teaStock.Остаток < newTea.Количество)
                        {
                            System.Windows.MessageBox.Show("Недостаточно товара на складе!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        teaStock.Остаток -= newTea.Количество;
                        context.ТоварыВЗаказе.Add(newTea);
                        context.SaveChanges();
                        refreshDg();
                        System.Windows.MessageBox.Show("Новый товар добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        changed = true;
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Новый товар не добавлен!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
           
        }

        private void btnDelete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var obj = dgDataInOrder.SelectedItem as ViewTeaInOrderModel;

            ТоварыВЗаказе orderToDelete = null;
            using (var context = new AppDbContext())
            {
                orderToDelete = context.ТоварыВЗаказе.Find(obj.КодТовараВЗаказе);
                var teaStock = context.Чай.Find(orderToDelete.КодЧая);
                if (orderToDelete != null && teaStock != null)
                {
                    // Восстанавливаем остаток на складе
                    teaStock.Остаток += orderToDelete.Количество;

                    context.ТоварыВЗаказе.Remove(orderToDelete);
                    context.SaveChanges();
                    refreshDg();
                    System.Windows.MessageBox.Show("Товар удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    System.Windows.MessageBox.Show("Товар в заказе не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (changed)
            {   
                e.Cancel = true;
                System.Windows.Forms.MessageBox.Show("Внесены изменения. Необходимо сохранить данные", "Предупреждение");
            }
            else
            {
               e.Cancel = false;
            }
        }

        private void btnPromo_MouseDown(object sender, MouseButtonEventArgs e)
        {   
            if (applyPromo)
            {
                System.Windows.MessageBox.Show("Промокод уже применен", "Ошибка");
                return;
            }
            using (var context = new AppDbContext())
            {
                var promoId = int.Parse(cbPromo.SelectedValue.ToString());
                var promo = context.Акции.Where(a => a.КодАкции == promoId)
                                  .Select(a => a.ПроцентСкидки)
                                  .FirstOrDefault();

                var teaInOrder = context.ТоварыВЗаказе.Where(p => p.КодЗаказа == orderId).ToList();

                foreach (var tea in teaInOrder) { 
                    tea.Сумма *= (1 - promo / 100.0m);
                    Console.WriteLine($"Цена изменена: {tea.Сумма}");
                }
                context.SaveChanges();
                refreshDg();
                applyPromo = true;  
            }
        }
    }
}
