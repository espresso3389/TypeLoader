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
    /// <summary>OpenTypeのGPOSテーブルの位置調整情報を管理します。</summary>
    /// <remarks>このクラスのコンストラクタはクラスライブラリの外部から呼び出すことはできません。</remarks>
    public sealed class ValueRecord
    {
        internal ValueRecord()
        {
        }

        /// <summary>ValueFormat</summary>
        public ushort ValueFormat { get; set; }
        /// <summary>Horizontal adjustment for placement.</summary>
        public short XPlacement { get; set; }
        /// <summary>Vertical adjustment for placement.</summary>
        public short YPlacement { get; set; }
        /// <summary>Horizontal adjustment for advance.</summary>
        public short XAdvance { get; set; }
        /// <summary>Vertical adjustment for advance.</summary>
        public short YAdvance { get; set; }
        /// <summary>Offset to Device table for horizontal placement.</summary>
        public ushort XPlaDevice { get; set; }
        /// <summary>Offset to Device table for vertical placement.</summary>
        public ushort YPlaDevice { get; set; }
        /// <summary>Offset to Device table for horizontal advance.</summary>
        public ushort XAdvDevice { get; set; }
        /// <summary>Offset to Device table for vertical advance.</summary>
        public ushort YAdvDevice { get; set; }
        /// <summary>位置調整情報が設定されているか否かを取得します。</summary>
        public bool IsEmpty
        {
            get
            {
                if (ValueFormat == 0)
                {
                    return true;
                }
                else
                {
                    if ((ValueFormat & (ushort)OpenType.ValueFormat.XPlacement) > 0)
                    {
                        if (XPlacement != 0)
                        {
                            return false;
                        }
                    }
                    if ((ValueFormat & (ushort)OpenType.ValueFormat.YPlacement) > 0)
                    {
                        if (YPlacement != 0)
                        {
                            return false;
                        }
                    }
                    if ((ValueFormat & (ushort)OpenType.ValueFormat.XAdvance) > 0)
                    {
                        if (XAdvance != 0)
                        {
                            return false;
                        }
                    }
                    if ((ValueFormat & (ushort)OpenType.ValueFormat.YAdvance) > 0)
                    {
                        if (YAdvance != 0)
                        {
                            return false;
                        }
                    }
                    if ((ValueFormat & (ushort)OpenType.ValueFormat.XPlaDevice) > 0)
                    {
                        if (XPlaDevice != 0)
                        {
                            return false;
                        }
                    }
                    if ((ValueFormat & (ushort)OpenType.ValueFormat.YPlaDevice) > 0)
                    {
                        if (YPlaDevice != 0)
                        {
                            return false;
                        }
                    }
                    if ((ValueFormat & (ushort)OpenType.ValueFormat.XAdvDevice) > 0)
                    {
                        if (XAdvDevice != 0)
                        {
                            return false;
                        }
                    }
                    if ((ValueFormat & (ushort)OpenType.ValueFormat.YAdvDevice) > 0)
                    {
                        if (YAdvDevice != 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
        }
    }
}
