using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ObservableCollections
{
    public class Market
    {
        public BindingList<float> Prices = new();

        public void AddPrice(float price)
        {
            Prices.Add(price);
        }
    }

    class Program
    {
        static void Main()
        {
            var market = new Market();
            market.Prices.ListChanged += (object sender, ListChangedEventArgs eventArgs) =>
            {
                if (eventArgs.ListChangedType == ListChangedType.ItemAdded)
                {
                    float price = ((BindingList<float>)sender)[eventArgs.NewIndex];
                    Console.WriteLine($"Binding list got a price of {price}");
                }
            };
            market.AddPrice(123);
        }
    }
}
