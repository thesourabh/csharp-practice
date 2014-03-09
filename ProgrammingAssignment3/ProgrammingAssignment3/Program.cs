using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleCards;

namespace ProgrammingAssignment3
{
    class Program
    {
        static void Main(string[] args)
        {

            // Declaring variables for deck, and hands for player and dealer
            Deck deck = new Deck();
            BlackjackHand dealer = new BlackjackHand("Dealer");
            BlackjackHand player = new BlackjackHand("Player");

            // Displaying welcome message
            Console.WriteLine("\t\tWELCOME!\n\nThis program will play a single hand.");

            // Shuffling the deck and dealing two cards to the player and the dealer
            deck.Shuffle();
            player.AddCard(deck.TakeTopCard());
            dealer.AddCard(deck.TakeTopCard());
            player.AddCard(deck.TakeTopCard());
            dealer.AddCard(deck.TakeTopCard());

            // Facing up all of the player's cards and the first of the dealer's cards
            player.ShowAllCards();
            dealer.ShowFirstCard();

            // Printing player's and dealer's current hands
            player.Print();
            dealer.Print();

            // Letting user decide whether to hit or not
            player.HitOrNot(deck);

            // Facing up remaining of dealer's cards and printing current hands of player and dealer
            dealer.ShowAllCards();
            player.Print();
            dealer.Print();

            // Displaying player and dealer score
            Console.WriteLine("Player Score: " + player.Score);
            Console.WriteLine("Dealer Score: " + dealer.Score);
        }
    }
}
