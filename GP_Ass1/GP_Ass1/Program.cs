using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GP_Ass1
{
    class Program
    {
        static void Main(string[] args)
        {

            //Declaration of necessary variables and printing the welcome message
            int gold;
            float hours, minutes, goldPerMinute;
            Console.WriteLine("\n\t\t\tWelcome!\n  This application will calculate "
                + "your average gold-collecting performance.");

            //Prompting the user for gold collected and hours spent
            Console.Write("\nHow much gold have you collected in the game? ");
            gold = int.Parse(Console.ReadLine());
            Console.Write("How many hours total have you played the game? ");
            hours = float.Parse(Console.ReadLine());

            //Calculating minutes and gold collected per minute
            minutes = hours * 60;
            goldPerMinute = gold / minutes;
            Console.WriteLine("\n\n\t\tSTATS\n");

            //Displaying the information
            Console.WriteLine("Gold Collected\t: " + gold);
            Console.WriteLine("Hours Played\t: " + hours);
            Console.WriteLine("Gold per minute\t: " + goldPerMinute);
        }
    }
}
