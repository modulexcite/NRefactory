﻿// Copyright (c) 2010-2013 AlphaSierraPapa for the SharpDevelop Team
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Linq;
using NUnit.Framework;

namespace ICSharpCode.NRefactory6.CSharp.Parser.TypeMembers
{
	[TestFixture]
	public class ConstructorDeclarationTests
	{
		[Test]
		public void ConstructorDeclarationTest1()
		{
			ConstructorDeclaration cd = ParseUtilCSharp.ParseTypeMember<ConstructorDeclaration>("MyClass() {}");
			Assert.IsTrue(cd.Initializer.IsNull);
		}
		
		[Test]
		public void ConstructorDeclarationTest2()
		{
			ConstructorDeclaration cd = ParseUtilCSharp.ParseTypeMember<ConstructorDeclaration>("MyClass() : this(5) {}");
			Assert.AreEqual(ConstructorInitializerType.This, cd.Initializer.ConstructorInitializerType);
			Assert.AreEqual(1, cd.Initializer.Arguments.Count());
		}
		
		[Test]
		public void ConstructorDeclarationTest3()
		{
			ConstructorDeclaration cd = ParseUtilCSharp.ParseTypeMember<ConstructorDeclaration>("MyClass() : base(1, 2, 3) {}");
			Assert.AreEqual(ConstructorInitializerType.Base, cd.Initializer.ConstructorInitializerType);
			Assert.AreEqual(3, cd.Initializer.Arguments.Count());
		}
		
		[Test]
		public void StaticConstructorDeclarationTest1()
		{
			ConstructorDeclaration cd = ParseUtilCSharp.ParseTypeMember<ConstructorDeclaration>("static MyClass() {}");
			Assert.IsTrue(cd.Initializer.IsNull);
			Assert.AreEqual(Modifiers.Static, cd.Modifiers);
		}
		
		[Test]
		public void ExternStaticConstructorDeclarationTest()
		{
			ConstructorDeclaration cd = ParseUtilCSharp.ParseTypeMember<ConstructorDeclaration>("extern static MyClass();");
			Assert.IsTrue(cd.Initializer.IsNull);
			Assert.AreEqual(Modifiers.Static | Modifiers.Extern, cd.Modifiers);
		}
	}
}
