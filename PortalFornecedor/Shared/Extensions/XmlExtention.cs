using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Tesseract;

namespace Shared.Extensions
{
    public static class XmlExtention
    {
        public static IEnumerable<XElement> TagElements(this XElement xmlElement, string tagName)
        {
            XNamespace xmlns = xmlElement.Attribute("xmlns").Value;
            var xml = xmlElement.Descendants(xmlns + tagName).Elements();
            return xml;
        }
        public static IEnumerable<XElement> TagElements(this IEnumerable<XElement> xmlElement, string tagName)
        {
            var xml = xmlElement.Descendants(tagName).Elements();
            return xml;
        }

        public static string TagElement(this IEnumerable<XElement> xmlElement, string tagName)
        {
            string tag = xmlElement
                .Where(x => x.Name.LocalName == tagName)
                .Select(x => x.Value).SingleOrDefault();
            return tag;
        }

        public static string ValueAtPath(XElement root, string path)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Invalid path.");
            }

            XElement element = root.XPathSelectElement(path);
 
            if (element != null)
                return element.Value;
            return null;

        }

        public static XElement CarregarXML(this string caminho)
        {
            XElement arquivoXml = XElement.Load(caminho);
            return arquivoXml;
        }

        public static string PathName(this string arquivo)
        {
            var nome = arquivo.Split(@"D:\Notas Fiscais\");
            return nome[1];
        }

        public static void ProcessarRaspagemDeNotas(this IWebDriver driver, string linkAcessoNFE, string btnGerarXml)
        {
            IReadOnlyList<IWebElement> links = driver
              .FindElements(By
              .CssSelector(linkAcessoNFE));

            foreach (var link in links)
            {
                link.Click();
                var homeWindow = driver.WindowHandles;
                driver.SwitchTo().Window(homeWindow[1]);
                driver.FindElement(By.XPath(btnGerarXml)).Click();
                driver.Close();
                driver.SwitchTo().Window(homeWindow[0]);
                Console.WriteLine("Raspagem de Notas Realizados..." + DateTime.Now.ToString());
            }

            driver.Quit();
        }

        public static void ProcessarRaspagemDeNotas(this IWebDriver driver, string linkAcessoNFE)
        {
            IReadOnlyList<IWebElement> links = driver
              .FindElements(By
              .CssSelector(linkAcessoNFE));

            foreach (var link in links)
            {
                link.Click();
                var homeWindow = driver.WindowHandles;
                driver.SwitchTo().Window(homeWindow[1]);
                driver.Close();
                driver.SwitchTo().Window(homeWindow[0]);
                Console.WriteLine("Raspagem de Notas Realizados..." + DateTime.Now.ToString());
            }

            driver.Quit();
        }

        //public static Bitmap TratarImagem(this Image img, string fileName)
        //{
        //    //Bitmap imagem = new Bitmap(SetPixelColor(img as Bitmap));
        //    //imagem = imagem.Clone(new Rectangle(0, 0, img.Width, img.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        //    //Erosion erosion = new Erosion();
        //    //Dilatation dilatation = new Dilatation();
        //    //Invert inverter = new Invert();
        //    //ColorFiltering cor = new ColorFiltering();
        //    //cor.Blue = new AForge.IntRange(100, 255);
        //    //cor.Red = new AForge.IntRange(100, 255);
        //    //cor.Green = new AForge.IntRange(100, 255);
        //    //Opening open = new Opening();
        //    //BlobsFiltering bc = new BlobsFiltering();
        //    //Closing close = new Closing();
        //    //GaussianSharpen gs = new GaussianSharpen();
        //    //ContrastCorrection cc = new ContrastCorrection();
        //    //bc.MinHeight = 10;
        //    //FiltersSequence seq = new FiltersSequence(cor, inverter, gs, inverter);
        //    //var filterImage = seq.Apply(imagem);
        //    ////string fileSave = @"C:\HtmlPaginas\" + fileName;
        //    ////filterImage.Save(fileSave);
           
        //    //// Convert byte[] to Base64 String
        //    ////string base64String = Convert.ToBase64String(imageBytes);
        //    ////return base64String;
        //    //return filterImage;
        //}

        public static string OCR(this Bitmap fileSave)
        {
            //byte[] bytes = Convert.FromBase64String(fileSave);
           
            using (MemoryStream m = new MemoryStream())
            {
                fileSave.Save(m, System.Drawing.Imaging.ImageFormat.Tiff);
                
                using (var engine = new TesseractEngine(@"tessdata", "por", EngineMode.Default))
                {
                    engine.SetVariable("tessedit_char_whitelist", "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
                    engine.SetVariable("tessedit_unrej_any_wd", true);

                    string texto;
                    using (var img = Pix.LoadTiffFromMemory(m.GetBuffer()))
                    {
                        using (var page = engine.Process(img))
                        {
                            texto = page.GetText();
                            System.Console.WriteLine("Taxa de Precisão: " + page.GetMeanConfidence());
                            System.Console.WriteLine("Captcha: " + texto);
                        }
                    }
                    return texto;
                }
            }

        }

        private static Bitmap SetPixelColor(this Bitmap imgBmp, bool hasBeenCleared = true) //type 0 dafault, image has has been cleared.
        {
            var bgColor = Color.White;
            var textColor = Color.Black;
            for (var x = 0; x < imgBmp.Width; x++)
            {
                for (var y = 0; y < imgBmp.Height; y++)
                {
                    var pixel = imgBmp.GetPixel(x, y);
                    var isCloserToWhite = hasBeenCleared ? ((pixel.R + pixel.G + pixel.B) / 3) > 140 : ((pixel.R + pixel.G + pixel.B) / 3) > 120;
                    imgBmp.SetPixel(x, y, isCloserToWhite ? bgColor : textColor);
                }
            }
            return imgBmp;
        }

        public static Image AbrirImagemURL(this string urlImage)
        {
            WebRequest request = WebRequest.Create(urlImage);
            using (var response = request.GetResponse())
            {
                using (Stream str = response.GetResponseStream())
                {
                    return Bitmap.FromStream(str);
                }
            }
        }


    }
}
