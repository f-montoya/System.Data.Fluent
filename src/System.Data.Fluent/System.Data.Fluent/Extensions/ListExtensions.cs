using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Data.Fluent
{
    internal static class ListExtensions
    {
        public static void AddRange(this IList list, IEnumerable values)
        {
            if (values == null) return;

            foreach(var value in values)
            {
                list.Add(value);
            }
        }

    }
}
