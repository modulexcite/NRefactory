//
// EmptyNamespaceTests.cs
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
using NUnit.Framework;
using ICSharpCode.NRefactory6.CSharp.CodeActions;
using ICSharpCode.NRefactory6.CSharp.Refactoring;

namespace ICSharpCode.NRefactory6.CSharp.CodeIssues
{
	public class EmptyNamespaceTests : InspectionActionTestBase
	{
		[Test]
		public void TestBasicCase()
		{
			var input = @"
namespace Foo
{
}";

			TestRefactoringContext context;
			var issues = GetIssues (new EmptyNamespaceIssue (), input, out context);
			Assert.AreEqual (1, issues.Count);
			CheckFix (context, issues, @"
");
		}

		[Test]
		public void TestCaseWithRegions()
		{
			var input = @"
namespace Foo
{
	#region Bar
	#endregion
}";

			TestRefactoringContext context;
			var issues = GetIssues (new EmptyNamespaceIssue (), input, out context);
			Assert.AreEqual (1, issues.Count);
			CheckFix (context, issues, @"
");
		}

		[Test]
		public void TestCaseWithUsing()
		{
			var input = @"
namespace Foo
{
	using System;
}";

			TestRefactoringContext context;
			var issues = GetIssues (new EmptyNamespaceIssue (), input, out context);
			Assert.AreEqual (1, issues.Count);
			CheckFix (context, issues, @"
");
		}

		[Test]
		public void TestCaseWithNesting()
		{
			var input = @"
namespace Foo
{
	namespace Bar
	{
	}
}";

			TestRefactoringContext context;
			var issues = GetIssues (new EmptyNamespaceIssue (), input, out context);
			Assert.AreEqual (1, issues.Count);
			CheckFix (context, issues, @"
namespace Foo
{
}");
		}

		[Test]
		public void TestDisabledForNonEmpty()
		{
			var input = @"
namespace Foo
{
	class Bar
	{
	}
}";

			TestRefactoringContext context;
			var issues = GetIssues (new EmptyNamespaceIssue (), input, out context);
			Assert.AreEqual (0, issues.Count);
		}

		[Test]
		public void TestDisabledForRegionsWithClasses()
		{
			var input = @"
namespace Foo
{
	#region Baz
		class Bar
		{
		}
	#endregion
}";

			TestRefactoringContext context;
			var issues = GetIssues (new EmptyNamespaceIssue (), input, out context);
			Assert.AreEqual (0, issues.Count);
		}

		[Test]
		public void TestDisable()
		{
			var input = @"// ReSharper disable once EmptyNamespace
namespace Foo
{
}";
			TestWrongContext<EmptyNamespaceIssue>(input);
		}
	}
}

