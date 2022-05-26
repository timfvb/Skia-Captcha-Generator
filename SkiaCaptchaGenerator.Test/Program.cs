using SkiaSharp;

namespace SkiaCaptchaGenerator.Test;

internal class Program
{
    private static void Main() => new Program().CreateCaptcha();
    
    private void CreateCaptcha()
    {
        // Generate a random word
        var word = Captcha.RandomWord(6);
     
        // Initialize SkiaCaptchaGenerator
        var generator = new Captcha(word, Level.Strong, new SKSize(800, 300));
        
        // Generate a captcha saved in filesystem
        generator.Generate("mycaptcha.png");
    }
}
