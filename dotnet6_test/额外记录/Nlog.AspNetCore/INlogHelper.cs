using System;

namespace Nlog.AspNetCore
{
    public interface INLogHelper
    {
        void LogError(Exception ex);
    }
}