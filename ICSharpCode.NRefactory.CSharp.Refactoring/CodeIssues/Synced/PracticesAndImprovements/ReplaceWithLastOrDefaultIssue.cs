//
// ReplaceWithLastOrDefaultIssue.cs
//
// Author:
//       Mike Krüger <mkrueger@xamarin.com>
//
// Copyright (c) 2013 Xamarin Inc. (http://xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.Text;
using System.Threading;
using ICSharpCode.NRefactory6.CSharp.Refactoring;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.FindSymbols;

namespace ICSharpCode.NRefactory6.CSharp.Refactoring
{
	[DiagnosticAnalyzer]
	[ExportDiagnosticAnalyzer("Replace with LastOrDefault<T>()", LanguageNames.CSharp)]
	[NRefactoryCodeDiagnosticAnalyzer(Description = "Replace with call to LastOrDefault<T>()", AnalysisDisableKeyword = "ReplaceWithLastOrDefault")]
	public class ReplaceWithLastOrDefaultIssue : GatherVisitorCodeIssueProvider
	{
		internal const string DiagnosticId  = "ReplaceWithLastOrDefaultIssue";
		const string Description            = "Expression can be simlified to 'LastOrDefault<T>()'";
		const string MessageFormat          = "Replace with 'LastOrDefault<T>()'";
		const string Category               = IssueCategories.PracticesAndImprovements;

		static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor (DiagnosticId, Description, MessageFormat, Category, DiagnosticSeverity.Info);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics {
			get {
				return ImmutableArray.Create(Rule);
			}
		}

		protected override CSharpSyntaxWalker CreateVisitor (SemanticModel semanticModel, Action<Diagnostic> addDiagnostic, CancellationToken cancellationToken)
		{
			return new GatherVisitor(semanticModel, addDiagnostic, cancellationToken);
		}

		class GatherVisitor : GatherVisitorBase<ReplaceWithLastOrDefaultIssue>
		{
			public GatherVisitor(SemanticModel semanticModel, Action<Diagnostic> addDiagnostic, CancellationToken cancellationToken)
				: base (semanticModel, addDiagnostic, cancellationToken)
			{
			}

//			readonly AstNode pattern =
//				new ConditionalExpression(
//					new InvocationExpression(
//						new MemberReferenceExpression(new AnyNode("expr"), "Any"),
//						new AnyNodeOrNull("param")
//					),
//					new InvocationExpression(
//						new MemberReferenceExpression(new Backreference("expr"), "Last"),
//						new Backreference("param")
//					),
//					new Choice {
//						new NullReferenceExpression(),
//						new DefaultValueExpression(new AnyNode())
//					}
//				);
//
//			public override void VisitConditionalExpression(ConditionalExpression conditionalExpression)
//			{
//				base.VisitConditionalExpression(conditionalExpression);
//				var match = pattern.Match(conditionalExpression);
//				if (!match.Success)
//					return;
//				var expression = match.Get<Expression>("expr").First();
//				var param      = match.Get<Expression>("param").First();
//
//				AddIssue(new CodeIssue(
//					conditionalExpression,
//					ctx.TranslateString(""),
//					ctx.TranslateString(""),
//					script => {
//						var invocation = new InvocationExpression(new MemberReferenceExpression(expression.Clone(), "LastOrDefault"));
//						if (param != null && !param.IsNull)
//							invocation.Arguments.Add(param.Clone());
//						script.Replace(
//							conditionalExpression,
//							invocation
//							);
//					}
//				));
//			}
		}
	}

	[ExportCodeFixProvider(ReplaceWithLastOrDefaultIssue.DiagnosticId, LanguageNames.CSharp)]
	public class ReplaceWithLastOrDefaultFixProvider : ICodeFixProvider
	{
		public IEnumerable<string> GetFixableDiagnosticIds()
		{
			yield return ReplaceWithLastOrDefaultIssue.DiagnosticId;
		}

		public async Task<IEnumerable<CodeAction>> GetFixesAsync(Document document, TextSpan span, IEnumerable<Diagnostic> diagnostics, CancellationToken cancellationToken)
		{
			var root = await document.GetSyntaxRootAsync(cancellationToken);
			var result = new List<CodeAction>();
			foreach (var diagonstic in diagnostics) {
				var node = root.FindNode(diagonstic.Location.SourceSpan);
				//if (!node.IsKind(SyntaxKind.BaseList))
				//	continue;
				var newRoot = root.RemoveNode(node, SyntaxRemoveOptions.KeepNoTrivia);
				result.Add(CodeActionFactory.Create(node.Span, diagonstic.Severity, diagonstic.GetMessage(), document.WithSyntaxRoot(newRoot)));
			}
			return result;
		}
	}
}