using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace CMP304_Assesment_Project
{
    /// <summary>
    /// temporaraly stores the individual ML.NET predictions
    /// </summary>
    class OutputData
    {
        [ColumnName("PredictedLabel")]
        public string Emotion { get; set; }

        [ColumnName("Score")]
        public float[] Confidence { get; set; }
    }
}
