using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeCobol.Compiler.CodeElements;
using JetBrains.Annotations;

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
        void StartCobolProgram([NotNull]ProgramIdentification programIdentification, LibraryCopyCodeElement libraryCopy);

        /// <summary>
        /// End The current Cobol Program
        /// <param name="end">Optional ProgramEnd code elemnt</param>
        /// </summary>
        void EndCobolProgram(ProgramEnd end);

        /// <summary>
        /// Start an Environment Division 
        /// </summary>
        /// <param name="header">EnvironmentDivision Headercode element</param>
        void StartEnvironmentDivision([NotNull]EnvironmentDivisionHeader header);

        /// <summary>
        /// End an Environment Division 
        /// </summary>
        void EndEnvironmentDivision();

        /// <summary>
        /// Start a Configuration Section
        /// </summary>
        /// <param name="header">Configuration Section heser code element</param>
        void StartConfigurationSection([NotNull]ConfigurationSectionHeader header);

        /// <summary>
        /// End a configuration section header.
        /// </summary>
        void EndConfigurationSection();

        /// <summary>
        /// Start a Source Computer Paragraph
        /// </summary>
        /// <param name="paragraph">The Source Computer paragraph code element</param>
        void StartSourceComputerParagraph([NotNull]SourceComputerParagraph paragraph);

        /// <summary>
        /// End a Source Computer Paragraph
        /// </summary>
        void EndSourceComputerParagraph();

        /// <summary>
        /// Start a Object Computer Paragraph
        /// </summary>
        /// <param name="paragraph">The Object Computer paragraph code element</param>
        void StartObjectComputerParagraph([NotNull]ObjectComputerParagraph paragraph);

        /// <summary>
        /// End a Object Computer Paragraph
        /// </summary>
        void EndObjectComputerParagraph();

        /// <summary>
        /// Start a Special Names Paragraph
        /// </summary>
        /// <param name="paragraph">The Special Names paragraph code element</param>
        void StartSpecialNamesParagraph([NotNull]SpecialNamesParagraph paragraph);

        /// <summary>
        /// End a Special Names Paragraph
        /// </summary>
        void EndSpecialNamesParagraph();

        /// <summary>
        /// Start a Repository Paragraph
        /// </summary>
        /// <param name="paragraph">The Repository paragraph code element</param>
        void StartRepositoryParagraph([NotNull]RepositoryParagraph paragraph);

        /// <summary>
        /// End a Repository Paragraph
        /// </summary>
        void EndRepositoryParagraph();

        /// <summary>
        /// Start an Input Output Section
        /// </summary>
        /// <param name="header">The Input Output Section header code element</param>
        void StartInputOutputSection([NotNull] InputOutputSectionHeader header);

        /// <summary>
        /// End an Input Output Section
        /// </summary>
        void EndInputOutputSection();

        /// <summary>
        /// Start a File Control Paragraph
        /// </summary>
        /// <param name="header">The File Control Paragraph header code element</param>
        void StartFileControlParagraph([NotNull] FileControlParagraphHeader header);

        /// <summary>
        /// End a File Control Paragraph
        /// </summary>
        void EndFileControlParagraph();

        /// <summary>
        /// Start a File Control Entry
        /// </summary>
        /// <param name="entry">The File Control Entry code element</param>
        void StartFileControlEntry([NotNull]FileControlEntry entry);

        /// <summary>
        /// End a File Control Entry
        /// </summary>
        void EndFileControlEntry();

        /// <summary>
        /// Start a data division
        /// </summary>
        /// <param name="header">Data Division header Code Element</param>
        void StartDataDivision([NotNull]DataDivisionHeader header);

        /// <summary>
        /// End a Data division
        /// </summary>
        void EndDataDivision();

        /// <summary>
        /// parent: DATA DIVISION
        /// Start a File Section
        /// </summary>
        /// <param name="header">File Section code element</param>
        void StartFileSection([NotNull]FileSectionHeader header);

        /// <summary>
        /// End a File Section
        /// </summary>
        void EndFileSection();

        /// <summary>
        /// Start a File Description Entry
        /// </summary>
        /// <param name="entry">The File Description Entry code element</param>
        void StartFileDescriptionEntry([NotNull] FileDescriptionEntry entry);

        /// <summary>
        /// End a File Description Entry
        /// </summary>
        void EndFileDescriptionEntry();

        /// <summary>
        /// Start DataDescriptionEntry
        /// </summary>
        /// <param name="entry">The DataDescriptionEntry code element</param>
        void StartDataDescriptionEntry([NotNull] DataDescriptionEntry entry);
        /// <summary>
        /// Start DataRedefinesEntry
        /// </summary>
        /// <param name="entry">The DataRedefinesEntry code element</param>
        void StartDataRedefinesEntry([NotNull] DataRedefinesEntry entry);
        /// <summary>
        /// Start DataRenamesEntry
        /// </summary>
        /// <param name="entry">The DataRenamesEntry code element</param>
        void StartDataRenamesEntry([NotNull] DataRenamesEntry entry);
        /// <summary>
        /// Start DataConditionEntry
        /// </summary>
        /// <param name="entry">The DataConditionEntry code element</param>
        void StarDataConditionEntry([NotNull] DataConditionEntry entry);

        /// <summary>
        /// Start a TypeCobol Type Definition entry
        /// </summary>
        /// <param name="typedef">The Type Definition Entry code element</param>
        void StartTypeDefinitionEntry([NotNull] DataTypeDescriptionEntry typedef);

        /// <summary>
        /// Start a WorkingStorageSection
        /// </summary>
        /// <param name="header">WorkingStorageSection Header code element</param>
        void StartWorkingStorageSection([NotNull] WorkingStorageSectionHeader header);

        /// <summary>
        /// End a WorkingStorageSection
        /// </summary>
        void EndWorkingStorageSection();

        /// <summary>
        /// Start a LocalStorageSection
        /// </summary>
        /// <param name="header">LocalStorageSection Header code element</param>
        void StartLocalStorageSection([NotNull] LocalStorageSectionHeader header);

        /// <summary>
        /// End a WorkingStorageSection
        /// </summary>
        void EndLocalStorageSection();

        /// <summary>
        /// Start a LinkageSection
        /// </summary>
        /// <param name="header">LinkageSection Header code element</param>
        void StartLinkageSection([NotNull] LinkageSectionHeader header);

        /// <summary>
        /// End a LinkageSection
        /// </summary>
        void EndLinkageSection();

        /// <summary>
        /// Start a Procedure Division
        /// </summary>
        /// <param name="header">The Procedure Division header code element</param>
        void StartProcedureDivision([NotNull] ProcedureDivisionHeader header);

        /// <summary>
        /// End a Procedure Division
        /// </summary>
        void EndProcedureDivision();

        /// <summary>
        /// Start Declaratives
        /// </summary>
        /// <param name="header">DeclarativesHeader code element</param>
        void StartDeclaratives([NotNull] DeclarativesHeader header);

        /// <summary>
        /// End Declaratives
        /// <param name="end">The DeclarativesEnd code element</param>
        /// </summary>
        void EndDeclaratives([NotNull] DeclarativesEnd end);

        /// <summary>
        /// Parent node: PROCEDURE DIVISION
        /// Start a DECLARE FUNCTION
        /// </summary>
        /// <param name="header">The Function Declaration Header cod element</param>
        void StartFunctionDeclaration([NotNull] FunctionDeclarationHeader header);

        /// <summary>
        /// Parent node: PROCEDURE DIVISION
        /// End a FUNCTION DECLARATION
        /// </summary>
        /// <param name="end">The FUNCTION DECLARE end code element</param>
        void EndFunctionDeclaration([NotNull] FunctionDeclarationEnd end);

        /// <summary>
        /// Parent node: DECLARE FUNCTION
        /// Start a Function PROCEDURE DIVISION
        /// </summary>
        /// <param name="header">The Function PROCEDURE DIVISION header code element</param>
        void StartFunctionProcedureDivision([NotNull] ProcedureDivisionHeader header);

        /// <summary>
        /// End a Function PROCEDURE DIVISION
        /// </summary>
        void EndFunctionProcedureDivision();
    }
}
