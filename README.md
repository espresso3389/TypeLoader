# TypeLoader
This is a PCL version of TypeLoader, which is originally by [watertrans](https://www.codeplex.com/site/users/view/watertrans) and released on [codeplex](https://typeloader.codeplex.com/).
This library handles vertical texts easily by processing OpenType/TrueType files directly.

## NuGet
A NuGet package is available on the following URL:

https://www.nuget.org/packages/WaterTrans.TypeLoader/

## Supported Platforms
This package supports following platforms (Profile111):

- .NET Framework 4.5
- ASP.NET Core 1.0
- Windows 8
- Windows Phone 8.1
- Xamarin.Android
- Xamarin.iOS (Either Unified or Classic)

## License
[MIT](https://github.com/espresso3389/TypeLoader/blob/master/LICENSE)

## Sample Usage
The following fragment illustrates how to use WaterTrans.TypeLoader with SkiaSharp:
```cs
// load typeface on SkiaSharp
var typeface = SKTypeface.FromFile(fontFilePath);

// load typeface on TypeLoader
WaterTrans.TypeLoader.TypefaceInfo tfInfo;
using (var fs = File.OpenRead(fontFilePath))
  tfInfo = new WaterTrans.TypeLoader.TypefaceInfo(s);

...

var text = "いづれの御時にか、女御、更衣あまたさぶらひたまひけるなかに、いとやむごとなききはにはあらぬが、すぐれて時めきたまふありけり。はじめよりわれはと思ひあがりたまへる御かたがた、めざましきものにナシおとしめそねみたまふ。同じほど、それより下臈の更衣たちは、ましてやすからず。";

SKCanvas canvas = ... // Canvas from somewhere
using (var paint = new SKPaint())
{
  paint.IsAntialias = true;
  paint.TextSize = FONT_SIZE;
  paint.Typeface = typeface;
  paint.IsVerticalText = true;

  drawText(canvas, stringToVerticalGlyphs(text, paint, tfInfo), 0, 0, paint);
}

...

// string to vertical glyph GID
ushort[] stringToVerticalGlyphs(string text, SKPaint paint, WaterTrans.TypeLoader.TypefaceInfo typefaceInfo)
{
  ushort[] glyphs;
  paint.Typeface.CharsToGlyphs(text, out glyphs);
  var conv = typefaceInfo.GetVerticalGlyphConverter();
  for (int i = 0; i < glyphs.Length; i++)
  {
    if (conv.CanConvert(glyphs[i]))
      glyphs[i] = conv.Convert(glyphs[i]);
  }
  return glyphs;
}

// SKCanvas.DrawText wrapper to deal with GID
public unsafe void drawText(SKCanvas canvas, ushort[] glyphs, float x, float y, SKPaint paint)
{
  paint.TextEncoding = SKTextEncoding.GlyphId;
  fixed (ushort* p = glyphs)
  {
    canvas.DrawText((IntPtr)p, glyphs.Length * 2, x, y, paint);
  }
}
```
