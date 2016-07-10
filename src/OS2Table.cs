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
    /// <summary>OS/2テーブル(フォントの各種の追加情報)の情報を管理します。</summary>
    /// <remarks>このクラスのコンストラクタはクラスライブラリの外部から呼び出すことはできません。</remarks>
    public sealed class OS2Table
    {
        internal OS2Table()
        {
        }

        /// <summary>address from beginning of font file.</summary>
        public long Address { get; set; }
        /// <summary>OS/2 table version number.</summary>
        public ushort Version { get; set; }
        /// <summary>Average weighted escapement.</summary>
        public short AvgCharWidth { get; set; }
        /// <summary>Weight class.</summary>
        public ushort WeightClass { get; set; }
        /// <summary>Width class.</summary>
        public ushort WidthClass { get; set; }
        /// <summary>Type flags.</summary>
        public short Type { get; set; }
        /// <summary>Subscript horizontal font size.</summary>
        public short SubscriptXSize { get; set; }
        /// <summary>Subscript vertical font size.</summary>
        public short SubscriptYSize { get; set; }
        /// <summary>Subscript x offset.</summary>
        public short SubscriptXOffset { get; set; }
        /// <summary>Subscript y offset.</summary>
        public short SubscriptYOffset { get; set; }
        /// <summary>Superscript horizontal font size.</summary>
        public short SuperscriptXSize { get; set; }
        /// <summary>Superscript vertical font size.</summary>
        public short SuperscriptYSize { get; set; }
        /// <summary>Superscript x offset.</summary>
        public short SuperscriptXOffset { get; set; }
        /// <summary>Superscript y offset.</summary>
        public short SuperscriptYOffset { get; set; }
        /// <summary>Strikeout size.</summary>
        public short StrikeoutSize { get; set; }
        /// <summary>Strikeout position.</summary>
        public short StrikeoutPosition { get; set; }
        /// <summary>Font-family class and subclass.</summary>
        public short FamilyClass { get; set; }
        /// <summary>PANOSE classification number</summary>
        public byte Panose1 { get; set; }
        /// <summary>PANOSE classification number</summary>
        public byte Panose2 { get; set; }
        /// <summary>PANOSE classification number</summary>
        public byte Panose3 { get; set; }
        /// <summary>PANOSE classification number</summary>
        public byte Panose4 { get; set; }
        /// <summary>PANOSE classification number</summary>
        public byte Panose5 { get; set; }
        /// <summary>PANOSE classification number</summary>
        public byte Panose6 { get; set; }
        /// <summary>PANOSE classification number</summary>
        public byte Panose7 { get; set; }
        /// <summary>PANOSE classification number</summary>
        public byte Panose8 { get; set; }
        /// <summary>PANOSE classification number</summary>
        public byte Panose9 { get; set; }
        /// <summary>PANOSE classification number</summary>
        public byte Panose10 { get; set; }
        /// <summary>Unicode Character Range</summary>
        public uint UnicodeRange1 { get; set; }
        /// <summary>Unicode Character Range</summary>
        public uint UnicodeRange2 { get; set; }
        /// <summary>Unicode Character Range</summary>
        public uint UnicodeRange3 { get; set; }
        /// <summary>Unicode Character Range</summary>
        public uint UnicodeRange4 { get; set; }
        /// <summary>Font Vendor Identification.</summary>
        public string VendorID { get; set; }
        /// <summary>Font selection flags.</summary>
        public ushort Selection { get; set; }
        /// <summary>The minimum Unicode index (character code) in this font.</summary>
        public ushort FirstCharIndex { get; set; }
        /// <summary>The maximum Unicode index (character code) in this font.</summary>
        public ushort LastCharIndex { get; set; }
        /// <summary>The typographic ascender for this font.</summary>
        public short TypoAscender { get; set; }
        /// <summary>The typographic descender for this font.</summary>
        public short TypoDescender { get; set; }
        /// <summary>The typographic line gap for this font.</summary>
        public short TypoLineGap { get; set; }
        /// <summary>The ascender metric for Windows.</summary>
        public ushort WinAscent { get; set; }
        /// <summary>The descender metric for Windows.</summary>
        public ushort WinDescent { get; set; }
        /// <summary>Code Page Character Range</summary>
        public uint CodePageRange1 { get; set; }
        /// <summary>Code Page Character Range</summary>
        public uint CodePageRange2 { get; set; }
    }
}
