//
// LowercaseLongLiteralIssue.cs
//
// Author:
//       Luís Reis <luiscubal@gmail.com>
//
// Copyright (c) 2013 Luís Reis
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
	[ExportDiagnosticAnalyzer("Long literal ends with 'l' instead of 'L'", LanguageNames.CSharp)]
	[NRefactoryCodeDiagnosticAnalyzer(Description = "Lowercase 'l' is often confused with '1'", AnalysisDisableKeyword = "LongLiteralEndingLowerL")]
	public class LongLiteralEndingLowerLIssue : GatherVisitorCodeIssueProvider
	{
		internal const string DiagnosticId  = "LongLiteralEndingLowerLIssue";
		const string Description            = "Long literal ends with 'l' instead of 'L'";
		const string MessageFormat          = "Make suffix upper case";
		const string Category               = IssueCategories.CodeQualityIssues;

		static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor (DiagnosticId, Description, MessageFormat, Category, DiagnosticSeverity.Warning);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics {
			get {
				return ImmutableArray.Create(Rule);
			}
		}

		protected override CSharpSyntaxWalker CreateVisitor (SemanticModel semanticModel, Action<Diagnostic> addDiagnostic, CancellationToken cancellationToken)
		{
			return new GatherVisitor(semanticModel, addDiagnostic, cancellationToken);
		}

		class GatherVisitor : GatherVisitorBase<LongLiteralEndingLowerLIssue>
		{
			public GatherVisitor(SemanticModel semanticModel, Action<Diagnostic> addDiagnostic, CancellationToken cancellationToken)
				: base (semanticModel, addDiagnostic, cancellationToken)
			{
			}
//
//			public override void VisitPrimitiveExpression(PrimitiveExpression primitiveExpression)
//			{
//				if (!(primitiveExpression.Value is long || primitiveExpression.Value is ulong))
//				{
//					//Literals such as "l" or 'l' are perfectly acceptable.
//					//Also, no point in visiting integer or boolean literals
//					return;
//				}
//
//				string literalValue = primitiveExpression.LiteralValue;
//				if (literalValue.Length < 2) {
//					return;
//				}
//
//				char prevChar = literalValue [literalValue.Length - 2];
//				char lastChar = literalValue [literalValue.Length - 1];
//
//				if (prevChar == 'u' || prevChar == 'U') {
//					//No problem, '3ul' is not confusing
//					return;
//				}
//
//				if (lastChar == 'l' || prevChar == 'l') {
//					AddIssue(new CodeIssue(primitiveExpression,
//					         ctx.TranslateString(""),
//					         ctx.TranslateString(""),
//					         script => {
//								object newValue = primitiveExpression.Value;
//								string newLiteralValue = primitiveExpression.LiteralValue.ToUpperInvariant();
//								script.Replace(primitiveExpression, new PrimitiveExpression(newValue, newLiteralValue));
//							}
//					));
//				}
//			}
		}
	}

	[ExportCodeFixProvider(LongLiteralEndingLowerLIssue.DiagnosticId, LanguageNames.CSharp)]
	public class LongLiteralEndingLowerLFixProvider : ICodeFixProvider
	{
		public IEnumerable<string> GetFixableDiagnosticIds()
		{
			yield return LongLiteralEndingLowerLIssue.DiagnosticId;
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