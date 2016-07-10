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

    /// <summary>二文字以上の連続するグリフを一つのグリフに変換します。</summary>
    /// <remarks>このクラスのコンストラクタはクラスライブラリの外部から呼び出すことはできません。<br/>
    /// このクラスはTypefaceInfoクラスのGetLigatureMapメソッドによってインスタンスが作成されます。<br/>
    /// 合字は二文字以上で行われます。合字探索の最適化のため、まずは最初のグリフが合字を構成するグリフであるかどうかをHasLigatureメソッドで確認してください。<br/>
    /// 最初のグリフが合字を構成するグリフである場合、GetLigatureMaxLengthメソッドによってそのグリフから始まる合字の最大の文字数を取得します。<br/>
    /// 最大の文字数を取得した後は、最初のグリフを含む最大文字数のグリフの配列を用意してCanConvertメソッドを呼び出してください。<br/>
    /// CanConvertメソッドの結果がFalseであった場合は配列のデータを一つ減らして、再度CanConvertメソッドを呼び出します。(配列のデータが1個になった場合は合字のグリフの探索は終了です)<br/>
    /// CanConvertメソッドの結果がTrueであった場合はConvertメソッドを呼び出して、合字のグリフインデックスを取得してください。</remarks>
    public sealed class LigatureGlyphConverter
    {

        //最初のグリフをキーとする最大文字数のリスト
        private Dictionary<ushort, int> ligatureMaxLength = new Dictionary<ushort, int>();
        //すべての合字リスト
        private Dictionary<string, ushort> ligature = new Dictionary<string, ushort>();

        internal LigatureGlyphConverter()
            : base()
        {
        }

        /// <summary>指定したグリフインデックスを最初のグリフとする合字が存在するかを返します。</summary>
        /// <param name="glyphIndex">確認するグリフインデックス</param>
        /// <returns>合字の最初のグリフとして存在する場合はTrueを返します。</returns>
        public bool HasLigature(ushort glyphIndex)
        {
            return ligatureMaxLength.ContainsKey(glyphIndex);
        }

        /// <summary>指定したグリフインデックスの配列の順序の合字が存在するかを返します。</summary>
        /// <param name="glyphIndex">合字グリフに変換するグリフインデックスのリスト</param>
        /// <returns>合字のグリフが存在する場合はTrueを返します。</returns>
        public bool CanConvert(ushort[] glyphIndex)
        {
            string key = GetKey(glyphIndex);
            return ligature.ContainsKey(key);
        }

        /// <summary>指定したグリフインデックスを最初のグリフとする合字の最大文字数を返します。</summary>
        /// <param name="glyphIndex">確認するグリフインデックス</param>
        /// <returns>合字が存在しない場合は1、最大2文字による合字がある場合は2、最大3文字による合字がある場合は3が返ります。(以降も同様)</returns>
        public int GetLigatureMaxLength(ushort glyphIndex)
        {
            if (ligatureMaxLength.ContainsKey(glyphIndex))
            {
                return ligatureMaxLength[glyphIndex];
            }
            else
            {
                return 1;
            }
        }

        /// <summary>指定したグリフインデックスの配列の合字を取得します。</summary>
        /// <param name="glyphIndex">合字グリフに変換するグリフインデックスのリスト</param>
        /// <returns>合字のグリフが存在する場合は合字のグリフインデックス、存在しない場合は0を返します。</returns>
        public ushort Convert(ushort[] glyphIndex)
        {

            string key = GetKey(glyphIndex);
            if (ligature.ContainsKey(key))
            {
                return ligature[key];
            }
            else
            {
                return 0;
            }

        }

        /// <summary>合字グリフのデータ件数を取得します。</summary>
        public int Count
        {
            get { return ligature.Count; }
        }


        internal void Add(LigatureSubstitution lig)
        {
            ushort first = lig.GlyphIndex[0];
            if (ligatureMaxLength.ContainsKey(first))
            {
                if (ligatureMaxLength[first] < lig.GlyphIndex.Count)
                {
                    ligatureMaxLength[first] = lig.GlyphIndex.Count;
                }
            }
            else
            {
                ligatureMaxLength.Add(first, lig.GlyphIndex.Count);
            }

            string key = GetKey(lig.GlyphIndex.ToArray());
            if (ligature.ContainsKey(key))
            {
                ligature[key] = lig.SubstitutionGlyphIndex;
            }
            else
            {
                ligature.Add(key, lig.SubstitutionGlyphIndex);
            }

        }

        private string GetKey(ushort[] lig)
        {
            if (lig.Length == 0)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            foreach (ushort gid in lig)
            {
                sb.Append(gid + "x");
            }
            sb.Length = sb.Length - 1;
            return sb.ToString();
        }

    }
}
