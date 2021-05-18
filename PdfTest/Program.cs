using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.XObjects;

namespace PdfTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var imgCount = 0;
            foreach (var path in Directory.GetFiles("./samples", "*.pdf"))
            {
                Console.WriteLine($"Opening {path}...");

                var sw = Stopwatch.StartNew();
                using var doc = PdfDocument.Open(path);
                Console.WriteLine($"{path} has {doc.NumberOfPages} pages. took {sw.ElapsedMilliseconds}ms");

                foreach (var page in doc.GetPages())
                {
                    foreach (var image in page.GetImages())
                    {
                        /*using var memStream = new MemoryStream(image.RawBytes.ToArray());
                        var img = Image.Load(memStream);

                        using var file = new FileStream($"./samples/{imgCount++}.jpg", FileMode.CreateNew);
                        img.Save(file, JpegFormat.Instance);;*/

                        if (image is XObjectImage ximg)
                        {
                            var filter = ximg.ImageDictionary.Data["Filter"];
                            if (filter.ToString() == "/JPXDecode")
                            {
                                File.WriteAllBytes($"./samples/{imgCount++}.jp2", image.RawBytes.ToArray());
                            }

                            /*if (filter.ToString().StartsWith("/JBig"))
                            {
                                // how to identify jbig2 from metadata?
                                File.WriteAllBytes($"./samples/{imgCount++}.jb2", image.RawBytes.ToArray());
                            }*/
                            else
                            {
                                File.WriteAllBytes($"./samples/{imgCount++}.png", image.RawBytes.ToArray());
                            }
                        }
                    }
                    
                    

                }
                
                sw.Stop();
            }
            
            Console.WriteLine("Done");
        }
    }
}