using CsvHelper;
using DataPreparation.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DataPreparation.Utils
{
    static class CsvUtil
    {
        public static void WriteCsv(List<OutputData> data, string fileName)
        {

            var path = Path.Combine(Environment.CurrentDirectory, @"../../../../KMeans/", $"{fileName}.csv");

            using (var writer = new StreamWriter(path))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(data);
            }
        }
        public static List<InputData> ReadCsv(string fileName)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "Data", $"{fileName}.csv");

            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var data = csv.GetRecords<InputData>();
                return data.ToList();
            }
        }
    }
}
