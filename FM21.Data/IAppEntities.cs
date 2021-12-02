using FM21.Entities;
using Microsoft.EntityFrameworkCore;

namespace FM21.Data
{
    public interface IAppEntities
    {
        DbSet<SiteMaster> SiteMaster { get; set; }
    }
}