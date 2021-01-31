using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.IO;

namespace CMP304_Assesment_Project
{
    internal class ProcessCsvData
    {
        /// <summary>
        /// calcutulates the models accuracy using ML.NET and stores the results to the DataGathered object
        /// </summary>
        /// <param name="path">a csv file path</param>
        /// <param name="data">DataGathered object</param>
        public void TestTheAI(string path, DataGathered data)
        {
            var mlContext = new MLContext();
            DataViewSchema modelSchema;
            //retrieves the model zip file
            ITransformer trainedModel = mlContext.Model.Load("model.zip", out modelSchema);

            //seperates and sorts the csv file columns
            var testDataView = mlContext.Data.LoadFromTextFile<CSVImporttedData>(@path, hasHeader: true, separatorChar: ',');

            var testMetrics = mlContext.MulticlassClassification.Evaluate(trainedModel.Transform(testDataView));

            //stores the test results data 
            data.testResults =
            $"* Metrics for Multi-class Classification model - Test Data \n" +
            $"* MicroAccuracy: {testMetrics.MicroAccuracy:0.###} \n" +
            $"* MacroAccuracy: {testMetrics.MacroAccuracy:0.###} \n" +
            $"* LogLoss: {testMetrics.LogLoss:#.###} \n" +
            $"* LogLossReduction: {testMetrics.LogLossReduction:#.###} \n";
        }
    }
}
