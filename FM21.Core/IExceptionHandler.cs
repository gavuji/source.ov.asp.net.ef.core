using System;

namespace FM21.Core
{
    public interface IExceptionHandler
    {
        void LogInformation(string Message);

        void LogError(Exception ex);

        void LogWarning(string Message);
    }
}