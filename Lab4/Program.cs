using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            // create a new deck and print the contents of the deck
            Deck deck = new Deck();
            deck.Print();
            // shuffle the deck and print the contents of the deck
            Console.WriteLine();
            deck.Shuffle();
            deck.Print();

            Card a = deck.TakeTopCard();

            Console.WriteLine("\n\n" + a.Rank + " of " + a.Suit);
            Card b = deck.TakeTopCard();

            Console.WriteLine(b.Rank + " of " + b.Suit);
            // take the top card from the deck and print the card rank and suit

            // take the top card from the deck and print the card rank and suit


        }
    }
}
