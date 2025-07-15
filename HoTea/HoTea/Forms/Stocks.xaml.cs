using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    /// Логика взаимодействия для Stocks.xaml
    /// </summary>
    public partial class Stocks : Window
    {
        public Stocks(Акция stock)
        {
            InitializeComponent();
            labelStockID.Content = stock.КодАкции.ToString();
            tbStockName.Text = stock.Название.ToString();
            dpStartDate.Text = stock.ДатаНачала.ToString();
            dpEndDate.Text = stock.ДатаОкончания.ToString();
            tbStockPercent.Text = stock.ПроцентСкидки.ToString();
        }
        public Stocks()
        {
            InitializeComponent();
            Title = "Добавление новой скидки";
        }
        public Акция GetData()
        {
            Акция stock = new Акция();
            try
            {
                if (int.TryParse((string)labelStockID.Content, out int id))
                {
                    stock.КодАкции = int.Parse((string)labelStockID.Content);
                    stock.Название = tbStockName.Text;
                    stock.ДатаНачала = DateTime.Parse(dpStartDate.Text);
                    stock.ДатаОкончания = DateTime.Parse(dpEndDate.Text);
                    stock.ПроцентСкидки = decimal.Parse(tbStockPercent.Text);
                } else
                {
                    stock.Название = tbStockName.Text;
                    stock.ДатаНачала = DateTime.Parse(dpStartDate.Text);
                    stock.ДатаОкончания = DateTime.Parse(dpEndDate.Text);
                    stock.ПроцентСкидки = decimal.Parse(tbStockPercent.Text);
                }
                return stock;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
            }


        }

      

        private void btnSaveInStocks_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
