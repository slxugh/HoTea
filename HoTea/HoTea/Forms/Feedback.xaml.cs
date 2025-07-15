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
    /// Логика взаимодействия для Feedback.xaml
    /// </summary>
    public partial class Feedback : Window
    {
        public Feedback(Отзыв feedback, List<Клиент> clientsList, List<Чай> teaList)
        {
            InitializeComponent();

            cbClient.ItemsSource = clientsList;
            cbTea.ItemsSource = teaList;

            cbClient.DisplayMemberPath = "ПолноеИмя";
            cbClient.SelectedValuePath = "КодКлиента";
            cbClient.SelectedValue = feedback.КодКлиента;
            cbTea.DisplayMemberPath = "Название";
            cbTea.SelectedValuePath = "КодЧая";
            cbTea.SelectedValue = feedback.КодЧая;

            labelFeedbackID.Content = feedback.КодОтзыва.ToString();
                
            tbMark.Text = feedback.Оценка.ToString();
            tbCommentInFeedback.Text = feedback.Комментарий.ToString();
            dpFeedbackDate.Text = feedback.ДатаОтзыва.ToString();
        }
        public Feedback(List<Клиент> clientsList, List<Чай> teaList)
        {
            InitializeComponent();
            Title = "Добавление нового отзыва";

            cbClient.ItemsSource = clientsList;
            cbTea.ItemsSource = teaList;

            cbClient.DisplayMemberPath = "ПолноеИмя";
            cbClient.SelectedValuePath = "КодКлиента";
            cbClient.SelectedIndex = 0;

            cbTea.DisplayMemberPath = "Название";
            cbTea.SelectedValuePath = "КодЧая";
            cbTea.SelectedIndex = 0;
        }
        public Отзыв GetData()
        {

            try
            {
                Отзыв feedback = new Отзыв();

                if (int.TryParse((string)labelFeedbackID.Content, out int id))
                {
                    feedback.КодОтзыва = int.Parse((string)labelFeedbackID.Content);
                    feedback.КодКлиента = (int)cbClient.SelectedValue;
                    feedback.КодЧая = (int)cbTea.SelectedValue;
                    feedback.Оценка = int.Parse(tbMark.Text);
                    feedback.Комментарий = tbCommentInFeedback.Text;
                    feedback.ДатаОтзыва = DateTime.Parse(dpFeedbackDate.Text);

                }
                else
                {
                    feedback.КодКлиента = (int)cbClient.SelectedValue;
                    feedback.КодЧая = (int)cbTea.SelectedValue;
                    feedback.Оценка = int.Parse(tbMark.Text);
                    feedback.Комментарий = tbCommentInFeedback.Text;
                    feedback.ДатаОтзыва = DateTime.Parse(dpFeedbackDate.Text);
                }

                return feedback;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                return null;
            }


        }

        

        private void btnSaveInFeedback_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
