using System.Collections.Generic;

namespace FM21.Core
{
    public class PagedEntityResponse<T> : PagedResponseBase where T : class
    {
        public IList<T> Data { get; set; }

        public PagedEntityResponse()
        {
            Data = new List<T>();
        }
    }
}