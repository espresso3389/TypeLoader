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

namespace WaterTrans.TypeLoader.AAT
{
    /// <summary>AATのmortテーブルのFeatureTable情報を管理します。</summary>
    /// <remarks>このクラスのコンストラクタはクラスライブラリの外部から呼び出すことはできません。</remarks>
    public sealed class FeatureTable
    {
        internal FeatureTable()
        {
        }

        /// <summary>The type of feature.</summary>
        public ushort FeatureType { get; set; }
        /// <summary>The feature's setting.</summary>
        public ushort FeatureSetting { get; set; }
        /// <summary>Flags for the settings that this feature and setting enables.</summary>
        public uint EnableFlags { get; set; }
        /// <summary>Complement of flags for the settings that this feature and setting disable.</summary>
        public uint DisableFlags { get; set; }
    }
}
