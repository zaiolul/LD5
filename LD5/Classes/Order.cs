using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LD5
{
    /// <summary>
    /// Class for order data
    /// </summary>
    public class Order
    {
        public string Name { get; set; }
        public int Count { get; set; }

        public Order(string name, int count)
        {
            Name = name;
            Count = count;
        }
        public override string ToString()
        {
            return String.Format("| {0, -11} | {1, 8} |", Name, Count);
        }
    }
}