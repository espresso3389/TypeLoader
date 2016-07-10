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
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using WaterTrans.TypeLoader.OpenType;
using WaterTrans.TypeLoader.AAT;

namespace WaterTrans.TypeLoader
{
    /// <summary>TypeLoaderの基幹クラスです。フォントファイルのデータを解析してアクセスのしやすいデータ構造に変換します。</summary>
    /// <remarks>
    /// 二種類のコンストラクタが用意されています。一方はTrueType（拡張子がttfのファイル）またはOpenType（拡張子がotfのファイル）フォントの読み込みに使用します。<br/>
    /// もう一方は、TrueType Collection（拡張子がttcのファイル）の読み込みに使用します。<br/>
    /// <br/>
    /// フォントファイル内部のデータは複数のテーブルによって構成されており、これらのテーブルへのアクセスはTableDirectoriesプロパティによって提供されます。<br/>
    /// 各テーブルは独自のデータ構造を持っており、公開されている仕様書に沿ってバイナリデータを読み込むことでその内容にアクセス可能です。<br/>
    /// なお、WPFのGlyphTypefaceクラスによって多くのフォント情報にアクセスが可能になっていますので、TypefaceInfoクラスはその情報を補完することを目的としています。<br/>
    /// <br/>
    /// 例えば、TypefaceInfoクラスはGlyphTypefaceクラスが読み込まない、日本語の組版には重要な以下の機能を実現するための情報をフォントファイルから直接読み込みます。<br/>
    /// ・縦書きグリフへの変換(GSUBテーブルまたはmortテーブル)<br/>
    /// ・句読点や約物の半角幅(GPOSテーブル)<br/>
    /// ・プロポーショナルメトリクス(GPOSテーブル)<br/>
    /// これらの情報にはTypefaceInfoクラスのテーブル名と同名のプロパティ経由でアクセスが可能となっていますが、独自のデータ構造を理解していなければ目的の情報にたどり着くことができません。<br/>
    /// <br/>
    /// そこでTypefaceInfoクラスには独自のデータ構造の理解しなくても利用可能にするための以下の便利なメソッドを提供しています。<br/>
    /// ・縦書きグリフへの変換情報を作成して返すGetVerticalGlyphConverterメソッド、GetAdvancedVerticalGlyphConverterメソッド<br/>
    /// ・句読点や約物を半角幅で組めるように位置調整情報を作成して返すGetHalfAdjustmentMetricsメソッド<br/>
    /// ・プロポーショナルメトリクスで組めるように位置調整情報を作成して返すGetProportionalAdjustmentMetricsメソッド<br/>
    /// ・欧文をカーニングメトリクスで組めるように位置調整情報を作成して返すGetKerningAdjustmentMetricsメソッド<br/>
    /// ・標準で適用することが望ましい合字変換情報を作成して返すGetLigatureGlyphConverterメソッド<br/>
    /// なお、フォントが該当する情報を持っていない場合、これらのメソッドは件数がゼロのデータを返します。<br/>
    /// <br/>
    /// TypefaceInfoクラスはフォントファイルの内容を読み込む際にアクセスしやすいように冗長なデータ構造に展開しています。<br/>
    /// 例えば、フォントファイル内部の仕様では多くの場面において『1～20までのグリフ』といった範囲指定による定義方法によってデータを圧縮していますが、TypefaceInfoクラスをこれをリストに展開しています。<br/>
    /// したがってTypefaceInfoのインスタンスが使うメモリの量はとても多くなる傾向にあります。必要がなくなったらすみやかに参照から解放するようにしてください。<br/>
    /// </remarks>
    public sealed class TypefaceInfo
    {
        /// <summary>TrueType, OpenType, TrueType Collectionのフォント情報を読み込みます</summary>
        /// <param name="strm">読み込むフォントファイルのStream</param>
        /// <remarks>TrueType Collectionの場合は最初のフォント情報を取得します。</remarks>
        public TypefaceInfo(Stream strm)
            : this(strm, 0)
        {
        }

        /// <summary>TrueType, OpenType, TrueType Collectionのフォント情報を読み込みます</summary>
        /// <param name="strm">読み込むフォントファイルのStream</param>
        /// <param name="num">TrueType Collectionのインデックス番号(ゼロ基点の番号)</param>
        /// <remarks>フォントファイルの拡張子が、ttf, otf, ttcのものがサポートされます。<br/>
        /// TrueType Collectionの場合は読み込む対象のフォントを一つ指定してください。</remarks>
        public TypefaceInfo(Stream strm, int num)
        {
            this.TableDirectories = new List<TableDirectory>();

            if (IsCollection(strm))
            {
                ReadCollectionTypeface(strm, num);
            }
            else
            {
                ReadTypeface(strm);
            }
        }

        /// <summary>Sfnt Major Version</summary>
        public ushort SfntVersionMajor { get; set; }
        /// <summary>Sfnt Minor Version</summary>
        public ushort SfntVersionMinor { get; set; }
        /// <summary>Number of tables.</summary>
        public ushort NumTables { get; set; }
        /// <summary>(Maximum power of 2 ＜＝ numTables) x 16.</summary>
        public ushort SearchRange { get; set; }
        /// <summary>Log2(maximum power of 2 ＜＝ numTables).</summary>
        public ushort EntrySelector { get; set; }
        /// <summary>NumTables x 16-searchRange.</summary>
        public ushort RangeShift { get; set; }
        /// <summary>TableDirectory</summary>
        public List<TableDirectory> TableDirectories { get; set; }
        /// <summary>Contains overall font metrics and checksum for font. Also contains font appearance (Mac).</summary>
        public HEADTable HEAD { get; set; }
        /// <summary>Contains overall horizontal metrics and caret slope. Also contains line spacing (Mac).</summary>
        public HHEATable HHEA { get; set; }
        /// <summary>Contains line spacing, font weight, font style, codepoint ranges (codepage and Unicode) covered by glyphs, overall appearance, sub- and super-script support, strike out information.</summary>
        public OS2Table OS2 { get; set; }
        /// <summary>Vertical metrics analagous to hmtx, hhea, hdmx. Used for vertical writing systems.</summary>
        public VHEATable VHEA { get; set; }
        /// <summary>Contains tables specifying AAT “glyph metamorphosis” effects: ligatures, contextual or non-contextual substitutions, insertions, and rearrangements. </summary>
        /// <remarks>縦書きへの置換以外の機能はすべて読み込みをスキップしています。</remarks>
        public MORTTable MORT { get; set; }
        /// <summary>Contains glyph substitution data. The following types of substitutions are supported: one to one, one to many, one to several alternates, many to one, and contextual use of the preceding.</summary>
        /// <remarks>文脈依存の置換などの高度な機能はすべて読み込みをスキップしています。</remarks>
        public CommonTable GSUB { get; set; }
        /// <summary>Contains glyph positioning data. The following types of positioning are supported: single glyph adjustment, pair adjustment, cursive attachment, mark (diacritic) to base, mark to ligature, mark to mark, and contextual use of the preceding.</summary>
        /// <remarks>単一グリフの位置調整、二つのグリフ間の位置調整以外の高度な機能はすべて読み込みをスキップしています。</remarks>
        public CommonTable GPOS { get; set; }

        /// <summary>縦書きグリフへの変換表を作成して返します</summary>
        /// <returns>縦書きグリフへの変換表</returns>
        /// <remarks>縦書きの場合はこのメソッドを呼び出して変換表を取得します。<br/>
        /// この変換表によって横書きグリフのインデックスを縦書きグリフのインデックスに変換できます。<br/>
        /// GSUBテーブルのvertフィーチャーまたはmortテーブルの情報に基づいています。これらの情報を持たないフォントの場合は件数がゼロのデータを返します。
        /// このメソッドでは、vertフィーチャーを基にした句読点や約物などの変換表を作成します。<br/>
        /// vrt2フィーチャーを基にした欧文用の文字などの変換表が必要な場合はGetAdvancedVerticalGlyphConverterメソッドを呼び出してください。<br/>
        /// vertフィーチャーが存在しない場合でmortテーブルに縦書き用の変換が定義されている場合はmortテーブルの内容を返します。<br/>
        /// </remarks>
        public SingleGlyphConverter GetVerticalGlyphConverter()
        {
            return GetVerticalGlyphConverter(false);
        }

        /// <summary>縦書きグリフへの変換表を作成して返します。</summary>
        /// <returns>縦書きグリフへの変換表が返ります。</returns>
        /// <remarks>縦書きの場合はこのメソッドを呼び出して変換表を取得します。<br/>
        /// この変換表によって横書きグリフのインデックスを縦書きグリフのインデックスに変換できます。<br/>
        /// GSUBテーブルのvertフィーチャーおよびvrt2フィーチャーまたはmortテーブルの情報に基づいています。これらの情報を持たないフォントの場合は件数がゼロのデータを返します。<br/>
        /// このメソッドでは、vrt2フィーチャーを基にした句読点や約物などに加えて欧文用の文字などの変換表を作成します。<br/>
        /// vertフィーチャーを基にした句読点や約物などに限った変換表が必要な場合はGetVerticalGlyphConverterメソッドを呼び出してください。<br/>
        /// また、vrt2フィーチャーが存在しない場合でvertフィーチャーのみが存在する場合はvertフィーチャーの内容を返します。<br/>
        /// vertフィーチャーとvrt2フィーチャーが存在しない場合でmortテーブルに縦書き用の変換が定義されている場合はmortテーブルの内容を返します。<br/>
        /// </remarks>
        public SingleGlyphConverter GetAdvancedVerticalGlyphConverter()
        {
            return GetVerticalGlyphConverter(true);
        }

        /// <summary>標準で適用することが望ましい合字変換表を作成して返します。</summary>
        /// <returns>複数のグリフの合字変換の表が返ります。</returns>
        /// <remarks>GSUBテーブルのligaフィーチャーの情報に基づいています。これらの情報を持たないフォントの場合は件数がゼロのデータを返します。</remarks>
        public LigatureGlyphConverter GetLigatureGlyphConverter()
        {
            LigatureGlyphConverter ret = new LigatureGlyphConverter();

            if (this.GSUB != null)
            {
                int featureIndex = -1;
                featureIndex = GetOpenTypeFeatureIndex(this.GSUB, "kana", "liga");
                if (featureIndex == -1)
                {
                    featureIndex = GetOpenTypeFeatureIndex(this.GSUB, "latn", "liga");
                }
                if (featureIndex != -1)
                {
                    foreach (ushort lookupIndex in this.GSUB.FeatureList[featureIndex].LookupListIndex)
                    {
                        foreach (OpenType.LigatureSubstitution lig in this.GSUB.LookupList[lookupIndex].LigatureSubstitutionList)
                        {
                            ret.Add(lig);
                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>プロポーショナルメトリクスで組めるように位置調整情報の表を作成して返します。</summary>
        /// <param name="vertical">縦書きの場合はTrueを指定してください。縦方向へのプロポーショナルメトリクスの位置調整情報を取得します。</param>
        /// <returns>一つのグリフに適用する位置調整情報の表が返ります。</returns>
        /// <remarks>GPOSテーブルのpaltフィーチャーおよびvpalフィーチャーの情報に基づいています。これらの情報を持たないフォントの場合は件数がゼロのデータを返します。</remarks>
        public SingleAdjustmentMetrics GetProportionalAdjustmentMetrics(bool vertical)
        {
            SingleAdjustmentMetrics ret = new SingleAdjustmentMetrics();

            if (this.GPOS != null)
            {
                int featureIndex = -1;
                if (vertical)
                {
                    featureIndex = GetOpenTypeFeatureIndex(this.GPOS, "kana", "vpal");
                }
                else
                {
                    featureIndex = GetOpenTypeFeatureIndex(this.GPOS, "kana", "palt");
                }
                if (featureIndex != -1)
                {
                    foreach (ushort lookupIndex in this.GPOS.FeatureList[featureIndex].LookupListIndex)
                    {
                        foreach (OpenType.SingleAdjustment sa in this.GPOS.LookupList[lookupIndex].SingleAdjustmentList)
                        {
                            ret.Add(sa.GlyphIndex, sa.ValueRecord, this.HEAD.UnitsPerEm);
                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>句読点や約物を半角幅で組めるように位置調整情報の表を作成して返します。</summary>
        /// <param name="vertical">縦書きの場合はTrueを指定してください。縦方向への半角幅の位置調整情報を取得します。</param>
        /// <returns>一つのグリフに適用する位置調整情報の表が返ります。</returns>
        /// <remarks>GPOSテーブルのhaltフィーチャーおよびvhalフィーチャーの情報に基づいています。これらの情報を持たないフォントの場合は件数がゼロのデータを返します。</remarks>
        public SingleAdjustmentMetrics GetHalfAdjustmentMetrics(bool vertical)
        {
            SingleAdjustmentMetrics ret = new SingleAdjustmentMetrics();

            if (this.GPOS != null)
            {
                int featureIndex = -1;
                if (vertical)
                {
                    featureIndex = GetOpenTypeFeatureIndex(this.GPOS, "kana", "vhal");
                }
                else
                {
                    featureIndex = GetOpenTypeFeatureIndex(this.GPOS, "kana", "halt");
                }
                if (featureIndex != -1)
                {
                    foreach (ushort lookupIndex in this.GPOS.FeatureList[featureIndex].LookupListIndex)
                    {
                        foreach (OpenType.SingleAdjustment sa in this.GPOS.LookupList[lookupIndex].SingleAdjustmentList)
                        {
                            ret.Add(sa.GlyphIndex, sa.ValueRecord, this.HEAD.UnitsPerEm);
                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>欧文をカーニングメトリクスで組めるように位置調整情報の表を作成して返します。</summary>
        /// <returns>前後のグリフに適用する位置調整情報の表が返ります。</returns>
        /// <remarks>GPOSテーブルのkernフィーチャーの情報に基づいています。これらの情報を持たないフォントの場合は件数がゼロのデータを返します。</remarks>
        public PairAdjustmentMetrics GetKerningAdjustmentMetrics()
        {
            PairAdjustmentMetrics ret = new PairAdjustmentMetrics();

            if (this.GPOS != null)
            {
                int featureIndex = -1;
                featureIndex = GetOpenTypeFeatureIndex(this.GPOS, "kana", "kern");
                if (featureIndex == -1)
                {
                    featureIndex = GetOpenTypeFeatureIndex(this.GPOS, "latn", "kern");
                }
                if (featureIndex != -1)
                {
                    foreach (ushort lookupIndex in this.GPOS.FeatureList[featureIndex].LookupListIndex)
                    {
                        foreach (OpenType.PairAdjustment pa in this.GPOS.LookupList[lookupIndex].PairAdjustmentList)
                        {
                            ret.Add(pa.FirstGlyphIndex, pa.FirstValueRecord, pa.SecondGlyphIndex, pa.SecondValueRecord, this.HEAD.UnitsPerEm);
                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// 指定したStreamに収録されている書体の数を返します。（TrueType Collection）
        /// </summary>
        /// <param name="strm">Stream</param>
        /// <returns>1以上の数字</returns>
        /// <remarks>TrueType Collection以外の場合はInvalidOperationExceptionが発生します。</remarks>
        public static int GetCollectionCount(Stream strm)
        {
            if (IsCollection(strm) == false)
            {
                throw new InvalidOperationException("TrueType Collectionではありません。");
            }

            long current = strm.Position;

            TypefaceReader reader = new TypefaceReader(strm);
            string ttctag = reader.ReadCharArray(4);
            ushort ttcVersionMajor = reader.ReadUInt16();
            ushort ttcVersionMinor = reader.ReadUInt16();
            uint ttcDirectoryCount = reader.ReadUInt32();
            strm.Position = current;
            return (int)ttcDirectoryCount;
        }

        /// <summary>
        /// 指定したStreamがTrueType Collectionかどうかを返します。
        /// </summary>
        /// <param name="strm">Stream</param>
        /// <returns>True:TrueType Collection</returns>
        public static bool IsCollection(Stream strm)
        {
            long current = strm.Position;
            TypefaceReader reader = new TypefaceReader(strm);
            string ttctag = reader.ReadCharArray(4);
            strm.Position = current;
            return (ttctag == "ttcf");
        }

        private SingleGlyphConverter GetVerticalGlyphConverter(bool enableAdvanced)
        {
            SingleGlyphConverter ret = new SingleGlyphConverter();

            if (this.GSUB != null)
            {
                int featureIndex = -1;
                if (enableAdvanced)
                {
                    featureIndex = GetOpenTypeFeatureIndex(this.GSUB, "kana", "vrt2");
                }
                if (featureIndex == -1)
                {
                    featureIndex = GetOpenTypeFeatureIndex(this.GSUB, "kana", "vert");
                }
                if (featureIndex != -1)
                {
                    foreach (ushort lookupIndex in this.GSUB.FeatureList[featureIndex].LookupListIndex)
                    {
                        foreach (OpenType.SingleSubstitution ssb in this.GSUB.LookupList[lookupIndex].SingleSubstitutionList)
                        {
                            ret.Add(ssb.GlyphIndex, ssb.SubstitutionGlyphIndex);
                        }
                    }
                }
            }
            else if (this.MORT != null)
            {
                foreach (MetamorphosisTable mt in this.MORT.Chains[0].MetamorphosisTables)
                {
                    if (mt.IsVerticalMetamorphosis)
                    {
                        foreach (OpenType.SingleSubstitution ssb in mt.SingleSubstitutionList)
                        {
                            ret.Add(ssb.GlyphIndex, ssb.SubstitutionGlyphIndex);
                        }
                        break;
                    }
                }
            }
            return ret;
        }

        private void ReadCollectionTypeface(Stream strm, int num)
        {
            TypefaceReader reader = new TypefaceReader(strm);
            string ttctag = reader.ReadCharArray(4);
            ushort ttcVersionMajor = reader.ReadUInt16();
            ushort ttcVersionMinor = reader.ReadUInt16();
            uint ttcDirectoryCount = reader.ReadUInt32();

            for (int i = 0; i <= Convert.ToInt32(ttcDirectoryCount - 1); i++)
            {
                uint ttcDirectoryOffset = reader.ReadUInt32();
                if (i == num)
                {
                    strm.Position = ttcDirectoryOffset;
                    ReadTypeface(strm);
                    return;
                }
            }

            throw new InvalidOperationException("指定したTrueType Collectionファイルに " + num + " 番目のフォントは存在しません。");

        }

        private void ReadTypeface(Stream strm)
        {
            TypefaceReader reader = new TypefaceReader(strm);
            ReadDirectory(reader);

            //headテーブルの読み込み
            TableDirectory head = GetTableDirectory("head");
            if (head != null)
            {
                this.HEAD = new HEADTable();
                ReadHEAD(reader, this.HEAD, head.Offset);
            }

            //hheaテーブルの読み込み
            TableDirectory hhea = GetTableDirectory("hhea");
            if (hhea != null)
            {
                this.HHEA = new HHEATable();
                ReadHHEA(reader, this.HHEA, hhea.Offset);
            }

            //Os2テーブルの読み込み
            TableDirectory os2 = GetTableDirectory("OS/2");
            if (os2 != null)
            {
                this.OS2 = new OS2Table();
                ReadOs2(reader, this.OS2, os2.Offset);
            }

            //vheaテーブルの読み込み
            TableDirectory vhea = GetTableDirectory("vhea");
            if (vhea != null)
            {
                this.VHEA = new VHEATable();
                ReadVHEA(reader, this.VHEA, vhea.Offset);
            }

            //mortテーブルの読み込み
            TableDirectory mort = GetTableDirectory("mort");
            if (mort != null)
            {
                this.MORT = new MORTTable();
                ReadMORT(reader, this.MORT, mort.Offset);
            }

            //GSUBテーブルの読み込み
            TableDirectory gsub = GetTableDirectory("GSUB");
            if (gsub != null)
            {
                this.GSUB = new CommonTable();
                ReadCommon(reader, this.GSUB, gsub.Offset);
                ReadGSUBSubTable(reader, this.GSUB);
            }

            //GPOSテーブルの読み込み
            TableDirectory gpos = GetTableDirectory("GPOS");
            if (gpos != null)
            {
                this.GPOS = new CommonTable();
                ReadCommon(reader, this.GPOS, gpos.Offset);
                ReadGPOSSubTable(reader, this.GPOS);
            }
        }

        private void ReadDirectory(TypefaceReader reader)
        {
            //テーブル構成情報の読み込み
            this.SfntVersionMajor = reader.ReadUInt16();
            this.SfntVersionMinor = reader.ReadUInt16();
            this.NumTables = reader.ReadUInt16();
            this.SearchRange = reader.ReadUInt16();
            this.EntrySelector = reader.ReadUInt16();
            this.RangeShift = reader.ReadUInt16();

            //テーブルの読み込み
            for (int i = 1; i <= this.NumTables; i++)
            {
                TableDirectory td = new TableDirectory();
                this.TableDirectories.Add(td);
                td.Tag = reader.ReadCharArray(4);
                td.CheckSum = reader.ReadUInt32();
                td.Offset = reader.ReadUInt32();
                td.Length = reader.ReadUInt32();
            }
        }

        private void ReadHEAD(TypefaceReader reader, HEADTable head, long address)
        {
            reader.BaseStream.Position = address;

            //headテーブル情報の読み込み
            head.Address = address;
            head.TableVersionNumberMajor = reader.ReadUInt16();
            head.TableVersionNumberMinor = reader.ReadUInt16();
            head.FontRevisionMajor = reader.ReadUInt16();
            head.FontRevisionMinor = reader.ReadUInt16();
            head.CheckSumAdjustment = reader.ReadUInt32();
            head.MagicNumber = reader.ReadUInt32();
            head.Flags = reader.ReadUInt16();
            head.UnitsPerEm = reader.ReadUInt16();
            head.Created = reader.ReadInt64();
            head.Modified = reader.ReadInt64();
            head.XMin = reader.ReadInt16();
            head.YMin = reader.ReadInt16();
            head.XMax = reader.ReadInt16();
            head.YMax = reader.ReadInt16();
            head.MacStyle = reader.ReadUInt16();
            head.LowestRecPPEM = reader.ReadUInt16();
            head.FontDirectionHint = reader.ReadInt16();
            head.IndexToLocFormat = reader.ReadInt16();
            head.GlyphDataFormat = reader.ReadInt16();
        }


        private void ReadHHEA(TypefaceReader reader, HHEATable hhea, long address)
        {
            reader.BaseStream.Position = address;

            //hheaテーブル情報の読み込み
            hhea.Address = address;
            hhea.TableVersionNumberMajor = reader.ReadUInt16();
            hhea.TableVersionNumberMinor = reader.ReadUInt16();
            hhea.Ascender = reader.ReadInt16();
            hhea.Descender = reader.ReadInt16();
            hhea.LineGap = reader.ReadInt16();
            hhea.AdvanceWidthMax = reader.ReadUInt16();
            hhea.MinLeftSideBearing = reader.ReadInt16();
            hhea.MinRightSideBearing = reader.ReadInt16();
            hhea.XMaxExtent = reader.ReadInt16();
            hhea.CaretSlopeRise = reader.ReadInt16();
            hhea.CaretSlopeRun = reader.ReadInt16();
            hhea.Reserved1 = reader.ReadInt16();
            hhea.Reserved2 = reader.ReadInt16();
            hhea.Reserved3 = reader.ReadInt16();
            hhea.Reserved4 = reader.ReadInt16();
            hhea.Reserved5 = reader.ReadInt16();
            hhea.MetricDataFormat = reader.ReadInt16();
            hhea.NumberOfHMetrics = reader.ReadUInt16();
        }


        private void ReadOs2(TypefaceReader reader, OS2Table os2, long address)
        {
            reader.BaseStream.Position = address;

            //OS/2テーブル情報の読み込み
            os2.Address = address;
            os2.Version = reader.ReadUInt16();
            os2.AvgCharWidth = reader.ReadInt16();
            os2.WeightClass = reader.ReadUInt16();
            os2.WidthClass = reader.ReadUInt16();
            os2.Type = reader.ReadInt16();
            os2.SubscriptXSize = reader.ReadInt16();
            os2.SubscriptYSize = reader.ReadInt16();
            os2.SubscriptXOffset = reader.ReadInt16();
            os2.SubscriptYOffset = reader.ReadInt16();
            os2.SuperscriptXSize = reader.ReadInt16();
            os2.SuperscriptYSize = reader.ReadInt16();
            os2.SuperscriptXOffset = reader.ReadInt16();
            os2.SuperscriptYOffset = reader.ReadInt16();
            os2.StrikeoutSize = reader.ReadInt16();
            os2.StrikeoutPosition = reader.ReadInt16();
            os2.FamilyClass = reader.ReadInt16();
            os2.Panose1 = reader.ReadByte();
            os2.Panose2 = reader.ReadByte();
            os2.Panose3 = reader.ReadByte();
            os2.Panose4 = reader.ReadByte();
            os2.Panose5 = reader.ReadByte();
            os2.Panose6 = reader.ReadByte();
            os2.Panose7 = reader.ReadByte();
            os2.Panose8 = reader.ReadByte();
            os2.Panose9 = reader.ReadByte();
            os2.Panose10 = reader.ReadByte();
            os2.UnicodeRange1 = reader.ReadUInt32();
            os2.UnicodeRange2 = reader.ReadUInt32();
            os2.UnicodeRange3 = reader.ReadUInt32();
            os2.UnicodeRange4 = reader.ReadUInt32();
            os2.VendorID = reader.ReadCharArray(4);
            os2.Selection = reader.ReadUInt16();
            os2.FirstCharIndex = reader.ReadUInt16();
            os2.LastCharIndex = reader.ReadUInt16();
            os2.TypoAscender = reader.ReadInt16();
            os2.TypoDescender = reader.ReadInt16();
            os2.TypoLineGap = reader.ReadInt16();
            os2.WinAscent = reader.ReadUInt16();
            os2.WinDescent = reader.ReadUInt16();
            os2.CodePageRange1 = reader.ReadUInt32();
            os2.CodePageRange2 = reader.ReadUInt32();
        }


        private void ReadVHEA(TypefaceReader reader, VHEATable vhea, long address)
        {
            reader.BaseStream.Position = address;

            //vheaテーブル情報の読み込み
            vhea.Address = address;
            vhea.TableVersionNumberMajor = reader.ReadUInt16();
            vhea.TableVersionNumberMinor = reader.ReadUInt16();
            vhea.Ascender = reader.ReadInt16();
            vhea.Descender = reader.ReadInt16();
            vhea.LineGap = reader.ReadInt16();
            vhea.AdvanceHeightMax = reader.ReadUInt16();
            vhea.MinTopSideBearing = reader.ReadInt16();
            vhea.MinBottomSideBearing = reader.ReadInt16();
            vhea.YMaxExtent = reader.ReadInt16();
            vhea.CaretSlopeRise = reader.ReadInt16();
            vhea.CaretSlopeRun = reader.ReadInt16();
            vhea.CaretOffset = reader.ReadInt16();
            vhea.Reserved1 = reader.ReadInt16();
            vhea.Reserved2 = reader.ReadInt16();
            vhea.Reserved3 = reader.ReadInt16();
            vhea.Reserved4 = reader.ReadInt16();
            vhea.MetricDataFormat = reader.ReadInt16();
            vhea.NumberOfVMetrics = reader.ReadUInt16();
        }



        private void ReadMORT(TypefaceReader reader, MORTTable mort, long address)
        {
            reader.BaseStream.Position = address;

            //mortテーブル情報の読み込み
            mort.Address = address;
            mort.TableVersionNumberMajor = reader.ReadUInt16();
            mort.TableVersionNumberMinor = reader.ReadUInt16();
            mort.NChains = reader.ReadUInt32();

            //mort chain headerの読み込み
            for (int i = 1; i <= Convert.ToInt32(mort.NChains); i++)
            {
                Chain mc = new Chain();
                mort.Chains.Add(mc);

                mc.DefaultFlags = reader.ReadUInt32();
                mc.ChainLength = reader.ReadUInt32();
                mc.NFeatureEntries = reader.ReadUInt16();
                mc.NSubtables = reader.ReadUInt16();

                //mort Feature Sub Tableの読み込み
                for (int j = 1; j <= Convert.ToInt32(mc.NFeatureEntries); j++)
                {
                    AAT.FeatureTable mf = new AAT.FeatureTable();

                    mc.FeatureTables.Add(mf);
                    mf.FeatureType = reader.ReadUInt16();
                    mf.FeatureSetting = reader.ReadUInt16();
                    mf.EnableFlags = reader.ReadUInt32();
                    mf.DisableFlags = reader.ReadUInt32();
                }

                //mort Metamorphosis Sub Tableの読み込み
                for (int j = 1; j <= Convert.ToInt32(mc.NSubtables); j++)
                {
                    MetamorphosisTable mm = new MetamorphosisTable();
                    mc.MetamorphosisTables.Add(mm);

                    mm.Length = reader.ReadUInt16();
                    mm.Coverage = reader.ReadUInt16();
                    mm.SubFeatureFlags = reader.ReadUInt32();
                    mm.IsVerticalMetamorphosis = Convert.ToBoolean(mm.Coverage & 0x8000);
                    mm.SubtableType = (mm.Coverage & 0x7);

                    //縦書き用の非文脈依存の置換以外は読み込みをスキップします
                    if (mm.IsVerticalMetamorphosis == true & mm.SubtableType == 4)
                    {
                        mm.Format = reader.ReadUInt16();

                        if (mm.Format == 2 | mm.Format == 4 | mm.Format == 6)
                        {
                            mm.Header.UnitSize = reader.ReadUInt16();
                            mm.Header.NUnits = reader.ReadUInt16();
                            mm.Header.SearchRange = reader.ReadUInt16();
                            mm.Header.EntrySelector = reader.ReadUInt16();
                            mm.Header.RangeShift = reader.ReadUInt16();
                        }

                        switch (mm.Format)
                        {
                            case 0:
                            case 2:
                            case 4:
                            case 8:
                                throw new NotImplementedException("mortテーブルのフォーマット" + mm.Format + "は未実装です。");
                            case 6:
                                for (int k = 1; k <= mm.Header.NUnits; k++)
                                {
                                    OpenType.SingleSubstitution sl = new OpenType.SingleSubstitution();
                                    mm.SingleSubstitutionList.Add(sl);
                                    sl.GlyphIndex = reader.ReadUInt16();
                                    sl.SubstitutionGlyphIndex = reader.ReadUInt16();
                                }
                                break;
                        }
                    }
                    else
                    {
                        reader.BaseStream.Seek(mm.Length - 8, System.IO.SeekOrigin.Current);
                    }
                }
            }
        }


        private void ReadCommon(TypefaceReader reader, CommonTable comn, long address)
        {
            reader.BaseStream.Position = address;

            //Commonテーブル情報の読み込み
            comn.Address = address;
            comn.TableVersionNumberMajor = reader.ReadUInt16();
            comn.TableVersionNumberMinor = reader.ReadUInt16();
            comn.OffsetScriptList = reader.ReadUInt16();
            comn.OffsetFeatureList = reader.ReadUInt16();
            comn.OffsetLookupList = reader.ReadUInt16();

            //ScriptListテーブル情報に移動
            reader.BaseStream.Position = comn.Address + comn.OffsetScriptList;

            comn.ScriptCount = reader.ReadUInt16();


            for (int i = 1; i <= comn.ScriptCount; i++)
            {
                ScriptTable sr = new ScriptTable();
                comn.ScriptList.Add(sr);

                sr.Tag = reader.ReadCharArray(4);
                sr.Offset = reader.ReadUInt16();

            }

            //ScriptTableの読み込み

            foreach (ScriptTable sr in comn.ScriptList)
            {
                reader.BaseStream.Position = comn.Address + comn.OffsetScriptList + sr.Offset;

                sr.Address = comn.Address + comn.OffsetScriptList + sr.Offset;
                sr.DefaultLangSysOffset = reader.ReadUInt16();
                sr.LangSysCount = reader.ReadUInt16();

                //DefaultLangSysTableが定義されている場合
                if (sr.DefaultLangSysOffset != 0)
                {
                    LangSysTable ls = new LangSysTable();
                    sr.LangSysList.Add(ls);

                    ls.Address = sr.Address + sr.DefaultLangSysOffset;
                    ls.IsDefault = true;
                    ls.Tag = "";
                    ls.Offset = sr.DefaultLangSysOffset;

                }

                //LangSysTableのリストを読み込み
                for (int i = 1; i <= sr.LangSysCount; i++)
                {
                    LangSysTable ls = new LangSysTable();
                    sr.LangSysList.Add(ls);

                    ls.IsDefault = false;
                    ls.Tag = reader.ReadCharArray(4);
                    ls.Offset = reader.ReadUInt16();
                    ls.Address = sr.Address + ls.Offset;

                }

                //LangSysTableの読み込み
                foreach (LangSysTable ls in sr.LangSysList)
                {
                    reader.BaseStream.Position = ls.Address;

                    ls.LookupOrder = reader.ReadUInt16();
                    ls.ReqFeatureIndex = reader.ReadUInt16();
                    ls.FeatureCount = reader.ReadUInt16();
                    for (int i = 1; i <= ls.FeatureCount; i++)
                    {
                        ls.FeatureIndexList.Add(reader.ReadUInt16());
                    }
                }
            }

            //FeatureListテーブル情報に移動
            reader.BaseStream.Position = comn.Address + comn.OffsetFeatureList;

            comn.FeatureCount = reader.ReadUInt16();

            for (int i = 1; i <= comn.FeatureCount; i++)
            {
                OpenType.FeatureTable ft = new OpenType.FeatureTable();
                comn.FeatureList.Add(ft);

                ft.Tag = reader.ReadCharArray(4);
                ft.Offset = reader.ReadUInt16();

            }

            //FeatureTableの読み込み

            foreach (OpenType.FeatureTable ft in comn.FeatureList)
            {
                reader.BaseStream.Position = comn.Address + comn.OffsetFeatureList + ft.Offset;

                ft.Address = comn.Address + comn.OffsetFeatureList + ft.Offset;
                ft.FeatureParams = reader.ReadUInt16();
                ft.LookupCount = reader.ReadUInt16();

                //LookupListIndexのリストを読み込み
                for (int i = 1; i <= ft.LookupCount; i++)
                {
                    ft.LookupListIndex.Add(reader.ReadUInt16());
                }
            }

            //LookupListテーブル情報に移動
            reader.BaseStream.Position = comn.Address + comn.OffsetLookupList;

            comn.LookupCount = reader.ReadUInt16();

            for (int i = 1; i <= comn.LookupCount; i++)
            {
                LookupTable lt = new LookupTable();
                comn.LookupList.Add(lt);
                lt.Offset = reader.ReadUInt16();
            }

            //LookupTableの読み込み
            foreach (LookupTable lt in comn.LookupList)
            {
                reader.BaseStream.Position = comn.Address + comn.OffsetLookupList + lt.Offset;

                lt.Address = comn.Address + comn.OffsetLookupList + lt.Offset;
                lt.LookupType = reader.ReadUInt16();
                lt.LookupFlag = reader.ReadUInt16();
                lt.SubTableCount = reader.ReadUInt16();

                //SubTableListのリストを読み込み
                for (int i = 1; i <= lt.SubTableCount; i++)
                {
                    lt.SubTableList.Add(reader.ReadUInt16());
                }

            }
        }
        
        private void ReadGSUBSubTable(TypefaceReader reader, CommonTable comn)
        {
            
            foreach (LookupTable lt in comn.LookupList)
            {
                foreach (ushort offset in lt.SubTableList)
                {
                    reader.BaseStream.Position = lt.Address + offset;

                    switch (lt.LookupType)
                    {
                        case (ushort)GsubLookupType.SingleSubstitution:
                            {
                                if (lt.SingleSubstitutionList == null)
                                {
                                    lt.SingleSubstitutionList = new List<SingleSubstitution>();
                                }
                                ushort format = reader.ReadUInt16();
                                switch (format)
                                {
                                    case 1:
                                        {
                                            //Calculated output glyph indices
                                            ushort coverageOffset = reader.ReadUInt16();
                                            ushort deltaGlyphID = reader.ReadUInt16();
                                            List<ushort> coverage = ReadCoverage(reader, lt.Address + offset + coverageOffset);
                                            for (int i = 0; i <= coverage.Count - 1; i++)
                                            {
                                                SingleSubstitution sl = new SingleSubstitution();
                                                lt.SingleSubstitutionList.Add(sl);
                                                sl.GlyphIndex = coverage[i];
                                                sl.SubstitutionGlyphIndex = (ushort)(coverage[i] + deltaGlyphID);
                                            }
                                            break;
                                        }
                                    case 2:
                                        {
                                            //Specified output glyph indices
                                            ushort coverageOffset = reader.ReadUInt16();
                                            ushort glyphCount = reader.ReadUInt16();
                                            List<ushort> glyphList = new List<ushort>();
                                            for (int i = 1; i <= glyphCount; i++)
                                            {
                                                glyphList.Add(reader.ReadUInt16());
                                            }

                                            List<ushort> coverage = ReadCoverage(reader, lt.Address + offset + coverageOffset);
                                            for (int i = 0; i <= coverage.Count - 1; i++)
                                            {
                                                SingleSubstitution sl = new SingleSubstitution();
                                                lt.SingleSubstitutionList.Add(sl);
                                                sl.GlyphIndex = coverage[i];
                                                sl.SubstitutionGlyphIndex = glyphList[i];
                                            }
                                            break;
                                        }
                                }
                                break;
                            }
                        case (ushort)GsubLookupType.MultipleSubstitution:
                            {
                                if (lt.MultipleSubstitutionList == null)
                                {
                                    lt.MultipleSubstitutionList = new List<MultipleSubstitution>();
                                }

                                ushort format = reader.ReadUInt16();
                                ushort coverageOffset = reader.ReadUInt16();
                                ushort sequenceCount = reader.ReadUInt16();
                                List<ushort> sequenceList = new List<ushort>();
                                for (int i = 1; i <= sequenceCount; i++)
                                {
                                    sequenceList.Add(reader.ReadUInt16());
                                }

                                List<ushort> coverage = ReadCoverage(reader, lt.Address + offset + coverageOffset);
                                for (int i = 0; i <= coverage.Count - 1; i++)
                                {
                                    OpenType.MultipleSubstitution ml = new OpenType.MultipleSubstitution();
                                    lt.MultipleSubstitutionList.Add(ml);
                                    ml.GlyphIndex = coverage[i];
                                    ushort sequenceOffset = sequenceList[i];
                                    reader.BaseStream.Position = lt.Address + offset + sequenceOffset;
                                    ushort glyphCount = reader.ReadUInt16();
                                    for (int j = 1; j <= glyphCount; j++)
                                    {
                                        ml.SubstitutionGlyphIndex.Add(reader.ReadUInt16());
                                    }
                                }
                                break;
                            }
                        case (ushort)GsubLookupType.AlternateSubstitution:
                            {
                                if (lt.AlternateSubstitutionList == null)
                                {
                                    lt.AlternateSubstitutionList = new List<AlternateSubstitution>();
                                }

                                ushort format = reader.ReadUInt16();
                                ushort coverageOffset = reader.ReadUInt16();
                                ushort alternateSetCount = reader.ReadUInt16();
                                List<ushort> alternateSetList = new List<ushort>();
                                for (int i = 1; i <= alternateSetCount; i++)
                                {
                                    alternateSetList.Add(reader.ReadUInt16());
                                }

                                List<ushort> coverage = ReadCoverage(reader, lt.Address + offset + coverageOffset);
                                for (int i = 0; i <= coverage.Count - 1; i++)
                                {
                                    AlternateSubstitution al = new AlternateSubstitution();
                                    lt.AlternateSubstitutionList.Add(al);
                                    al.GlyphIndex = coverage[i];
                                    ushort alternateSetOffset = alternateSetList[i];
                                    reader.BaseStream.Position = lt.Address + offset + alternateSetOffset;
                                    ushort glyphCount = reader.ReadUInt16();
                                    for (int j = 1; j <= glyphCount; j++)
                                    {
                                        al.SubstitutionGlyphIndex.Add(reader.ReadUInt16());
                                    }
                                }
                                break;
                            }
                        case (ushort)GsubLookupType.LigatureSubstitution:
                            {
                                if (lt.LigatureSubstitutionList == null)
                                {
                                    lt.LigatureSubstitutionList = new List<LigatureSubstitution>();
                                }

                                ushort format = reader.ReadUInt16();
                                ushort coverageOffset = reader.ReadUInt16();
                                ushort ligatureSetCount = reader.ReadUInt16();
                                List<ushort> ligatureSetList = new List<ushort>();
                                for (int i = 1; i <= ligatureSetCount; i++)
                                {
                                    ligatureSetList.Add(reader.ReadUInt16());
                                }

                                List<ushort> coverage = ReadCoverage(reader, lt.Address + offset + coverageOffset);
                                for (int i = 0; i <= coverage.Count - 1; i++)
                                {
                                    ushort ligatureSetOffset = ligatureSetList[i];
                                    reader.BaseStream.Position = lt.Address + offset + ligatureSetOffset;
                                    ushort ligatureCount = reader.ReadUInt16();
                                    List<ushort> ligatureList = new List<ushort>();
                                    for (int j = 1; j <= ligatureCount; j++)
                                    {
                                        ligatureList.Add(reader.ReadUInt16());
                                    }
                                    foreach (ushort ligatureOffset in ligatureList)
                                    {
                                        LigatureSubstitution ll = new LigatureSubstitution();
                                        lt.LigatureSubstitutionList.Add(ll);
                                        ll.GlyphIndex.Add(coverage[i]);
                                        reader.BaseStream.Position = lt.Address + offset + ligatureSetOffset + ligatureOffset;
                                        ll.SubstitutionGlyphIndex = reader.ReadUInt16();
                                        ushort componentCount = reader.ReadUInt16();
                                        for (int k = 1; k <= componentCount - 1; k++)
                                        {
                                            ll.GlyphIndex.Add(reader.ReadUInt16());
                                        }
                                    }
                                }
                                break;
                            }
                        default:
                            continue;
                    }
                }
            }
        }

        private void ReadGPOSSubTable(TypefaceReader reader, CommonTable comn)
        {
            foreach (OpenType.LookupTable lt in comn.LookupList)
            {
                foreach (ushort offset in lt.SubTableList)
                {
                    reader.BaseStream.Position = lt.Address + offset;

                    switch (lt.LookupType)
                    {
                        case (ushort)GposLookupType.SingleAdjustment:
                            {
                                if (lt.SingleAdjustmentList == null)
                                {
                                    lt.SingleAdjustmentList = new List<SingleAdjustment>();
                                }

                                ushort format = reader.ReadUInt16();
                                switch (format)
                                {
                                    case 1:
                                        {
                                            //Single positioning value
                                            ushort coverageOffset = reader.ReadUInt16();
                                            ushort valueFormat = reader.ReadUInt16();
                                            ValueRecord valueRecord = ReadValueRecord(reader, valueFormat);
                                            List<ushort> coverage = ReadCoverage(reader, lt.Address + offset + coverageOffset);

                                            for (int i = 0; i <= coverage.Count - 1; i++)
                                            {
                                                SingleAdjustment sa = new SingleAdjustment();
                                                lt.SingleAdjustmentList.Add(sa);
                                                sa.GlyphIndex = coverage[i];
                                                sa.ValueRecord = valueRecord;
                                            }

                                            break;
                                        }
                                    case 2:
                                        {
                                            //Array of positioning values
                                            ushort coverageOffset = reader.ReadUInt16();
                                            ushort valueFormat = reader.ReadUInt16();
                                            ushort valueCount = reader.ReadUInt16();
                                            List<ValueRecord> valueRecords = new List<ValueRecord>();
                                            for (int i = 1; i <= valueCount; i++)
                                            {
                                                valueRecords.Add(ReadValueRecord(reader, valueFormat));
                                            }

                                            List<ushort> coverage = ReadCoverage(reader, lt.Address + offset + coverageOffset);
                                            for (int i = 0; i <= coverage.Count - 1; i++)
                                            {
                                                SingleAdjustment sa = new SingleAdjustment();
                                                lt.SingleAdjustmentList.Add(sa);
                                                sa.GlyphIndex = coverage[i];
                                                sa.ValueRecord = valueRecords[i];
                                            }

                                            break;
                                        }
                                }
                                break;
                            }
                        case (ushort)GposLookupType.PairAdjustment:
                            {
                                if (lt.PairAdjustmentList == null)
                                {
                                    lt.PairAdjustmentList = new List<PairAdjustment>();
                                }

                                ushort format = reader.ReadUInt16();
                                switch (format)
                                {
                                    case 1:
                                        {
                                            //Adjustments for glyph pairs
                                            ushort coverageOffset = reader.ReadUInt16();
                                            ushort valueFormat1 = reader.ReadUInt16();
                                            ushort valueFormat2 = reader.ReadUInt16();
                                            ushort pairsetCount = reader.ReadUInt16();
                                            List<ushort> pairsetOffset = new List<ushort>();
                                            for (int i = 1; i <= pairsetCount; i++)
                                            {
                                                pairsetOffset.Add(reader.ReadUInt16());
                                            }

                                            List<ushort> coverage = ReadCoverage(reader, lt.Address + offset + coverageOffset);

                                            for (int i = 0; i <= pairsetOffset.Count - 1; i++)
                                            {
                                                reader.BaseStream.Position = lt.Address + offset + pairsetOffset[i];
                                                ushort pairValueCount = reader.ReadUInt16();
                                                for (int j = 1; j <= pairValueCount; j++)
                                                {
                                                    PairAdjustment pa = new PairAdjustment();
                                                    lt.PairAdjustmentList.Add(pa);
                                                    pa.FirstGlyphIndex = coverage[i];
                                                    pa.SecondGlyphIndex = reader.ReadUInt16();
                                                    pa.FirstValueRecord = ReadValueRecord(reader, valueFormat1);
                                                    pa.SecondValueRecord = ReadValueRecord(reader, valueFormat2);
                                                }
                                            }

                                            break;
                                        }
                                    case 2:
                                        {
                                            //Class pair adjustment
                                            ushort coverageOffset = reader.ReadUInt16();
                                            ushort valueFormat1 = reader.ReadUInt16();
                                            ushort valueFormat2 = reader.ReadUInt16();
                                            ushort classDefOffset1 = reader.ReadUInt16();
                                            ushort classDefOffset2 = reader.ReadUInt16();
                                            ushort classCount1 = reader.ReadUInt16();
                                            ushort classCount2 = reader.ReadUInt16();
                                            ValueRecord[,] classValueRecord1 = new ValueRecord[classCount1, classCount2];
                                            ValueRecord[,] classValueRecord2 = new ValueRecord[classCount1, classCount2];
                                            for (int i = 0; i <= classCount1 - 1; i++)
                                            {
                                                for (int j = 0; j <= classCount2 - 1; j++)
                                                {
                                                    classValueRecord1[i, j] = ReadValueRecord(reader, valueFormat1);
                                                    classValueRecord2[i, j] = ReadValueRecord(reader, valueFormat2);
                                                }
                                            }

                                            List<ushort> coverage = ReadCoverage(reader, lt.Address + offset + coverageOffset);
                                            Dictionary<ushort, ushort> classDef1 = ReadClass(reader, lt.Address + offset + classDefOffset1);
                                            Dictionary<ushort, ushort> classDef2 = ReadClass(reader, lt.Address + offset + classDefOffset2);

                                            foreach (ushort gid1 in classDef1.Keys)
                                            {
                                                foreach (ushort gid2 in classDef2.Keys)
                                                {
                                                    ushort classId1 = classDef1[gid1];
                                                    ushort classId2 = classDef2[gid2];
                                                    if (classValueRecord1[classId1, classId2].IsEmpty == false | classValueRecord2[classId1, classId2].IsEmpty == false)
                                                    {
                                                        OpenType.PairAdjustment pa = new OpenType.PairAdjustment();
                                                        lt.PairAdjustmentList.Add(pa);
                                                        pa.FirstGlyphIndex = gid1;
                                                        pa.SecondGlyphIndex = gid2;
                                                        pa.FirstValueRecord = classValueRecord1[classId1, classId2];
                                                        pa.SecondValueRecord = classValueRecord2[classId1, classId2];
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                }
                            }
                            break;
                        default:
                            continue;
                    }
                }
            }
        }

        private int GetOpenTypeFeatureIndex(CommonTable comn, string scriptTag, string featureTag)
        {
            int ret = -1;
            foreach (ScriptTable st in comn.ScriptList)
            {
                if (st.Tag == scriptTag)
                {
                    foreach (ushort featureIndex in st.LangSysList[0].FeatureIndexList)
                    {
                        if (comn.FeatureList[featureIndex].Tag == featureTag)
                        {
                            ret = featureIndex;
                            break; 
                        }
                    }
                    break;
                }
            }
            return ret;
        }

        private List<ushort> ReadCoverage(TypefaceReader reader, long address)
        {
            reader.BaseStream.Position = address;
            List<ushort> coverage = new List<ushort>();
            ushort coverageFormat = reader.ReadUInt16();

            if (coverageFormat == 1)
            {
                ushort glyphCount = reader.ReadUInt16();
                for (int i = 1; i <= glyphCount; i++)
                {
                    coverage.Add(reader.ReadUInt16());
                }
            }
            else
            {
                ushort rangeCount = reader.ReadUInt16();
                for (int i = 1; i <= rangeCount; i++)
                {
                    ushort startGlyphId = reader.ReadUInt16();
                    ushort endGlyphId = reader.ReadUInt16();
                    ushort startCoverageIndex = reader.ReadUInt16();

                    for (ushort id = startGlyphId; id <= endGlyphId; id++)
                    {
                        coverage.Add(id);
                    }
                }
            }
            return coverage;
        }

        private Dictionary<ushort, ushort> ReadClass(TypefaceReader reader, long address)
        {
            reader.BaseStream.Position = address;
            Dictionary<ushort, ushort> classGlyph = new Dictionary<ushort, ushort>();
            ushort classFormat = reader.ReadUInt16();
            if (classFormat == 1)
            {
                ushort glyphIndex = reader.ReadUInt16();
                ushort glyphCount = reader.ReadUInt16();
                for (int i = 1; i <= glyphCount; i++)
                {
                    classGlyph.Add(glyphIndex, reader.ReadUInt16());
                    glyphIndex = (ushort)(glyphIndex + Convert.ToUInt16(1));
                }
            }
            else
            {
                ushort rangeCount = reader.ReadUInt16();
                for (int i = 1; i <= rangeCount; i++)
                {
                    ushort startGlyphId = reader.ReadUInt16();
                    ushort endGlyphId = reader.ReadUInt16();
                    ushort classValue = reader.ReadUInt16();
                    for (ushort id = startGlyphId; id <= endGlyphId; id++)
                    {
                        classGlyph.Add(id, classValue);
                    }
                }
            }
            return classGlyph;
        }

        private OpenType.ValueRecord ReadValueRecord(TypefaceReader reader, ushort valueFormat)
        {
            ValueRecord vr = new ValueRecord();
            vr.ValueFormat = valueFormat;
            if (valueFormat == 0)
            {
                return vr;
            }
            if ((valueFormat & (ushort)ValueFormat.XPlacement) > 0)
            {
                vr.XPlacement = reader.ReadInt16();
            }
            if ((valueFormat & (ushort)ValueFormat.YPlacement) > 0)
            {
                vr.YPlacement = reader.ReadInt16();
            }
            if ((valueFormat & (ushort)ValueFormat.XAdvance) > 0)
            {
                vr.XAdvance = reader.ReadInt16();
            }
            if ((valueFormat & (ushort)ValueFormat.YAdvance) > 0)
            {
                vr.YAdvance = reader.ReadInt16();
            }
            if ((valueFormat & (ushort)ValueFormat.XPlaDevice) > 0)
            {
                vr.XPlaDevice = reader.ReadUInt16();
            }
            if ((valueFormat & (ushort)ValueFormat.YPlaDevice) > 0)
            {
                vr.YPlaDevice = reader.ReadUInt16();
            }
            if ((valueFormat & (ushort)ValueFormat.XAdvDevice) > 0)
            {
                vr.XAdvDevice = reader.ReadUInt16();
            }
            if ((valueFormat & (ushort)ValueFormat.YAdvDevice) > 0)
            {
                vr.YAdvDevice = reader.ReadUInt16();
            }
            return vr;
        }

        private TableDirectory GetTableDirectory(string tag)
        {
            foreach (TableDirectory tb in this.TableDirectories)
            {
                if (tb.Tag == tag) { return tb; }
            }
            return null;
        }
    }
}
