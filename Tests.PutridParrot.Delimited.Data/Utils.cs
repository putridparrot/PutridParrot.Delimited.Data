﻿using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Tests.PutridParrot.Delimited.Data
{
	[ExcludeFromCodeCoverage]
	public static class Utils
	{
		// might be nice to add this to a StringExtensions class in a Stream namespace or the likes
		public static Stream ToStream(string data)
		{
			return ToStream(data, Encoding.ASCII);
		}

		public static Stream ToStream(string data, Encoding encoding)
		{
			var bytes = encoding.GetBytes(data);

			var ms = new MemoryStream();
			ms.Write(bytes, 0, bytes.Length);
			ms.Seek(0, SeekOrigin.Begin);

			return ms;
		}
	}
}
