using System;
using System.Collections.Generic;
using System.Linq;
using TFlex.Model;
using TFlex.Model.Model3D;

namespace CourseworkFirst
{
    public static class MyExtensions
    {
        public static IEnumerable<Fragment3D> BreadthSearch(this Document document)
        {
            var queue = new Queue<Document>();
            queue.Enqueue(document);
            while (queue.Count != 0)
            {
                var doc = queue.Dequeue();
                foreach (var f in doc.GetFragments3D())
                {
                    if (f.IsProduct())
                    {
                        var fDoc = f.GetFragmentDocument(true, false);
                        queue.Enqueue(fDoc);
                    }
                    yield return f;
                }
            }
        }

        public static Document GetDocument(string docPath, bool with3D)
        {
            return TFlex.Application.OpenFragmentDocument(docPath, with3D, false);
        }

        public static Document GetDocument(this Fragment3D f)
        {
            return TFlex.Application.OpenFragmentDocument(f.FullFilePath, false, false);
        }

        public static bool IsProduct(this Fragment3D f)
        {
            return GetExternalLinks(f.FullFilePath, false)
                .FirstOrDefault() != null;
        }

        public static string[] GetExternalLinks(string path, bool firstLevel)
        {
            return TFlex.Application.GetDocumentExternalFileLinks(path, true, false, firstLevel);
        }

        public static string[] GetProductLinks(this Document doc)
        {
            return GetExternalLinks(doc.FileName, true)
                .Where(str => GetExternalLinks(str, false).FirstOrDefault() != null)
                .Concat(new string[] { doc.FileName })
                .ToArray();
        }

        public static string GetVariable(this Document doc, string name)
        {
            return doc.FindVariable(name).TextValue;
        }

        public static string GetNumber(this Fragment3D f)
        {
            return f.GetDocument().GetVariable("$Обозначение");
        }

        public static string GetName(this Fragment3D f)
        {
            return f.GetDocument().GetVariable("$Наименование");
        }


    }
}