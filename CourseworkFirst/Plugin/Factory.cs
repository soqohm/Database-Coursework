using System;
using System.Collections.Generic;
using System.Windows.Forms;

using TFlex;

namespace CourseworkFirst
{
    public class MyFactory : PluginFactory
    {
        public override string Name => "ЧелГУ";
        public override Guid ID => new Guid("{4d1a7673-0fb4-4a80-a2ac-224b88d6bc21}");

        public List<Button> Buttons = new List<Button>()
        {
            new Button(
                "Добавить в БД",
                "Добавить атрибуты входящих деталей и сборок",
                Ico.main,
                "ЧелГУ",
                "Курсовая по базам данных",
                () => WorkwithDB.AddToDatabase()),

            new Button(
                "Отчет А",
                "Вычислить общую статистику по деталям и сборкам (количество уникальных, адаптивных и общее количество)",
                Ico.counts,
                "ЧелГУ",
                "Курсовая по базам данных",
                () => Macro.ProductInfoFirstReport()),

            new Button(
                "Отчет Б",
                "Узнать, сколько болтов, винтов, гаек и шайб в автомобиле (без учета стандартов, общее количество)",
                Ico.counts,
                "ЧелГУ",
                "Курсовая по базам данных",
                () => Macro.ProductInfoSecondReport()),

            new Button(
                "Отчет С",
                "Вывести сборки с адаптивными деталями, отсортированные по убыванию количества последних",
                Ico.counts,
                "ЧелГУ",
                "Курсовая по базам данных",
                () => Macro.DatabaseInfoThirdReport())
        };
        public override Plugin CreateInstance() => new MyPlugin(this, Buttons);
    }
}