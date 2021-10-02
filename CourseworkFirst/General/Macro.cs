using System;
using System.Diagnostics;
using System.Windows.Forms;
using TFlex.Model;

namespace CourseworkFirst
{
    public class Macro
    {
        public static void New(Action<Document> newMacro)
        {
            var doc = TFlex.Application.ActiveDocument;
            if (doc == null)
                return;
            var sw = Stopwatch.StartNew();

            newMacro(doc);

            sw.Stop();
            var message = GetMessage(sw.ElapsedMilliseconds);
            MessageBox.Show(message, "Сообщение", MessageBoxButtons.OK);
        }

        public static void NewwithLog(Action<Document, string> newMacro, string logName)
        {
            New(doc =>
            {
                var path = doc.FilePath + logName;
                newMacro(doc, path);
                Logs.OpenLog(path);
            });
        }

        public static string GetMessage(long time)
        {
            var t = time / 1000;
            if (t < 60)
                return string.Format("Потрачено: {0} сек.", Math.Round((double)t, 1));
            else
                return string.Format("Потрачено: {0} мин.", Math.Round((double)t / 60, 1));
        }

        public static void ProductInfoFirstReport()
        {
            NewwithLog((doc, path) => ProductInfo.FirstReport(doc).WriteLog(path), "! ProductInfo_FirstReport.log");
        }

        public static void ProductInfoSecondReport()
        {
            NewwithLog((doc, path) => ProductInfo.SecondReport(doc).WriteLog(path), "! ProductInfo_SecondReport.log");
        }

        public static void DatabaseInfoThirdReport()
        {
            using (var db = new MyModel())
            {
                NewwithLog((doc, path) => DatabaseInfo.ThirdReport(db).WriteLog(path), "! DatabaseInfo_ThirdReport.log");
            }
        }

        
    }
}