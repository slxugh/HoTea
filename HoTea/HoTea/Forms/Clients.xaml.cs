using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace lab9
{
    /// <summary>
    /// Логика взаимодействия для Clients.xaml
    /// </summary>
    public partial class Clients : Window
    {
        public AppDbContext context;
        public Clients(Клиент client)
        {
            InitializeComponent();
            labelClientID.Content = client.КодКлиента.ToString();
            tbEmail.Text = client.Почта.ToString();
            tbName.Text = client.ПолноеИмя.ToString();
            tbPhone.Text = client.Телефон.ToString();
            tbPhone.MaxLength = 12;
        }
        public Clients()
        {
            
            InitializeComponent();
            labelClientID.Content = "Auto";
            Title = "Добавление нового клиента";
            tbPhone.Text = "+7";
            tbPhone.MaxLength = 12;
        }

        
        public Клиент GetData()
        {
            try {
                Клиент client = new Клиент();
                if (int.TryParse((string)labelClientID.Content, out int id)){
                    client.КодКлиента = id;
                    client.Почта = tbEmail.Text;
                    client.ПолноеИмя = tbName.Text;
                    client.Телефон = tbPhone.Text;
                }
                else
                {
                    client.Почта = tbEmail.Text;
                    client.ПолноеИмя = tbName.Text;
                    client.Телефон = tbPhone.Text;
                }
                return client;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
            }

            
        }

       
        private void tbPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!tbPhone.Text.StartsWith("+7"))
            {
                tbPhone.Text = "+7";
                tbPhone.SelectionStart = tbPhone.Text.Length;
            }
          

        }

        private void tbPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]$");
        }

        private void tbPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (tbPhone.SelectionStart < 2 &&
                (e.Key == Key.Back || e.Key == Key.Delete))
            {
                e.Handled = true;
            }
        }

        private void btnSaveInClietn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
    }
}
