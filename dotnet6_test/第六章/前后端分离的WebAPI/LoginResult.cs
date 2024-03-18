using System.Diagnostics;

namespace 前后端分离的WebAPI
{
    public record LoginResult(bool IsOk, ProcessInfo[] Processes);
}
