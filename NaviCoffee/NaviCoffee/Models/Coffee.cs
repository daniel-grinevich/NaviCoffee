using System;
using System.Collections.Generic;
using System.Text;

namespace NaviCoffee.Models
{
    public enum CoffeeType { Black = 1, Espresso = 2, Latte = 3};
    public class Coffee
    {
        public int CoffeeId { get; set; }
//        public int Id { get; set; }
        public CoffeeType CoffeeType { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public Ingredients Ingredients { get; set; }
        public string Barcode { get; set; }
        public string PriceString { get { return Price.ToString("C"); } }

        public bool IsVisibleIngredients { get;set;}

        private bool _hasSyrup;
        public bool HasSyrup
        {
            get
            {
                return _hasSyrup;
            }
            set
            {
                _hasSyrup = value;

                if (HasSyrup)
                {
                    Ingredients |= Ingredients.Syrup;
                }
                else
                {
                    Ingredients &= ~Ingredients.Syrup;
                }
            }
        }

        private bool _hasMilk;
        public bool HasMilk
        { get
            {
                return _hasMilk;
            }
            set
            {
                _hasMilk = value;

                if (HasMilk)
                {
                    Ingredients |= Ingredients.Milk;
                }
                else
                {
                    Ingredients &= ~Ingredients.Milk;
                }
            }
        }

        public override string ToString()
        {
            return $"Id: {CoffeeId} CoffeeType: {CoffeeType},  Name {Name} Ingredients: {Ingredients} Barcode: {Barcode}";
        }
    }
}
