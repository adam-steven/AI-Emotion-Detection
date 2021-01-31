using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CMP304_Assesment_Project
{
    /// <summary>
    /// Interaction logic for Pridictions.xaml
    /// </summary>
    public partial class Pridictions : Window
    {
        DataGathered data;
        int imageCounter = 0;

        public Pridictions(DataGathered dataGathered)
        {
            InitializeComponent();
            data = dataGathered;
        }

        //on the form load automaticly requests the first prediction to be displayed 
        private void Pridictions_Loaded(object sender, RoutedEventArgs e)
        {
            showData();
        }

        //requests the previous prediction to be displayed 
        private void leftBtn_Click(object sender, RoutedEventArgs e)
        {
            //checks if there is a previous prediction
            if (imageCounter > 0)
                imageCounter--;

            showData();
        }

        //requests the next prediction to be displayed 
        private void rightBtn_Click(object sender, RoutedEventArgs e)
        {
            //checks if there is a next prediction
            if (imageCounter < data.processedImagesPath.Count - 1)
                imageCounter++;

            showData();
        }

        //update the form elements 
        private void showData()
        {
            //displays an image given its file path
            image.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/" + data.processedImagesPath[imageCounter]));
            //displays the images corisponding prediction
            predictionLbl.Content = "Emotion: " + data.processedImagesEmotion[imageCounter] + "\n" + "Confidence: " + getConfidence();

        }

        //caluculates the score precentages and returns a string of all precentages above 40%
        private string getConfidence()
        {
            //adds up the scores 
            float totalScore = 0;
            for(int i =0;i<6;i++)
            {
                totalScore += data.processedImagesScore[imageCounter][i];
            }

            //caluculates the score precentages and add the valid scores to a string with there corrisponding emotion
            string Confidence = "";
            if (data.processedImagesScore[imageCounter][0] / totalScore > 0.4)
                Confidence += " Anger(" + Math.Floor((data.processedImagesScore[imageCounter][0] / totalScore) * 100) + "%)";
            if (data.processedImagesScore[imageCounter][1] / totalScore > 0.4)
                Confidence += " Sadness(" + Math.Floor((data.processedImagesScore[imageCounter][1] / totalScore) * 100) + "%)";
            if (data.processedImagesScore[imageCounter][2] / totalScore > 0.4)
                Confidence += " Joy(" + Math.Floor((data.processedImagesScore[imageCounter][2] / totalScore) * 100) + "%)";
            if (data.processedImagesScore[imageCounter][3] / totalScore > 0.4)
                Confidence += " Disgust(" + Math.Floor((data.processedImagesScore[imageCounter][3] / totalScore) * 100) + "%)";
            if (data.processedImagesScore[imageCounter][4] / totalScore > 0.4)
                Confidence += " Fear(" + Math.Floor((data.processedImagesScore[imageCounter][4] / totalScore) * 100) + "%)";
            if (data.processedImagesScore[imageCounter][5] / totalScore > 0.4)
                Confidence += " Surprise(" + Math.Floor((data.processedImagesScore[imageCounter][5] / totalScore) * 100) + "%)";

            //chack if no score was above 40%
            if(Confidence == "")
            {
                //adds the predictions score to the list
                switch(data.processedImagesEmotion[imageCounter])
                {
                    case "anger":
                        Confidence += " Anger(" + Math.Floor((data.processedImagesScore[imageCounter][0] / totalScore) * 100) + "%)";
                        break;
                    case "sadness":
                        Confidence += " Sadness(" + Math.Floor((data.processedImagesScore[imageCounter][1] / totalScore) * 100) + "%)";
                        break;
                    case "joy":
                        Confidence += " Joy(" + Math.Floor((data.processedImagesScore[imageCounter][2] / totalScore) * 100) + "%)";
                        break;
                    case "disgust":
                        Confidence += " Disgust(" + Math.Floor((data.processedImagesScore[imageCounter][3] / totalScore) * 100) + "%)";
                        break;
                    case "fear":
                        Confidence += " Fear(" + Math.Floor((data.processedImagesScore[imageCounter][4] / totalScore) * 100) + "%)";
                        break;
                    case "surprise":
                        Confidence += " Surprise(" + Math.Floor((data.processedImagesScore[imageCounter][5] / totalScore) * 100) + "%)";
                        break;
                }
            }

            return Confidence;
        }
    }
}
