using System.Collections.Generic;
using System.IO;
using System.Linq;
using TFlex.Model;
using TFlex.Model.Model3D;

namespace CourseworkFirst
{
    public static class WorkwithDB
    {
        public static void AddToDatabase()
        {
            var document = TFlex.Application.ActiveDocument;
            if (document == null)
                return;

            var path = document.FilePath + "! лог записи в БД.log";
            if (File.Exists(path))
                File.Delete(path);

            var queue = new Queue<string>(document.GetProductLinks());
            while (queue.Count != 0)
            {
                using (var db = new MyModel())
                {
                    var doc = MyExtensions.GetDocument(queue.Dequeue(), true);
                    doc.GetProductPart().AddProductPart(db, path);
                }
            }
        }

        public static void AddProductPart(this List<ProductPart> productparts, MyModel db, string logPath)
        {
            var product = productparts.First().Product.Sync(db);

            foreach (var pp in productparts)
            {
                pp.Part = pp.Part.Sync(db);
                pp.Product = product;

                if (pp.Sync(db) == null)
                {
                    File.AppendAllText(logPath, pp.Parse() + "\r\n");
                    db.ProductParts.Add(pp);
                }
                db.SaveChanges();
            }
        }

        public static Part GetPart(this Fragment3D f)
        {
            var doc = f.GetDocument();

            return new Part
            {
                Number = doc.GetVariable("$Обозначение"),
                Name = doc.GetVariable("$Наименование"),
                IsAdaptive = f.Associative
            };
        }

        public static Product GetProduct(this Document doc)
        {
            return new Product
            {
                Number = doc.GetVariable("$Обозначение"),
                Name = doc.GetVariable("$Наименование"),
                IsAdaptive = doc.IsAdaptive()
            };
        }

        public static bool IsAdaptive(this Document doc)
        {
            return doc.GetFragments3D().Where(f => f.Associative).FirstOrDefault() != null;
        }

        public static List<ProductPart> GetProductPart(this Document doc)
        {
            var product = doc.GetProduct();

            return doc.GetFragments3D()
                .Select(f => f.GetPart())
                .GroupBy(f => f.Number)
                .Select(group => new ProductPart
                {
                    Part = group.FirstOrDefault(),
                    Count = group.Count(),
                    Product = product
                })
                .ToList();
        }

        public static Part Sync(this Part part, MyModel db)
        {
            var partfromdb = db.Parts
                .Where(p => p.Number == part.Number)
                .FirstOrDefault();

            if (partfromdb != null)
                return partfromdb;
            return part;
        }

        public static Product Sync(this Product product, MyModel db)
        {
            var productfromdb = db.Products
                .Where(p => p.Number == product.Number)
                .FirstOrDefault();

            if (productfromdb != null)
                return productfromdb;
            return product;
        }

        public static ProductPart Sync(this ProductPart pp, MyModel db)
        {
            return db.ProductParts
                .Where(p => p.Product.Number == pp.Product.Number && p.Part.Number == pp.Part.Number)
                .FirstOrDefault();
        }

        public static string Parse(this ProductPart pp)
        {
            return $"{pp.Part.Number}  {pp.Count}  {pp.Product.Number}";
        }
    }
}