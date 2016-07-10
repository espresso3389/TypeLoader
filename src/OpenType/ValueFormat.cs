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
    /// <summary>XPlacement bit enumeration</summary>
    public enum ValueFormat : ushort
    {
        /// <summary>Includes horizontal adjustment for placement.</summary>
        XPlacement = 0x1,
        /// <summary>Includes vertical adjustment for placement.</summary>
        YPlacement = 0x2,
        /// <summary>Includes horizontal adjustment for advance.</summary>
        XAdvance = 0x4,
        /// <summary>Includes vertical adjustment for advance.</summary>
        YAdvance = 0x8,
        /// <summary>Includes horizontal Device table for placement.</summary>
        XPlaDevice = 0x10,
        /// <summary>Includes vertical Device table for placement.</summary>
        YPlaDevice = 0x20,
        /// <summary>Includes horizontal Device table for advance.</summary>
        XAdvDevice = 0x40,
        /// <summary>Includes vertical Device table for advance.</summary>
        YAdvDevice = 0x80
    }
}
