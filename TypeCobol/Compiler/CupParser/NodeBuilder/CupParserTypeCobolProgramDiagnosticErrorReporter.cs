using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUVienna.CS_CUP.Runtime;

namespace TypeCobol.Compiler.CupParser.NodeBuilder
{
    /// <summary>
    /// TypeCobol Program Diagnosctic Error Reporter.
    /// </summary>
    public class CupParserTypeCobolProgramDiagnosticErrorReporter : ICupParserErrorReporter
    {
        public bool ReportFatalError(lr_parser parser, string message, object info)
        {
            throw new NotImplementedException();
        }

        public bool ReportError(lr_parser parser, string message, object info)
        {
            throw new NotImplementedException();
        }

        public bool SyntaxError(lr_parser parser, Symbol curToken)
        {
            throw new NotImplementedException();
        }

        public bool UnrecoveredSyntaxError(lr_parser parser, Symbol curToken)
        {
            throw new NotImplementedException();
        }
    }
}
