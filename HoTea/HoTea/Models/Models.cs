using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab9
{
    [Table("Филиалы", Schema = "dbo")]
    public class Филиал
    {
        [Key]
        public int КодФилиала { get; set; }

        [Required]
        [MaxLength(100)]
        public string Название { get; set; }

        [MaxLength(255)]
        public string Адрес { get; set; }

        [MaxLength(50)]
        public string Город { get; set; }

        [MaxLength(50)]
        public string НомерТелефона { get; set; }

        public virtual ICollection<Заказ> Заказы { get; set; }

        public Филиал()
        {
            Заказы = new HashSet<Заказ>();
        }
    }


    [Table("ТипыЧая", Schema = "dbo")]
    public class ТипЧая
    {
        [Key]
        public int КодТипЧая { get; set; }

        [Required]
        [MaxLength(50)]
        public string Название { get; set; }

        public virtual ICollection<Чай> Чаи { get; set; }

        public ТипЧая()
        {
            Чаи = new HashSet<Чай>();
        }
    }


    [Table("Чай", Schema = "dbo")]
    public class Чай
    {
        [Key]
        public int КодЧая { get; set; }

        [Required]
        [MaxLength(100)]
        public string Название { get; set; }

        [ForeignKey("ТипЧая")]
        public int КодТипЧая { get; set; }
        public virtual ТипЧая ТипЧая { get; set; }

        [ForeignKey("Поставщик")]
        public int КодПоставщика { get; set; }
        public virtual Поставщик Поставщик { get; set; }

        [MaxLength(255)]
        public string Описание { get; set; }

        [Required]
        public decimal Цена { get; set; }

        [Required]
        public int Остаток { get; set; }

        public virtual ICollection<Отзыв> Отзывы { get; set; }
        public virtual ICollection<ТоварыВЗаказе> ТоварыВЗаказе { get; set; }


        public Чай()
        {
            Отзывы = new HashSet<Отзыв>();
            ТоварыВЗаказе = new HashSet<ТоварыВЗаказе>();
        }
    }


    [Table("Поставщики", Schema = "dbo")]
    public class Поставщик
    {
        [Key]
        public int КодПоставщика { get; set; }

        [Required]
        [MaxLength(100)]
        public string Название { get; set; }

        [MaxLength(50)]
        public string Страна { get; set; }

        [MaxLength(30)]
        public string Телефон { get; set; }

        public virtual ICollection<Чай> Чаи { get; set; }

        public Поставщик()
        {
            Чаи = new HashSet<Чай>();
        }
    }


    [Table("Клиенты", Schema = "dbo")]
    public class Клиент
    {
        [Key]
        public int КодКлиента { get; set; }

        [Required]
        [MaxLength(100)]
        public string ПолноеИмя { get; set; }

        [MaxLength(100)]
        public string Почта { get; set; }

        [MaxLength(30)]
        public string Телефон { get; set; }

        public virtual ICollection<Заказ> Заказы { get; set; }
        public virtual ICollection<Отзыв> Отзывы { get; set; }

        public Клиент()
        {
            Заказы = new HashSet<Заказ>();
            Отзывы = new HashSet<Отзыв>();
        }
    }


    [Table("Заказы", Schema = "dbo")]
    public class Заказ
    {
        [Key]
        public int КодЗаказа { get; set; }

        [ForeignKey("Клиент")]
        public int КодКлиента { get; set; }
        public Клиент Клиент { get; set; }

        [ForeignKey("Филиал")]
        public int КодФилиала { get; set; }
        public Филиал Филиал { get; set; }

        [Required]
        public DateTime ДатаЗаказа { get; set; }

        [Required]
        public decimal ОбщаяСумма { get; set; }
        public virtual ICollection<ТоварыВЗаказе> ТоварыВЗаказе { get; set; }

        public Заказ()
        {
            ТоварыВЗаказе = new HashSet<ТоварыВЗаказе>();
        }
    }


    [Table("Отзывы", Schema = "dbo")]
    public class Отзыв
    {
        [Key]
        public int КодОтзыва { get; set; }

        [ForeignKey("Клиент")]
        public int КодКлиента { get; set; }
        public virtual Клиент Клиент { get; set; }

        [ForeignKey("Чай")]
        public int КодЧая { get; set; }
        public virtual Чай Чай { get; set; }

        [Required]
        [Range(1, 5)]
        public int Оценка { get; set; }

        [MaxLength(500)]
        public string Комментарий { get; set; }

        [Required]
        public DateTime ДатаОтзыва { get; set; }
    }


    [Table("Акции", Schema = "dbo")]
    public class Акция
    {
        [Key]
        public int КодАкции { get; set; }

        [Required]
        [MaxLength(100)]
        public string Название { get; set; }

        [Required]
        public DateTime ДатаНачала { get; set; }

        [Required]
        public DateTime ДатаОкончания { get; set; }

        [Required]
        public decimal ПроцентСкидки { get; set; }
    }

    [Table("ТоварыВЗаказе", Schema = "dbo")]
    public class ТоварыВЗаказе
    {
        [Key]
        public int КодТовараЗаказа { get; set; }

        [ForeignKey("Заказ")]
        public int КодЗаказа { get; set; }
        public virtual Заказ Заказ { get; set; }

        [ForeignKey("Чай")]
        public int КодЧая { get; set; }
        public virtual Чай Чай { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть больше 0")]
        public int Количество { get; set; }

        [Required]
        public decimal Цена { get; set; }

        [Required]
        public decimal Сумма { get; set; }
    }

    [Table("Сотрудники")]
    public class Сотрудник
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int КодСотрудника { get; set; } // Единственный первичный ключ

        [Required]
        [MaxLength(100)]
        [Index(IsUnique = true)] // Уникальный индекс для Login
        public string Логин { get; set; } // Теперь просто уникальный

        [Required]
        [MaxLength(100)]
        public string ФИО { get; set; }

        [Required]
        [MaxLength(100)]
        public string Роль { get; set; }

        [Required]
        [MaxLength(256)]
        public string Пароль { get; set; }


    }
}
