/*
 * The MIT License
 * 
 * Copyright © 2010 WaterTrans
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 * 
 * Email: support@watertrans.com
 * 
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace WaterTrans.TypeLoader
{
    /// <summary>
    /// フォントデータにアクセスするための内部処理クラス
    /// </summary>
    internal sealed class TypefaceReader
    {
        private Stream stream;
        private BinaryReader reader;

        public TypefaceReader(Stream input)
        {
            stream = input;
            reader = new BinaryReader(input);
        }

        public Stream BaseStream
        {
            get { return stream; }
        }

        public string ReadCharArray(int len)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                sb.Append(ReadChar());
            }
            return sb.ToString();
        }

        public char ReadChar()
        {
            return (char)ReadByte();
        }

        public byte ReadByte()
        {
            return reader.ReadByte();
        }

        public ushort ReadUInt16()
        {
            return BitConverter.ToUInt16(ReadBytesInternal(2), 0);
        }

        public Int16 ReadInt16()
        {
            return BitConverter.ToInt16(ReadBytesInternal(2), 0);
        }

        public UInt32 ReadUInt32()
        {
            return BitConverter.ToUInt32(ReadBytesInternal(4), 0);
        }

        public UInt64 ReadUInt64()
        {
            return BitConverter.ToUInt64(ReadBytesInternal(8), 0);
        }

        public Int64 ReadInt64()
        {
            return BitConverter.ToInt64(ReadBytesInternal(8), 0);
        }

        private byte[] ReadBytesInternal(int count)
        {
            byte[] buff = reader.ReadBytes(count);
            Array.Reverse(buff);
            return buff;
        }

    }
}
