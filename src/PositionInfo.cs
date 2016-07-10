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
    /// <summary>位置調整情報を保持します。</summary>
    /// <remarks>それぞれの調整情報はフォントのemを1とした割合で表されます。X座標は右が正、Y座標は上が正となります。<br/>
    /// たとえばフォントを10mmで描画しているときにXPlacementが-0.1、XAdvanceが-0.2だった場合、これは左方向に文字を1mmずらして、仮想ボディの幅を2mm縮めることを意味します。</remarks>
    public struct PositionInfo
    {
        /// <summary>グリフの描画基点のX方向へのオフセット(右方向が正、左方向が負)</summary>
        public double XPlacement { get; set; }
        /// <summary>グリフの描画基点のY方向へのオフセット(上方向が正、下方向が負)</summary>
        public double YPlacement { get; set; }
        /// <summary>グリフの仮想ボディの幅への調整(拡大が正、縮小が負)</summary>
        public double XAdvance { get; set; }
        /// <summary>グリフの仮想ボディの高さへの調整(拡大が正、縮小が負)</summary>
        public double YAdvance { get; set; }
    }
}
