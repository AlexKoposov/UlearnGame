using System;
using System.Drawing;
using System.IO;

namespace Project_Jumper
{
    class ImageExtensions
    {
        public static Bitmap RotateImage(Image image, float angle)
        {
            if (image == null) throw new ArgumentNullException();
            PointF offset = new PointF((float)image.Width / 2, (float)image.Height / 2);
            Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
            rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            Graphics g = Graphics.FromImage(rotatedBmp);
            g.TranslateTransform(offset.X, offset.Y);
            g.RotateTransform(angle);
            g.TranslateTransform(-offset.X, -offset.Y);
            g.DrawImage(image, new PointF(0, 0));
            return rotatedBmp;
        }

        public static void GetSprite(ref Image inGameSprite, string spriteName, string currentPath, Size imageSize)
        {
            inGameSprite = FitInSize(new Bitmap(Path.Combine(currentPath, $"Resources\\{spriteName}.png")), imageSize);
        }

        public static Image FitInSize(Image image, Size imageSize) =>
            new Bitmap(image, imageSize);
    }
}
