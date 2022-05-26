using SkiaSharp;

namespace SkiaCaptchaGenerator
{
    public class Captcha
    {
        // ReSharper disable once CommentTypo
        
        /*
         * ========================================
         * CAPTCHA-GENERATOR WITH SKIASHARP PROJECT
         * AUTHOR: https://github.com/timfvb
         * https://github.com/timfvb/Skia-Captcha-Generator
         * ======================================== 
         */
        
        private string Word { get; set; }
        private Level SolveLevel { get; set; }
        private SKSize PictureSize { get; set; }

        public Captcha(string word, Level solveLevel, SKSize pictureSize)
        {
            Word = word;
            SolveLevel = solveLevel;
            PictureSize = pictureSize;
        }
        
        /// <summary>
        /// Generate a new captcha 
        /// </summary>
        /// <param name="saveAs">Save path</param>
        public void Generate(string saveAs)
        {
            // Init captcha generator
            var captcha = new CaptchaGenerator(Word, SolveLevel, PictureSize);
            
            // Generate captcha
            using var image = captcha.Generate().Snapshot();
            
            // Encode captcha picture to SkiaData
            using var data = image.Encode(SKEncodedImageFormat.Png, 80);
            
            // Write stream to file 
            using var stream = File.OpenWrite(saveAs);
            data.SaveTo(stream);
            
            // Close streams
            stream.Flush();
            stream.Close();
        }
        
        /// <summary>
        /// Generate a new captcha 
        /// </summary>
        /// <returns>SkiaSharp Surface</returns>
        public SKSurface Generate()
        {
            // Init captcha generator
            var captcha = new CaptchaGenerator(Word, SolveLevel, PictureSize);
            
            return captcha.Generate(); 
        }

        /// <summary>
        /// Generates a random word
        /// </summary>
        /// <param name="length">Length of the word</param>
        /// <returns>Randomized word</returns>
        public static string RandomWord(int length)
        {
            var random = new Random();
            
            // Generation Seed
            const string chars = "abcdefghijklmnopqrstuvwxyz" +
                                 "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                                 "0123456789";
            
            // Build and return a random word
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}