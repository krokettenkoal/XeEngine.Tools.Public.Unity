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
using System.Text;

namespace Xe
{
    public struct Pointi {
        public int X, Y;

        public Pointi(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public struct Sizei
    {
        public int Width, Height;
    }

    public struct Recti
    {
        public int Left, Top, Right, Bottom;

        public int X
        {
            get => Left;
            set
            {
                var diff = value - Left;
                Left = value;
                Right += diff;
            }
        }

        public int Y
        {
            get => Top;
            set
            {
                var diff = value - Top;
                Top = value;
                Bottom += diff;
            }
        }

        public int Width
        {
            get => Right - Left;
        }

        public int Height
        {
            get => Bottom - Top;
        }
        
        public bool IntersectsWith(Recti rect)
        {
            return rect.Left < Right && Left < rect.Right && rect.Top < Bottom && Top < rect.Bottom;
        }

        public static Recti FromSize(int x, int y,  int width, int height)
        {
            return new Recti { Left = x, Top = y, Right = width + x, Bottom = height + y };
        }
    }

    public static partial class Math
	{
		public static sbyte Lerp(sbyte x, sbyte y, double t) => (sbyte)(x + t * (y - x));
		public static byte Lerp(byte x, byte y, double t) => (byte)(x + t * (y - x));
		public static short Lerp(short x, short y, double t) => (short)(x + t * (y - x));
		public static ushort Lerp(ushort x, ushort y, double t) => (ushort)(x + t * (y - x));
		public static int Lerp(int x, int y, double t) => (int)(x + t * (y - x));
		public static uint Lerp(uint x, uint y, double t) => (uint)(x + t * (y - x));
		public static long Lerp(long x, long y, double t) => (long)(x + t * (y - x));
		public static ulong Lerp(ulong x, ulong y, double t) => (ulong)(x + t * (y - x));
		public static float Lerp(float x, float y, double t) => (float)(x + t * (y - x));
		public static double Lerp(double x, double y, double t) => x + t * (y - x);

		public static sbyte Min(sbyte x, sbyte y) => System.Math.Min(x, y);
		public static byte Min(byte x, byte y) => System.Math.Min(x, y);
		public static short Min(short x, short y) => System.Math.Min(x, y);
		public static ushort Min(ushort x, ushort y) => System.Math.Min(x, y);
		public static int Min(int x, int y) => System.Math.Min(x, y);
		public static uint Min(uint x, uint y) => System.Math.Min(x, y);
		public static long Min(long x, long y) => System.Math.Min(x, y);
		public static ulong Min(ulong x, ulong y) => System.Math.Min(x, y);
		public static float Min(float x, float y) => System.Math.Min(x, y);
		public static double Min(double x, double y) => System.Math.Min(x, y);
		public static decimal Min(decimal x, decimal y) => System.Math.Min(x, y);

		public static sbyte Max(sbyte x, sbyte y) => System.Math.Max(x, y);
		public static byte Max(byte x, byte y) => System.Math.Max(x, y);
		public static short Max(short x, short y) => System.Math.Max(x, y);
		public static ushort Max(ushort x, ushort y) => System.Math.Max(x, y);
		public static int Max(int x, int y) => System.Math.Max(x, y);
		public static uint Max(uint x, uint y) => System.Math.Max(x, y);
		public static long Max(long x, long y) => System.Math.Max(x, y);
		public static ulong Max(ulong x, ulong y) => System.Math.Max(x, y);
		public static float Max(float x, float y) => System.Math.Max(x, y);
		public static double Max(double x, double y) => System.Math.Max(x, y);
		public static decimal Max(decimal x, decimal y) => System.Math.Max(x, y);

		public static sbyte Abs(sbyte x, sbyte y) => System.Math.Abs(x);
		public static short Abs(short x, short y) => System.Math.Abs(x);
		public static int Abs(int x, int y) => System.Math.Abs(x);
		public static long Abs(long x, long y) => System.Math.Abs(x);
		public static float Abs(float x, float y) => System.Math.Abs(x);
		public static double Abs(double x, double y) => System.Math.Abs(x);
		public static decimal Abs(decimal x, decimal y) => System.Math.Abs(x);

		public static sbyte Range(sbyte x, sbyte min, sbyte max) => x > min ? x < max ? x : max : min;
		public static byte Range(byte x, byte min, byte max) => x > min ? x < max ? x : max : min;
		public static short Range(short x, short min, short max) => x > min ? x < max ? x : max : min;
		public static ushort Range(ushort x, ushort min, ushort max) => x > min ? x < max ? x : max : min;
		public static int Range(int x, int min, int max) => x > min ? x < max ? x : max : min;
		public static uint Range(uint x, uint min, uint max) => x > min ? x < max ? x : max : min;
		public static long Range(long x, long min, long max) => x > min ? x < max ? x : max : min;
		public static ulong Range(ulong x, ulong min, ulong max) => x > min ? x < max ? x : max : min;
		public static float Range(float x, float min, float max) => x > min ? x < max ? x : max : min;
		public static double Range(double x, double min, double max) => x > min ? x < max ? x : max : min;
		public static decimal Range(decimal x, decimal min, decimal max) => x > min ? x < max ? x : max : min;

        public static int Round(double x)
        {
            return (int)System.Math.Round(x);
        }
        public static int Floor(double x)
        {
            return (int)System.Math.Floor(x);
        }
    }
}
