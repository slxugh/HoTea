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
using System.Xml.Linq;

namespace lab9
{
    /// <summary>
    /// Логика взаимодействия для Supplier.xaml
    /// </summary>
    public partial class Supplier : Window
    {
       
        public Supplier(Поставщик supplier)
        {
            InitializeComponent();
            labelSupplierID.Content = supplier.КодПоставщика.ToString();
            tbNameSupplier.Text = supplier.Название;
            tbCountry.Text = supplier.Страна.ToString();
            tbPhone.Text = supplier.Телефон.ToString();
            tbPhone.MaxLength = 12;
        }
        public Supplier()
        {
            InitializeComponent();
            Title = "Добавление нового поставщика";
            tbPhone.Text = "+7";
            tbPhone.MaxLength = 12;
        }

        public Поставщик GetData()
        {
            Поставщик supplier = new Поставщик();
            try
            {
                if (int.TryParse((string)labelSupplierID.Content, out int id))
                {
                    supplier.КодПоставщика = int.Parse((string)labelSupplierID.Content);
                    supplier.Название = tbNameSupplier.Text;
                    supplier.Страна = tbCountry.Text;
                    supplier.Телефон = tbPhone.Text;
                    
                } else
                {
                    supplier.Название = tbNameSupplier.Text;
                    supplier.Страна = tbCountry.Text;
                    supplier.Телефон = tbPhone.Text;
                }
                return supplier;
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

        private void btnSaveInSupplier_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
