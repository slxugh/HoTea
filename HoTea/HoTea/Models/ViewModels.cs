using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab9.Forms
{
    public class ViewClientModel
    {
        public int КодКлиента { get; set; }
        public string ФИОКлиента { get; set; }
        public string Почта { get; set; }
        public string Телефон { get; set; }
    }

    public class ViewBranchesModel
    {
        public int КодФилиала { get; set; }
        public string Название { get; set; }
        public string Город { get; set; }
        public string Адрес { get; set; }
        public string Телефон { get; set; }
    }

    public class ViewTypesTeaModel
    {
        public int КодТипЧая { get; set; }
        public string Название { get; set; }
    }

    public class ViewSupplierModel
    {
        public int КодПоставщика { get; set; }
        public string Название { get; set; }
        public string Страна { get; set; }
        public string Телефон { get; set; }
    }

    public class ViewStocksModel
    {
        public int КодАкции { get; set; }
        public string Название { get; set; }
        public DateTime ДатаНачала { get; set; }
        public DateTime ДатаОкончания{ get; set; }

        public decimal ПроцентСкидки { get; set; }
      
    }

    public class ViewOrderModel
    {
        public int КодЗаказа { get; set; }
        public string ФИОКлиента { get; set; }
        public string НазваниеФилиала { get; set; }
        public DateTime ДатаЗаказа { get; set; }
        public decimal ОбщаяСумма { get; set; }
    }
    public class ViewTeaModel
    {
        public int КодЧая { get; set; }
        public string Название { get; set; }
        public string ТипЧая { get; set; }
        public string Поставщик { get; set; }
        public string Описание { get; set; }
        public decimal Цена { get; set; }
        public int Остаток { get; set; }
    }

    public class TeaSalesModel
    {
        public string Name { get; set; } // Название чая
        public int Quantity { get; set; } // Количество проданных единиц
        public decimal TotalPrice { get; set; } // Сумма продаж
    }
    public class ViewFeedbackModel
    {
        public int КодОтзыва { get; set; }
        public string ФИОКлиента { get; set; }
        public string НазваниеЧая { get; set; }
        public int Оценка { get; set; }
        public string Комментарий { get; set; }
        public DateTime ДатаОтзыва { get; set; }
    }

    public class ViewTeaInOrderModel
    {
        public int КодТовараВЗаказе {  get; set; }
        public string Чай { get; set; }
        public int Количество { get; set; }
        public decimal Цена { get; set; }
        public decimal Сумма { get; set; }
    }
}
