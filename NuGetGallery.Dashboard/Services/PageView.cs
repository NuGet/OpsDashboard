using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuGetGallery.Dashboard.Services
{
    public class PageView<T>
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public IEnumerable<T> Entries { get; private set; }
        public int LastPage { get { return (int)Math.Ceiling((double)TotalCount / PageSize); } }
        public bool HasNext { get { return PageIndex < LastPage; } }
        public bool HasPrevious { get { return PageIndex > 0; } }

        public PageView(int pageIndex, int pageSize, int totalCount, IEnumerable<T> entries)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            Entries = entries;
        }
    }
}
