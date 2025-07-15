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
    /// Логика взаимодействия для Branches.xaml
    /// </summary>
    public partial class Branches : Window
    {
        public Branches(Филиал branch)
        {
            InitializeComponent();
            labelBranchID.Content = branch.КодФилиала.ToString();
            tbBranchName.Text = branch.Название.ToString();
            tbBranchEmail.Text = branch.Адрес.ToString();
            tbBranchCity.Text = branch.Город.ToString();
            tbBranchPhone.Text = branch.НомерТелефона.ToString();
            tbBranchPhone.MaxLength = 12;
        }
        public Branches()
        {
            InitializeComponent();
            Title = "Добавление нового филиала";
            tbBranchPhone.Text = "+7";
            tbBranchPhone.MaxLength = 12;
        }
        public Филиал GetData()
        {
            Филиал branch = new Филиал();
            try
            {
                if (int.TryParse((string)labelBranchID.Content, out int id))
                {
                    branch.КодФилиала = int.Parse((string)labelBranchID.Content);
                    branch.Название = tbBranchName.Text;
                    branch.Адрес = tbBranchEmail.Text;
                    branch.Город = tbBranchCity.Text;
                    branch.НомерТелефона = tbBranchPhone.Text;
                } else
                {
                    branch.Название = tbBranchName.Text;
                    branch.Адрес = tbBranchEmail.Text;
                    branch.Город = tbBranchCity.Text;
                    branch.НомерТелефона = tbBranchPhone.Text;
                }
                return branch;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
            }

        }

       

        private void tbBranchPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (tbBranchPhone.SelectionStart < 2 &&
                (e.Key == Key.Back || e.Key == Key.Delete))
            {
                e.Handled = true;
            }
        }

        private void tbBranchPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]$");
        }

        private void tbBranchPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!tbBranchPhone.Text.StartsWith("+7"))
            {
                tbBranchPhone.Text = "+7";
                tbBranchPhone.SelectionStart = tbBranchPhone.Text.Length;
            }

        }

        private void btnSaveInClient_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
