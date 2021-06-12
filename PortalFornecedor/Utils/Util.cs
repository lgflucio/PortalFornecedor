using OpenQA.Selenium;
using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Serialization;

namespace Utils
{
    public static class Util
    {
        public static string PegarValorEntreParenteses(string campo, string stringParaSerRemovida)
        {
            int indexParentese1 = campo.IndexOf('(');
            int indexParentese2 = campo.IndexOf(')');
            string valorEntreParenteses = campo.Substring(indexParentese1 + 1, indexParentese2 - indexParentese1 - 1);
            return valorEntreParenteses.Replace(stringParaSerRemovida, string.Empty);
        }
        public static string ObjXmlToStringXML<TTipoObjeto>(TTipoObjeto objetoXML)
        {

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TTipoObjeto));
            StringWriter stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, objetoXML);
            return stringWriter.ToString();
        }

        public static TTipoObjeto Converterobjeto<TTipoObjeto>(string objetoXML)
        {

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TTipoObjeto));
            StringReader _stringReader = new StringReader(objetoXML);
            return (TTipoObjeto)xmlSerializer.Deserialize(_stringReader);
        }

        public static XmlDocument ConverterObjetoXMlEmXML<TTipoObjeto>(TTipoObjeto objetoXML)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(ObjXmlToStringXML<TTipoObjeto>(objetoXML));
            return xmlDoc;
        }


        public static string DesformatarCNPJCPF(string value)
        {
            return value != null ? value.Replace(".", string.Empty)
                                        .Replace(" ", string.Empty)
                                        .Replace("/", string.Empty)
                                        .Replace("-", string.Empty) : "";
        }
        public static string GerarCampoChaveTabelaRFE(string copetenciaSemBarra, string cpfOuCnpjPrestador, string numeroNFSe)
        {
            if (cpfOuCnpjPrestador.Length < 12)
                cpfOuCnpjPrestador = cpfOuCnpjPrestador.PadLeft(3, '0');
            var dtEmicaoAnoMesDia = DateTime.Parse(copetenciaSemBarra).ToString("yyyyMMdd");
            return $"{dtEmicaoAnoMesDia}{cpfOuCnpjPrestador}{numeroNFSe.PadLeft(20, '0')}";
        }
        public static X509Certificate2 GetCertificate(string caminhoArquivoCertificado, string senhaCertificado)
        {
            byte[] pfxData = null;

            try
            {
                pfxData = File.ReadAllBytes(caminhoArquivoCertificado);
            }
            catch (Exception ex)
            {
                throw new Exception("Load1: " + ex.Message);
            }

            try
            {
                return new X509Certificate2(pfxData, senhaCertificado, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);
            }
            catch (Exception ex)
            {
                throw new Exception("Load2: " + ex.Message);
            }
        }

        public static string FormatarCNPJ(string cnpj)
        {
            return Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");
        }

        public static string GetImageCaptcha(IWebDriver driver, By pathImage)
        {
            ITakesScreenshot ssdriver = driver as ITakesScreenshot;
            Screenshot screenshot = ssdriver.GetScreenshot();

            Screenshot tempImage = screenshot;
            byte[] imageBytes = Convert.FromBase64String(screenshot.AsBase64EncodedString);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            Bitmap screenImage = new Bitmap(ms);
            IWebElement my_image = driver.FindElement(pathImage);

            Point point = my_image.Location;
            int width = my_image.Size.Width;
            int height = my_image.Size.Height;

            Rectangle section = new Rectangle(point, new Size(width, height));
            Bitmap final_image = CropImage(screenImage, section);
            using (var m = new MemoryStream())
            {
                using (var bitmap = new Bitmap(final_image))
                {
                    bitmap.Save(m, System.Drawing.Imaging.ImageFormat.Jpeg);
                    string SigBase64 = Convert.ToBase64String(m.GetBuffer()); //Get Base64
                    return SigBase64;
                }
            }
              
        }

        public static Bitmap CropImage(Bitmap source, Rectangle section)
        {
            Bitmap bmp = new Bitmap(section.Width, section.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);
            return bmp;
        }
    }
}
