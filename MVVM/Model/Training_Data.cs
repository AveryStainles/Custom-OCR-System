using System.Collections.Generic;

namespace Custom_Optical_Character_Recognition_System.MVVM.Model
{
    public class Training_Data
    {
        public string Value { get; set; }
        public int TotalImagesUsedToTrain { get; set; } = 0;
        public IEnumerable<double> RowAverages { get; set; }
        public IEnumerable<double> ColumnAverages { get; set; }
    }
}
