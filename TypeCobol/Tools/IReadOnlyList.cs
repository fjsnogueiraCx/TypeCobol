using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeCobol.Tools
{
    public interface IReadOnlyList<out T> : IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable
    {

    }
}
