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

    /// <summary>前後のグリフに適用する位置調整情報の情報を提供します。</summary>
    /// <remarks>このクラスのコンストラクタはクラスライブラリの外部から呼び出すことはできません。<br/>
    /// このクラスはTypefaceInfoクラスのGetKerningAdjustmentMetricsメソッドによってインスタンスが作成されます。<br/>
    /// </remarks>
    public sealed class PairAdjustmentMetrics
    {

        private Dictionary<string, int> data = new Dictionary<string, int>();
        private List<PositionInfo> firstPositionInfo = new List<PositionInfo>();
        private List<PositionInfo> secondPositionInfo = new List<PositionInfo>();

        internal PairAdjustmentMetrics()
        {
        }

        /// <summary>指定した前後のグリフインデックスに位置調整情報が存在するかを返します。</summary>
        /// <param name="firstGlyphIndex">前の文字のグリフインデックス</param>
        /// <param name="secondGlyphIndex">後の文字のグリフインデックス</param>
        /// <returns>位置調整情報が存在する場合はTrue</returns>
        public bool HasAdjustment(ushort firstGlyphIndex, ushort secondGlyphIndex)
        {
            return data.ContainsKey(GetKey(firstGlyphIndex, secondGlyphIndex));
        }

        /// <summary>指定した前後のグリフインデックスの前の文字に対する位置調整情報を取得します。</summary>
        /// <param name="firstGlyphIndex">前の文字のグリフインデックス</param>
        /// <param name="secondGlyphIndex">後の文字のグリフインデックス</param>
        /// <returns>位置調整情報を持っていないグリフに対して実行すると位置調整の行われない空の内容のPositionInfoが返ります。</returns>
        public PositionInfo GetFirstAdjustment(ushort firstGlyphIndex, ushort secondGlyphIndex)
        {
            string key = GetKey(firstGlyphIndex, secondGlyphIndex);
            if (data.ContainsKey(key))
            {
                return firstPositionInfo[data[key]];
            }
            else
            {
                return new PositionInfo();
            }
        }

        /// <summary>指定した前後のグリフインデックスの後の文字に対する位置調整情報を取得します。</summary>
        /// <param name="firstGlyphIndex">前の文字のグリフインデックス</param>
        /// <param name="secondGlyphIndex">後の文字のグリフインデックス</param>
        /// <returns>位置調整情報を持っていないグリフに対して実行すると位置調整の行われない空の内容のPositionInfoが返ります。</returns>
        public PositionInfo GetSecondAdjustment(ushort firstGlyphIndex, ushort secondGlyphIndex)
        {
            string key = GetKey(firstGlyphIndex, secondGlyphIndex);
            if (data.ContainsKey(key))
            {
                return secondPositionInfo[data[key]];
            }
            else
            {
                return new PositionInfo();
            }
        }

        /// <summary>表のデータ件数を取得します。</summary>
        public int Count
        {
            get { return data.Count; }
        }


        internal void Add(ushort firstGlyphIndex, ValueRecord firstValue, ushort secondGlyphIndex, ValueRecord secondValue, ushort unitsPerEm)
        {
            string key = GetKey(firstGlyphIndex, secondGlyphIndex);

            var fpinfo = new PositionInfo();
            fpinfo.XPlacement = (double)firstValue.XPlacement / unitsPerEm;
            fpinfo.YPlacement = (double)firstValue.YPlacement / unitsPerEm;
            fpinfo.XAdvance = (double)firstValue.XAdvance / unitsPerEm;
            fpinfo.YAdvance = (double)firstValue.YAdvance / unitsPerEm;

            var spinfo = new PositionInfo();
            spinfo.XPlacement = (double)secondValue.XPlacement / unitsPerEm;
            spinfo.YPlacement = (double)secondValue.YPlacement / unitsPerEm;
            spinfo.XAdvance = (double)secondValue.XAdvance / unitsPerEm;
            spinfo.YAdvance = (double)secondValue.YAdvance / unitsPerEm;

            if (data.ContainsKey(key))
            {
                data[key] = firstPositionInfo.Count;
            }
            else
            {
                data.Add(key, firstPositionInfo.Count);
            }

            firstPositionInfo.Add(fpinfo);
            secondPositionInfo.Add(spinfo);

        }

        private string GetKey(ushort firstGlyphIndex, ushort secondGlyphIndex)
        {
            return firstGlyphIndex.ToString() + "x" + secondGlyphIndex.ToString();
        }

    }
}
