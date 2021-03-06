﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace CMP304_Assesment_Project
{
    /// <summary>
    /// temporaraly stores the individual ML.NET inputs 
    /// </summary>
    class CSVImporttedData
    {
        [LoadColumn(0)]
        public string Emotion { get; set; }

        [LoadColumn(1)]
        public float LeftEyebrow { get; set; }

        [LoadColumn(2)]
        public float RightEyebrow { get; set; }

        [LoadColumn(3)]
        public float LeftLip { get; set; }

        [LoadColumn(4)]
        public float RightLip { get; set; }

        [LoadColumn(5)]
        public float LipHeight { get; set; }

        [LoadColumn(6)]
        public float LipWidth { get; set; }

        [LoadColumn(7)]
        public float LeftEyeHieght { get; set; }

        [LoadColumn(8)]
        public float RightEyeHieght { get; set; }

        [LoadColumn(9)]
        public float MouthOpen { get; set; }

    }
}
