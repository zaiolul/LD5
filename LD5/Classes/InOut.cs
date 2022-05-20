using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace LD5
{
    /// <summary>
    /// Class for reading and writing data
    /// </summary>
    public class InOut
    {
        /// <summary>
        /// Gets all data from files
        /// </summary>
        /// <param name="filesPaths"></param>
        /// <returns>List of data from each file with file names </returns>
        /// <exception cref="AggregateException"></exception>
        public static List<Tuple<string, List<Valuable>>> ReadValuables(string[] filesPaths)
        {
            List<Exception> exceptions = new List<Exception>();

            List < Tuple<string, List<Valuable>> > data = new List <Tuple<string, List<Valuable>>> ();
            foreach (string path in filesPaths)
            {
                List<Valuable> valuables = new List<Valuable>();
                string[] allDirs = path.Split('\\');
                string fileName = allDirs[allDirs.Length - 1];
                if (File.ReadAllText(path).Length == 0)
                {
                    exceptions.Add(new Exception(String.Format("{0} failas yra tuščias.", fileName)));
                }
                else
                {
                    using (StreamReader reader = new StreamReader(path, System.Text.Encoding.UTF8))
                    {
                        int id = int.Parse(reader.ReadLine());
                        string line = "";
                        int i = 1;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] parts = line.Split(',');
                            if (parts.Length != 3)
                            {
                                exceptions.Add(new Exception(String.Format("Netinkamas duomenų laukų kiekis {0} faile, {1} eilutėje: {2}", fileName, i, line)));
                            }
                            else
                            {
                                int count;
                                if (!int.TryParse(parts[1], out count))
                                {
                                    exceptions.Add(new Exception(String.Format("Netinkamas kiekio lauko formatas {0} faile, {1} eilutėje: {2}", fileName, i, line)));
                                }
                                else
                                {
                                    valuables.Add(new Valuable(id,
                                   parts[0],
                                   count,
                                   decimal.Parse(parts[2])));
                                }
                            }
                            
                            i++;
                        }
                    }
                    data.Add(new Tuple<string, List<Valuable>>(fileName, valuables));
                }
               
            }
            if(exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }

            return data;
        }
        /// <summary>
        /// Reads order file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Order data</returns>
        public static List<Order> ReadOrder(string fileName)
        {
            List <Order> data = new List<Order>();
            using (StreamReader reader = new StreamReader(fileName, System.Text.Encoding.UTF8))
            {
                string line = "";
                while((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    data.Add(new Order(parts[0],
                        int.Parse(parts[1])));
                }
            }
            return data;
        }
        /// <summary>
        /// Method for printing data to .txt file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        /// <param name="header"></param>
        /// <param name="format"></param>
        public static void PrintToTxtFile<T>(List<Tuple<string, List<T>>> data, string fileName, string header, string format)
        {
            foreach (var entry in data)
            {
                PrintToTxtFile(entry.Item2, fileName, entry.Item1 + " " + header, format);
            }
        }
        /// <summary>
        /// Method for printing data to .txt file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        /// <param name="header"></param>
        /// <param name="format"></param>
        public static void PrintToTxtFile<T>(List<T> data, string fileName, string header, string format)
        {

            using (StreamWriter writer = new StreamWriter(fileName, true, System.Text.Encoding.UTF8))
            {
                writer.WriteLine(header);
                writer.WriteLine(format);
                foreach (T entry in data)
                {
                    writer.WriteLine(entry.ToString());
                }
                writer.WriteLine();
            }
        }
    }
}