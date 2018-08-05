// MIT License
// 
// Copyright(c) 2018 Luciano (Xeeynamo) Ciccariello
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

// Part of this software belongs to XeEngine toolset and United Lines Studio
// and it is currently used to create commercial games by Luciano Ciccariello.
// Please do not redistribuite this code under your own name, stole it or use
// it artfully, but instead support it and its author. Thank you.

using System.IO;
using System.Text;

namespace Xe
{
    public static partial class Extensions
	{
		/// <summary>
		/// Read a string with the given encoding, until a null terminator is reached.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="maxLength"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public static string ReadCString(this BinaryReader reader,
			int maxLength = 0x100,
			Encoding encoding = null)
		{
			if (encoding == null)
				encoding = Encoding.UTF8;

			int length;
			var data = new byte[maxLength];
			for (length = 0; length < maxLength;)
			{
				byte c = reader.ReadByte();
				if (c == 0)
					break;
				data[length++] = c;
			}
			return encoding.GetString(data, 0, length);
		}

		/// <summary>
		/// Read a string with the given encoding, until a null terminator is reached.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public static string ReadCString(this BinaryReader reader, Encoding encoding = null)
		{
			if (encoding == null)
				encoding = Encoding.UTF8;

			var memStream = new MemoryStream(0x100);
			while (true)
			{
				byte c = reader.ReadByte();
				if (c == 0)
					break;
				memStream.WriteByte(c);
			}
			return encoding.GetString(memStream.GetBuffer(), 0, (int)memStream.Length);
		}
	}
}
