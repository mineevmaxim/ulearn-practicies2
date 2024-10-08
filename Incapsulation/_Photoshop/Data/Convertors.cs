using System;
using System.Drawing;

namespace MyPhotoshop
{
    public static class Convertors
    {
        public static Photo Bitmap2Photo(Bitmap bmp)
        {
            var photo = new Photo(bmp.Width, bmp.Height);
            for (var x = 0; x < bmp.Width; x++)
            for (var y = 0; y < bmp.Height; y++)
            {
                var pixel = bmp.GetPixel(x, y);
                photo[x, y] = new Pixel
                (
                    (double)pixel.R / 255,
                    (double)pixel.G / 255,
                    (double)pixel.B / 255
                );
            }

            return photo;
        }

        static int ToChannel(double val)
        {
            if (val < 0 || val > 1)
                throw new Exception(string.Format("Wrong channel value {0} (the value must be between 0 and 1", val));
            return (int)(val * 255);
        }

        public static Bitmap Photo2Bitmap(Photo photo)
        {
            var bmp = new Bitmap(photo.width, photo.height);
            for (var x = 0; x < bmp.Width; x++)
            for (var y = 0; y < bmp.Height; y++)
            {
                var pixel = photo[x, y];
                bmp.SetPixel(x, y, Color.FromArgb(
                    ToChannel(pixel.R),
                    ToChannel(pixel.G),
                    ToChannel(pixel.B)
                ));
            }

            return bmp;
        }
    }
}