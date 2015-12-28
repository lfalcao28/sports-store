namespace MVCGenericLibrary.Model.Helpers.Collections
{
    public interface IPagedList
    {
        int TotalCount { get; }
        int TotalPages { get; }
        int PageIndex { get; }

        int PageSize { get; }

        bool HasPreviousPage { get; }

        bool HasNextPage { get; }
    }
}