using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace face.image.extractor.consoleapp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting to extract image...");
                //Image file path
                string imageFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "5d4dba24b22c9.jpg");
                string shapePredictorFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "shape_predictor_68_face_landmarks.dat");
                //Initiate Extract class
                FaceExtractor extractImage = new FaceExtractor(shapePredictorFilePath);
                //Call GetImage method by given your image file path. You should be get list of images if face image exist on that given image
                List<Image> images = extractImage.GetImage(imageFilePath);
                Console.WriteLine("Face extraction done!");
                Console.WriteLine("Number of face extracted: " + images.Count);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Occured: " + ex.Message);
                if (ex.InnerException != null) Console.WriteLine("InnerException: " + ex.Message);
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }
}
