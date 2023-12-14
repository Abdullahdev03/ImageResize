using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Core.Services;

public class ImageCompressor
{
    
    private static IWebHostEnvironment _environment;
    
    public ImageCompressor(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> CompressImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return ("File is empty or null");
        }

        await using var imageStream = file.OpenReadStream();
        // Load the image using Magick.NET
        using var image = new MagickImage(imageStream);
        // Example: Resize the image
        image.Resize(new MagickGeometry(300, 200));

        // Example: Add a watermark
        using (var watermark = new MagickImage("path/to/watermark.png"))
        {
            image.Composite(watermark, Gravity.Southeast, CompositeOperator.Over);
        }

        // Save the modified image to a memory stream
        using (var outputStream = new MemoryStream())
        {
            await image.WriteAsync(outputStream);

            // Set the position of the stream back to the beginning
            outputStream.Position = 0;

            // Return the modified image
            return outputStream.ToArray().ToString()!;
        }
    }
}