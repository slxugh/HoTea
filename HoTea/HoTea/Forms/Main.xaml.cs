
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using lab9.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace lab9
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Сотрудник employee;
        public AppDbContext context;
        public List<string> listTablesForAdmin = new List<string>()
        {
            "Клиенты",
            "Заказы",
            "Поставщики",
            "Чаи",
            "Типы чая",
            "Филиалы",
            "Акции",
            "Отзывы",
            

        };
        public List<string> roles = new List<string>()
        {
            "Администратор",
            "Менеджер"
        };

        public List<string> listTablesForStaff = new List<string>()
        {
            "Клиенты",
            "Заказы",
            "Поставщики",
            "Чаи",
            "Филиалы",
            "Акции",
            "Отзывы",
            

        };
        public Main(Сотрудник emp)
        {
            employee = emp;
            InitializeComponent();
         
            context = new AppDbContext();
            labelInfo.Content = $"Роль: {employee.Роль}";
            if (employee.Роль == "Менеджер")
            {
                btnEmp.Visibility = Visibility.Collapsed;
            }

            lbTables.ItemsSource = listTablesForAdmin;

        }

        public (List<Клиент>, List<Филиал>, List<Чай>) GetDataForEditOrders()
        {
            var clientsList = context.Клиенты.ToList();
            var branchesList = context.Филиалы.ToList();
            var teaList = context.Чай.ToList();
            return (clientsList, branchesList, teaList);

        }

        public (List<ТипЧая>, List<Поставщик>) GetDataForEditTea()
        {
            var typesTeaList = context.ТипыЧая.ToList();
            var suppliersList = context.Поставщики.ToList();
            return (typesTeaList, suppliersList);
        }

        public (List<Клиент>, List<Чай>) GetDataForEditFeedback()
        {
            var clientsList = context.Клиенты.ToList();
            var teaList = context.Чай.ToList();
            return (clientsList, teaList);
        }


        public void resetDg()
        {
            dgData.ItemsSource = null;

        }
        public void refreshDg()
        {
            if ((string)lbTables.SelectedItem == "Клиенты")
            {
                resetDg();
                var clientsData = context.Клиенты
                                .AsEnumerable()
                                .Select(o => new ViewClientModel
                                {
                                    КодКлиента = o.КодКлиента,
                                    ФИОКлиента = o.ПолноеИмя,
                                    Телефон = o.Телефон ,
                                    Почта = o.Почта

                                })
                                .ToList();
                dgData.ItemsSource = clientsData;
                dgData.Columns[0].Visibility = Visibility.Collapsed;
            }
            else if ((string)lbTables.SelectedItem == "Заказы")
            {
                resetDg();
                var ordersData = context.Заказы
                                .Include(o => o.Клиент)
                                .Include(o => o.Филиал)
                                .AsEnumerable()
                                .Select(o => new ViewOrderModel
                                {
                                    КодЗаказа = o.КодЗаказа,
                                    ФИОКлиента = o.Клиент.ПолноеИмя.ToString(),
                                    НазваниеФилиала = o.Филиал.Название.ToString(),
                                    ДатаЗаказа = o.ДатаЗаказа,
                                    ОбщаяСумма = o.ОбщаяСумма
                                })
                                .ToList();

                
                dgData.ItemsSource = ordersData;
          

            }
            else if ((string)lbTables.SelectedItem == "Типы чая")
            {
                resetDg();
                var typesTeaData = context.ТипыЧая
                              .AsEnumerable()
                              .Select(o => new ViewTypesTeaModel
                              {
                                  КодТипЧая = o.КодТипЧая,
                                  Название = o.Название
                              })
                              .ToList();
                dgData.ItemsSource = typesTeaData;
                dgData.Columns[0].Visibility = Visibility.Collapsed;

            }
            else if ((string)lbTables.SelectedItem == "Поставщики")
            {
                resetDg();
                var supplierData = context.Поставщики
                               .AsEnumerable()
                               .Select(o => new ViewSupplierModel
                               {
                                   КодПоставщика = o.КодПоставщика,
                                   Название = o.Название,
                                   Страна = o.Страна,
                                   Телефон = o.Телефон
                               })
                               .ToList();
                dgData.ItemsSource = supplierData;
                dgData.Columns[0].Visibility = Visibility.Collapsed;

            }
            else if ((string)lbTables.SelectedItem == "Чаи")
            {
                resetDg();
                var teaData = context.Чай
                               .Include(o => o.Поставщик)
                               .Include(o => o.ТипЧая)
                               .AsEnumerable()
                               .Select(o => new ViewTeaModel
                               {
                                   КодЧая = o.КодЧая,
                                   Название = o.Название,
                                   ТипЧая = o.ТипЧая.Название,
                                   Поставщик = o.Поставщик.Название,
                                   Описание = o.Описание,
                                   Цена = o.Цена,
                                   Остаток = o.Остаток,
                               })
                               .ToList();
                dgData.ItemsSource = teaData;
                dgData.Columns[0].Visibility = Visibility.Collapsed;

            }
            else if ((string)lbTables.SelectedItem == "Филиалы")
            {
                resetDg();
                var branchesData = context.Филиалы
                               .AsEnumerable()
                               .Select(o => new ViewBranchesModel
                               {
                                   КодФилиала = o.КодФилиала,
                                   Название = o.Название,
                                   Город = o.Город,
                                   Телефон = o.НомерТелефона,
                                   Адрес = o.Адрес
                               })
                               .ToList();
                dgData.ItemsSource = branchesData;
                dgData.Columns[0].Visibility = Visibility.Collapsed;

            }
            else if ((string)lbTables.SelectedItem == "Акции")
            {
                resetDg();
                dgData.ItemsSource = context.Акции.ToList();
                dgData.Columns[0].Visibility = Visibility.Collapsed;

            }
            else if ((string)lbTables.SelectedItem == "Отзывы")
            {
                resetDg();
                var feedbackData = context.Отзывы
                                .Include(o => o.Клиент)
                                .Include(o => o.Чай)
                                .AsEnumerable()
                                .Select(o => new ViewFeedbackModel
                                {
                                    КодОтзыва = o.КодОтзыва,
                                    ФИОКлиента = o.Клиент.ПолноеИмя,
                                    НазваниеЧая = o.Чай.Название,
                                    Оценка = o.Оценка,
                                    Комментарий = o.Комментарий,
                                    ДатаОтзыва = o.ДатаОтзыва,

                                })
                                .ToList();
                dgData.ItemsSource = feedbackData;
                dgData.Columns[0].Visibility = Visibility.Collapsed;

            }
            UpdateDgCount();
           
        }

        private void lbTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            labelStart.Visibility = Visibility.Collapsed;

            if ((string)lbTables.SelectedItem == "Клиенты")
            {
                if (employee.Роль == "Менеджер")
                {
                    HideBtn(false);
                }

                try
                {
                    resetDg();
                    var clientsData = context.Клиенты
                                .AsEnumerable()
                                .Select(o => new ViewClientModel
                                {
                                    КодКлиента = o.КодКлиента,
                                    ФИОКлиента = o.ПолноеИмя,
                                    Телефон = o.Телефон,
                                    Почта = o.Почта

                                })
                                .ToList();
                    dgData.ItemsSource = clientsData;
                    dgData.Columns[0].Visibility = Visibility.Collapsed;


                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                    System.Windows.MessageBox.Show(ex.InnerException.Message);
                }

            }
            else if ((string)lbTables.SelectedItem == "Заказы")
            {
                if (employee.Роль == "Менеджер")
                {
                    HideBtn(false);
                }
                try
                {
                    resetDg();
                    var ordersData = context.Заказы
                                .Include(o => o.Клиент)
                                .Include(o => o.Филиал)
                                .AsEnumerable()
                                .Select(o => new ViewOrderModel
                                {
                                    КодЗаказа = o.КодЗаказа,
                                    ФИОКлиента = o.Клиент.ПолноеИмя.ToString(),
                                    НазваниеФилиала = o.Филиал.Название.ToString(),
                                    ДатаЗаказа = o.ДатаЗаказа,
                                    ОбщаяСумма = o.ОбщаяСумма
                                })
                                .ToList();
                    dgData.ItemsSource = ordersData;
                    
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }

            }
            else if ((string)lbTables.SelectedItem == "Типы чая")
            {
                if (employee.Роль == "Менеджер")
                {
                    HideBtn(true);
                }
                try
                {
                    resetDg();
                    var typesTeaData = context.ТипыЧая
                               .AsEnumerable()
                               .Select(o => new ViewTypesTeaModel
                               {
                                   КодТипЧая = o.КодТипЧая,
                                   Название = o.Название
                               })
                               .ToList();
                    dgData.ItemsSource = typesTeaData;
                    dgData.Columns[0].Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
            else if ((string)lbTables.SelectedItem == "Поставщики")
            {
                if (employee.Роль == "Менеджер")
                {
                    HideBtn(true);
                }
                try
                {
                    resetDg();
                    var supplierData = context.Поставщики
                               .AsEnumerable()
                               .Select(o => new ViewSupplierModel
                               {
                                   КодПоставщика = o.КодПоставщика,
                                   Название = o.Название,
                                   Страна = o.Страна,
                                   Телефон = o.Телефон
                               })
                               .ToList();
                    dgData.ItemsSource = supplierData;
                    dgData.Columns[0].Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
            else if ((string)lbTables.SelectedItem == "Чаи")
            {
                if (employee.Роль == "Менеджер")
                {
                    HideBtn(true);
                }
                try
                {
                    resetDg();
                    var teaData = context.Чай
                                .Include(o => o.Поставщик)
                                .Include(o => o.ТипЧая)
                                .AsEnumerable()
                                .Select(o => new ViewTeaModel
                                {   
                                    КодЧая = o.КодЧая,
                                    Название = o.Название,
                                    ТипЧая = o.ТипЧая.Название,
                                    Поставщик = o.Поставщик.Название,
                                    Описание = o.Описание,
                                    Цена = o.Цена,
                                    Остаток = o.Остаток,
                                })
                                .ToList();
                    dgData.ItemsSource = teaData;
                    dgData.Columns[0].Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
            else if ((string)lbTables.SelectedItem == "Филиалы")
            {
                if (employee.Роль == "Менеджер")
                {
                    HideBtn(true);
                }
                try
                {
                    resetDg();
                    var branchesData = context.Филиалы
                              .AsEnumerable()
                              .Select(o => new ViewBranchesModel
                              {
                                  КодФилиала = o.КодФилиала,
                                  Название = o.Название,
                                  Город = o.Город,
                                  Телефон = o.НомерТелефона,
                                  Адрес = o.Адрес
                              })
                              .ToList();
                    dgData.ItemsSource = branchesData;
                    dgData.Columns[0].Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
            else if ((string)lbTables.SelectedItem == "Акции")
            {
                if (employee.Роль == "Менеджер")
                {
                    HideBtn(true);
                }
                try
                {
                    resetDg();
                    var stocksData = context.Акции.ToList();
                    dgData.ItemsSource = stocksData;
                    dgData.Columns[0].Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
            else if ((string)lbTables.SelectedItem == "Отзывы")
            {
                if (employee.Роль == "Менеджер")
                {
                    HideBtn(true);
                }
                try
                {
                    resetDg();
                    var feedbackData = context.Отзывы
                                .Include(o => o.Клиент)
                                .Include(o => o.Чай)
                                .AsEnumerable()
                                .Select(o => new ViewFeedbackModel
                                {   
                                    КодОтзыва = o.КодОтзыва,
                                    ФИОКлиента = o.Клиент.ПолноеИмя,
                                    НазваниеЧая = o.Чай.Название,
                                    Оценка = o.Оценка,
                                    Комментарий = o.Комментарий,
                                    ДатаОтзыва = o.ДатаОтзыва,

                                })
                                .ToList();
                    dgData.ItemsSource = feedbackData;
                    dgData.Columns[0].Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
            UpdateDgCount();
           
        }

        private void dgData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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
                try
                {
                    objType = dg.SelectedItem.GetType().Name;
                    /*System.Windows.MessageBox.Show(objType);*/
                } catch
                {

                }
                if (objType == "ViewClientModel")
                {
                    var selectedСlient = dg.SelectedItem as ViewClientModel;
                    var clientToEdit = context.Клиенты.FirstOrDefault(o => o.КодКлиента == selectedСlient.КодКлиента);
                    Clients clients = new Clients(clientToEdit);
                    clients.Owner = this;
                    clients.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    if (clients.ShowDialog() == true)
                    {
                        var client = clients.GetData();
                        if (client != null)
                        {
                            var clientToUpdate = context.Клиенты.Find(client.КодКлиента);
                            if (clientToUpdate != null)
                            {
                                // Обновляем поля
                                clientToUpdate.ПолноеИмя = client.ПолноеИмя;
                                clientToUpdate.Почта = client.Почта;
                                clientToUpdate.Телефон = client.Телефон;

                                // Сохраняем изменения в базе данных
                                int affectedRows = context.SaveChanges();
                                refreshDg();
                               System.Windows.MessageBox.Show($"Запись изменена!\nЗатронуто строк: {affectedRows}", "Успех", MessageBoxButton.OK,MessageBoxImage.Information);
                            }
                        }
                        else
                        {
                             System.Windows.MessageBox.Show("Запись не изменена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }


                }
                else if (objType == "ViewOrderModel")
                {   

                    var selectedOrder = dg.SelectedItem as ViewOrderModel;
                    var orderToEdit = context.Заказы
                        .Include(o => o.Клиент)
                        .Include(o => o.Филиал)
                        .FirstOrDefault(o => o.КодЗаказа == selectedOrder.КодЗаказа);

                    if (orderToEdit == null) return;

                    var dataForEditOrder = GetDataForEditOrders();
                    Order orders = new Order(orderToEdit, dataForEditOrder.Item1, dataForEditOrder.Item2, dataForEditOrder.Item3);
                    orders.Owner = this;
                    orders.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    if (orders.ShowDialog() == true)
                    {
                        var order = orders.GetData();
                        if (order != null)
                        {

                            var orderToUpdate = context.Заказы.Find(order.КодЗаказа);
                            if (orderToUpdate != null)
                            {

                                orderToUpdate.ДатаЗаказа = order.ДатаЗаказа;
                                orderToUpdate.ОбщаяСумма = order.ОбщаяСумма;
                                orderToUpdate.КодКлиента = order.КодКлиента;
                                orderToUpdate.КодФилиала = order.КодФилиала;
                                int affectedRows =  context.SaveChanges();
                                refreshDg();
                               System.Windows.MessageBox.Show($"Запись изменена!\nЗатронуто строк: {affectedRows}", "Успех", MessageBoxButton.OK,MessageBoxImage.Information);
                            }
                        }
                        else
                        {
                             System.Windows.MessageBox.Show("Запись не изменена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else if (objType == "ViewTypesTeaModel")
                {
                    if (employee.Роль == "Менеджер")
                    {
                        System.Windows.MessageBox.Show("Недостаточно прав", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Hand);
                        return;
                    }
                    var selectedTypesTea = dg.SelectedItem as ViewTypesTeaModel;
                    var typesTeaToEdit = context.ТипыЧая.FirstOrDefault(o => o.КодТипЧая == selectedTypesTea.КодТипЧая);
                    TypesTea typesTeas = new TypesTea(typesTeaToEdit);
                    typesTeas.Owner = this;
                    typesTeas.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    if (typesTeas.ShowDialog() == true)
                    {
                        var typesTea = typesTeas.GetData();
                        if (typesTea != null)
                        {

                            var typesTeaToUpdate = context.ТипыЧая.Find(typesTea.КодТипЧая);
                            if (typesTeaToUpdate != null)
                            {

                                typesTeaToUpdate.Название = typesTea.Название;
                                int affectedRows = context.SaveChanges();
                                refreshDg();
                               System.Windows.MessageBox.Show($"Запись изменена!\nЗатронуто строк: {affectedRows}", "Успех", MessageBoxButton.OK,MessageBoxImage.Information);
                            }
                        }
                        else
                        {
                             System.Windows.MessageBox.Show("Запись не изменена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else if (objType == "ViewSupplierModel")
                {
                    if (employee.Роль == "Менеджер")
                    {
                        System.Windows.MessageBox.Show("Недостаточно прав", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Hand);
                        return;
                    }
                    var selectedSupplier = dg.SelectedItem as ViewSupplierModel;
                    var clientToEdit = context.Поставщики.FirstOrDefault(o => o.КодПоставщика == selectedSupplier.КодПоставщика);
                    Supplier suppliers = new Supplier(clientToEdit);
                    suppliers.Owner = this;
                    suppliers.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    if (suppliers.ShowDialog() == true)
                    {
                        var supplier = suppliers.GetData();
                        if (supplier != null)
                        {

                            var supplierToUpdate = context.Поставщики.Find(supplier.КодПоставщика);
                            if (supplierToUpdate != null)
                            {
                                // Обновляем поля
                                supplierToUpdate.Название = supplier.Название;
                                supplierToUpdate.Телефон = supplier.Телефон;
                                supplierToUpdate.Страна = supplier.Страна;

                                // Сохраняем изменения в базе данных
                                context.SaveChanges();
                                refreshDg();
                               System.Windows.MessageBox.Show("Запись изменена!", "Успех", MessageBoxButton.OK,MessageBoxImage.Information);
                            }
                        }
                        else
                        {
                             System.Windows.MessageBox.Show("Запись не изменена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else if (objType == "ViewTeaModel")
                {
                    if (employee.Роль == "Менеджер")
                    {
                        System.Windows.MessageBox.Show("Недостаточно прав", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Hand);
                        return;
                    }
                    var selectedTea = dg.SelectedItem as ViewTeaModel;
                    var teaToEdit = context.Чай
                        .Include(o => o.Поставщик)
                        .Include(o => o.ТипЧая)
                        .FirstOrDefault(o => o.КодЧая == selectedTea.КодЧая);
                    if (teaToEdit == null) return;
                    var dataForEditTea = GetDataForEditTea();

                    AllTea allTea = new AllTea(teaToEdit, dataForEditTea.Item1, dataForEditTea.Item2);
                    allTea.Owner = this;
                    allTea.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    if (allTea.ShowDialog() == true)
                    {
                        var tea = allTea.GetData();
                        if (tea != null)
                        {

                            var teaToUpdate = context.Чай.Find(tea.КодЧая);
                            if (teaToUpdate != null)
                            {
                                
                                teaToUpdate.Название = tea.Название;
                                teaToUpdate.КодПоставщика = tea.КодПоставщика;
                                teaToUpdate.КодТипЧая = tea.КодТипЧая;
                                teaToUpdate.Описание = tea.Описание;
                                teaToUpdate.Цена = tea.Цена;
                                teaToUpdate.Остаток = tea.Остаток;

                                // Сохраняем изменения в базе данных
                                int affectedRows = context.SaveChanges();
                                refreshDg();
                                System.Windows.MessageBox.Show($"Запись изменена!\nЗатронуто строк: {affectedRows}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Запись не изменена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else if (objType == "ViewBranchesModel")
                {
                    if (employee.Роль == "Менеджер")
                    {
                        System.Windows.MessageBox.Show("Недостаточно прав", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Hand);
                        return;
                    }
                    var selectedBranch = dg.SelectedItem as ViewBranchesModel;
                    var branchToEdit = context.Филиалы.FirstOrDefault(o => o.КодФилиала == selectedBranch.КодФилиала);

                    Branches branches = new Branches(branchToEdit);
                    branches.Owner = this;
                    branches.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    if (branches.ShowDialog() == true)
                    {
                        var branch = branches.GetData();
                        if (branch != null)
                        {

                            var branchToUpdate = context.Филиалы.Find(branch.КодФилиала);
                            if (branchToUpdate != null)
                            {
                               
                                branchToUpdate.Название = branch.Название;
                                branchToUpdate.Адрес = branch.Адрес;
                                branchToUpdate.Город = branch.Город;
                                branchToUpdate.НомерТелефона = branch.НомерТелефона;

                                
                                int affectedRows = context.SaveChanges();
                                refreshDg();
                               System.Windows.MessageBox.Show($"Запись изменена!\nЗатронуто строк: {affectedRows}", "Успех", MessageBoxButton.OK,MessageBoxImage.Information);
                            }
                        }
                        else
                        {
                             System.Windows.MessageBox.Show("Запись не изменена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                }
                else if (objType == "Акция")
                {
                    if (employee.Роль == "Менеджер")
                    {
                        System.Windows.MessageBox.Show("Недостаточно прав", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Hand);
                        return;
                    }
                    var stock = dg.SelectedItem as Акция;
                    Stocks stocks = new Stocks(stock);
                    stocks.Owner = this;
                    stocks.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    if (stocks.ShowDialog() == true)
                    {
                        stock = stocks.GetData();
                        if (stock != null)
                        {

                            var stockToUpdate = context.Акции.Find(stock.КодАкции);
                            if (stockToUpdate != null)
                            {
                                // Обновляем поля
                                stockToUpdate.Название = stockToUpdate.Название;
                                stockToUpdate.ДатаНачала = stock.ДатаНачала;
                                stockToUpdate.ДатаОкончания = stock.ДатаОкончания;
                                stockToUpdate.ПроцентСкидки = stock.ПроцентСкидки;


                                // Сохраняем изменения в базе данных
                                context.SaveChanges();
                                refreshDg();
                               System.Windows.MessageBox.Show("Запись изменена!", "Успех", MessageBoxButton.OK,MessageBoxImage.Information);
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Запись не изменена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else if (objType == "ViewFeedbackModel")
                {
                    if (employee.Роль == "Менеджер")
                    {
                        System.Windows.MessageBox.Show("Недостаточно прав", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Hand);
                        return;
                    }
                    var selectedFeedback = dg.SelectedItem as ViewFeedbackModel;
                    var feedbackToEdit = context.Отзывы
                        .Include(o => o.Клиент)
                        .Include(o => o.Чай)
                        .FirstOrDefault(o => o.КодОтзыва == selectedFeedback.КодОтзыва);
                    if (feedbackToEdit == null) return;
                    var dataForEditTea = GetDataForEditFeedback();
                    Feedback feedbacks = new Feedback(feedbackToEdit, dataForEditTea.Item1, dataForEditTea.Item2);
                    feedbacks.Owner = this;
                    feedbacks.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    if (feedbacks.ShowDialog() == true)
                    {
                        var feedback = feedbacks.GetData();
                        if (feedback != null)
                        {

                            var feedbackToUpdate = context.Отзывы.Find(feedback.КодОтзыва);
                            if (feedbackToUpdate != null)
                            {
                                feedbackToUpdate.КодКлиента = feedback.КодКлиента;
                                feedbackToUpdate.КодЧая = feedback.КодЧая;
                                feedbackToUpdate.Оценка = feedback.Оценка;
                                feedbackToUpdate.Комментарий = feedback.Комментарий;
                                feedbackToUpdate.ДатаОтзыва = feedback.ДатаОтзыва;


                                int affectedRows = context.SaveChanges();
                                refreshDg();
                               System.Windows.MessageBox.Show($"Запись изменена!\nЗатронуто строк: {affectedRows}", "Успех", MessageBoxButton.OK,MessageBoxImage.Information);
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Запись не изменена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }



        }
        public void HideBtn(bool hide)
        {
            if (hide)
            {

                btnCreate.Visibility = Visibility.Collapsed;
                btnDelete.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnCreate.Visibility = Visibility.Visible;
                btnDelete.Visibility = Visibility.Visible;
            }
        }
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
         


                if ((string)lbTables.SelectedItem == "Клиенты")
                {
                   
                    
                Clients clients = new Clients();
                clients.Owner = this;
                clients.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                if (clients.ShowDialog() == true)
                    {
                        Клиент newClient = clients.GetData();
                        if (newClient != null)
                        {
                            context.Клиенты.Add(newClient);
                            context.SaveChanges();
                            refreshDg();
                            System.Windows.MessageBox.Show("Запись добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        } 
                        else
                        {
                           System.Windows.MessageBox.Show("Запись не добавлена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    } 
                } 
                else if ((string)lbTables.SelectedItem == "Заказы")
                {    
                    Заказ newOrder = new Заказ();
                    var dataForEditOrder = GetDataForEditOrders();
                    Order order = new Order(dataForEditOrder.Item1, dataForEditOrder.Item2, dataForEditOrder.Item3);
                    order.Owner = this;
                    order.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    if (order.ShowDialog() == true)
                        {
                            newOrder = order.GetData();
                            if (newOrder != null)
                            {
                                context.Заказы.Add(newOrder);
                                context.SaveChanges();
                                refreshDg();
                                System.Windows.MessageBox.Show("Запись добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("Запись не добавлена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            }

                        }
                }
                else if ((string)lbTables.SelectedItem == "Типы чая")
                {   
                    
                    ТипЧая newTypeTea = new ТипЧая();
                    TypesTea typeTea = new TypesTea();
                typeTea.Owner = this;
                typeTea.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                if (typeTea.ShowDialog() == true)
                    {
                        newTypeTea = typeTea.GetData();
                        if (newTypeTea != null)
                        {
                            context.ТипыЧая.Add(newTypeTea);
                            context.SaveChanges();
                            refreshDg();
                            System.Windows.MessageBox.Show("Запись добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                           System.Windows.MessageBox.Show("Запись не добавлена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                

                }
                else if ((string)lbTables.SelectedItem == "Поставщики")
                {
                    Поставщик newSupplier = new Поставщик();
                    Supplier supplier = new Supplier();
                supplier.Owner = this;
                supplier.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                if (supplier.ShowDialog() == true)
                    {
                        newSupplier = supplier.GetData();
                        if (newSupplier != null)
                        {
                            context.Поставщики.Add(newSupplier);
                            context.SaveChanges();
                            refreshDg();
                            System.Windows.MessageBox.Show("Запись добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                           System.Windows.MessageBox.Show("Запись не добавлена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
        
                }
                else if ((string)lbTables.SelectedItem == "Чаи")
                {
                    Чай newTea = new Чай();
                    var dataForEditTea = GetDataForEditTea();
                    AllTea allTea = new AllTea(dataForEditTea.Item1, dataForEditTea.Item2);
                allTea.Owner = this;
                allTea.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                if (allTea.ShowDialog() == true)
                    {
                        newTea = allTea.GetData();
                        if (newTea != null)
                        {
                            context.Чай.Add(newTea);
                            context.SaveChanges();
                            refreshDg();
                            System.Windows.MessageBox.Show("Запись добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                           System.Windows.MessageBox.Show("Запись не добавлена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                }
                else if ((string)lbTables.SelectedItem == "Филиалы")
                {
                    Филиал newBranch = new Филиал();
                    Branches branch = new Branches();
                branch.Owner = this;
                branch.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                if (branch.ShowDialog() == true)
                    {
                        newBranch = branch.GetData();
                        if (newBranch != null)
                        {
                            context.Филиалы.Add(newBranch);
                            context.SaveChanges();
                            refreshDg();
                            System.Windows.MessageBox.Show("Запись добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                           System.Windows.MessageBox.Show("Запись не добавлена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                }
                else if ((string)lbTables.SelectedItem == "Акции")
                {
                    Акция newStock = new Акция();
                    Stocks stock = new Stocks();
                stock.Owner = this;
                stock.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                if (stock.ShowDialog() == true)
                    {
                        newStock = stock.GetData();
                        if (newStock != null)
                        {
                            context.Акции.Add(newStock);
                            context.SaveChanges();
                            refreshDg();
                            System.Windows.MessageBox.Show("Запись добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                           System.Windows.MessageBox.Show("Запись не добавлена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
    
                }
                else if ((string)lbTables.SelectedItem == "Отзывы")
                {
                    Отзыв newFeedback = new Отзыв();
                    var dataForEditFeedback = GetDataForEditFeedback();
                    Feedback feedback = new Feedback(dataForEditFeedback.Item1, dataForEditFeedback.Item2);
                feedback.Owner = this;
                feedback.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                if (feedback.ShowDialog() == true)
                    {
                     newFeedback = feedback.GetData();
                        if (newFeedback != null)
                        {
                            context.Отзывы.Add(newFeedback);
                            context.SaveChanges();
                            refreshDg();
                            System.Windows.MessageBox.Show("Запись добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Запись не добавлена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                }


        }

        public void UpdateDgCount()
        {
            int count = 0;
            foreach (var t in dgData.Items)
            {
                count++;
            }
            labelItemCount.Content = $"Количество записей: {count}";
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {   
            
            Delete();
            refreshDg();


        }

        private bool HasDependencies(string table, int id)
        {
            switch (table)
            {
                case "Клиенты":
                    return context.Заказы.Any(o => o.КодКлиента == id) ||
                           context.Отзывы.Any(r => r.КодКлиента == id);

                case "Филиалы":
                    return context.Заказы.Any(o => o.КодФилиала == id);

                case "ТипыЧая":
                    return context.Чай.Any(t => t.КодТипЧая == id);

                case "Чай":
                    return context.Отзывы.Any(r => r.КодЧая == id) ||
                           context.ТоварыВЗаказе.Any(tz => tz.КодЧая == id);

                case "Поставщики":
                    return context.Чай.Any(t => t.КодПоставщика == id);

                default:
                    return true; 
            }
        }
        private void Delete()
        {
            if (dgData.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Выберете запись для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if ((string)lbTables.SelectedItem == "Клиенты")
            {
                var obj = dgData.SelectedItem as ViewClientModel;
                // Находим продукт по Id
                var clientToDelete = context.Клиенты.Find(obj.КодКлиента);

                if (HasDependencies("Клиенты", obj.КодКлиента))
                {
                    System.Windows.MessageBox.Show($"Невозможно удалить запись из клиенты: есть связанные данные.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (clientToDelete != null)
                {
                    context.Клиенты.Remove(clientToDelete);
                    
                    context.SaveChanges();
                    System.Windows.MessageBox.Show("Успешное удаление", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Обработка случая, когда продукт не найден
                    System.Windows.MessageBox.Show("Клиент не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if ((string)lbTables.SelectedItem == "Заказы")
            {
                var obj = dgData.SelectedItem as ViewOrderModel;
                // Находим продукт по Id
                var orderToDelete = context.Заказы.Find(obj.КодЗаказа);

                if (orderToDelete != null)
                {
                    // Получаем все товары, входящие в заказ, группируем их по чаю
                    var teaQuantityAdjustments = context.ТоварыВЗаказе
                        .Where(tz => tz.КодЗаказа == orderToDelete.КодЗаказа)
                        .GroupBy(tz => tz.КодЧая)
                        .Select(g => new { КодЧая = g.Key, Количество = g.Sum(tz => tz.Количество) })
                        .ToList();

                    // Корректируем остатки на складе ОДИН раз
                    foreach (var item in teaQuantityAdjustments)
                    {
                        var teaStock = context.Чай.Find(item.КодЧая);
                        if (teaStock != null)
                        {
                            teaStock.Остаток += item.Количество; // Возвращаем ровно столько, сколько было в заказе
                        }
                    }

                    // Удаляем заказ (каскадно удалит связанные товары)
                    context.Заказы.Remove(orderToDelete);
                    context.SaveChanges();

                    System.Windows.MessageBox.Show("Заказ успешно удален, остатки обновлены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    refreshDg();
                }
                else
                {
                    System.Windows.MessageBox.Show("Заказ не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else if ((string)lbTables.SelectedItem == "Типы чая")
            {
                var obj = dgData.SelectedItem as ViewTypesTeaModel;
                // Находим продукт по Id
                var typeTeaToDelete = context.ТипыЧая.Find(obj.КодТипЧая);
                if (HasDependencies("ТипыЧая", obj.КодТипЧая))
                {
                    System.Windows.MessageBox.Show($"Невозможно удалить запись из типы чая: есть связанные данные.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (typeTeaToDelete != null)
                {
                    context.ТипыЧая.Remove(typeTeaToDelete);

                    context.SaveChanges();
                    System.Windows.MessageBox.Show("Успешное удаление", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Обработка случая, когда продукт не найден
                    System.Windows.MessageBox.Show("Тип чая не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if ((string)lbTables.SelectedItem == "Поставщики")
            {
                var obj = dgData.SelectedItem as ViewSupplierModel;
                // Находим продукт по Id
                var supplierToDelete = context.Поставщики.Find(obj.КодПоставщика);
                if (HasDependencies("Поставщики", obj.КодПоставщика))
                {
                    System.Windows.MessageBox.Show($"Невозможно удалить запись из поставщики: есть связанные данные.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (supplierToDelete != null)
                {
                    context.Поставщики.Remove(supplierToDelete);

                    context.SaveChanges();
                    System.Windows.MessageBox.Show("Успешное удаление", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Обработка случая, когда продукт не найден
                    System.Windows.MessageBox.Show("Поставщик не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else if ((string)lbTables.SelectedItem == "Чаи")
            {
                var obj = dgData.SelectedItem as ViewTeaModel;
                // Находим продукт по Id
                var teaToDelete = context.Чай.Find(obj.КодЧая);
                if (HasDependencies("Чай", obj.КодЧая))
                {
                    System.Windows.MessageBox.Show($"Невозможно удалить запись из чаи: есть связанные данные.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (teaToDelete != null)
                {
                    context.Чай.Remove(teaToDelete);

                    context.SaveChanges();
                    System.Windows.MessageBox.Show("Успешное удаление", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Обработка случая, когда продукт не найден
                    System.Windows.MessageBox.Show("Чай не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else if ((string)lbTables.SelectedItem == "Филиалы")
            {
                var obj = dgData.SelectedItem as ViewBranchesModel;
                // Находим продукт по Id
                var branchToDelete = context.Филиалы.Find(obj.КодФилиала);

                if (HasDependencies("Филиалы", obj.КодФилиала))
                {
                    System.Windows.MessageBox.Show($"Невозможно удалить запись из филиалы: есть связанные данные.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (branchToDelete != null)
                {
                    context.Филиалы.Remove(branchToDelete);

                    context.SaveChanges();
                    System.Windows.MessageBox.Show("Успешное удаление", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Обработка случая, когда продукт не найден
                    System.Windows.MessageBox.Show("Филиал не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else if ((string)lbTables.SelectedItem == "Акции")
            {
                var obj = dgData.SelectedItem as Акция;
                // Находим продукт по Id
                var stockToDelete = context.Акции.Find(obj.КодАкции);

                if (stockToDelete != null)
                {
                    context.Акции.Remove(stockToDelete);

                    context.SaveChanges();
                    System.Windows.MessageBox.Show("Успешное удаление", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Обработка случая, когда продукт не найден
                    System.Windows.MessageBox.Show("Акция не найдена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                

            }
            else if ((string)lbTables.SelectedItem == "Отзывы")
            {
                var obj = dgData.SelectedItem as ViewFeedbackModel;
                // Находим продукт по Id
                var feedbackToDelete = context.Отзывы.Find(obj.КодОтзыва);

                if (feedbackToDelete != null)
                {
                    context.Отзывы.Remove(feedbackToDelete);

                    context.SaveChanges();
                    System.Windows.MessageBox.Show("Успешное удаление", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Обработка случая, когда продукт не найден
                    System.Windows.MessageBox.Show("Отзыв не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            } 
            else
            {
                System.Windows.MessageBox.Show("Таблица не выбрана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbSearch.Text.Length == 0)
            {
                refreshDg();
            } else
            {
                resetDg();
                if ((string)lbTables.SelectedItem == "Клиенты")
                {
               
                    dgData.ItemsSource = SearchInClients(tbSearch.Text);
                    dgData.Columns[0].Visibility = Visibility.Collapsed;
                } 
                else if ((string)lbTables.SelectedItem == "Заказы")
                {
                    dgData.ItemsSource = SearchInOrder(tbSearch.Text);
                }
                else if ((string)lbTables.SelectedItem == "Типы чая")
                {
                    dgData.ItemsSource = SearchInTypesTea(tbSearch.Text);
                    dgData.Columns[0].Visibility = Visibility.Collapsed;
                }
                else if ((string)lbTables.SelectedItem == "Поставщики")
                {
                    dgData.ItemsSource = SearchInSupplier(tbSearch.Text);
                    dgData.Columns[0].Visibility = Visibility.Collapsed;
                }
                else if ((string)lbTables.SelectedItem == "Чаи")
                {
                    dgData.ItemsSource = SearchInTea(tbSearch.Text);
                    dgData.Columns[0].Visibility = Visibility.Collapsed;
                }
                else if ((string)lbTables.SelectedItem == "Филиалы")
                {
                    dgData.ItemsSource = SearchInBranch(tbSearch.Text);
                    dgData.Columns[0].Visibility = Visibility.Collapsed;
                }
                else if ((string)lbTables.SelectedItem == "Акции")
                {
                    dgData.ItemsSource = SearchInStock(tbSearch.Text);
                    dgData.Columns[0].Visibility = Visibility.Collapsed;
                }
                else if ((string)lbTables.SelectedItem == "Отзывы")
                {
                    dgData.ItemsSource = SearchInFeedback(tbSearch.Text);
                    dgData.Columns[0].Visibility = Visibility.Collapsed;
                }
                UpdateDgCount();



            }




        }
        public List<ViewClientModel> SearchInClients(string searchTerm)
        {
            
            searchTerm = searchTerm.ToLower();

            var results = context.Клиенты
                .Where(p => p.ПолноеИмя.ToLower().Contains(searchTerm) ||
                            p.Почта.ToLower().Contains(searchTerm) ||
                            p.Телефон.ToLower().Contains(searchTerm))
                .Select(p => new ViewClientModel
                {
                    КодКлиента = p.КодКлиента,
                    ФИОКлиента = p.ПолноеИмя,
                    Почта = p.Почта,
                    Телефон = p.Телефон
                })
                .ToList();

            return results;
        }

        public List<ViewOrderModel> SearchInOrder(string searchTerm)
        {

            try
            {
                searchTerm = searchTerm.ToLower();

                var results = context.Заказы
                    .Where(p => p.Клиент.ПолноеИмя.ToString().Contains(searchTerm) ||
                           p.ДатаЗаказа.ToString().Contains(searchTerm) || 
                           p.КодЗаказа.ToString().Contains(searchTerm) || 
                           p.ОбщаяСумма.ToString().Contains(searchTerm) ||
                           p.Филиал.Название.ToString().Contains(searchTerm))
                    .Select(p => new ViewOrderModel
                    {
                        КодЗаказа = p.КодЗаказа,
                        ФИОКлиента = p.Клиент.ПолноеИмя,
                        НазваниеФилиала = p.Филиал.Название,
                        ДатаЗаказа = p.ДатаЗаказа,
                        ОбщаяСумма = p.ОбщаяСумма
                    })
                    .ToList();

                return results;
            }
            catch (Exception ex)
            { 
                System.Windows.MessageBox.Show(ex.Message);
            }
            return null;

        }
        public List<ViewTypesTeaModel> SearchInTypesTea(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            var results = context.ТипыЧая
                .Where(p => p.Название.ToLower().Contains(searchTerm))
                .Select(p => new ViewTypesTeaModel
                {
                    КодТипЧая = p.КодТипЧая,
                    Название = p.Название
                })
                .ToList();

            return results;
        }

        public List<ViewSupplierModel> SearchInSupplier(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            var results = context.Поставщики
                .Where(p => p.Название.ToLower().Contains(searchTerm) ||
                            p.Страна.ToLower().Contains(searchTerm) ||
                            p.Телефон.ToLower().Contains(searchTerm))
                .Select(p => new ViewSupplierModel
                {
                    КодПоставщика = p.КодПоставщика,
                    Название = p.Название,
                    Страна = p.Страна,
                    Телефон = p.Телефон
                })
                .ToList();

            return results;
        }

        public List<ViewTeaModel> SearchInTea(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            var results = context.Чай
                .Where(p => p.Название.ToLower().Contains(searchTerm) ||
                            p.КодТипЧая.ToString().Contains(searchTerm) ||
                            p.Описание.ToLower().Contains(searchTerm) ||
                            p.Остаток.ToString().Contains(searchTerm) ||
                            p.Цена.ToString().Contains(searchTerm) || 
                            p.Поставщик.Название.Contains(searchTerm))
                .Select(p => new ViewTeaModel
                {
                    КодЧая = p.КодЧая,
                    Название = p.Название,
                    ТипЧая = p.ТипЧая.Название, // Берём название типа чая
                    Поставщик = p.Поставщик.Название, // Берём название поставщика
                    Описание = p.Описание,
                    Цена = p.Цена,
                    Остаток = p.Остаток
                })
                .ToList();

            return results;
        }

        public List<ViewBranchesModel> SearchInBranch(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            var results = context.Филиалы
                .Where(p => p.Название.ToLower().Contains(searchTerm) ||
                            p.Адрес.ToLower().Contains(searchTerm) ||
                            p.Город.ToLower().Contains(searchTerm) ||
                            p.НомерТелефона.ToLower().Contains(searchTerm))
                .Select(p => new ViewBranchesModel
                {
                    КодФилиала = p.КодФилиала,
                    Название = p.Название,
                    Город = p.Город,
                    Адрес = p.Адрес,
                    Телефон = p.НомерТелефона
                })
                .ToList();

            return results;
        }

        public List<ViewStocksModel> SearchInStock(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            var results = context.Акции
                .Where(p => p.Название.ToLower().Contains(searchTerm) ||
                            p.ДатаНачала.ToString().Contains(searchTerm) ||
                            p.ДатаОкончания.ToString().Contains(searchTerm) ||
                            p.ПроцентСкидки.ToString().Contains(searchTerm))
                .Select(p => new ViewStocksModel
                {
                    КодАкции = p.КодАкции,
                    Название = p.Название,
                    ДатаНачала = p.ДатаНачала,
                    ДатаОкончания = p.ДатаОкончания,
                    ПроцентСкидки = p.ПроцентСкидки
                })
                .ToList();

            return results;
        }

        public List<ViewFeedbackModel> SearchInFeedback(string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            var results = context.Отзывы
                .Where(p => p.Оценка.ToString().Contains(searchTerm) ||
                            p.Комментарий.ToLower().Contains(searchTerm) ||
                            p.ДатаОтзыва.ToString().Contains(searchTerm) ||
                            p.Оценка.ToString().Contains(searchTerm) ||
                            p.Чай.Название.ToString().Contains(searchTerm) || 
                            p.Клиент.ПолноеИмя.ToString().Contains(searchTerm))
                .Select(p => new ViewFeedbackModel
                {
                    КодОтзыва = p.КодОтзыва,
                    ФИОКлиента = p.Клиент.ПолноеИмя,
                    НазваниеЧая = p.Чай.Название,
                    Оценка = p.Оценка,
                    Комментарий = p.Комментарий,
                    ДатаОтзыва = p.ДатаОтзыва
                })
                .ToList();

            return results;
        }




        private void dgData_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (lbTables.SelectedItem == null)
            {
                e.Handled = true; // Отмена показа меню, если ничего не выбрано
            } 
        }

        private void btnEmp_MouseDown(object sender, MouseButtonEventArgs e)
        {   try
            {
                var employees = context.Сотрудники.ToList();

                Employee employee = new Employee(roles, employees);
                employee.Owner = this;
                employee.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                employee.ShowDialog();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Ошибка в поле '{validationError.PropertyName}': {validationError.ErrorMessage}");
                    }
                }
            }
        }

        private void btnExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
          
            MessageBoxResult result = System.Windows.MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.No)
            {
                this.Close();
            }
        }

        private void btnReport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Report report = new Report();
            report.Owner = this;
            report.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            report.ShowDialog();
        }
    }
    
}
