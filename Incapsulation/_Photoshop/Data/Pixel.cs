using System;

namespace MyPhotoshop
{
    public struct Pixel
    {
        public double R
        {
            get => r;
            set { r = CheckValueAndReturn(value); }
        }

        public double G
        {
            get => g;
            set { g = CheckValueAndReturn(value); }
        }

        private double r;
        private double g;
        private double b;

        public double B
        {
            get => b;
            set { b = CheckValueAndReturn(value); }
        }

        public Pixel(double r, double g, double b)
        {
            this.r = this.g = this.b = 0;
            R = r;
            G = g;
            B = b;
        }

        public static double Trim(double value)
        {
            if (value < 0) return 0;
            if (value > 1) return 1;
            return value;
        }

        public static Pixel operator *(Pixel pixel, double number)
        {
            return new Pixel(
                Trim(pixel.R * number),
                Trim(pixel.G * number),
                Trim(pixel.B * number)
            );
        }
        
        public static Pixel operator *(double number, Pixel pixel)
        {
            return pixel * number;
        }

        public static Pixel operator *(Pixel p1, Pixel p2)
        {
            return new Pixel
            (
                p1.R * p2.R,
                p1.G * p2.G,
                p1.B * p2.B
            );
        }

        private double CheckValueAndReturn(double value)
        {
            if (value < 0 || value > 1) throw new ArgumentException();
            return value;
        }
    }
}