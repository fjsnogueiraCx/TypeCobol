﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeCobol.Compiler.CodeElements;
using TypeCobol.Compiler.CodeElements.Expressions;
using TypeCobol.Compiler.CodeModel;
using TypeCobol.Compiler.Text;
using TypeCobol.Tools;

namespace TypeCobol.Compiler.Nodes {
    /// <summary>
    ///     Tree node, including:
    ///     - strongly-typed CodeElement data
    ///     - parent/children relations
    ///     - unique identification accross the tree
    /// </summary>
    public abstract class Node : IVisitable{
        protected List<Node> children = new List<Node>();

        /// <summary>TODO: Codegen should do its stuff without polluting this class.</summary>
        public bool? Comment = null;

        protected Node(CodeElement CodeElement) {
            this.CodeElement = CodeElement;
        }

        /// <summary>CodeElement data (weakly-typed)</summary>
        public virtual CodeElement CodeElement { get; private set; }

        /// <summary>Parent node (weakly-typed)</summary>
        public Node Parent { get; private set; }

        /// <summary>List of children  (weakly-typed, read-only).</summary>
        /// If you want to modify this list, use the
        /// <see cref="Add" />
        /// and
        /// <see cref="Remove" />
        /// methods.
        public IReadOnlyList<Node> Children {
#if NET40
            get { return new ReadOnlyList<Node>(children); }
#else
            get { return children; }
#endif
        }

        /// <summary>
        /// Get the Index position of the given child.
        /// </summary>
        /// <param name="child">The child to get the index position</param>
        /// <returns>The Index position if the child exist, -1 otherwise.</returns>
        public int ChildIndex(Node child)
        {
            if (children == null)
                return -1;
            return children.IndexOf(child);
        }

        /// <summary>
        /// This is usefull durring Code generation phase in order to create un Array of Node.
        /// Each Node will have it Node Index in the Array.
        /// </summary>
        public int NodeIndex { get; set; }

        /// <summary>
        /// Some interresting flags. Note each flag must be a power of 2
        /// for instance: 0x1 << 0; 0x01 << 1; 0x01 << 2 ... 0x01 << 32
        /// </summary>
        public enum Flag : int
        {
            /// <summary>
            /// Flag that indicates that the node has been visited for Type Cobol Qualification style detection.
            /// </summary>
            HasBeenTypeCobolQualifierVisited = 0x01 << 0,

            /// <summary>
            /// This flag is used during code generation to mark extra nodes added during linearization phase.
            /// </summary>
            ExtraGeneratedLinearNode = 0x01 << 1,
            /// <summary>
            /// This flag is used during code generation to mark node having no position
            /// thus they will be generated at this end of the current buffer.
            /// </summary>
            NoPosGeneratedNode = 0x01 << 2,
            /// <summary>
            /// The Node for the End of function Declaration.
            /// </summary>
            EndFunctionDeclarationNode = 0x01 << 3,
            /// <summary>
            /// Mark this node as persistent that is to say it cannot be removed.
            /// </summary>
            PersistentNode = 0x01 << 4,
            /// <summary>
            /// A node that have been generated by The Generator Factory.
            /// </summary>
            FactoryGeneratedNode = 0x01 << 5,
            /// <summary>
            /// A node that have been generated by The Generator Factory.
            /// It is necessary to keek the sequence of insertion
            /// </summary>
            FactoryGeneratedNodeKeepInsertionIndex = 0x01 << 6,
            /// <summary>
            /// When generating this node aNew Line must be introduced first
            /// </summary>
            FactoryGeneratedNodeWithFirstNewLine = 0x01 << 7,
            /// <summary>
            /// Flag for a PROCEDURE DIVION USING PntTab-Pnt: see issue #519
            /// </summary>
            ProcedureDivisionUsingPntTabPnt = 0x01 << 8,
            /// <summary>
            /// During Generation Force this node to be generated by getting his Lines.
            /// </summary>
            ForceGetGeneratedLines = 0x01 << 9,
            /// <summary>
            /// A Node that can be ignored by the generaror
            /// </summary>
            GeneratorCanIgnoreIt = 0x01 << 10,
            /// <summary>
            /// This node has been marked has erased by the generator
            /// </summary>
            GeneratorErasedNode = 0x01 << 11,
            /// <summary>
            /// Generate in a recursive way a a Node Generated by a Factory
            /// </summary>
            FullyGenerateRecursivelyFactoryGeneratedNode = 0x01 << 12,
            /// <summary>
            /// Flag the node as a node comming from intrinsic files
            /// </summary>
            NodeIsIntrinsic = 0x01 <<13,
            /// <summary>
            /// Flag the node that belongs to the working stoage section (Usefull for DataDefinition)
            /// </summary>
            WorkingSectionNode = 0x01 << 14,
            /// <summary>
            /// Flag the node that belongs to the linkage section (usefull for DataDefinition)
            /// </summary>
            LinkageSectionNode = 0x01 << 15,
            /// <summary>
            /// Flag node belongs to Local Storage Section (usefull for DataDefinition)
            /// </summary>
            LocalStorageSectionNode = 0x01 << 16,
            /// <summary>
            /// Flag node belongs to File Section (usefull for DataDefinition)
            /// </summary>
            FileSectionNode = 0x01 << 17,


        };
        /// <summary>
        /// A 32 bits value for flags associated to this Node
        /// </summary>
        public uint Flags 
        { 
            get; 
            internal set; 
        }

        /// <summary>
        /// Test the value of a flag
        /// </summary>
        /// <param name="flag">The flag to test</param>
        /// <returns>true if the flag is set, false otherwise</returns>
        public bool IsFlagSet(Flag flag)
        {
            return (Flags & (uint)flag) != 0;
        }

        /// <summary>
        /// Set the value of a flag.
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="value"></param>
        /// <param name="bRecurse">True if the setting must be recursive over the Children</param>
        public void SetFlag(Flag flag, bool value, bool bRecurse = false)
        {            
            Flags = value  ? (Flags | (uint)flag) : (Flags & ~(uint)flag);
            if (bRecurse)
            {
                foreach (var child in children)
                {
                    child.SetFlag(flag, value, bRecurse);
                }
            }
        }

        /// <summary>
        /// Used by the Generator to specify a Layout the current Node
        /// </summary>
        public ColumnsLayout ? Layout
        {
            get;
            set;
        }

        public virtual string Name {
            get { return string.Empty; }
        }

        public virtual QualifiedName QualifiedName {
            get {
                if (string.IsNullOrEmpty(Name)) return null;

                string qn = Name;
                var parent = this.Parent;
                while (parent != null)
                {
                    if (!string.IsNullOrEmpty(parent.Name)) {
                        qn = parent.Name + "." + qn;
                    }
                    if (parent is FunctionDeclaration) //If it's a procedure, we can exit we don't need the program name
                        break;
                    parent = parent.Parent;
                }
                
                return new URI(qn);
            }
        }

        /// <summary>Non-unique identifier of this node. Depends on CodeElement type and name (if applicable).</summary>
        public virtual string ID {
            get { return null; }
        }

        /// <summary>Node unique identifier (scope: tree this Node belongs to)</summary>
        public string URI {
            get {
                if (ID == null) return null;
                var puri = Parent == null ? null : Parent.URI;
                if (puri == null) return ID;
                return puri + '.' + ID;
            }
        }

       


        /// <summary>First Node with null Parent among the parents of this Node.</summary>
        public Node Root {
            get {
                var current = this;
                while (current.Parent != null) current = current.Parent;
                return current;
            }
        }

        /// <summary>
        ///     How far removed from SourceFile is this Node?
        ///     Values are 0 if SourceFile is this, 1 of SourceFile is this.Parent,
        ///     2 if SourceFile is this.Parent.Parent, and so on.
        /// </summary>
        public int Generation {
            get {
                var generation = 0;
                var parent = Parent;
                while (parent != null) {
                    generation++;
                    parent = parent.Parent;
                }
                return generation;
            }
        }


        public SymbolTable SymbolTable { get; set; }

        public object this[string attribute] {
            get { return Attributes.Get(this, attribute); }
        }

        public virtual IEnumerable<ITextLine> Lines {
            get {
                var lines = new List<ITextLine>();
                if (CodeElement == null || CodeElement.ConsumedTokens == null) return lines;
                foreach (var token in CodeElement.ConsumedTokens) {//JCM: Don't take in account imported token.
                    if (!(token is TypeCobol.Compiler.Preprocessor.ImportedToken)) {
                        if (!lines.Contains(token.TokensLine))
                            lines.Add(token.TokensLine);
                    }
                }
                return lines;
            }
        }
        public virtual IEnumerable<ITextLine> SelfAndChildrenLines {
            get {
                var lines = new List<ITextLine>();
                lines.AddRange(Lines);
                foreach (var child in children) {
                    lines.AddRange(child.SelfAndChildrenLines);
                }
                return lines;
            }
        }



        /// <summary>
        /// Marker for Code Generation to know if this Node will generate code.
        /// TODO this method should be in CodeGen project
        /// </summary>
        public bool NeedGeneration { get; set; }

        public IList<N> GetChildren<N>() where N : Node {
            return children.OfType<N>().ToList();
        }

        public IList<CodeElementHolder<T>> GetCodeElementHolderChildren<T>() where T : CodeElement {
            var results = new List<CodeElementHolder<T>>();
            foreach (var child in children) {
                if (child.CodeElement == null) continue;
                if (Reflection.IsTypeOf(child.CodeElement.GetType(), typeof(T)))
                    results.Add((CodeElementHolder<T>) child);
            }
            return results;
        }

        /// <summary>
        /// Get the Program Node corresponding to a Child
        /// </summary>
        /// <param name="child">The Child Node</param>
        /// <returns>The Program Node</returns>
        public Program GetProgramNode()
        {
            Node child = this;
            while (child != null && !(child is Program))
                child = child.Parent;
            return (Program)child;
        }
        
        /// <summary>Search for all children of a specific Name</summary>
        /// <param name="name">Name we search for</param>
        /// <param name="deep">true for deep search, false for shallow search</param>
        /// <returns>List of all children with the proper name ; empty list if there is none</returns>
        public IList<T> GetChildren<T>(string name, bool deep) where T : Node {
            var results = new List<T>();
            foreach (var child in children) {
                var typedChild = child as T;
                if (typedChild != null && name.Equals(child.Name, StringComparison.InvariantCultureIgnoreCase)) results.Add(typedChild);
                if (deep) results.AddRange(child.GetChildren<T>(name, true));
            }
            return results;
        }


        /// <summary>Adds a node as a children of this one.</summary>
        /// <param name="child">Child to-be.</param>
        /// <param name="index">Child position in children list.</param>
        public virtual void Add(Node child, int index = -1) {
            if (index < 0) children.Add(child);
            else children.Insert(index, child);
            child.Parent = this;
        }

        /// <summary>Removes a child from this node.</summary>
        /// <param name="node">Child to remove. If this is not one of this Node's current children, nothing happens.</param>
        public void Remove(Node child) {
            children.Remove(child);
            child.Parent = null;
        }

        /// <summary>Removes this node from its Parent's children list and set this.Parent to null.</summary>
        public void Remove() {
            if (Parent != null) Parent.Remove(this);
        }

        /// <summary>Position of a specific child among its siblings.</summary>
        /// <param name="child">Child to be searched for.</param>
        /// <returns>Position in the children list.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">As List</exception>
        public int IndexOf(Node child) {
            return children.IndexOf(child);
        }

        /// <summary>Delete all childrens of this node.</summary>
        public void Clear() {
            foreach (var child in children) child.Parent = null;
            children.Clear();
        }

        /// <summary>
        /// Get All Childrens.
        /// </summary>
        /// <param name="lines">A List to store all children.</param>
        public void ListChildren(List<Node> list)
        {
            if (list == null) return;
            foreach (var child in children)
            {
                list.Add(child);
                child.ListChildren(list);
            }
        }

        /// <summary>Get this node or one of its children that has a given URI.</summary>
        /// <param name="uri">Node unique identifier to search for</param>
        /// <returns>Node n for which n.URI == uri, or null if no such Node was found</returns>
        public Node Get(string uri) {
            string gen_uri = URI;
            if (gen_uri != null)
            {
                if (uri.IndexOf('(') >= 0 && uri.IndexOf(')') > 0)
                {//Pattern matching URI                    
                    System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(uri);
                    if (re.IsMatch(URI))
                    {
                        return this;
                    }
                }
                if (gen_uri.EndsWith(uri))
                {
                    return this;
                }
            }
            foreach (var child in Children)
            {
                var found = child.Get(uri);
                if (found != null) return found;
            }
            return null;
        }

        /// <summary>As <see cref="Get" /> method, but can specify the type of Node to retrieve.</summary>
        /// <typeparam name="N"></typeparam>
        /// <param name="uri"></param>
        /// <returns>null if a node with the given URI is found but is not of the proper type</returns>
        public N Get<N>(string uri) where N : Node
        {
            var node = Get(uri);
            try
            {
                return (N)node;
            }
            catch (InvalidCastException)
            {
                return default(N);
            }
        }


        public override string ToString() {
            var str = new StringBuilder();
            Dump(str, 0);
            return str.ToString();
        }

        /// <summary>
        /// Don't override this method, implement VisitNode on child
        /// </summary>
        /// <param name="astVisitor"></param>
        /// <returns></returns>
        public bool AcceptASTVisitor(IASTVisitor astVisitor) {
            bool continueVisit = astVisitor.BeginNode(this) && VisitNode(astVisitor);

            if (continueVisit && CodeElement != null)
            {
                CodeElement.AcceptASTVisitor(astVisitor);
            }

            if (continueVisit) {
                //To Handle concurrent modifications during traverse.
                //Get the array of Children that must be traverse.
                Node[] childrenNodes = children.ToArray();
                foreach (Node child in childrenNodes)
                {
                    if (!continueVisit && astVisitor.IsStopVisitingChildren)
                    {
                        break;
                    }
                    continueVisit = child.AcceptASTVisitor(astVisitor);                    
                }
            }

            astVisitor.EndNode(this);
            return continueVisit;
        }

        public abstract bool VisitNode(IASTVisitor astVisitor);


        private void Dump(StringBuilder str, int i)
        {
            for (var c = 0; c < i; c++) str.Append("  ");
            if (Comment == true) str.Append('*');
            if (!string.IsNullOrEmpty(Name)) str.AppendLine(Name);
            else if (!string.IsNullOrEmpty(ID)) str.AppendLine(ID);
            else if (CodeElement == null) str.AppendLine("?");
            else str.AppendLine(CodeElement.ToString());
            foreach (var child in Children) child.Dump(str, i + 1);
        }

        /// <summary>TODO: Codegen should do its stuff without polluting this class.</summary>
        public void RemoveAllChildren() {
            children.Clear();
        }

        /// <summary>Implementation of the GoF Visitor pattern.</summary>
        public void Accept(NodeVisitor visitor) {
            visitor.Visit(this);
        }

        /// <summary>
        ///     Return true if this Node is inside a COPY
        /// </summary>
        /// <returns></returns>
        public bool IsInsideCopy() {
            return CodeElement != null && CodeElement.IsInsideCopy();
        }
    }

// --- Temporary base classes for data definition noes ---

    public interface ITypedNode {
        DataType DataType { get; }
        int Length { get; }
    }

    /// <summary>Implementation of the GoF Visitor pattern.</summary>
    public interface NodeVisitor {
        void Visit(Node node);
    }


    public interface CodeElementHolder<T> where T : CodeElement {}

    public static class CodeElementHolderExtension {
        /// <summary>CodeElement data (strongly-typed)</summary>
        /// <typeparam name="T">Class (derived from <see cref="CodeElement" />) of the data.</typeparam>
        /// <param name="holder">We want this <see cref="Node" />'s data.</param>
        /// <returns>This <see cref="Node" />'s CodeElement data, but strongly-typed.</returns>
        public static T CodeElement<T>(this CodeElementHolder<T> holder) where T : CodeElement {
            var node = holder as Node;
            if (node == null) throw new ArgumentException("CodeElementHolder must be a Node.");
            return (T) node.CodeElement;
        }
    }

    /// <summary>A <see cref="Node" /> who can type its parent more strongly should inherit from this.</summary>
    /// <typeparam name="C">Class (derived from <see cref="Node{T}" />) of the parent node.</typeparam>
    public interface Child<P> where P : Node {}

    /// <summary>Extension method to get a more strongly-typed parent than just Node.</summary>
    public static class ChildExtension {
        /// <summary>Returns this node's parent in as strongly-typed.</summary>
        /// <typeparam name="P">Class (derived from <see cref="Node{T}" />) of the parent.</typeparam>
        /// <param name="child">We want this <see cref="Node" />'s parent.</param>
        /// <returns>This <see cref="Node" />'s parent, but strongly-typed.</returns>
        public static P Parent<P>(this Child<P> child) where P : Node {
            var node = child as Node;
            if (node == null) throw new ArgumentException("Child must be a Node.");
            return (P) node.Parent;
        }
    }

    /// <summary>A <see cref="Node" /> who can type its children more strongly should inherit from this.</summary>
    /// <typeparam name="C">Class (derived from <see cref="Node{T}" />) of the children nodes.</typeparam>
    public interface Parent<C> where C : Node {}

    /// <summary>Extension method to get children more strongly-typed than just Node.</summary>
    public static class ParentExtension {
        /// <summary>
        ///     Returns a read-only list of strongly-typed children of a <see cref="Node" />.
        ///     The children are more strongly-typed than the ones in the Node.Children property.
        ///     The list is read-only because the returned list is a copy of the Node.Children list property ;
        ///     thus, writing node.StrongChildren().Add(child) will be a compilation error.
        ///     Strongly-typed children are to be iterated on, but to modify a Node's children list you have
        ///     to use the (weakly-typed) Node.Children property.
        /// </summary>
        /// <typeparam name="C">Class (derived from <see cref="Node{T}" />) of the children.</typeparam>
        /// <param name="parent">We want this <see cref="Node" />'s children.</param>
        /// <returns>Strongly-typed list of a <see cref="Node" />'s children.</returns>
        public static IReadOnlyList<C> Children<C>(this Parent<C> parent) where C : Node {
            var node = parent as Node;
            if (node == null) throw new ArgumentException("Parent must be a Node.");
            //TODO? maybe use ConvertAll or Cast from LINQ, but only
            // if the performance is better or if it avoids a copy.
            var result = new List<C>();
            foreach (var child in node.Children) result.Add((C) child);
#if NET40
            return new ReadOnlyList<C>(result);
#else
            return result.AsReadOnly();
#endif

        }
    }


    /// <summary>SourceFile of any Node tree, with null CodeElement.</summary>
    public class SourceFile : Node, CodeElementHolder<CodeElement> {
        public SourceFile() : base(null)
        {
            GeneratedCobolHashes = new Dictionary<string, string>();
        }
        public override bool VisitNode(IASTVisitor astVisitor)
        {
            return astVisitor.Visit(this);
        }

        /// <summary>
        /// Dictionary of hashes and signatures for the different function and procedure. Allows to avoid duplicates. 
        /// </summary>
        public Dictionary<string, string> GeneratedCobolHashes { get; set; }


        public IEnumerable<Program> Programs {
            get
            {
                return this.children.Where(c => c is Program && !((Program)c).IsNested).Select(c => c as Program);
            }
        }

        public IEnumerable<Class> Classes
        {
            get
            {
                return this.children.OfType<Class>();
            }
        }











    }

    public class LibraryCopy : Node, CodeElementHolder<LibraryCopyCodeElement>, Child<Program> {
        public LibraryCopy(LibraryCopyCodeElement ce) : base(ce) {}

        public override string ID {
            get { return "copy"; }
        }

        public override bool VisitNode(IASTVisitor astVisitor)
        {
            return astVisitor.Visit(this);
        }
    }

    public class Class : Node, CodeElementHolder<ClassIdentification> {
        public Class(ClassIdentification identification) : base(identification) {}

        public override string ID {
            get { return "class";  }
        }
        public override string Name { get { return this.CodeElement().ClassName.Name; } }

        public override bool VisitNode(IASTVisitor astVisitor) {
            return astVisitor.Visit(this);
        }
    }

    public class Factory : Node, CodeElementHolder<FactoryIdentification> {
        public Factory(FactoryIdentification identification) : base(identification) {}

        public override string ID {
            get { return "TODO#248"; }
        }

        public override bool VisitNode(IASTVisitor astVisitor)
        {
            return astVisitor.Visit(this);
        }
    }

    public class Method : Node, CodeElementHolder<MethodIdentification> {
        public Method(MethodIdentification identification) : base(identification) {}

        public override string ID {
            get { return "Method"; }
        }

        public override string Name { get { return this.CodeElement().MethodName.Name; } }

        public override bool VisitNode(IASTVisitor astVisitor)
        {
            return astVisitor.Visit(this);
        }
    }

    public class Object : Node, CodeElementHolder<ObjectIdentification> {
        public Object(ObjectIdentification identification) : base(identification) {}

        public override string ID {
            get { return "TODO#248"; }
        }

        public override bool VisitNode(IASTVisitor astVisitor)
        {
            return astVisitor.Visit(this);
        }
    }

    public class End : Node, CodeElementHolder<CodeElementEnd> {
        public End(CodeElementEnd end) : base(end) {}

        public override string ID {
            get { return "end"; }
        }

        public override bool VisitNode(IASTVisitor astVisitor)
        {
            return astVisitor.Visit(this);
        }
    }
} // end of namespace TypeCobol.Compiler.Nodes