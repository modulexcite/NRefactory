//
// ConvertToStaticMethodAnalyzer.cs
//
// Author:
//       Ciprian Khlud <ciprian.mustiata@yahoo.com>
//
// Copyright (c) 2013 Ciprian Khlud
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
using NUnit.Framework;
using ICSharpCode.NRefactory6.CSharp.Refactoring;

namespace ICSharpCode.NRefactory6.CSharp.Diagnostics
{
	[TestFixture]
	[Ignore("TODO: Issue not ported yet")]
	public class MemberCanBeMadeStaticTests : InspectionActionTestBase
	{
		[Test]
		public void TestPrivateMethod()
		{
			TestWithSubIssue<MemberCanBeMadeStaticAnalyzer>(
				@"class TestClass
{
	void $Test ()
	{
		int a = 2;
	}
}",
				@"class TestClass
{
	static void Test ()
	{
		int a = 2;
	}
}", MemberCanBeMadeStaticAnalyzer.DiagnosticIdPrivate
			);
		}

		[Test]
		public void TestPrivateMethodPublicSkip()
		{
			TestWrongContextWithSubIssue<MemberCanBeMadeStaticAnalyzer>(
				@"class TestClass
{
	void $Test ()
	{
		int a = 2;
	}
}", MemberCanBeMadeStaticAnalyzer.DiagnosticIdNonPrivate
			);
		}

		[Test]
		public void TestPublicMethod()
		{
			TestWithSubIssue<MemberCanBeMadeStaticAnalyzer>(
				@"class TestClass
{
	public void $Test ()
	{
		int a = 2;
	}
}",
				@"class TestClass
{
	public static void Test ()
	{
		int a = 2;
	}
}", MemberCanBeMadeStaticAnalyzer.DiagnosticIdNonPrivate
			);
		}

		[Test]
		public void TestPublicMethodSkip()
		{
			TestWrongContextWithSubIssue<MemberCanBeMadeStaticAnalyzer>(
				@"class TestClass
{
	public void $Test ()
	{
		int a = 2;
	}
}", MemberCanBeMadeStaticAnalyzer.DiagnosticIdPrivate
			);
		}

		[Test]
		public void MethodThatCallsInstanceMethodOnParameter()
		{
			TestWithSubIssue<MemberCanBeMadeStaticAnalyzer>(
				@"class TestClass
{
	string $Test (string txt)
	{
		return txt.Trim ();
	}
}",
				@"class TestClass
{
	static string Test (string txt)
	{
		return txt.Trim ();
	}
}", MemberCanBeMadeStaticAnalyzer.DiagnosticIdPrivate
			);
		}

		[Test]
		public void TestWithVirtualFunction()
		{
			
			var input = @"class TestClass
{
	public virtual void $Test ()
	{
		int a = 2;
	}
}";
			TestWrongContextWithSubIssue<MemberCanBeMadeStaticAnalyzer>(input, MemberCanBeMadeStaticAnalyzer.DiagnosticIdPrivate);
		}

		[Test]
		public void TestWithInterfaceImplementation()
		{
			var input = @"interface IBase {
    void Test();
}
class TestClass : IBase
{
	public void $Test ()
	{
		int a = 2;
	}
}";
			TestWrongContextWithSubIssue<MemberCanBeMadeStaticAnalyzer>(input, MemberCanBeMadeStaticAnalyzer.DiagnosticIdNonPrivate);
		}

		[Test]
		public void TestWithStaticFunction()
		{

			var input = @"class TestClass
{
	static void $Test ()
	{
		int a = 2;
	}
}";
			TestWrongContextWithSubIssue<MemberCanBeMadeStaticAnalyzer>(input, MemberCanBeMadeStaticAnalyzer.DiagnosticIdPrivate);
		}

		[Test]
		public void TestDoNotWarnOnAttributes()
		{

			var input = @"using System;
class TestClass
{
	[Obsolete]
	public void $Test ()
	{
		int a = 2;
	}
}";
			TestWrongContextWithSubIssue<MemberCanBeMadeStaticAnalyzer>(input, MemberCanBeMadeStaticAnalyzer.DiagnosticIdNonPrivate);
		}

		[Test]
		public void TestDoNotWarnOnEmptyMethod()
		{

			var input = @"using System;
class TestClass
{
	public void $Test ()
	{
	}
}";
			TestWrongContextWithSubIssue<MemberCanBeMadeStaticAnalyzer>(input, MemberCanBeMadeStaticAnalyzer.DiagnosticIdNonPrivate);
		}

		[Test]
		public void TestDoNotWarnOnInterfaceMethod()
		{

			var input = @"using System;
interface ITestInterface
{
	void $Test ();
}";
			TestWrongContextWithSubIssue<MemberCanBeMadeStaticAnalyzer>(input, MemberCanBeMadeStaticAnalyzer.DiagnosticIdPrivate);
		}

		[Test]
		public void TestDoNotWarnOnNotImplementedMethod()
		{
			var input = @"using System;
class TestClass
{
	public void $Test ()
	{
		throw new NotImplementedExceptionIssue();
	}
}";
			TestWrongContextWithSubIssue<MemberCanBeMadeStaticAnalyzer>(input, MemberCanBeMadeStaticAnalyzer.DiagnosticIdNonPrivate);
		}

		[Test]
		public void TestPropertyAccess()
		{
			var input = @"using System;
class TestClass
{
	public int Foo { get; set; }
	public void $Test ()
	{
		System.Console.WriteLine (Foo);
	}
}";
			TestWrongContextWithSubIssue<MemberCanBeMadeStaticAnalyzer>(input, MemberCanBeMadeStaticAnalyzer.DiagnosticIdNonPrivate);
		}

		[Test]
		public void DoNotWarnOnMarshalByRefObject()
		{
			
			var input = @"class TestClass : System.MarshalByRefObject
{
	public void $Test ()
	{
		int a = 2;
	}
}";
			TestWrongContextWithSubIssue<MemberCanBeMadeStaticAnalyzer>(input, MemberCanBeMadeStaticAnalyzer.DiagnosticIdNonPrivate);
		}

		[Test]
		public void TestProperty()
		{
			TestWithSubIssue<MemberCanBeMadeStaticAnalyzer>(
				@"class TestClass
{
	int Test {
		get {
			return 2;
		}
	}
}",
				@"class TestClass
{
	static int Test {
		get {
			return 2;
		}
	}
}", MemberCanBeMadeStaticAnalyzer.DiagnosticIdPrivate
			);
		}


		[Test]
		public void TestCustomEvent()
		{
			TestWithSubIssue<MemberCanBeMadeStaticAnalyzer>(
				@"using System;

class TestClass
{
	static event EventHandler Foo;

	event EventHandler Bar {
		add { Foo += value; }
		remove { Foo -= value; }
	}
}",
				@"using System;

class TestClass
{
	static event EventHandler Foo;

	static event EventHandler Bar {
		add { Foo += value; }
		remove { Foo -= value; }
	}
}", MemberCanBeMadeStaticAnalyzer.DiagnosticIdPrivate
				);
		}

		
		[Test]
		public void TestCustomEventOnNotImplemented()
		{
			TestWrongContextWithSubIssue<MemberCanBeMadeStaticAnalyzer>(
				@"using System;

class TestClass
{
	static event EventHandler Foo;

	event EventHandler Bar {
		add { throw new NotImplementedException (); }
		remove { throw new NotImplementedException (); }
	}
}", MemberCanBeMadeStaticAnalyzer.DiagnosticIdPrivate
				);
		}

		[Test]
		public void TestDisable()
		{
			TestWrongContextWithSubIssue<MemberCanBeMadeStaticAnalyzer>(
				@"class TestClass
{
// ReSharper disable once MemberCanBeMadeStatic.Local
	void Test ()
	{
		int a = 2;
	}
}", MemberCanBeMadeStaticAnalyzer.DiagnosticIdPrivate);
		}
	}
}
