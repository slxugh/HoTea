using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace lab9.Forms
{
    /// <summary>
    /// Логика взаимодействия для Employee.xaml
    /// </summary>
    public partial class Employee : Window
    {
        public Employee(List<string> Roles, List<Сотрудник> employees)
        {
            InitializeComponent();
            cbRole.ItemsSource = Roles;
            cbRoleNew.ItemsSource = Roles;
            cbEmp.ItemsSource = employees;
            if(employees.Count > 0)
            {
                tbFIO.Text = employees[0].ФИО;
                tbLogin.Text = employees[0].Логин;
            }
            cbEmp.DisplayMemberPath = "ФИО";
            cbEmp.SelectedValuePath = "КодСотрудника";

            cbEmp.SelectedIndex = 0;
            cbRole.SelectedIndex = 0;
            cbRoleNew.SelectedIndex = 0;

        }

        public void SaveData()
        {
       
            using (var context = new AppDbContext())
            {
                

                    Сотрудник newEmp = new Сотрудник();
                    if (tcEmp.SelectedIndex == 0)
                    {
                        
                        newEmp.КодСотрудника = int.Parse(cbEmp.SelectedValue.ToString());
                        newEmp.Логин = tbLogin.Text;
                        var empToUpdate = context.Сотрудники.Find(newEmp.КодСотрудника);
                        empToUpdate.ФИО = tbFIO.Text;
                        empToUpdate.Логин = newEmp.Логин;
                        empToUpdate.Роль = cbRole.SelectedValue.ToString();
                        empToUpdate.Пароль = HashGenerator.ComputeSHA512(pbPassword.Password);
                        context.SaveChanges();
                        System.Windows.MessageBox.Show("Сотрудник успешно изменен", "Успех");
                    }
                    else
                    {
                        if (context.Сотрудники.FirstOrDefault(s => s.Логин == tbLoginNew.Text) == null)
                        {
                            newEmp.ФИО = tbFIONew.Text;
                            newEmp.Логин = tbLoginNew.Text;
                            newEmp.Роль = cbRole.SelectedValue.ToString();
                            newEmp.Пароль = HashGenerator.ComputeSHA512(pbPasswordNew.Password);
                            context.Сотрудники.Add(newEmp);
                            context.SaveChanges();
                            System.Windows.MessageBox.Show("Сотрудник успешно добавлен", "Успех");
                        }
                        else
                        {
                            MessageBox.Show("Такой пользователь уже существует", "Ошибка");
                        }
                    }
               
                
            }
        
        }

        private void btnSaveNew_MouseDown(object sender, MouseButtonEventArgs e)
        {
           
            if (tbFIONew.Text != "" && tbLoginNew.Text != "" && pbPasswordNew.Password != "")
            {   
                        
                        
                SaveData();
                refresh();
            }
            else
            {
                MessageBox.Show("Не все поля заполнены", "Ошибка");
            }
                
            
        }

        private void btnSave_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (tbFIO.Text != "" && tbLogin.Text != "" && pbPassword.Password != "")
            {
                SaveData();
                refresh();
            }
            else
            {
                MessageBox.Show("Не все поля заполнены", "Ошибка");
            }
        }

        private void refresh()
        {
            cbEmp.ItemsSource = null;
            tbFIO.Text = string.Empty;
            tbLogin.Text = string.Empty;
            pbPassword.Password = string.Empty;
            List<Сотрудник> emp = null;
            using (var context = new AppDbContext())
            {
               emp = context.Сотрудники.ToList();
            }
            cbEmp.ItemsSource = emp;
            tbFIO.Text = emp[0].ФИО;
            tbLogin.Text = emp[0].Логин;
            cbEmp.SelectedItem = emp[0];


        }
        private void btnDelete_MouseDown(object sender, MouseButtonEventArgs e)
        {   
            using (var context = new AppDbContext())
            {
                if (context.Сотрудники.Count() < 2)
                {
                    MessageBox.Show("Невозможно удалить единственного пользователя", "Ошибка");
                }
                else
                {
                    var emp = cbEmp.SelectedItem as Сотрудник;

                    var findEmp = context.Сотрудники.FirstOrDefault(s => s.Логин == emp.Логин);
                    context.Сотрудники.Remove(findEmp);
                    context.SaveChanges();
                    System.Windows.MessageBox.Show("Успешное удаление", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    refresh();
                }
            }
        }

        private void cbEmp_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbEmp.ItemsSource != null) {
                var emp = cbEmp.SelectedItem as Сотрудник;
                tbFIO.Text = emp.ФИО;
                tbLogin.Text = emp.Логин;
                cbRole.SelectedValue = emp.Роль;
            }

        }
    }
}
