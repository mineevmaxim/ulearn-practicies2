using System.Diagnostics;

namespace MyPhotoshop
{
    public abstract class PixelFilter : ParametrizedFilter
    {
        public PixelFilter(IParameters parameters) : base(parameters)
        {
        }

        protected abstract Pixel ProcessPixel(Pixel original, IParameters parameters);

        public override Photo Process(Photo original, IParameters parameters)
        {
            var filtered = new Photo(original.width, original.height);
            for (var x = 0; x < original.width; x++)
            for (var y = 0; y < original.height; y++)
                filtered[x, y] = ProcessPixel(original[x, y], parameters);

            return filtered;
        }
    }
}