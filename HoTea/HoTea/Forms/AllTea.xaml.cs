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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace lab9
{
    /// <summary>
    /// Логика взаимодействия для AllTea.xaml
    /// </summary>
    public partial class AllTea : Window
    {
        public AllTea(Чай tea, List<ТипЧая> typesTeaList, List<Поставщик> suppliersList)
        {
            InitializeComponent();

            labelTeaID.Content = tea.КодЧая.ToString();
            tbTeaName.Text = tea.Название.ToString();
            cbTypeTea.ItemsSource = typesTeaList;
            cbSupplier.ItemsSource = suppliersList;

            cbTypeTea.DisplayMemberPath = "Название";
            cbTypeTea.SelectedValuePath = "КодТипЧая";
            cbTypeTea.SelectedValue = tea.КодТипЧая;

            cbSupplier.DisplayMemberPath = "Название";
            cbSupplier.SelectedValuePath = "КодПоставщика";
            cbSupplier.SelectedValue = tea.КодПоставщика;


            tbDescriptionInTea.Text = tea.Описание.ToString();
            tbPriceInTea.Text = tea.Цена.ToString();
            tbRemains.Text = tea.Остаток.ToString();
        }
        public AllTea(List<ТипЧая> typesTeaList, List<Поставщик> suppliersList)
        {
            InitializeComponent();

            Title = "Добавление нового чая";

            cbTypeTea.ItemsSource = typesTeaList;
            cbSupplier.ItemsSource = suppliersList;

            cbTypeTea.DisplayMemberPath = "Название";
            cbTypeTea.SelectedValuePath = "КодТипЧая";
            cbTypeTea.SelectedIndex = 0;

            cbSupplier.DisplayMemberPath = "Название";
            cbSupplier.SelectedValuePath = "КодПоставщика";
            cbSupplier.SelectedIndex = 0;
        }
        public Чай GetData()
        {

            try
            {

                Чай tea = new Чай();

                if (int.TryParse((string)labelTeaID.Content, out int id))
                {
                    tea.КодЧая = int.Parse((string)labelTeaID.Content);
                    tea.Название = tbTeaName.Text;
                    tea.КодТипЧая = (int)cbTypeTea.SelectedValue;
                    tea.КодПоставщика = (int)cbSupplier.SelectedValue;
                    tea.Описание = tbDescriptionInTea.Text;
                    tea.Цена = decimal.Parse(tbPriceInTea.Text);
                    tea.Остаток = int.Parse(tbRemains.Text);
                }
                else
                {
                    tea.Название = tbTeaName.Text;
                    tea.КодТипЧая = (int)cbTypeTea.SelectedValue;
                    tea.КодПоставщика = (int)cbSupplier.SelectedValue;
                    tea.Описание = tbDescriptionInTea.Text;
                    tea.Цена = decimal.Parse(tbPriceInTea.Text);
                    tea.Остаток = int.Parse(tbRemains.Text);
                }

               
                return tea;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
            }


        }

       

        private void btnSaveInTea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
