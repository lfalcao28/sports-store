#region Usings

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace MVCGenericLibrary.Model.Helpers.Collections
{
    public class PagedList<T> : List<T>, IPagedList
    {

        private void CreatePagedList(int total, int pageSize, int pageIndex)
        {
            TotalCount = total;
            PageSize = pageSize;
            PageIndex = pageIndex;
            TotalPages = (int) Math.Ceiling(total/(double) pageSize);
        }

        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            CreatePagedList(source.Count(), pageSize, pageIndex);
            AddRange(source.Skip(pageIndex * pageSize).Take(pageSize));
        }

        public PagedList(IEnumerable<T> source, int total, int index, int pageSize)
        {
            CreatePagedList(total, pageSize, index);
            AddRange(source);
        }

        public int TotalPages { get; private set; }

        public int TotalCount { get; private set;  }

        public int PageIndex { get; private set; }

        public int PageSize { get; private set; }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 0); }
        }

        public bool HasNextPage
        {
            get { return (PageIndex + 1 < TotalPages); }
        }
    }
}