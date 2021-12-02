using System.Threading.Tasks;

namespace FM21.Service
{
    public interface IBaseService
    {
        int? RequestUserID { get; }
        string RequestLanguage { get; }
        Task<int> Save();
    }
}