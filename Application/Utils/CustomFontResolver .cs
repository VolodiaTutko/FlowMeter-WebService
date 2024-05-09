using PdfSharp.Fonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public class CustomFontResolver : IFontResolver
    {
        public byte[] GetFont(string faceName)
        {
            // Повертаємо шрифт, якщо його знайдено
            try
            {
                string fontFileName = "arialmt.ttf";
                string fontPath = Path.Combine(Directory.GetCurrentDirectory().Replace("FlowMeter-WebService\\", ""), "Application", "Utils", fontFileName);
                
                Console.WriteLine(fontPath);
                return File.ReadAllBytes(fontPath);
            }
            catch (Exception)
            {
                // Обробляйте помилку, якщо шрифт не знайдено
                return null;
            }
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            // Повертаємо інформацію про шрифт
            return new FontResolverInfo(familyName);
        }
    }
}
