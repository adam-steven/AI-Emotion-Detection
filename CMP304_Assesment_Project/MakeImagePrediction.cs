using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace CMP304_Assesment_Project
{
    internal class MakeImagePrediction
    {
        /// <summary>
        /// predicts a single images emotion given the needed facial points and stores these predictions to the DataGathered object
        /// </summary>
        /// <param name="data">DataGathered object</param>
        /// <param name="ImagePath">Image path</param>
        /// <param name="leftEyebrow">facial point</param>
        /// <param name="rightEyebrow">facial point</param>
        /// <param name="leftLip">facial point</param>
        /// <param name="rightLip">facial point</param>
        /// <param name="lipWidth">facial point</param>
        /// <param name="lipHeight">facial point</param>
        /// <param name="leftEyeHieght">facial point</param>
        /// <param name="rightEyeHieght">facial point</param>
        /// <param name="mouthOpen">facial point</param>
        public void PredictEmotion(DataGathered data, string ImagePath, double leftEyebrow, double rightEyebrow, double leftLip, double rightLip, double lipWidth, double lipHeight, double leftEyeHieght, double rightEyeHieght, double mouthOpen)
        {
            var mlContext = new MLContext();
            DataViewSchema modelSchema;
            //retrieves the model zip file
            ITransformer trainedModel = mlContext.Model.Load("model.zip", out modelSchema);

            var predictor = mlContext.Model.CreatePredictionEngine<CSVImporttedData, OutputData>(trainedModel);

            //predicts the images emotion
            var prediction = predictor.Predict(new CSVImporttedData()
            {
                LeftEyebrow = (float)leftEyebrow,
                RightEyebrow = (float)rightEyebrow,
                LeftLip = (float)leftLip,
                RightLip = (float)rightLip,
                LipHeight = (float)lipHeight,
                LipWidth = (float)lipWidth,
                LeftEyeHieght = (float)leftEyeHieght,
                RightEyeHieght = (float)rightEyeHieght,
                MouthOpen = (float)mouthOpen
            });

            //checks is the DataGathered object's list have not been initialized 
            if (data.processedImagesPath == null)
            {
                //initializes the lists
                data.processedImagesPath = new List<string>();
                data.processedImagesEmotion = new List<string>();
                data.processedImagesScore = new List<float[]>();
            }

            //adds the predition, scores and image path to the lists 
            data.processedImagesPath.Add(ImagePath);
            data.processedImagesEmotion.Add(prediction.Emotion);
            data.processedImagesScore.Add(prediction.Confidence);
        }
    }
}
