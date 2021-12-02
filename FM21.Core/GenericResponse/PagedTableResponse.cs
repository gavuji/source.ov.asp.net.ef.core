namespace FM21.Core
{
    public class PagedTableResponse<T> : PagedResponseBase where T : class
    {
        public T Data { get; set; }
    }
}