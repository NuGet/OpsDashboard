using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NuGetGallery.Dashboard.Views.Helpers
{
    public static class LinqExtensions
    {
        public static IEnumerable<IGrouping<int, T>> GroupIn<T>(this IEnumerable<T> self, int groupSize)
        {
            return self.Select((val, idx) => Tuple.Create(idx / groupSize, val))
                       .GroupBy(t => t.Item1, t => t.Item2)
                       .OrderBy(g => g.Key);
        }
    }
}