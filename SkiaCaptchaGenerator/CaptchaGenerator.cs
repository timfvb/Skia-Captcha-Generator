using SkiaSharp;

namespace SkiaCaptchaGenerator
{
    internal class CaptchaGenerator
    {
        private string Word { get; set; }
        private Level CaptchaLevel { get; set; }
        private SKSize PictureSize { get; set; }
        
        public CaptchaGenerator(string word, Level captchaLevel, SKSize pictureSize)
        {
            Word = word;
            CaptchaLevel = captchaLevel;
            PictureSize = pictureSize;
        }

        public SKSurface Generate()
        {
            // Create new empty default surface
            var captchaSurface = CreateSurface((int)PictureSize.Width, (int)PictureSize.Height);
            
            // Colorize the canvas with the color rgb(220, 8, 23)
            var canvas = captchaSurface.Canvas;
            canvas.Clear(SKColor.FromHsl(220, 8, 23));
            
            // Get Level Settings
            var tLevel = TranslateLevel(CaptchaLevel);

            var fPaint = new SKPaint {
                TextSize = 95f,
                IsAntialias = true,
                Color = new SKColor(0, 0, 255),
                Typeface = SKTypeface.CreateDefault()
            };
            
            // Set a random start point 
            var coordRandom = new Random();
            var coordPoint = new SKPoint(coordRandom.Next(0, (int)PictureSize.Width - tLevel.MaxLetterDistance * Word.Length),
                PictureSize.Height / 2);
            
            foreach (var l in Word)
            { 
                // Create a secure toggle for better y-coord accuracy
                var secureToggle = coordPoint.Y - tLevel.PitchLock > PictureSize.Height ? tLevel.MaxShift : 0;
                
                // Change the y-coord
                coordPoint.Y += coordRandom.Next(tLevel.MinShift, tLevel.MaxShift + secureToggle); 
                
                // Draw the captcha char
                canvas.DrawText(l.ToString(), coordPoint, fPaint);
                
                // Raise the x-coord
                coordPoint.X += coordRandom.Next(tLevel.MinLetterDistance, tLevel.MaxLetterDistance);
            }

            // Create security lines on the canvas
            DrawSecurityLines(canvas, PictureSize);

            return captchaSurface;
        }
        
        private static SKSurface CreateSurface(int width, int height)
        {
            var info = new SKImageInfo(width, height);
            var surface = SKSurface.Create(info);

            return surface;
        }

        private static void DrawSecurityLines(SKCanvas graphic, SKSize bSize)
        {
            if (graphic == null) 
                throw new ArgumentNullException(nameof(graphic));
            
            var rLines = new Random();
                        
            for (var i = 0; i < 14; i++) 
            {
                // Generate random coords
                var x0 = rLines.Next(0, (int)bSize.Width);
                var y0 = rLines.Next(0, (int)bSize.Height);
                var x1 = rLines.Next(0, (int)bSize.Width);
                var y1 = rLines.Next(0, (int)bSize.Height);

                // Generate random paint type selection number
                var cvStateRandom = new Random();
                var cv = cvStateRandom.Next(1, 7);

                // Set the paint from selection number
                var paint = cv switch
                {
                    1 => new SKPaint {Style = SKPaintStyle.Stroke, Color = SKColors.Blue, StrokeWidth = 3.5f},
                    2 => new SKPaint {Style = SKPaintStyle.Stroke, Color = SKColors.Blue, StrokeWidth = 3.5f},
                    3 => new SKPaint {Style = SKPaintStyle.Stroke, Color = SKColors.Blue, StrokeWidth = 5.5f},
                    4 => new SKPaint {Style = SKPaintStyle.Stroke, Color = new SKColor(54, 57, 63), StrokeWidth = 4.5f},
                    5 => new SKPaint {Style = SKPaintStyle.Stroke, Color = new SKColor(54, 57, 63), StrokeWidth = 1.5f},
                    6 => new SKPaint {Style = SKPaintStyle.Stroke, Color = new SKColor(54, 57, 63), StrokeWidth = 1.5f},
                    _ => new SKPaint {Style = SKPaintStyle.Stroke, Color = SKColors.Blue, StrokeWidth = 2.5f}
                };
                
                // Set picture points
                var p0 = new SKPoint(x0, y0);
                var p1 = new SKPoint(x1, y1);
                
                // Draw the security line to the picture points
                graphic.DrawLine(p0, p1, paint);
            }
        }
        
        private static TranslatedLevel TranslateLevel(Level currentLevel)
        {
            var translatedLevel = new TranslatedLevel();
            
            // Presets for the safety level
            switch (currentLevel)
            {
                case Level.Weak:
                    translatedLevel.PitchLock = 85;
                    translatedLevel.MinShift = -5;
                    translatedLevel.MaxShift = 30;
                    translatedLevel.MinLetterDistance = 40;
                    translatedLevel.MaxLetterDistance = 60;
                    break;
                case Level.Simple:
                    translatedLevel.PitchLock = 85;
                    translatedLevel.MinShift = -10;
                    translatedLevel.MaxShift = 40;
                    translatedLevel.MinLetterDistance = 60;
                    translatedLevel.MaxLetterDistance = 70;
                    break;
                case Level.Normal:
                    translatedLevel.PitchLock = 100;
                    translatedLevel.MinShift = -15;
                    translatedLevel.MaxShift = 45;
                    translatedLevel.MinLetterDistance = 60;
                    translatedLevel.MaxLetterDistance = 100;
                    break;
                case Level.Strong:
                    translatedLevel.PitchLock = 120;
                    translatedLevel.MinShift = -25;
                    translatedLevel.MaxShift = 50;
                    translatedLevel.MinLetterDistance = 80;
                    translatedLevel.MaxLetterDistance = 110;
                    break;
                case Level.SuperStrong:
                    translatedLevel.PitchLock = 170;
                    translatedLevel.MinShift = -45;
                    translatedLevel.MaxShift = 55;
                    translatedLevel.MinLetterDistance = 110;
                    translatedLevel.MaxLetterDistance = 125;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currentLevel), currentLevel, null);
            }
            
            return translatedLevel;
        }
    }
}