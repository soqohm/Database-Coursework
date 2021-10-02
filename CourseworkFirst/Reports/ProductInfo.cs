using System.Collections.Generic;
using System.Linq;
using TFlex.Model;
using TFlex.Model.Model3D;

namespace CourseworkFirst
{
    public static class ProductInfo
    {
        public static string[] FirstReport(this Document doc)
        {
            var docSnapshot = doc.BreadthSearch();
            return new string[]
            {
                "\r\nОшибки открытия файлов: " + (docSnapshot.Count() - docSnapshot.NoBroken().Count()) +
                " (влияют на дальнейший расчет)",
                "\r\n\nДеталей: \t" + docSnapshot.NoBroken().Part().Count(),
                "  уникальных: \t" + docSnapshot.NoBroken().Part().NoLibrary().Unique().Count(),
                "  адаптивных: \t" + docSnapshot.NoBroken().Adaptive().Part().Count(),
                "\r\nСборок: \t" + docSnapshot.NoBroken().Product().Count(),
                "  уникальных: \t" + docSnapshot.NoBroken().Product().NoLibrary().Unique().Count(),
                "  адаптивных: \t" + docSnapshot.NoBroken().Adaptive().Product().Count(),
                "\r\nВсего: \t\t" + docSnapshot.NoBroken().Count(),
                "  уникальных: \t" + docSnapshot.NoBroken().NoLibrary().Unique().Count(),
                "  адаптивных: \t" + docSnapshot.NoBroken().Adaptive().Count()
            };
        }

        public static string[] SecondReport(this Document doc)
        {
            var docSnapshot = doc.BreadthSearch();
            return new string[]
            {
            "\r\nИтого, в сборке:",
            "\r\n\r\nВсего элементов: \t" + docSnapshot.Count(),
            "\r\nБолтов: \t\t" + docSnapshot.NoBroken().FindInName("Болт").Count(),
            "\r\nВинтов: \t\t" + docSnapshot.NoBroken().FindInName("Винт").Count(),
            "\r\nГаек: \t\t\t" + docSnapshot.NoBroken().FindInName("Гайка").Count(),
            "\r\nШайб: \t\t\t" + docSnapshot.NoBroken().FindInName("Шайба").Count()
            };
        }

        public static IEnumerable<Fragment3D> FindInName(this IEnumerable<Fragment3D> fragments, string substring)
        {
            return fragments
                .Where(f => f.GetName().Contains(substring));
        }

        public static IEnumerable<Fragment3D> NoLibrary(this IEnumerable<Fragment3D> fragments)
        {
            return fragments
                .Where(f => f.FilePath[0] != '<');
        }

        public static IEnumerable<Fragment3D> Unique(this IEnumerable<Fragment3D> fragments)
        {
            return fragments
                .Distinct(new Fragment3DComparer());
        }

        public static IEnumerable<Fragment3D> Part(this IEnumerable<Fragment3D> fragments)
        {
            return fragments
                .Where(f => !f.IsProduct());
        }

        public static IEnumerable<Fragment3D> Product(this IEnumerable<Fragment3D> fragments)
        {
            return fragments
                .Where(f => f.IsProduct());
        }

        public static IEnumerable<Fragment3D> Adaptive(this IEnumerable<Fragment3D> fragments)
        {
            return fragments
                .Where(f => f.Associative);
        }

        public static IEnumerable<Fragment3D> NoBroken(this IEnumerable<Fragment3D> fragments)
        {
            return fragments
                .Where(f => f.GetDocument() != null);
        }

        public class Fragment3DComparer : IEqualityComparer<Fragment3D>
        {
            public bool Equals(Fragment3D f1, Fragment3D f2)
            {
                return f1.GetNumber() == f2.GetNumber();
            }

            public int GetHashCode(Fragment3D f)
            {
                return f.GetNumber().GetHashCode();
            }
        }


    }
}