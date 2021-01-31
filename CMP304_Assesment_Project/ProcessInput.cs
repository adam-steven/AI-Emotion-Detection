using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CMP304_Assesment_Project
{
    internal class ProcessInput
    {
        /// <summary>
        /// Checks Wether the inputted files are images or csv files and calls the appropriate function
        /// </summary>
        /// <param name="files">list of file paths</param>
        /// <param name="data">new DataGathered object</param>
        public static void SortFiles(List<string> files, DataGathered data)
        {
            //stores all images files
            List<string> imagePaths = new List<string>();
            //stores all non images or csv files
            List<string> nonImagePaths = new List<string>();
            //a list of all the image types that work with Dlib.net
            string[] imageEctentions = new string[] { ".jpg", ".jpeg", ".png", ".bmp" };

            //loops the all the file paths 
            for (int i = 0; i < files.Count; i++)
            {
                //checks if the file is a csv
                if (Path.GetExtension(files[i]).Contains(".csv"))
                {
                    //sends the csv file to the ProcessCsvData class and stops sorting files 
                    ProcessCsvData processCSV = new ProcessCsvData();
                    processCSV.TestTheAI(files[i], data);
                    return;
                }

                //checks if the file is an image using the list of accepted image types
                bool pathVaild = false;
                for (int j = 0; j < imageEctentions.Length; j++)
                {
                    if (Path.GetExtension(files[i]).Contains(imageEctentions[j]))
                        pathVaild = true;
                } 

                //adds the files to the appropriate list
                if(pathVaild)
                    imagePaths.Add(files[i]);
                else
                    nonImagePaths.Add(files[i]);
            }

            //sends the image files to the ProcessImageData class and stores the non-image files displaying to the user
            ProcessImageData processImages = new ProcessImageData();
            data.nonCompatibleFiles = nonImagePaths;
            processImages.DetectFaces(imagePaths, data);
        }


    }
}
