using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab9
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        : base("name=HoTeaContext")
        {
           
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }
        

        public DbSet<Филиал> Филиалы { get; set; }
        public DbSet<ТипЧая> ТипыЧая { get; set; }
        public DbSet<Чай> Чай { get; set; }
        public DbSet<Поставщик> Поставщики { get; set; }
        public DbSet<Клиент> Клиенты { get; set; }
        public DbSet<Заказ> Заказы { get; set; }
        public DbSet<Отзыв> Отзывы { get; set; }
        public DbSet<Акция> Акции { get; set; }
        public DbSet<ТоварыВЗаказе> ТоварыВЗаказе { get; set; }
        public DbSet<Сотрудник> Сотрудники { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Заказ>()
                .HasRequired(o => o.Клиент)
                .WithMany(c => c.Заказы)
                .HasForeignKey(o => o.КодКлиента);

            modelBuilder.Entity<Заказ>()
                .HasRequired(o => o.Филиал)
                .WithMany(f => f.Заказы)
                .HasForeignKey(o => o.КодФилиала);

            modelBuilder.Entity<Чай>()
                .HasRequired(t => t.ТипЧая)
                .WithMany(tp => tp.Чаи)
                .HasForeignKey(t => t.КодТипЧая);

            modelBuilder.Entity<Чай>()
                .HasRequired(t => t.Поставщик)
                .WithMany(p => p.Чаи)
                .HasForeignKey(t => t.КодПоставщика);

            modelBuilder.Entity<Отзыв>()
                .HasRequired(r => r.Клиент)
                .WithMany(c => c.Отзывы)
                .HasForeignKey(r => r.КодКлиента);

            modelBuilder.Entity<Отзыв>()
                .HasRequired(r => r.Чай)
                .WithMany(t => t.Отзывы)
                .HasForeignKey(r => r.КодЧая);

            modelBuilder.Entity<ТоварыВЗаказе>()
                .HasRequired(tz => tz.Заказ)
                .WithMany(z => z.ТоварыВЗаказе)
                .HasForeignKey(tz => tz.КодЗаказа)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ТоварыВЗаказе>()
                .HasRequired(tz => tz.Чай)
                .WithMany(t => t.ТоварыВЗаказе)
                .HasForeignKey(tz => tz.КодЧая);

            modelBuilder.Entity<Сотрудник>()
                 .HasKey(s => s.КодСотрудника); 

            modelBuilder.Entity<Сотрудник>()
                .HasIndex(s => s.Логин)
                .IsUnique();
        }




    }
}
