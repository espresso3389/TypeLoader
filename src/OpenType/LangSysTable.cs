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

namespace WaterTrans.TypeLoader.OpenType
{
    /// <summary>OpenTypeのGPOSまたはGSUBテーブルのLangSysTable情報を管理します。</summary>
    /// <remarks>このクラスのコンストラクタはクラスライブラリの外部から呼び出すことはできません。</remarks>
    public sealed class LangSysTable
    {
        internal LangSysTable()
        {
            this.FeatureIndexList = new List<ushort>();
        }

        /// <summary>address from beginning of font file.</summary>
        public long Address { get; set; }
        /// <summary>Default LangSysTable.</summary>
        public bool IsDefault { get; set; }
        /// <summary>4-byte LangSysTag identifier.</summary>
        public string Tag { get; set; }
        /// <summary>Offset to LangSys table.— from beginning of Script table</summary>
        public ushort Offset { get; set; }
        /// <summary>= NULL (reserved for an offset to a reordering table)</summary>
        public ushort LookupOrder { get; set; }
        /// <summary>Index of a feature required for this language system.— if no required features = 0xFFFF</summary>
        public ushort ReqFeatureIndex { get; set; }
        /// <summary>Number of FeatureIndex values for this language system.— excludes the required feature</summary>
        public ushort FeatureCount { get; set; }
        /// <summary>Array of indices into the FeatureList.— in arbitrary order</summary>
        public List<ushort> FeatureIndexList { get; set; }
    }
}
