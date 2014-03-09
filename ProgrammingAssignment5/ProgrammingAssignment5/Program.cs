using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            // Declaring variables to hold random number generator, each player's rolls and wins
            // and whether or not a new game of war has to be player
            Random rand = new Random();
            int playerOneRoll, playerTwoRoll, playerOneWins, playerTwoWins;
            char playAgain = 'Y';

            // Displaying the welcome message
            Console.WriteLine("\t\t\tWelcome");
            Console.WriteLine("\n\tThis program plays games of War\n");

            // While loop to quit if value of playAgain is N
            while (playAgain != 'N')
            {
                playerOneWins = 0;
                playerTwoWins = 0;

                // For loop to run exactly 21 battles
                for (int i = 0; i < 21; i++)
                {

                    // Rolling for both players and declaring 'WAR!' and rerolling if a tie
                    playerOneRoll = rand.Next(1, 14);
                    playerTwoRoll = rand.Next(1, 14);
                    while (playerOneRoll == playerTwoRoll)
                    {
                        Console.WriteLine("   WAR! P1:" + playerOneRoll +
                            "\tP2:"+playerTwoRoll);
                        playerOneRoll = rand.Next(1, 14);
                        playerTwoRoll = rand.Next(1, 14);
                    }

                    // Printing details of the battle and who won, and incremementing win count
                    Console.Write("BATTLE: P1:" + playerOneRoll +
                        "\tP2:" + playerTwoRoll);
                    if (playerOneRoll > playerTwoRoll)
                    {
                        Console.WriteLine("\tP1 Wins!");
                        playerOneWins++;
                    }
                    else
                    {
                        Console.WriteLine("\tP2 Wins!");
                        playerTwoWins++;
                    }

                }

                // Printing the overall winner and asking player if they want to play again
                if (playerOneWins > playerTwoWins)
                    Console.WriteLine("\n\nP1 is the overall Winner with " + playerOneWins + " battles!");
                else
                    Console.WriteLine("\n\nP2 is the overall Winner with " + playerTwoWins + " battles!");
                Console.Write("\n\nDo you want to play again (y/n)? ");
                playAgain = char.Parse(Console.ReadLine().ToUpper());
            }
        }
    }
}
