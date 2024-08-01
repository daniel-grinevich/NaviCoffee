using System;
namespace NaviCoffee.WebApi.Models
{
    public enum CoffeeType { Black = 1, Espresso = 2, Latte = 3 };
    public class Coffee
    {
        public int CoffeeId { get; set; }

        // TODO: Ooops forgot to push this to the webapi when the phone adds an order.  for now.  fake it.
        public CoffeeType CoffeeType 
        { 
            get
            {
                switch (Name)
                {
                    case "Black":
                        return CoffeeType.Black;
                    case "Espresso":
                        return CoffeeType.Espresso;
                    case "Latte":
                        return CoffeeType.Latte;
                    default:
                        return CoffeeType.Black;
                }
            }
        
        }

        public string Name { get; set; }
        public string Label { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public Ingredients Ingredients { get; set; }
        public string Barcode { get; set; }
        public string PriceString { get { return Price.ToString("C"); } }

        public bool IsVisibleIngredients { get; set; }

        public override string ToString()
        {
            return $"Id: {CoffeeId} CoffeeType: {CoffeeType},  Name {Name} Ingredients: {Ingredients} Barcode: {Barcode}";
        }
    }

    [Flags]
    public enum Ingredients
    {
        None = 0,   // Just water?
        Coffee = 1,   // Possibly add an addition enum later for 'blend', etc
        Espresso = 2,
        Milk = 4,   // Same deal here...this might end up as a separate data point since
                    //   milk can really be half and half, 2%, whole milk, almond milk, etc.
        Sugar = 8,
        Syrup = 16,   // Add additional syrup details later if we move forward after the prototype
        Warm = 32,
        Cold = 64,
    }
}
