using lab9;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string name = "Чай улун А";
            int key = 5;
            using (var context = new AppDbContext())
            {
               

                // Использование метода Find
                var entity = context.Чай.Find(key);

               

                if (entity != null)
                {
                    Assert.AreEqual(name, entity.Название);
                }
                else
                {
                    Console.WriteLine("Сущность не найдена.");
                }
            }


        }


        [TestMethod]
        public void TestMethod2()
        {
            bool act = SqlServerChecker.IsSqlServerAvailable();
            Assert.AreEqual(true, act);
        }



        [TestMethod]
        public void TestMethod3()
        {
            ТипЧая tea = new ТипЧая();
            tea.Название = "Тест удаления";
            tea.КодТипаЧая = 5;
            using (var context = new AppDbContext())
            {

                context.ТипыЧая.Add(tea);
              
                var entity = context.ТипыЧая.Find(tea.КодТипаЧая);

                context.ТипыЧая.Remove(entity);

                entity = context.ТипыЧая.Find(tea.КодТипаЧая);
                Assert.AreEqual(null, entity);
            }
        }
    }
}
