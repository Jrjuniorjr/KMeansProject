using DataPreparation.Models;
using DataPreparation.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataPreparation
{
    class Program
    {
        static void Main(string[] args)
        {
            var transactions = CsvUtil.ReadCsv("Transactions");
            var customers = transactions.Select(t => t.Customer).Distinct();
            var offers = transactions.Select(t => t.Offer).Distinct().ToList();
            offers.Sort();
            var results = new List<OutputData>();

            foreach (var customer in customers)
            {
                var result = new OutputData();
                result.Customer = customer;
                foreach (var offer in offers)
                {
                    int qtd = transactions.Where(t => t.Offer == offer && t.Customer.Equals(customer)).Count();
                    result.GetType().GetProperty("Offer" + offer).SetValue(result, qtd);
                }
                results.Add(result);

            }
            CsvUtil.WriteCsv(results, "DataPrepared");
        }
    }
}
