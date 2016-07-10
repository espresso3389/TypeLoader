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
using WaterTrans.TypeLoader.AAT;

namespace WaterTrans.TypeLoader
{
    /// <summary>AATのmortテーブル(OpenTypeのGSUBと同様の機能を提供)の情報を管理します。</summary>
    /// <remarks>このクラスのコンストラクタはクラスライブラリの外部から呼び出すことはできません。<br/>
    /// Apple Advanced Typographyによって拡張された機能で、DynaFontのフォントがこのテーブルに縦書きグリフへの変換情報を定義しています。<br/>
    /// 縦書きへの置換以外の機能はすべて読み込みをスキップしています。</remarks>
    public sealed class MORTTable
    {
        internal MORTTable()
        {
            this.Chains = new List<Chain>();
        }

        /// <summary>address from beginning of font file.</summary>
        public long Address { get; set; }
        /// <summary>MortChain</summary>
        public List<Chain> Chains { get; set; }
        /// <summary>0x00010000 for version 1.0.</summary>
        public ushort TableVersionNumberMajor { get; set; }
        /// <summary>0x00010000 for version 1.0.</summary>
        public ushort TableVersionNumberMinor { get; set; }
        /// <summary>Number of metamorphosis chains contained in this table.</summary>
        public uint NChains { get; set; }
    }
}
