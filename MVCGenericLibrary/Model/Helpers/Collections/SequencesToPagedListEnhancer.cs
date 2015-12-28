namespace MVCGenericLibrary.Model.Helpers.Collections
{
    using System.Collections.Generic;
    using System.Linq;

    public static class SequencesToPagedListEnhancer
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int index, int pageSize)
        {
            return new PagedList<T>(source, index, pageSize);
        }

        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int index)
        {
            return new PagedList<T>(source, index, 2);
        }


        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int total, int index, int pageSize)
        {
            return new PagedList<T>(source, total, index, pageSize);
        }

        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int total, int index)
        {
            return new PagedList<T>(source, total, index, 2);
        }
    }
}