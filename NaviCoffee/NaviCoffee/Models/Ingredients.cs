using System;
using System.Collections.Generic;
using System.Text;

namespace NaviCoffee.Models
{
    [Flags]
    public enum Ingredients
    {
        None   = 0,   // Just water?
        Coffee = 1,   // Possibly add an addition enum later for 'blend', etc
        Espresso = 2,
        Milk   = 4,   // Same deal here...this might end up as a separate data point since
                      //   milk can really be half and half, 2%, whole milk, almond milk, etc.
        Sugar  = 8,
        Syrup  = 16,   // Add additional syrup details later if we move forward after the prototype
        Warm   = 32,
        Cold   = 64,
    }
}
