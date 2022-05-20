using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LD5
{
    /// <summary>
    /// Class for Valuable data
    /// </summary>
    public class Valuable
    {
        public int StorageID { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }

        public Valuable(int storageID, string name, int count, decimal price)
        {
            StorageID = storageID;
            Name = name;
            Count = count;
            Price = price;
        }
        public override string ToString()
        {
            return String.Format("| {0, 13} | {1, -11} | {2, 8} | {3, 5} |", StorageID, Name, Count, Price);
        }
    }
}