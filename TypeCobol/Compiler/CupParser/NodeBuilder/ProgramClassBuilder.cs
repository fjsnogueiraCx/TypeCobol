﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using TypeCobol.Compiler.CodeElements;
using TypeCobol.Compiler.CodeModel;
using TypeCobol.Compiler.Nodes;
using TypeCobol.Compiler.Parser;
using Analytics;

namespace TypeCobol.Compiler.CupParser.NodeBuilder
{
    /// <summary>
    /// The Program Class Builder
    /// </summary>
    public class ProgramClassBuilder : IProgramClassBuilder
    {
        /// <summary>
        /// Program object resulting of the visit the parse tree
        /// </summary>
        private Program Program { get; set; }
        public SyntaxTree<CodeElement> SyntaxTree { get; set; }

        // Programs can be nested => track current programs being analyzed
        private Stack<Program> programsStack = null;

        private Program CurrentProgram
        {
            get { return programsStack.Peek(); }
            set { programsStack.Push(value); }
        }

        /// <summary>Class object resulting of the visit the parse tree</summary>
        public CodeModel.Class Class { get; private set; }

        private readonly SymbolTable TableOfIntrisic = new SymbolTable(null, SymbolTable.Scope.Intrinsic);
        private SymbolTable TableOfGlobals;
        private SymbolTable TableOfNamespaces;

        public SymbolTable CustomSymbols
        {
            private get { throw new InvalidOperationException(); }
            set
            {
                if (value != null)
                {
                    SymbolTable intrinsicTable = value.GetTableFromScope(SymbolTable.Scope.Intrinsic);
                    SymbolTable nameSpaceTable = value.GetTableFromScope(SymbolTable.Scope.Namespace);

                    intrinsicTable.DataEntries.Values.ToList().ForEach(d => d.ForEach(da => da.SetFlag(Node.Flag.NodeIsIntrinsic, true)));
                    intrinsicTable.Types.Values.ToList().ForEach(d => d.ForEach(da => da.SetFlag(Node.Flag.NodeIsIntrinsic, true)));
                    intrinsicTable.Functions.Values.ToList().ForEach(d => d.ForEach(da => da.SetFlag(Node.Flag.NodeIsIntrinsic, true)));

                    TableOfIntrisic.CopyAllDataEntries(intrinsicTable.DataEntries.Values);
                    TableOfIntrisic.CopyAllTypes(intrinsicTable.Types);
                    TableOfIntrisic.CopyAllFunctions(intrinsicTable.Functions, AccessModifier.Public);

                    if (nameSpaceTable != null)
                    {
                        TableOfNamespaces = new SymbolTable(TableOfIntrisic, SymbolTable.Scope.Namespace); //Set TableOfNamespace with program if dependencies were given. (See CLI.cs runOnce2() LoadDependencies())
                        TableOfNamespaces.CopyAllPrograms(nameSpaceTable.Programs.Values);
                    }

                }
                // TODO#249: use a COPY for these
                foreach (var type in DataType.BuiltInCustomTypes)
                    TableOfIntrisic.AddType(DataType.CreateBuiltIn(type)); //Add default TypeCobol types BOOLEAN and DATE
            }
        }



        public NodeDispatcher<CodeElement> Dispatcher { get; internal set; }

        public Node CurrentNode { get { return SyntaxTree.CurrentNode; } }

        public Dictionary<CodeElement, Node> NodeCodeElementLinkers = new Dictionary<CodeElement, Node>();

        private void Enter([NotNull] Node node, CodeElement context = null, SymbolTable table = null)
        {
            node.SymbolTable = table ?? SyntaxTree.CurrentNode.SymbolTable;
            SyntaxTree.Enter(node, context);

            if (node.CodeElement != null)
                NodeCodeElementLinkers.Add(node.CodeElement, node);
        }

        private void Exit()
        {
            var node = SyntaxTree.CurrentNode;
            var context = SyntaxTree.CurrentContext;
            Dispatcher.OnNode(node, context, CurrentProgram);
            SyntaxTree.Exit();
        }

        private void AttachEndIfExists(CodeElementEnd end)
        {
            if (end == null) return;
            Enter(new End(end));
            Exit();
        }

        private Node GetTopLevelItem(long level)
        {
            var parent = CurrentNode;
            while (parent != null)
            {
                var data = parent.CodeElement as DataDefinitionEntry;
                if (data == null) return null;
                if (data.LevelNumber == null || data.LevelNumber.Value < level) return parent;
                parent = parent.Parent;
            }
            return null;
        }

        /// <summary>Exit() every Node that is not the top-level item for a data of a given level.</summary>
        /// <param name="levelnumber">
        /// Level number of the next data definition that will be Enter()ed.
        /// If null, a value of 1 is assumed.
        /// </param>
        private void SetCurrentNodeToTopLevelItem(IntegerValue levelnumber)
        {
            long level = levelnumber != null ? levelnumber.Value : 1;
            Node parent;

            if (level == 1 || level == 77)
            {
                parent = null;
            }
            else
            {
                parent = GetTopLevelItem(level);
            }
            if (parent != null)
            {
                // Exit() previous sibling and all of its last children
                while (parent != CurrentNode) Exit();
            }
            else
            {
                ExitLastLevel1Definition();
            }
        }

        /// <summary>Exit last level-01 data definition entry, as long as all its subordinates.</summary>
        private void ExitLastLevel1Definition()
        {
            while (CurrentNode.CodeElement != null && TypeCobol.Tools.Reflection.IsTypeOf(CurrentNode.CodeElement.GetType(), typeof(DataDefinitionEntry))) Exit();
        }

        private void AddToSymbolTable(DataDescription node)
        {
            if (node.IsPartOfATypeDef) return;
            var table = node.SymbolTable;
            if (node.CodeElement().IsGlobal)
                table = table.GetTableFromScope(SymbolTable.Scope.Global);

            table.AddVariable(node);
        }

        public virtual void StartCobolProgram(ProgramIdentification programIdentification, LibraryCopyCodeElement libraryCopy)
        {
            if (Program == null)
            {
                Program = new SourceProgram(TableOfGlobals, programIdentification);
                programsStack = new Stack<Program>();
                CurrentProgram = Program;
                Enter(CurrentProgram, programIdentification, CurrentProgram.SymbolTable);
            }
            else
            {
                var enclosing = CurrentProgram;
                CurrentProgram = new NestedProgram(enclosing, programIdentification);
                Enter(CurrentProgram, programIdentification, CurrentProgram.SymbolTable);
            }

            if (libraryCopy != null)
            { // TCRFUN_LIBRARY_COPY
                var cnode = new LibraryCopy(libraryCopy);
                Enter(cnode, libraryCopy, CurrentProgram.SymbolTable);
                Exit();
            }

            TableOfNamespaces.AddProgram(CurrentProgram); //Add Program to Namespace table. 
        }

        public virtual void EndCobolProgram(TypeCobol.Compiler.CodeElements.ProgramEnd end)
        {
            AttachEndIfExists(end);
            Exit();
            programsStack.Pop();

            if (programsStack.Count == 0) //Means that we ended a main program, reset Table and program in case of a new program declaration before EOF. 
            {
                TableOfGlobals = new SymbolTable(TableOfNamespaces, SymbolTable.Scope.Global);
                Program = null;
            }
        }

        public virtual void StartEnvironmentDivision(EnvironmentDivisionHeader header)
        {
            Enter(new EnvironmentDivision(header), header);
        }

        public virtual void EndEnvironmentDivision()
        {
            Exit();
        }

        public virtual void StartConfigurationSection(ConfigurationSectionHeader header)
        {
            Enter(new ConfigurationSection(header), header);
        }

        public virtual void EndConfigurationSection()
        {
            Exit();
        }

        public virtual void StartSourceComputerParagraph(SourceComputerParagraph paragraph)
        {
            Enter(new SourceComputer(paragraph));
        }

        public virtual void EndSourceComputerParagraph()
        {
            Exit();
        }

        public virtual void StartObjectComputerParagraph(ObjectComputerParagraph paragraph)
        {
            Enter(new ObjectComputer(paragraph));
        }

        public virtual void EndObjectComputerParagraph()
        {
            Exit();
        }

        public virtual void StartSpecialNamesParagraph(SpecialNamesParagraph paragraph)
        {
            Enter(new SpecialNames(paragraph));
        }

        public virtual void EndSpecialNamesParagraph()
        {
            Exit();
        }

        public virtual void StartRepositoryParagraph(RepositoryParagraph paragraph)
        {
            Enter(new Repository(paragraph));
        }

        public virtual void EndRepositoryParagraph()
        {
            Exit();
        }

        public virtual void StartInputOutputSection(InputOutputSectionHeader header)
        {
            Enter(new InputOutputSection(header), header);
        }

        public virtual void EndInputOutputSection()
        {
            Exit();
        }

        public virtual void StartFileControlParagraph(FileControlParagraphHeader header)
        {
            Enter(new FileControlParagraphHeaderNode(header), header);
        }

        public virtual void EndFileControlParagraph()
        {
            Exit();
        }

        public virtual void StartFileControlEntry(FileControlEntry entry)
        {
            var fileControlEntry = new FileControlEntryNode(entry);
            Enter(fileControlEntry, entry);
        }

        public virtual void EndFileControlEntry()
        {
            Exit();
        }

        public virtual void StartDataDivision(DataDivisionHeader header)
        {
            Enter(new DataDivision(header), header);
        }

        public virtual void EndDataDivision()
        {
            Exit();
        }

        public virtual void StartFileSection(FileSectionHeader header)
        {
            Enter(new FileSection(header), header);
        }

        public virtual void EndFileSection()
        {
            ExitLastLevel1Definition();
            Exit();
        }

        public virtual void StartFileDescriptionEntry(FileDescriptionEntry entry)
        {
            Enter(new FileDescriptionEntryNode(entry), entry);
        }

        public virtual void EndFileDescriptionEntry()
        {
            Exit();
        }

        public virtual void StartDataDescriptionEntry(DataDescriptionEntry entry)
        {
            var dataTypeDescriptionEntry = entry as DataTypeDescriptionEntry;
            if (dataTypeDescriptionEntry != null) StartTypeDefinitionEntry(dataTypeDescriptionEntry);
            else
            {
                SetCurrentNodeToTopLevelItem(entry.LevelNumber);

                //Update DataType of CodeElement by searching info on the declared Type into SymbolTable.
                //Note that the AST is not complete here, but you can only refer to a Type that has previously been defined.
                var node = new DataDescription(entry);
                Enter(node);

                if (entry.Indexes != null && entry.Indexes.Any())
                {
                    var table = node.SymbolTable;
                    foreach (var index in entry.Indexes)
                    {
                        if (node.CodeElement().IsGlobal)
                            table = table.GetTableFromScope(SymbolTable.Scope.Global);

                        var indexNode = new IndexDefinition(index);
                        Enter(indexNode, null, table);
                        if (!indexNode.IsPartOfATypeDef) //If index is inside a Typedef do not add to symboltable
                            table.AddVariable(indexNode);
                        Exit();
                    }
                }

                var types = node.SymbolTable.GetType(node);
                if (types.Count == 1)
                {
                    entry.DataType.RestrictionLevel = types[0].DataType.RestrictionLevel;
                }
                //else do nothing, it's an error that will be handled by Cobol2002Checker

                var parent = node.Parent;
                while (parent != null)
                {
                    if (parent is WorkingStorageSection)
                    {
                        //Set flag to know that this node belongs to working storage section
                        node.SetFlag(Node.Flag.WorkingSectionNode, true);
                        break;
                    }
                    else if (parent is LinkageSection)
                    {
                        //Set flag to know that this node belongs to linkage section
                        node.SetFlag(Node.Flag.LinkageSectionNode, true);
                        break;
                    }
                    else if (parent is LocalStorageSection)
                    {
                        //Set flag to know that this node belongs to local storage section
                        node.SetFlag(Node.Flag.LocalStorageSectionNode, true);
                        break;
                    }
                    else if (parent is FileSection)
                    {
                        node.SetFlag(Node.Flag.FileSectionNode, true);
                        break;
                    }
                    parent = parent.Parent;
                }

                AddToSymbolTable(node);
            }
        }

        public virtual void StartDataRedefinesEntry(DataRedefinesEntry entry)
        {
            SetCurrentNodeToTopLevelItem(entry.LevelNumber);
            var node = new DataRedefines(entry);
            Enter(node);
            if (!node.IsPartOfATypeDef) node.SymbolTable.AddVariable(node);
        }

        public virtual void StartDataRenamesEntry(DataRenamesEntry entry)
        {
            SetCurrentNodeToTopLevelItem(entry.LevelNumber);
            var node = new DataRenames(entry);
            Enter(node);
            if (!node.IsPartOfATypeDef) node.SymbolTable.AddVariable(node);
        }

        public virtual void StarDataConditionEntry(DataConditionEntry entry)
        {
            SetCurrentNodeToTopLevelItem(entry.LevelNumber);
            var node = new DataCondition(entry);
            Enter(node);
            if (!node.IsPartOfATypeDef) node.SymbolTable.AddVariable(node);
        }

        public virtual void StartTypeDefinitionEntry(DataTypeDescriptionEntry typedef)
        {
            SetCurrentNodeToTopLevelItem(typedef.LevelNumber);
            var node = new TypeDefinition(typedef);
            Enter(node);
            SymbolTable table;
            if (node.CodeElement().IsGlobal) // TCTYPE_GLOBAL_TYPEDEF
                table = node.SymbolTable.GetTableFromScope(SymbolTable.Scope.Global);
            else
                table = node.SymbolTable.GetTableFromScope(SymbolTable.Scope.Declarations);
            table.AddType(node);

            AnalyticsWrapper.Telemetry.TrackEvent("[Type-Declared] " + node.Name, EventType.TypeCobolUsage);
        }

        public virtual void StartWorkingStorageSection(WorkingStorageSectionHeader header)
        {
            Enter(new WorkingStorageSection(header), header);
        }

        public virtual void EndWorkingStorageSection()
        {
            ExitLastLevel1Definition();
            Exit(); // Exit WorkingStorageSection
        }

        public virtual void StartLocalStorageSection(LocalStorageSectionHeader header)
        {
            Enter(new LocalStorageSection(header), header);
        }

        public virtual void EndLocalStorageSection()
        {
            ExitLastLevel1Definition();
            Exit(); // Exit LocalStorageSection
        }

        public virtual void StartLinkageSection(LinkageSectionHeader header)
        {
            Enter(new LinkageSection(header), header);
        }

        public virtual void EndLinkageSection()
        {
            ExitLastLevel1Definition();
            Exit(); // Exit LinkageSection
        }

        public virtual void StartProcedureDivision(ProcedureDivisionHeader header)
        {
            Enter(new ProcedureDivision(header), header);
        }

        public virtual void EndProcedureDivision()
        {
            Exit();
        }

        public virtual void StartDeclaratives(DeclarativesHeader header)
        {
            Enter(new Declaratives(header), header);
        }

        public virtual void EndDeclaratives(DeclarativesEnd end)
        {
            AttachEndIfExists(end);
            Exit();
        }

        private Tools.UIDStore uidfactory = new Tools.UIDStore();
        public virtual void StartFunctionDeclaration(FunctionDeclarationHeader header)
        {
            header.SetLibrary(CurrentProgram.Identification.ProgramName.Name);
            var node = new FunctionDeclaration(header)
            {
                Label = uidfactory.FromOriginal(header?.FunctionName.Name),
                Library = CurrentProgram.Identification.ProgramName.Name
            };
            //DO NOT change this without checking all references of Library. 
            // (SymbolTable - function, type finding could be impacted) 

            //Function must be added to Declarations scope
            var declarationSymbolTable = SyntaxTree.CurrentNode.SymbolTable.GetTableFromScope(SymbolTable.Scope.Declarations);
            declarationSymbolTable.AddFunction(node);
            Enter(node, header, new SymbolTable(declarationSymbolTable, SymbolTable.Scope.Function));

            var declaration = (FunctionDeclarationHeader)CurrentNode.CodeElement;
            var funcProfile = ((FunctionDeclaration)CurrentNode).Profile; //Get functionprofile to set parameters

            foreach (var parameter in declaration.Profile.InputParameters) //Set Input Parameters
            {
                var paramNode = new ParameterDescription(parameter);
                paramNode.SymbolTable = CurrentNode.SymbolTable;
                paramNode.SetFlag(Node.Flag.LinkageSectionNode, true);
                funcProfile.InputParameters.Add(paramNode);

                paramNode.SetParent(CurrentNode);
                CurrentNode.SymbolTable.AddVariable(paramNode);
            }
            foreach (var parameter in declaration.Profile.OutputParameters) //Set Output Parameters
            {
                var paramNode = new ParameterDescription(parameter);
                paramNode.SymbolTable = CurrentNode.SymbolTable;
                paramNode.SetFlag(Node.Flag.LinkageSectionNode, true);
                funcProfile.OutputParameters.Add(paramNode);

                paramNode.SetParent(CurrentNode);
                CurrentNode.SymbolTable.AddVariable(paramNode);
            }
            foreach (var parameter in declaration.Profile.InoutParameters) //Set Inout Parameters
            {
                var paramNode = new ParameterDescription(parameter);
                paramNode.SymbolTable = CurrentNode.SymbolTable;
                paramNode.SetFlag(Node.Flag.LinkageSectionNode, true);
                funcProfile.InoutParameters.Add(paramNode);

                paramNode.SetParent(CurrentNode);
                CurrentNode.SymbolTable.AddVariable(paramNode);
            }

            if (declaration.Profile.ReturningParameter != null) //Set Returning Parameters
            {
                var paramNode = new ParameterDescription(declaration.Profile.ReturningParameter);
                paramNode.SymbolTable = CurrentNode.SymbolTable;
                paramNode.SetFlag(Node.Flag.LinkageSectionNode, true);
                ((FunctionDeclaration)CurrentNode).Profile.ReturningParameter = paramNode;

                paramNode.SetParent(CurrentNode);
                CurrentNode.SymbolTable.AddVariable(paramNode);
            }

            AnalyticsWrapper.Telemetry.TrackEvent("[Function-Declared] " + declaration.FunctionName, EventType.TypeCobolUsage);
        }

        public virtual void EndFunctionDeclaration(FunctionDeclarationEnd end)
        {
            Enter(new FunctionEnd(end), end);
            Exit();
            Exit();// exit DECLARE FUNCTION
        }

        public virtual void StartFunctionProcedureDivision(ProcedureDivisionHeader header)
        {
            if (header.UsingParameters != null && header.UsingParameters.Count > 0)
                DiagnosticUtils.AddError(header, "TCRFUN_DECLARATION_NO_USING");//TODO#249

            Enter(new ProcedureDivision(header), header);
        }

        public virtual void EndFunctionProcedureDivision()
        {
            Exit();
        }

        public virtual void StartSection(SectionHeader header)
        {
            var section = new Section(header);
            Enter(section, header);
            section.SymbolTable.AddSection(section);
        }

        public virtual void EndSection()
        {
            Exit();
        }

        public virtual void StartParagraph(ParagraphHeader header)
        {
            var paragraph = new Paragraph(header);
            Enter(paragraph, header);
            paragraph.SymbolTable.AddParagraph(paragraph);
        }

        public virtual void EndParagraph()
        {
            Exit();
        }

        public virtual void StartSentence()
        {
            Enter(new Sentence(), null);
        }

        public virtual void EndSentence(SentenceEnd end)
        {
            AttachEndIfExists(end);
            if (CurrentNode is Sentence) Exit();//TODO remove this and check what happens when exiting last CALL in FIN-STANDARD in BigBatch file (ie. CheckPerformance test)
        }

        public virtual void StartExecStatement(ExecStatement execStmt)
        {
            Enter(new Exec(execStmt), execStmt);
        }

        public virtual void EndExecStatement()
        {
            Exit();
        }

        public virtual void OnContinueStatement(ContinueStatement stmt)
        {
            Enter(new Continue(stmt), stmt);
            Exit();
        }

        public virtual void OnEntryStatement(EntryStatement stmt)
        {
            Enter(new Entry(stmt), stmt);
            Exit();
        }

        public virtual void OnAcceptStatement(AcceptStatement stmt)
        {
            Enter(new Accept(stmt), stmt);
            Exit();
        }

        public virtual void OnInitializeStatement(InitializeStatement stmt)
        {
            Enter(new Initialize(stmt), stmt);
            Exit();
        }

        public virtual void OnInspectStatement(InspectStatement stmt)
        {
            Enter(new Inspect(stmt), stmt);
            Exit();
        }

        public virtual void OnMoveStatement(MoveStatement stmt)
        {
            Enter(new Move(stmt), stmt);
            Exit();
        }

        public virtual void OnSetStatement(SetStatement stmt)
        {
            Enter(new Set(stmt), stmt);
            Exit();
        }

        public virtual void OnStopStatement(StopStatement stmt)
        {
            Enter(new Stop(stmt), stmt);
            Exit();
        }

        public virtual void OnExitMethodStatement(ExitMethodStatement stmt)
        {
            Enter(new ExitMethod(stmt), stmt);
            Exit();
        }

        public virtual void OnExitProgramStatement(ExitProgramStatement stmt)
        {
            Enter(new ExitProgram(stmt), stmt);
            Exit();
        }

        public virtual void OnGobackStatement(GobackStatement stmt)
        {
            Enter(new Goback(stmt), stmt);
            Exit();
        }

        public virtual void OnCloseStatement(CloseStatement stmt)
        {
            Enter(new Close(stmt), stmt);
            Exit();
        }

        public virtual void OnDisplayStatement(DisplayStatement stmt)
        {
            Enter(new Display(stmt), stmt);
            Exit();
        }

        public virtual void OnOpenStatement(OpenStatement stmt)
        {
            Enter(new Open(stmt), stmt);
            Exit();
        }

        public virtual void OnMergeStatement(MergeStatement stmt)
        {
            Enter(new Merge(stmt), stmt);
            Exit();
        }

        public virtual void OnReleaseStatement(ReleaseStatement stmt)
        {
            Enter(new Release(stmt), stmt);
            Exit();
        }

        public virtual void OnSortStatement(SortStatement stmt)
        {
            Enter(new Sort(stmt), stmt);
            Exit();
        }

        public virtual void OnAlterStatement(AlterStatement stmt)
        {
            Enter(new Alter(stmt), stmt);
            Exit();
        }

        public void OnExitStatement(ExitStatement stmt)
        {
            Enter(new Exit(stmt), stmt);
            Exit();
        }

        public virtual void OnGotoStatement(GotoStatement stmt)
        {
            Enter(new Goto(stmt), stmt);
            Exit();
        }

        public virtual void OnPerformProcedureStatement(PerformProcedureStatement stmt)
        {
            Enter(new PerformProcedure(stmt), stmt);
            Exit();
        }

        public virtual void OnCancelStatement(CancelStatement stmt)
        {
            Enter(new Cancel(stmt), stmt);
            Exit();
        }

        public virtual void OnProcedureStyleCall(ProcedureStyleCallStatement stmt, CallStatementEnd end)
        {
            Enter(new ProcedureStyleCall(stmt), stmt);
            Exit();
            AttachEndIfExists(end);
        }

        public virtual void OnExecStatement(ExecStatement stmt)
        {
            Enter(new Exec(stmt), stmt);
            Exit();
        }
    }
}
