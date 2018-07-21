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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Xe
{
    public delegate void LogFunc(Log.Level level, string message, string member, string sourceFile, int sourceLine);

    public static class Log
    {
        public enum Level
        {
            Error, Warning, Message
        }
        public static event LogFunc OnLog;
        public static void Error(string str,
            [CallerMemberName] string member = null,
            [CallerFilePath] string sourceFilePat = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (OnLog != null)
            {
                lock (OnLog)
                {
                    OnLog.Invoke(Level.Error, $"{str}\n", member, sourceFilePat, sourceLineNumber);
                }
            }
        }

        public static void Warning(string str,
            [CallerMemberName] string member = null,
            [CallerFilePath] string sourceFilePat = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (OnLog != null)
            {
                lock (OnLog)
                {
                    OnLog.Invoke(Level.Warning, $"{str}\n", member, sourceFilePat, sourceLineNumber);
                }
            }
        }

        public static void Message(string str,
            [CallerMemberName] string member = null,
            [CallerFilePath] string sourceFilePat = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (OnLog != null)
            {
                lock (OnLog)
                {
                    OnLog.Invoke(Level.Message, $"{str}\n", member, sourceFilePat, sourceLineNumber);
                }
            }
        }
    }
}
