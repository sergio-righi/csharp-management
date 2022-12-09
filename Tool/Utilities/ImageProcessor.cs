using System.IO;
using System.Linq;
using System.Drawing;
using System;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Tool.Utilities
{
    public class ImageProcessor
    {
        //public static readonly int[] _dimOriginal = { 750, 500 };
        //public static readonly int[] _dimNoticiaGrande = { 750, 500 };
        //public static readonly int[] _dimNoticiaPequeno = { 375, 250 };

        //public static readonly int[] _dimUsuario = { 250, 250 };

        public static bool Organize(Image image, string fileName, string folderName = "original\\", long quality = 95L)
        {
            try
            {
                string serverPath = Path.Combine(FileManagement.Write, "image\\");

                //if (image.Width == _dimNoticia[0] && image.Height == _dimNoticia[1])
                //{
                //    Bitmap original = new Bitmap(image);
                //    Bitmap big = Resize(image, _dimNoticiaGrande);
                //    Bitmap small = Resize(image, _dimNoticiaPequeno);

                //    Save(big, serverPath + "large\\", fileName, quality);
                //    Save(small, serverPath + "small\\", fileName, quality);
                //    Save(original, serverPath + "original\\", fileName, quality);
                //}
                //else
                //{
                //    Save(image, serverPath + folderName, fileName, quality);
                //}

                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        public static bool Delete(string oldName, string extension, string newName = null, string origin = null)
        {
            try
            {
                origin = Path.Combine(origin ?? FileManagement.GetExtensionDirectory(extension), $"{oldName}{extension}");

                if (System.IO.File.Exists(origin))
                {
                    if (!string.IsNullOrWhiteSpace(newName))
                    {
                        string destiny = Path.Combine(FileManagement.Write, $"{newName}{extension}");

                        if (System.IO.File.Exists(destiny))
                        {
                            System.IO.File.Delete(destiny);
                        }

                        System.IO.File.Move(origin, destiny);
                    }
                    else
                    {
                        if (System.IO.File.Exists(origin))
                        {
                            System.IO.File.Delete(origin);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static string Save(Image image, string savePath, string saveName = null, long quality = 95L)
        {
            saveName = $"{saveName ?? DateTime.Now.Ticks.ToString()}.jpeg";

            using (var memoryStream = new MemoryStream())
            {
                ImageCodecInfo[] info = ImageCodecInfo.GetImageEncoders();
                EncoderParameters encoderParameters;
                encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                image.Save(Path.Combine(savePath, saveName), info[1], encoderParameters);
                image.Dispose();
            }

            return saveName;
        }

        public static Bitmap Resize(Image imagem, int[] dimensao)
        {
            Bitmap imageResized;

            try
            {
                if (imagem.Width > dimensao[0] || imagem.Height > dimensao[1])
                {
                    double proportionX = Math.Min(dimensao[0], imagem.Width) / (double)imagem.Width;
                    double proportionY = Math.Min(dimensao[1], imagem.Height) / (double)imagem.Height;

                    double proportion = proportionX < proportionY ? proportionX : proportionY;

                    int newWidth = Convert.ToInt32(imagem.Width * proportion);
                    int newHeight = Convert.ToInt32(imagem.Height * proportion);

                    imageResized = new Bitmap(newWidth, newHeight);

                    using (var graphic = Graphics.FromImage(imageResized))
                    {
                        graphic.SmoothingMode = SmoothingMode.HighQuality;
                        graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphic.CompositingQuality = CompositingQuality.HighQuality;
                        graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        graphic.DrawImage(imagem, 0, 0, imageResized.Width, imageResized.Height);
                    }

                    return imageResized;
                }
            }
            catch (Exception)
            {
                return null;
            }

            return new Bitmap(imagem);
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }
    }
}