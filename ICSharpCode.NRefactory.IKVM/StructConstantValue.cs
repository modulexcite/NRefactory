//
// StructConstantValue.cs
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
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.Semantics;

namespace ICSharpCode.NRefactory
{
	[Serializable]
	sealed class StructConstantValue<T> : IConstantValue, ISupportsInterning where T : struct
	{
		readonly ITypeReference type;
		readonly T value;

		public StructConstantValue(ITypeReference type, T value)
		{
			if (type == null)
				throw new ArgumentNullException("type");
			this.type = type;
			this.value = value;
		}

		public ResolveResult Resolve(ITypeResolveContext context)
		{
			return new ConstantResolveResult(type.Resolve(context), value);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase",
			Justification = "The C# keyword is lower case")]
		public override string ToString()
		{
			if (value is bool)
				return value.ToString().ToLowerInvariant();
			else
				return value.ToString();
		}

		int ISupportsInterning.GetHashCodeForInterning()
		{
			return type.GetHashCode() ^ value.GetHashCode ();
		}

		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			var scv = other as StructConstantValue<T>;
			return scv != null && type == scv.type && Equals (value, scv.value);
		}
	}
}

