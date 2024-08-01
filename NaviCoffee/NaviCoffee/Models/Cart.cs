using System;
using System.Collections.Generic;
using System.Text;

namespace NaviCoffee.Models
{
    public class Cart
    {
        public Cart()
        {
            Coffee = new List<Coffee>();
        }

        public List<Coffee> Coffee { get; set; }

        public string Barcode { get; set; }
    }
}
