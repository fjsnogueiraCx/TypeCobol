using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeCobol.LanguageServices.Editor
{
    public class DiagnosticsEnventArgs : EventArgs
    {
        public IEnumerable<Compiler.Diagnostics.Diagnostic> Diagnostics { get; set; }
    }

    public class MissingCopiesEventArgs : EventArgs
    {
        public List<string> Copies { get; set; }
    }

    public class LoadingIssueEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
