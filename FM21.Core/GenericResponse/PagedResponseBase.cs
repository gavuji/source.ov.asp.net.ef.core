namespace FM21.Core
{
    public abstract class PagedResponseBase : ResponseBase
    {
        protected PagedResponseBase()
            : base(ResultType.Success)
        {

        }

        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
    }
}