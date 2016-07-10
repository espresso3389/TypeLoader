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

namespace WaterTrans.TypeLoader
{
    /// <summary>一つのグリフを他のグリフに変換します。</summary>
    /// <remarks>このクラスのコンストラクタはクラスライブラリの外部から呼び出すことはできません。<br/>
    /// このクラスはTypefaceInfoクラスのGetVerticalGlyphConverterメソッド、GetAdvancedVerticalGlyphConverterメソッドによってインスタンスが作成されます。</remarks>
    public sealed class SingleGlyphConverter
    {
        private Dictionary<ushort, ushort> data = new Dictionary<ushort, ushort>();
        
        internal SingleGlyphConverter()
            : base()
        {
        }

        /// <summary>指定したグリフインデックスに対応する別のグリフが存在するかを返します。</summary>
        /// <param name="glyphIndex">確認するグリフインデックス</param>
        /// <returns>別のグリフが存在する場合はTrueを返します。</returns>
        public bool CanConvert(ushort glyphIndex)
        {
            return data.ContainsKey(glyphIndex);
        }

        /// <summary>指定したグリフインデックスに対応する別のグリフのインデックスを取得します。</summary>
        /// <param name="glyphIndex">変換するグリフインデックス</param>
        /// <returns>別のグリフが存在する場合は別のグリフインデックス、存在しない場合は引数の値をそのまま返します。</returns>
        public ushort Convert(ushort glyphIndex)
        {
            if (data.ContainsKey(glyphIndex))
            {
                return data[glyphIndex];
            }
            else
            {
                return glyphIndex;
            }
        }

        /// <summary>グリフ変換リストのデータ件数を取得します。</summary>
        public int Count
        {
            get { return data.Count; }
        }


        internal void Add(ushort glyphIndexFrom, ushort glyphIndexTo)
        {
            if (data.ContainsKey(glyphIndexFrom))
            {
                data[glyphIndexFrom] = glyphIndexTo;
            }
            else
            {
                data.Add(glyphIndexFrom, glyphIndexTo);
            }

        }

    }
}
