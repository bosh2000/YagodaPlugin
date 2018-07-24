using System;
using System.Collections.Generic;

namespace Resto.Front.Api.YagodaPlugCore
{
    /// <summary>
    /// Класс для формирования Json при проведении покупки.
    /// </summary>
    public class Purchase
    {
        /// <summary>
        /// Телефон покупателя, в случае если его нет в базу Ягоды, создаеться новый.
        /// </summary>
        public string buyerTel { get; set; }

        /// <summary>
        /// Номер чека в платежной системе продавца.
        /// </summary>
        public string checkNo { get; set; }

        /// <summary>
        /// Дата\время чека.
        /// </summary>
        public DateTime checkDateTime { get; set; }

        /// <summary>
        /// ID чека в торговой системе,
        /// </summary>
        public string uuid { get; set; }

        /// <summary>
        /// Сумма чека.
        /// </summary>
        public string checkAmount { get; set; }

        public string bonusAmount { get; set; }

        /// <summary>
        /// Сумма оплаты в бонусах.
        /// </summary>
        public string payByBonus { get; set; }

        /// <summary>
        /// Наименование продавца.
        /// </summary>
        public string seller { get; set; }

        /// <summary>
        /// Кодовое слово.
        /// </summary>
        public string codeWord { get; set; }

        /// <summary>
        /// Список товаров в чеке.
        /// </summary>
        public List<Good> goods { get; set; }
    }

    /// <summary>
    /// Класс описывающий товары в чеке.
    /// </summary>
    public class Good
    {
        /// <summary>
        /// Наименование товара.
        /// </summary>
        public string goodsName { get; set; }

        /// <summary>
        /// Количество товара.
        /// </summary>
        public int qty { get; set; }

        /// <summary>
        /// Единица измерения.
        /// </summary>
        public string unit { get; set; }

        /// <summary>
        /// Цена за единицу товара.
        /// </summary>
        public double price { get; set; }

        /// <summary>
        /// Общая сумма за товар (количество *цена)
        /// </summary>
        public double cost { get; set; }

        public GoodsGroup goodsGroup { get; set; }
    }

    public class GoodsGroup
    {
        /// <summary>
        ///  Код группы.
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// Наименование группы.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Ссылка на родительскую группу, если есть.
        /// </summary>
        public ParentGroup parentGroup { get; set; }
    }

    /// <summary>
    /// Класс описывающий родительскую группу товаров.
    /// </summary>

    public class ParentGroup
    {
        /// <summary>
        /// Код группы в торговой системею
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// Наименование группы.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Ссылка на родительскую группу , если есть.
        /// </summary>
        public object parentGroup { get; set; }
    }
}