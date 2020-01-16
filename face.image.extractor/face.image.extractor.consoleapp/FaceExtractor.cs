using DlibDotNet;
using DlibDotNet.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace face.image.extractor.consoleapp
{
    /// <summary>
    /// Face extractor
    /// </summary>
    public class FaceExtractor
    {
        private readonly ShapePredictor _ShapePredictor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shapePredictorFilePath"></param>
        public FaceExtractor(string shapePredictorFilePath)
        {
            this._ShapePredictor = ShapePredictor.Deserialize(shapePredictorFilePath);
        }

        /// <summary>
        /// Returns all images
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public List<Image> GetImage(string imagePath)
        {
            List<Image> images = new List<Image>();

            try
            {
                using (var faceDetector = Dlib.GetFrontalFaceDetector())
                using (var img = Dlib.LoadImage<RgbPixel>(imagePath))
                {
                    Dlib.PyramidUp(img);

                    var dets = faceDetector.Operator(img);

                    var shapes = new List<FullObjectDetection>();
                    foreach (var rect in dets)
                    {
                        var shape = this._ShapePredictor.Detect(img, rect);
                        if (shape.Parts <= 2)
                            continue;
                        shapes.Add(shape);
                    }

                    if (shapes.Any())
                    {
                        /*
                         * if you want to draw line on images then you may uncomment this commeting blocks
                        var lines = Dlib.RenderFaceDetections(shapes);
                        
                        foreach (var line in lines)
                            Dlib.DrawLine(img, line.Point1, line.Point2, new RgbPixel
                            {
                                Green = 255
                            });
                       
                        foreach (var l in lines)
                            l.Dispose();
                        */

                        var chipLocations = Dlib.GetFaceChipDetails(shapes);

                        using (var faceChips = Dlib.ExtractImageChips<RgbPixel>(img, chipLocations))
                        {
                            Image image = null;
                            //Iterate face chips and add image one by one if multiple images existed
                            foreach (var face in faceChips)
                            {
                                image = face.ToBitmap();
                                images.Add(image);
                            }
                        }

                        foreach (var c in chipLocations)
                            c.Dispose();
                    }

                    foreach (var s in shapes)
                        s.Dispose();
                }
                return images;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

