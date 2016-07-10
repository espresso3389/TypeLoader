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
    /// <summary>AATのmortテーブルのChain情報を管理します。</summary>
    /// <remarks>このクラスのコンストラクタはクラスライブラリの外部から呼び出すことはできません。</remarks>
    public sealed class Chain
    {
        internal Chain()
        {
            this.FeatureTables = new List<FeatureTable>();
            this.MetamorphosisTables = new List<MetamorphosisTable>();
        }

        /// <summary>The default sub-feature flags for this chain.</summary>
        public uint DefaultFlags { get; set; }
        /// <summary>The length of the chain in bytes, including this header.</summary>
        public uint ChainLength { get; set; }
        /// <summary>The number of entries in the chain's feature subtable.</summary>
        public ushort NFeatureEntries { get; set; }
        /// <summary>The number of subtables in the chain.</summary>
        public ushort NSubtables { get; set; }
        /// <summary>FeatureTable</summary>
        public List<FeatureTable> FeatureTables { get; set; }
        /// <summary>MetamorphosisTable</summary>
        public List<MetamorphosisTable> MetamorphosisTables  { get; set; }
    }
}
