using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeCobol.Compiler.CodeElements;

namespace TypeCobol.Compiler.CupParser.NodeBuilder
{
    /// <summary>
    /// Interface of a Program class builder based
    /// </summary>
    public interface IProgramClassBuilder
    {
        /// <summary>
        /// Start a Cobol Program
        /// </summary>
        /// <param name="programIdentification">The Program Identification Code Element</param>
        /// <param name="libraryCopy">The Library Copy Code Element</param>
        void StartCobolProgram(ProgramIdentification programIdentification, LibraryCopyCodeElement libraryCopy);

        /// <summary>
        /// End The current Cobol Program
        /// </summary>
        void EndCobolProgram();
    }
}
