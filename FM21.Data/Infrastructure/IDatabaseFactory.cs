using System;

namespace FM21.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        AppEntities Get();
    }
}