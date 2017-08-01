using System.Collections;
using System.Collections.Generic;

namespace TypeCobol.Tools
{
    public interface IReadOnlyCollection<out T> : IEnumerable<T>, IEnumerable
    {
        int Count { get; }
        T this[int index] { get; }
    }
}