using Serilog;
using System;

namespace FM21.Core
{
    public class ExceptionHandler : IExceptionHandler
    {
        public void LogInformation(string Message)
        {
            Log.Information("[Information] Message :" + Message);
        }

        public void LogError(Exception ex)
        {
            Log.Fatal(Environment.NewLine + "[Error] Message :" + ex.Message
                + Environment.NewLine + "Stack Trace :" + ex.StackTrace
                + Environment.NewLine + "Inner Exception :" + ex.InnerException + Environment.NewLine);
        }

        public void LogWarning(string Message)
        {
            Log.Warning("[Warning] Message :" + Message);
        }
    }
}