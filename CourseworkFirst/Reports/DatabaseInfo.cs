using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseworkFirst
{
    public static class DatabaseInfo
    {
        public static string[] ThirdReport(this MyModel db)
        {
            var result = new List<Tuple<string, int>>();
            var dbSnapshot = db.Products.Where(p => (bool)p.IsAdaptive);

            foreach (var e in dbSnapshot)
            {
                var counts = e.ProductParts
                    .Where(p => (bool)p.Part.IsAdaptive)
                    .Select(p => p.Count)
                    .Sum();
                result.Add(Tuple.Create($"{e.Number}   {e.Name}", counts));
            }

            return result
                .OrderByDescending(t => t.Item2)
                .Select(t => $"{t.Item2}   {t.Item1}")
                .ToArray();
        }


    }
}