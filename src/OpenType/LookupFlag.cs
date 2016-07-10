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

namespace WaterTrans.TypeLoader.OpenType
{
    /// <summary>LookupFlag bit enumeration</summary>
    public enum LookupFlag : ushort
    {
        /// <summary>If set, implied glyph order is right to left.- otherwise, glyph order is left to right</summary>
        RightToLeft = 0x1,
        /// <summary>If set, skips over base glyphs.</summary>
        IgnoreBaseGlyphs = 0x2,
        /// <summary>If set, skips over ligatures.</summary>
        IgnoreLigatures = 0x4,
        /// <summary>If set, skips over combining marks.</summary>
        IgnoreMarks = 0x8,
        /// <summary>For future use.</summary>
        Reserved = 0xfff0
    }
}
