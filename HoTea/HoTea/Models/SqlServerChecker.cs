
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System;
using lab9;
using System.Windows;
using System.Configuration;

public class SqlServerChecker
{
    public static bool IsSqlServerAvailable()
    {
        try
        {
            using (var context = new AppDbContext()) {
            
                
                context.Database.Connection.Open();
                context.Database.Connection.Close();
                return true; 
            }
        }
        catch (DbUpdateException ex)
        {
           
          System.Windows.MessageBox.Show($"Ошибка обновления базы данных: {ex.Message}\nСтрока подключения: ", "Ошибка БД");
            return false;
        }
        catch (Exception ex)
        {
           
            System.Windows.MessageBox.Show($"Ошибка: {ex.Message}\nCервер: {ConfigurationManager.AppSettings["HoTeaContext"]}", "Ошибка");
            return false;
        }
    }
}