#nullable enable

using System;

namespace WebMonk.Context
{
    public interface IPrefixManager
    {
        IDisposable NewPrefix(string prefix, object? parent);
        string CurrentPrefix { get; }
        object? CurrentParent { get; }
        object? RootParent { get; }
    }
}