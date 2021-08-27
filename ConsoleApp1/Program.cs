using System;

namespace WolfGoatCabbage
{
    class Program
    {
        static void Main(string[] args)
        {
            Goat goat = new Goat();
            Wolf wolf = new Wolf();
            Cabbage cabbage = new Cabbage();

            RiverSide leftSide = new RiverSide(new IPassenger[] {goat, wolf, cabbage});
            RiverSide rightSide = new RiverSide();

            Man man = new Man();
            RiverSideState startSide = RiverSideState.Left;
            Boat boat = new Boat(man, leftSide, rightSide, startSide);
            boat.Transfer();


            Console.ReadKey();
        }
    }
}
