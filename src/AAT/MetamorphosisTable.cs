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

namespace WaterTrans.TypeLoader.AAT
{

    /// <summary>AATのmortテーブルのMetamorphosisTable情報を管理します。</summary>
    public sealed class MetamorphosisTable
    {
        internal MetamorphosisTable()
        {
            this.Header = new BinarySearchHeader();
            this.SingleSubstitutionList = new List<SingleSubstitution>();
        }

        /// <summary>Length of subtable in bytes, including this header.</summary>
        public ushort Length { get; set; }
        /// <summary>Length of subtable in bytes, including this header.</summary>
        public ushort Coverage { get; set; }
        /// <summary>Flags for the settings that this subtable describes.</summary>
        public uint SubFeatureFlags { get; set; }
        /// <summary>Coverage field mask 0x8000 If set to 1, this subtable should be applied only to vertical text.</summary>
        public bool IsVerticalMetamorphosis { get; set; }
        /// <summary>Coverage field mask 0x0007.</summary>
        public int SubtableType { get; set; }
        /// <summary>ormat of this lookup table. There are five lookup table formats, each with a format number.</summary>
        public ushort Format { get; set; }
        /// <summary>BinarySearchingTable</summary>
        public BinarySearchHeader Header { get; set; }
        /// <summary>SingleSubstitution</summary>
        public List<SingleSubstitution> SingleSubstitutionList { get; set; }
    }
}
