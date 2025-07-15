using System;
using System.Collections.Generic;
using System.Data.Sql;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using Azure.Core;
using System.Data.Entity;
using System.Configuration;
using System.Collections.Specialized;
using lab9.Forms;
namespace lab9
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class Connect : Window
    {

        public Connect()
        {
            InitializeComponent();
            labelStatus.Foreground = new SolidColorBrush(Colors.Red);
            

        }
        private void blockConnectUI(bool state)
        {
            if (state)
            {
                Opacity = 0.5;
                Title = "HoTea | База данных открыта";
                labelStatus.Content = "Подключено";
                ConnectForm.IsEnabled = false;
                labelStatus.Foreground = new SolidColorBrush(Colors.Green);
              
            } else
            {
                ConnectForm.IsEnabled = true;
                labelStatus.Foreground = new SolidColorBrush(Colors.Red);
                labelStatus.Content = "Отключено";
                Title = "HoTea | Страница подключения";
                Opacity = 1;
            }

        }

        

        private Сотрудник CheckCred(string login, string password)
        {   
            var calcHash = HashGenerator.ComputeSHA512(password);
            using (var context = new AppDbContext())
            {
                var emp = context.Сотрудники.FirstOrDefault(s => s.Логин == login);
                if (emp != null && emp.Пароль == calcHash)
                {
                    return emp;
                } else
                {
                    return null;
                }

            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SqlServerChecker.IsSqlServerAvailable())
            {
                (sender as Image).Opacity = 1.0;
                (sender as Image).Opacity = 0.7;
                var emp = CheckCred(tbLogin.Text, pbPassword.Password);
                if (emp != null)
                {
                    blockConnectUI(true);
                    tbLogin.Text = string.Empty;
                    pbPassword.Password = string.Empty;
                   
                    Main main = new Main(emp);
                    main.Owner = this;
                    main.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    main.ShowDialog();
                    blockConnectUI(false);

                }
                else
                {
                    MessageBox.Show("Неверные данные", "Ошибка");
                }

            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            (sender as Image).Opacity = 1.0;
        }
    }
}
