﻿// 
// ConstantConditionIssue.cs
// 
// Author:
//      Mansheng Yang <lightyang0@gmail.com>
// 
// Copyright (c) 2012 Mansheng Yang <lightyang0@gmail.com>
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
	[ExportDiagnosticAnalyzer("Condition is always 'true' or always 'false'", LanguageNames.CSharp)]
	[NRefactoryCodeDiagnosticAnalyzer(Description = "Condition is always 'true' or always 'false'")]
	public class ConstantConditionIssue : GatherVisitorCodeIssueProvider
	{
		internal const string DiagnosticId  = "ConstantConditionIssue";
		const string Description            = "";
		const string MessageFormat          = "";
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

		class GatherVisitor : GatherVisitorBase<ConstantConditionIssue>
		{
			public GatherVisitor(SemanticModel semanticModel, Action<Diagnostic> addDiagnostic, CancellationToken cancellationToken)
				: base(semanticModel, addDiagnostic, cancellationToken)
			{
			}
//
//			public override void VisitConditionalExpression(ConditionalExpression conditionalExpression)
//			{
//				base.VisitConditionalExpression(conditionalExpression);
//
//				CheckCondition(conditionalExpression.Condition);
//			}
//
//			public override void VisitIfElseStatement(IfElseStatement ifElseStatement)
//			{
//				base.VisitIfElseStatement(ifElseStatement);
//
//				CheckCondition(ifElseStatement.Condition);
//			}
//
//			public override void VisitWhileStatement(WhileStatement whileStatement)
//			{
//				base.VisitWhileStatement(whileStatement);
//
//				CheckCondition(whileStatement.Condition);
//			}
//
//			public override void VisitDoWhileStatement(DoWhileStatement doWhileStatement)
//			{
//				base.VisitDoWhileStatement(doWhileStatement);
//
//				CheckCondition(doWhileStatement.Condition);
//			}
//
//			public override void VisitForStatement(ForStatement forStatement)
//			{
//				base.VisitForStatement(forStatement);
//
//				CheckCondition(forStatement.Condition);
//			}
//
//			void CheckCondition(Expression condition)
//			{
//				if (condition is PrimitiveExpression)
//					return;
//
//				var resolveResult = ctx.Resolve(condition);
//				if (!(resolveResult.IsCompileTimeConstant && resolveResult.ConstantValue is bool))
//					return;
//
//				var value = (bool)resolveResult.ConstantValue;
//				var conditionalExpr = condition.Parent as ConditionalExpression;
//				var ifElseStatement = condition.Parent as IfElseStatement;
//				var valueStr = value.ToString().ToLowerInvariant();
//
//				CodeAction action;
//				if (conditionalExpr != null) {
//					var replaceExpr = value ? conditionalExpr.TrueExpression : conditionalExpr.FalseExpression;
//					action = new CodeAction(
//						string.Format(ctx.TranslateString("Replace '?:' with '{0}' branch"), valueStr),
//						script => script.Replace(conditionalExpr, replaceExpr.Clone()),
//						condition);
//				} else if (ifElseStatement != null) {
//					action = new CodeAction(
//						string.Format(ctx.TranslateString("Replace 'if' with '{0}' branch"), valueStr),
//						script => {
//							var statement = value ? ifElseStatement.TrueStatement : ifElseStatement.FalseStatement;
//							var blockStatement = statement as BlockStatement;
//							if (statement.IsNull || (blockStatement != null && blockStatement.Statements.Count == 0)) {
//								script.Remove(ifElseStatement);
//								return;
//							}
//
//							TextLocation start, end;
//							if (blockStatement != null) {
//								start = blockStatement.Statements.FirstOrNullObject().StartLocation;
//								end = blockStatement.Statements.LastOrNullObject().EndLocation;
//							} else {
//								start = statement.StartLocation;
//								end = statement.EndLocation;
//							}
//							RemoveText(script, ifElseStatement.StartLocation, start);
//							RemoveText(script, end, ifElseStatement.EndLocation);
//							script.FormatText(ifElseStatement.Parent);
//						}, condition);
//				} else {
//					action = new CodeAction(
//						string.Format(ctx.TranslateString("Replace expression with '{0}'"), valueStr),
//						script => script.Replace(condition, new PrimitiveExpression(value)), 
//						condition
//					);
//				}
//				AddIssue(new CodeIssue(condition, string.Format(ctx.TranslateString("Condition is always '{0}'"), valueStr), 
//					new [] { action }));
//			}
//			public override void VisitConditionalExpression(ConditionalExpression conditionalExpression)
//			{
//				base.VisitConditionalExpression(conditionalExpression);
//
//				CheckCondition(conditionalExpression.Condition);
//			}
//
//			public override void VisitIfElseStatement(IfElseStatement ifElseStatement)
//			{
//				base.VisitIfElseStatement(ifElseStatement);
//
//				CheckCondition(ifElseStatement.Condition);
//			}
//
//			public override void VisitWhileStatement(WhileStatement whileStatement)
//			{
//				base.VisitWhileStatement(whileStatement);
//
//				CheckCondition(whileStatement.Condition);
//			}
//
//			public override void VisitDoWhileStatement(DoWhileStatement doWhileStatement)
//			{
//				base.VisitDoWhileStatement(doWhileStatement);
//
//				CheckCondition(doWhileStatement.Condition);
//			}
//
//			public override void VisitForStatement(ForStatement forStatement)
//			{
//				base.VisitForStatement(forStatement);
//
//				CheckCondition(forStatement.Condition);
//			}
//
//			void CheckCondition(Expression condition)
//			{
//				if (condition is PrimitiveExpression)
//					return;
//
//				var resolveResult = ctx.Resolve(condition);
//				if (!(resolveResult.IsCompileTimeConstant && resolveResult.ConstantValue is bool))
//					return;
//
//				var value = (bool)resolveResult.ConstantValue;
//				var conditionalExpr = condition.Parent as ConditionalExpression;
//				var ifElseStatement = condition.Parent as IfElseStatement;
//				var valueStr = value.ToString().ToLowerInvariant();
//
//				CodeAction action;
//				if (conditionalExpr != null) {
//					var replaceExpr = value ? conditionalExpr.TrueExpression : conditionalExpr.FalseExpression;
//					action = new CodeAction(
//						string.Format(ctx.TranslateString("Replace '?:' with '{0}' branch"), valueStr),
//						script => script.Replace(conditionalExpr, replaceExpr.Clone()),
//						condition);
//				} else if (ifElseStatement != null) {
//					action = new CodeAction(
//						string.Format(ctx.TranslateString("Replace 'if' with '{0}' branch"), valueStr),
//						script => {
//							var statement = value ? ifElseStatement.TrueStatement : ifElseStatement.FalseStatement;
//							var blockStatement = statement as BlockStatement;
//							if (statement.IsNull || (blockStatement != null && blockStatement.Statements.Count == 0)) {
//								script.Remove(ifElseStatement);
//								return;
//							}
//
//							TextLocation start, end;
//							if (blockStatement != null) {
//								start = blockStatement.Statements.FirstOrNullObject().StartLocation;
//								end = blockStatement.Statements.LastOrNullObject().EndLocation;
//							} else {
//								start = statement.StartLocation;
//								end = statement.EndLocation;
//							}
//							RemoveText(script, ifElseStatement.StartLocation, start);
//							RemoveText(script, end, ifElseStatement.EndLocation);
//							script.FormatText(ifElseStatement.Parent);
//						}, condition);
//				} else {
//					action = new CodeAction(
//						string.Format(ctx.TranslateString("Replace expression with '{0}'"), valueStr),
//						script => script.Replace(condition, new PrimitiveExpression(value)), 
//						condition
//					);
//				}
//				AddIssue(new CodeIssue(condition, string.Format(ctx.TranslateString("Condition is always '{0}'"), valueStr), 
//					new [] { action }));
//			}
//
//			void RemoveText(Script script, TextLocation start, TextLocation end)
//			{
//				var startOffset = script.GetCurrentOffset(start);
//				var endOffset = script.GetCurrentOffset(end);
//				if (startOffset < endOffset)
//					script.RemoveText(startOffset, endOffset - startOffset);
//			}
		}
	}

	[ExportCodeFixProvider(ConstantConditionIssue.DiagnosticId, LanguageNames.CSharp)]
	public class ConstantConditionFixProvider : ICodeFixProvider
	{
		public IEnumerable<string> GetFixableDiagnosticIds()
		{
			yield return ConstantConditionIssue.DiagnosticId;
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