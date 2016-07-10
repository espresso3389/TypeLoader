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
    /// <summary>vheadテーブル(フォントの縦書きに関しての情報)の情報を管理します。</summary>
    /// <remarks>このクラスのコンストラクタはクラスライブラリの外部から呼び出すことはできません。</remarks>
    public sealed class VHEATable
    {
        internal VHEATable() 
        {
        }

        /// <summary>address from beginning of font file.</summary>
        public long Address;
        /// <summary>0x00010000 for version 1.0.</summary>
        public ushort TableVersionNumberMajor;
        /// <summary>0x00010000 for version 1.0.</summary>
        public ushort TableVersionNumberMinor;
        /// <summary>Distance in FUnits from the centerline to the previous line’s descent.</summary>
        public Int16 Ascender;
        /// <summary>Distance in FUnits from the centerline to the next line’s ascent.</summary>
        public Int16 Descender;
        /// <summary>Reserved; set to 0.</summary>
        public Int16 LineGap;
        /// <summary>The maximum advance height measurement in FUnits found in the font. This value must be consistent with the entries in the vertical metrics table.</summary>
        public ushort AdvanceHeightMax;
        /// <summary>The minimum top sidebearing measurement found in the font, in FUnits. This value must be consistent with the entries in the vertical metrics table.</summary>
        public Int16 MinTopSideBearing;
        /// <summary>The minimum bottom sidebearing measurement found in the font, in FUnits. This value must be consistent with the entries in the vertical metrics table.</summary>
        public Int16 MinBottomSideBearing;
        /// <summary>Defined as yMaxExtent=minTopSideBearing+(yMax-yMin).</summary>
        public Int16 YMaxExtent;
        /// <summary>The value of the caretSlopeRise field divided by the value of the caretSlopeRun Field determines the slope of the caret. A value of 0 for the rise and a value of 1 for the run specifies a horizontal caret. A value of 1 for the rise and a value of 0 for the run specifies a vertical caret. Intermediate values are desirable for fonts whose glyphs are oblique or italic. For a vertical font, a horizontal caret is best.</summary>
        public Int16 CaretSlopeRise;
        /// <summary>See the caretSlopeRise field. Value=1 for nonslanted vertical fonts.</summary>
        public Int16 CaretSlopeRun;
        /// <summary>The amount by which the highlight on a slanted glyph needs to be shifted away from the glyph in order to produce the best appearance. Set value equal to 0 for nonslanted fonts.</summary>
        public Int16 CaretOffset;
        /// <summary>(reserved)</summary>
        public Int16 Reserved1;
        /// <summary>(reserved)</summary>
        public Int16 Reserved2;
        /// <summary>(reserved)</summary>
        public Int16 Reserved3;
        /// <summary>(reserved)</summary>
        public Int16 Reserved4;
        /// <summary>0 for current format.</summary>
        public Int16 MetricDataFormat;
        /// <summary>Number of advance heights in the vertical metrics table.</summary>
        public ushort NumberOfVMetrics;
    }
}
