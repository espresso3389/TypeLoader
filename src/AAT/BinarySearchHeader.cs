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
using System.Collections.Generic;
using System.Text;

namespace WaterTrans.TypeLoader.AAT
{
    /// <summary>AATのBinarySearch情報を管理します。</summary>
    /// <remarks>このクラスのコンストラクタはクラスライブラリの外部から呼び出すことはできません。</remarks>
    public sealed class BinarySearchHeader
    {
        internal BinarySearchHeader()
        {
        }

        /// <summary>Size of a lookup unit for this search in bytes.</summary>
        public ushort UnitSize { get; set; }
        /// <summary>Number of units of the preceding size to be searched.</summary>
        public ushort NUnits { get; set; }
        /// <summary>The value of unitSize times the largest power of 2 that is less than or equal to the value of nUnits.</summary>
        public ushort SearchRange { get; set; }
        /// <summary>The log base 2 of the largest power of 2 less than or equal to the value of nUnits.</summary>
        public ushort EntrySelector { get; set; }
        /// <summary>The value of unitSize times the difference of the value of nUnits minus the largest power of 2 less than or equal to the value of nUnits.</summary>
        public ushort RangeShift { get; set; }
    }
}
