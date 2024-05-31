using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Custom_Optical_Character_Recognition_System
{
    internal class Read_Write_HelperClass
    {

        public string ReadFromFile(string filePath)
        {
            string allData = "";
            string hasMoreData;
            try
            {
                StreamReader reader = new StreamReader(filePath);
                hasMoreData = reader.ReadLine();

                while (hasMoreData != null)
                {
                    allData += "\n" + hasMoreData;
                    hasMoreData = reader.ReadLine();
                }
                reader.Close();
            }
            catch (Exception e)
            {
                return $"{e.Message}";
            }

            return allData;
        }

        public void WriteToFile(string filePath, string data)
        {
            try
            {
                StreamWriter writer = new StreamWriter(filePath);
                writer.WriteLine(data);
                writer.Close();
            }
            catch (Exception e)
            {
                // TODO body to catch clause
                return;
            }
        }

        public List<double> GetListFromDataPath(string data_path)
        {
            return ReadFromFile(data_path).Split(',').Select(val => double.Parse(val)).ToList();
        }
    }
}
