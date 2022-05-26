# Skia Captcha Generator by timfvb

The **Skia Captcha Generator** is a simple way to generate a captcha based on the SkiaSharp library.

By using the SkiaSharp libary, some complications with image processing by other operating systems such as Debian are avoided.

![functionbanner](https://github.com/timfvb/Skia-Captcha-Generator/blob/main/functionbanner.png?raw=true)

## Usage

```csharp
using SkiaSharp;
using SkiaCaptchaGenerator;

// Generate a random word
var word = Captcha.RandomWord(6);
     
// Initialize SkiaCaptchaGenerator
var generator = new Captcha(word, Level.Strong, new SKSize(800, 300));
        
// Generate a captcha saved in filesystem
generator.Generate("mycaptcha.png");
```
## Example

![captcha](https://github.com/timfvb/Skia-Captcha-Generator/blob/main/mycaptcha.png?raw=true)

## Important
To use the Skia Captcha Generator library, you need the SkiaSharp package.

I recommend only using SkiaSharp version 2.80.3 or higher.

## License
[MIT](https://choosealicense.com/licenses/mit/)
