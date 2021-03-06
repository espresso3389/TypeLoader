﻿/*
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
    /// <summary>OpenTypeのGPOSまたはGSUBテーブルのFeatureTable情報を管理します。</summary>
    /// <remarks>このクラスのコンストラクタはクラスライブラリの外部から呼び出すことはできません。</remarks>
    public sealed class FeatureTable
    {
        internal FeatureTable()
        {
            this.LookupListIndex = new List<ushort>();
        }

        /// <summary>address from beginning of font file.</summary>
        public long Address { get; set; }
        /// <summary>4-byte feature identification tag.</summary>
        public string Tag { get; set; }
        /// <summary>Offset to Script table.- from beginning of ScriptList</summary>
        public ushort Offset { get; set; }
        /// <summary>NULL (reserved for offset to FeatureParams)</summary>
        public ushort FeatureParams { get; set; }
        /// <summary>Number of LookupList indices for this feature.</summary>
        public ushort LookupCount { get; set; }
        /// <summary>Array of LookupList indices for this feature.- zero-based (first lookup is LookupListIndex = 0)</summary>
        public List<ushort> LookupListIndex { get; set; }
    }
}
