namespace TypeCobol.Compiler.CodeModel {

	using Antlr4.Runtime;
	using System;
	using System.Collections.Generic;
	using TypeCobol.Compiler.Nodes;
	using TypeCobol.Compiler.CodeElements;



    /// <summary>
    /// Syntax context
    /// </summary>
    /// <typeparam name="TCtx">Generic context type</typeparam>
	public class SyntaxTree<TCtx> where TCtx: class  {
		/// <summary>Tree root</summary>
		public SourceFile Root { get; private set; }
		/// <summary>Branch currently in construction</summary>
		private readonly Stack<Tuple<Node,TCtx>> _branch;

		public SyntaxTree(): this(new SourceFile(), null) { }
		public SyntaxTree(SourceFile root, TCtx context) {
			this.Root = root ?? new SourceFile();
			_branch = new Stack<Tuple<Node,TCtx>>();
			_branch.Push(new Tuple<Node,TCtx>(Root, context));
		}
		/// <returns>Tip of Branch.</returns>
		public Node CurrentNode => _branch.Peek().Item1;

        public TCtx CurrentContext => _branch.Peek().Item2;

        /// <summary>Add a node as the Head's first child.</summary>
		/// <param name="child">Node to add</param>
		/// <param name="context">Context child was created from</param>
		public void Enter(Node child, TCtx context) {
			CurrentNode.Add(child);
			_branch.Push(new Tuple<Node,TCtx>(child, context));
		}
		/// <summary>Head's parent becomes the new Head.</summary>
		/// <returns>Exited node</returns>
		public void Exit() {
			_branch.Pop();// will throw InvalidOperationException if Branch is empty
		}



		public override string ToString() {
			return Root.ToString();
		}

		internal string BranchToString() {
			var str = new System.Text.StringBuilder();
			var reversed = new List<Node>();
			foreach(var node in _branch) reversed.Insert(0, node.Item1);
			foreach(var node in reversed) str.Append(node).Append(" > ");
			str.Length -= 2;
			return str.ToString();
		}
	}
}
