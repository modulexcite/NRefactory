//
// EqualExpressionComparisonIssueTests.cs
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
using NUnit.Framework;
using ICSharpCode.NRefactory6.CSharp.Refactoring;
using ICSharpCode.NRefactory6.CSharp.CodeActions;

namespace ICSharpCode.NRefactory6.CSharp.CodeIssues
{
	[TestFixture]
	public class EqualExpressionComparisonIssueTests : InspectionActionTestBase
	{
		[Test]
		public void TestEquality ()
		{
			Test<EqualExpressionComparisonIssue>(@"class Foo
{
	static int Bar (object o)
	{
		if (o == o) {
		}
		return 5;
	}
}", @"class Foo
{
	static int Bar (object o)
	{
		if (true) {
		}
		return 5;
	}
}");
		}


		[Test]
		public void TestInequality ()
		{
			Test<EqualExpressionComparisonIssue>(@"class Foo
{
	static int Bar (object o)
	{
		if (o != o) {
		}
		return 5;
	}
}", @"class Foo
{
	static int Bar (object o)
	{
		if (false) {
		}
		return 5;
	}
}");
		}


		[Test]
		public void TestEquals ()
		{
			Test<EqualExpressionComparisonIssue>(@"class Foo
{
	static int Bar (object o)
	{
		if ((1 + 2).Equals(1 + 2)) {
		}
		return 5;
	}
}", @"class Foo
{
	static int Bar (object o)
	{
		if (true) {
		}
		return 5;
	}
}");
		}

		[Test]
		public void TestNotEquals ()
		{
			Test<EqualExpressionComparisonIssue>(@"class Foo
{
	static int Bar (object o)
	{
		if (!(1 + 2).Equals(1 + 2)) {
		}
		return 5;
	}
}", @"class Foo
{
	static int Bar (object o)
	{
		if (false) {
		}
		return 5;
	}
}");
		}

		[Test]
		public void TestStaticEquals ()
		{
			Test<EqualExpressionComparisonIssue>(@"class Foo
{
	static int Bar (object o)
	{
		if (Equals(o, o)) {
		}
		return 5;
	}
}", @"class Foo
{
	static int Bar (object o)
	{
		if (true) {
		}
		return 5;
	}
}");
		}

		[Test]
		public void TestNotStaticEquals ()
		{
			Test<EqualExpressionComparisonIssue>(@"class Foo
{
	static int Bar (object o)
	{
		if (!Equals(o, o)) {
		}
		return 5;
	}
}", @"class Foo
{
	static int Bar (object o)
	{
		if (false) {
		}
		return 5;
	}
}");
		}

		[Test]
		public void TestDisable ()
		{
			TestWrongContext<EqualExpressionComparisonIssue>(@"class Foo
{
	static int Bar (object o)
	{
		// ReSharper disable once EqualExpressionComparison
		if (o == o) {
		}
		return 5;
	}
}");
		}

	}
}

