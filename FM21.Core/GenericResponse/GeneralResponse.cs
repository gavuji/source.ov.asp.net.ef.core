namespace FM21.Core
{
    public class GeneralResponse<T> : ResponseBase
    {
        public T Data { get; set; }
     
        public GeneralResponse()
            :base(ResultType.Success)
        {

        }
    }

}