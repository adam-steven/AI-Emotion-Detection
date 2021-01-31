using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DlibDotNet;
using DlibDotNet.Extensions;
using Dlib = DlibDotNet.Dlib;

namespace CMP304_Assesment_Project
{
    internal class ProcessImageData
    {
        private static double leftEyebrow, rightEyebrow, leftLip, rightLip, lipWidth, lipHeight, leftEyeHieght, rightEyeHieght, mouthOpen;
        MakeImagePrediction prediction = new MakeImagePrediction();

        /// <summary>
        /// detects faces in the images using Dlib.net 
        /// </summary>
        /// <param name="filePaths">list of image file paths</param>
        /// <param name="data">DataGathered object</param>
        public void DetectFaces(List<string> filePaths , DataGathered data)
        {
            // Set up Dlib Face Detector
            using (var fd = Dlib.GetFrontalFaceDetector())
            // ... and Dlib Shape Detector
            using (var sp = ShapePredictor.Deserialize("shape_predictor_68_face_landmarks.dat"))
            {
                for (int x = 0; x < filePaths.Count; x++)
                {
                    // load input image
                    var img = Dlib.LoadImage<RgbPixel>(@filePaths[x]);

                    // find all faces in the image
                    var faces = fd.Operator(img);
                    // for each face draw over the facial landmarks
                    foreach (var face in faces)
                    {
                        // find the landmark points for this face
                        var shape = sp.Detect(img, face);

                        // draw the landmark points on the image
                        for (var i = 0; i < shape.Parts; i++)
                        {
                            var point = shape.GetPart((uint)i);
                            var rect = new Rectangle(point);

                            //color/ draw the left eyebrow and left inner eye to ensure the points were placed correctly 
                            if (i == 39 || i == 21 || i == 20 || i == 19 || i == 18)
                                Dlib.DrawRectangle(img, rect, color: new RgbPixel(50, 50, 255), thickness: 4);
                            else //colour/ draw the rest of the points
                                Dlib.DrawRectangle(img, rect, color: new RgbPixel(255, 255, 0), thickness: 4);
                        }

                        //calculate the needed facial points
                        leftEyebrow = CalculateDistance(
                                shape.GetPart((uint)39),
                                shape.GetPart((uint)21),
                                shape.GetPart((uint)21),
                                shape.GetPart((uint)20),
                                shape.GetPart((uint)19),
                                shape.GetPart((uint)18));

                        rightEyebrow = CalculateDistance(
                            shape.GetPart((uint)42),
                            shape.GetPart((uint)22),
                            shape.GetPart((uint)22),
                            shape.GetPart((uint)23),
                            shape.GetPart((uint)24),
                            shape.GetPart((uint)25));

                        leftLip = CalculateDistance(
                           shape.GetPart((uint)33),
                           shape.GetPart((uint)51),
                           shape.GetPart((uint)50),
                           shape.GetPart((uint)49),
                           shape.GetPart((uint)48),
                           new Point());

                        rightLip = CalculateDistance(
                           shape.GetPart((uint)33),
                           shape.GetPart((uint)51),
                           shape.GetPart((uint)52),
                           shape.GetPart((uint)53),
                           shape.GetPart((uint)54),
                           new Point());

                        lipWidth = CalculateDistance(
                           shape.GetPart((uint)33),
                           shape.GetPart((uint)51),
                           shape.GetPart((uint)48),
                           shape.GetPart((uint)54),
                           new Point(),
                           new Point());

                        lipHeight = CalculateDistance(
                           shape.GetPart((uint)33),
                           shape.GetPart((uint)51),
                           shape.GetPart((uint)51),
                           shape.GetPart((uint)57),
                           new Point(),
                           new Point());

                        leftEyeHieght = CalculateDistance(
                           shape.GetPart((uint)27),
                           shape.GetPart((uint)39),
                           shape.GetPart((uint)38),
                           shape.GetPart((uint)40),
                           new Point(),
                           new Point());

                        rightEyeHieght = CalculateDistance(
                           shape.GetPart((uint)27),
                           shape.GetPart((uint)42),
                           shape.GetPart((uint)43),
                           shape.GetPart((uint)47),
                           new Point(),
                           new Point());

                        mouthOpen = CalculateDistance(
                           shape.GetPart((uint)33),
                           shape.GetPart((uint)51),
                           shape.GetPart((uint)62),
                           shape.GetPart((uint)66),
                           new Point(),
                           new Point());
                    }

                    //get the number of images in the OutputImages files to prevent overwriting errors 
                    int fCount = Directory.GetFiles("OutputImages", "*", SearchOption.TopDirectoryOnly).Length;

                    //export the modified image to the OutputImages
                    Dlib.SaveJpeg(img, @"OutputImages\output" + fCount + ".jpg");

                    //sends the needed facial points and the exportted modified image to the MakeImagePrediction class
                    prediction.PredictEmotion(data, "OutputImages/output" + fCount + ".jpg", leftEyebrow, rightEyebrow, leftLip, rightLip, lipWidth, lipHeight, leftEyeHieght, rightEyeHieght, mouthOpen);
                }
            }
            return;
        }

        //calculate a needed facial point given a set of Dlib.net facial point
        private static double CalculateDistance(Point StationaryPoint, Point point1, Point point2, Point point3, Point point4, Point point5)
        {
            double CalculatedDistance = 0f;

            var pointDistance0 = (point1 - StationaryPoint).Length;

            if (point4.Length != 0)
            {
                CalculatedDistance += (point2 - StationaryPoint).Length / pointDistance0;
                CalculatedDistance += (point3 - StationaryPoint).Length / pointDistance0;
                CalculatedDistance += (point4 - StationaryPoint).Length / pointDistance0;
                if (point5.Length != 0)
                    CalculatedDistance += (point5 - StationaryPoint).Length / pointDistance0;
            }
            else
                CalculatedDistance += (point3 - point2).Length / pointDistance0;

            return CalculatedDistance;
        }
    }
}
