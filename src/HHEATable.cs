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
    /// <summary>hheaテーブル(フォントの横書きに関しての情報)の情報を管理します。</summary>
    /// <remarks>このクラスのコンストラクタはクラスライブラリの外部から呼び出すことはできません。</remarks>
    public sealed class HHEATable
    {
        internal HHEATable()
        {
        }

        /// <summary>address from beginning of font file.</summary>
        public long Address { get; set; }
        /// <summary>0x00010000 for version 1.0.</summary>
        public ushort TableVersionNumberMajor { get; set; }
        /// <summary>0x00010000 for version 1.0.</summary>
        public ushort TableVersionNumberMinor { get; set; }
        /// <summary>Typographic ascent.</summary>
        public short Ascender { get; set; }
        /// <summary>Typographic descent.</summary>
        public short Descender { get; set; }
        /// <summary>Typographic line gap. Negative LineGap values are treated as zero in Windows 3.1, System 6, and System 7.</summary>
        public short LineGap { get; set; }
        /// <summary>Maximum advance width value in ‘hmtx’ table.</summary>
        public ushort AdvanceWidthMax { get; set; }
        /// <summary>Minimum left sidebearing value in ‘hmtx’ table.</summary>
        public short MinLeftSideBearing { get; set; }
        /// <summary>Minimum right sidebearing value; calculated as Min(aw - lsb - (xMax - xMin)).</summary>
        public short MinRightSideBearing { get; set; }
        /// <summary>Max(lsb + (xMax - xMin)).</summary>
        public short XMaxExtent { get; set; }
        /// <summary>Used to calculate the slope of the cursor (rise/run); 1 for vertical.</summary>
        public short CaretSlopeRise { get; set; }
        /// <summary>0 for vertical.</summary>
        public short CaretSlopeRun { get; set; }
        /// <summary>(reserved)</summary>
        public short Reserved1 { get; set; }
        /// <summary>(reserved)</summary>
        public short Reserved2 { get; set; }
        /// <summary>(reserved)</summary>
        public short Reserved3 { get; set; }
        /// <summary>(reserved)</summary>
        public short Reserved4 { get; set; }
        /// <summary>(reserved)</summary>
        public short Reserved5 { get; set; }
        /// <summary>0 for current format.</summary>
        public short MetricDataFormat { get; set; }
        /// <summary>Number of hMetric entries in ‘hmtx’table; may be smaller than the total number of glyphs in the font.</summary>
        public ushort NumberOfHMetrics { get; set; }
    }
}
