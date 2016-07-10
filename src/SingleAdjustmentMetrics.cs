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
using WaterTrans.TypeLoader.OpenType;

namespace WaterTrans.TypeLoader
{
    /// <summary>一つのグリフに適用する位置調整情報の情報を提供します。</summary>
    /// <remarks>このクラスのコンストラクタはクラスライブラリの外部から呼び出すことはできません。<br/>
    /// このクラスはTypefaceInfoクラスのGetHalfAdjustmentMetricsメソッド、GetProportionalAdjustmentMetricsメソッドによってインスタンスが作成されます。<br/>
    /// </remarks>
    public sealed class SingleAdjustmentMetrics
    {
        private Dictionary<ushort, PositionInfo> data = new Dictionary<ushort, PositionInfo>();
        
        internal SingleAdjustmentMetrics()
        {
        }

        /// <summary>指定したグリフインデックスに位置調整情報が存在するかを返します。</summary>
        /// <param name="glyphIndex">確認するグリフインデックス</param>
        /// <returns>位置調整情報が存在する場合はTrue</returns>
        public bool HasAdjustment(ushort glyphIndex)
        {
            return data.ContainsKey(glyphIndex);
        }

        /// <summary>指定したグリフインデックスの位置調整情報を取得します。</summary>
        /// <param name="glyphIndex">取得するグリフインデックス</param>
        /// <returns>位置調整情報を持っていないグリフに対して実行すると位置調整の行われない空の内容のPositionInfoが返ります。</returns>
        public PositionInfo GetAdjustment(ushort glyphIndex)
        {
            if (data.ContainsKey(glyphIndex))
            {
                return data[glyphIndex];
            }
            else
            {
                return new PositionInfo();
            }
        }

        /// <summary>位置調整情報のデータ件数を取得します。</summary>
        public int Count
        {
            get { return data.Count; }
        }


        internal void Add(ushort glyphIndex, ValueRecord value, ushort unitsPerEm)
        {
            var pinfo = new PositionInfo();
            pinfo.XPlacement = (double)value.XPlacement / unitsPerEm;
            pinfo.YPlacement = (double)value.YPlacement / unitsPerEm;
            pinfo.XAdvance = (double)value.XAdvance / unitsPerEm;
            pinfo.YAdvance = (double)value.YAdvance / unitsPerEm;

            if (data.ContainsKey(glyphIndex))
            {
                data[glyphIndex] = pinfo;
            }
            else
            {
                data.Add(glyphIndex, pinfo);
            }

        }

    }
}
