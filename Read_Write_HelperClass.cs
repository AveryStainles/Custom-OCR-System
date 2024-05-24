using System;
using System.IO;

namespace AverySecretProject
{
    internal class Read_Write_HelperClass
    {

        public string ReadFromFile(string filePath)
        {
            string allData = "";
            string hasMoreData;
            StreamReader? reader = null;
            try
            {
                reader = new StreamReader(filePath);
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

            StreamWriter? writer = null;
            try
            {
                writer = new StreamWriter(filePath);
                writer.WriteLine(data);
                writer.Close();
            }
            catch (Exception e)
            {
                // TODO body to catch clause
                return;
            }
        }
    }
}
