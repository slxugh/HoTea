using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace lab9
{
    /// <summary>
    /// Логика взаимодействия для TypesTea.xaml
    /// </summary>
    public partial class TypesTea : Window
    {
        public TypesTea(ТипЧая typesTea)
        {
            InitializeComponent();
            labelTypeTeaID.Content = typesTea.КодТипЧая.ToString();
            tbTypesTeaName.Text = typesTea.Название.ToString();
        }
        public TypesTea()
        {
            InitializeComponent();
            Title = "Добавление нового типа чая";
        }

        public ТипЧая GetData()
        {
            ТипЧая typeTea = new ТипЧая();
            try
            {
                if (int.TryParse((string)labelTypeTeaID.Content, out int id))
                {
                    typeTea.КодТипЧая = id;
                    typeTea.Название = tbTypesTeaName.Text;
                    
                } else
                {
                    typeTea.Название = tbTypesTeaName.Text;
                }
                return typeTea;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
            }
        }

       

        private void btnSaveInSupplier_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
