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

    /// <summary>headテーブル(フォントの概要)の情報を管理します。</summary>
    /// <remarks>このクラスのコンストラクタはクラスライブラリの外部から呼び出すことはできません。</remarks>
    public sealed class HEADTable
    {
        internal HEADTable()
        {
        }

        /// <summary>address from beginning of font file.</summary>
        public long Address { get; set; }
        /// <summary>0x00010000 for version 1.0.</summary>
        public ushort TableVersionNumberMajor { get; set; }
        /// <summary>0x00010000 for version 1.0.</summary>
        public ushort TableVersionNumberMinor { get; set; }
        /// <summary>Set by font manufacturer.</summary>
        public ushort FontRevisionMajor { get; set; }
        /// <summary>Set by font manufacturer.</summary>
        public ushort FontRevisionMinor { get; set; }
        /// <summary>To compute:  set it to 0, sum the entire font as ULONG, then store 0xB1B0AFBA - sum.</summary>
        public uint CheckSumAdjustment { get; set; }
        /// <summary>Set to 0x5F0F3CF5.</summary>
        public uint MagicNumber { get; set; }
        /// <summary>Bit 0 - baseline for font at y=0;Bit 1 - left sidebearing at x=0;Bit 2 - instructions may depend on point size;Bit 3 - force ppem to integer values for all internal scaler math; may use fractional ppem sizes if this bit is clear;Bit 4 - instructions may alter advance width (the advance widths might not scale linearly);Note: All other bits must be zero.</summary>
        public ushort Flags { get; set; }
        /// <summary>Valid range is from 16 to 16384</summary>
        public ushort UnitsPerEm { get; set; }
        /// <summary>International date (8-byte field).</summary>
        public long Created { get; set; }
        /// <summary>International date (8-byte field).</summary>
        public long Modified { get; set; }
        /// <summary>For all glyph bounding boxes.</summary>
        public long XMin { get; set; }
        /// <summary>For all glyph bounding boxes.</summary>
        public long YMin { get; set; }
        /// <summary>For all glyph bounding boxes.</summary>
        public long XMax { get; set; }
        /// <summary>For all glyph bounding boxes.</summary>
        public long YMax { get; set; }
        /// <summary>Bit 0 bold (if set to 1); Bit 1 italic (if set to 1)Bits 2-15 reserved (set to 0).</summary>
        public ushort MacStyle { get; set; }
        /// <summary>Smallest readable size in pixels.</summary>
        public ushort LowestRecPPEM { get; set; }
        /// <summary> 0   Fully mixed directional glyphs; 1   Only strongly left to right; 2   Like 1 but also contains neutrals ;-1   Only strongly right to left;-2   Like -1 but also contains neutrals.</summary>
        public short FontDirectionHint { get; set; }
        /// <summary>0 for short offsets, 1 for long.</summary>
        public short IndexToLocFormat { get; set; }
        /// <summary>0 for current format.</summary>
        public short GlyphDataFormat { get; set; }
    }
}
