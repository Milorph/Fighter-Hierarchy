/*
 * Robert Widjaja 
 * Inheritance & Dependency Injection
 * Revision History -- 4/28/2023 -> finalized documentation
 * Platform: Windows
 * P3.cs
 * 
 * 
 * 
 * The program then proceeds to perform a series of actions on the fighters, such as moving the infantry units to different locations,
 * attempting to move the turrets (which should fail), changing the attack range of each fighter, and attempting to target enemies within range.
 * Finally, the program prints out the number of targets each fighter has vanquished. 
 * The program then repeats this process multiple times with different sets of randomly generated fighters,
 * demonstrating the flexibility and scalability of the program.
 * 
 * ASSUMPTIONS:
 * 
 * Input values for each fighter are valid and non-negative.
 * Each fighter's Strength value will always be greater than or equal to its MinimumStrength value.
 * The Artillery array provided to each fighter will always contain at least one element, and that each element will have a valid and non-negative value.
 * Each Infantry object will always have a non-negative value for Shift.
 * Each Turret object will never be able to move, and that attempts to do so will have no effect.
 * Each Turret object will either revive or become permanently dead based on the number of failed requests, as defined by the ReviveBound property.
 * The GetArtillery() method of the IArtilleryProvider interface will always return a valid and non-null array of integers.
 * The input values for each GenerateFighters() method call will always generate valid and non-negative input values for each fighter
 * 
 * IMPORTANT:
 *   Program uses random values to place at random row, columns and attack range. Run code again if no targets return true and no targets are vanquished.
 */
using FighterProgram;
using System;
using System.Collections.Generic;

namespace FighterProgram
{
    class P3
    {
        //Functionally Decompose Driver
        static void Main(string[] args)
        {
            // If CLIENT gets all 0 values for vanquish, try again till you reach random numbers that work with each other.
            for (int i = 0; i < 10; i++)
            {
                List<Fighter> fighters = GenerateFighters();
                MoveInfantry(fighters);
                AttemptToMoveTurrets(fighters);
                ChangeAttackRanges(fighters);
                AttemptToTargetEnemies(fighters);
                PrintTargetsVanquished(fighters);
                Console.WriteLine();
            }
        }

        // Generates a list of fighters using random number generators
        static List<Fighter> GenerateFighters()
        {
            List<Fighter> fighters = new List<Fighter>();

            Random random = new Random();

            for (int i = 0; i < 2; i++)
            {
                int row = random.Next(5);
                int column = random.Next(5);
                int strength = random.Next(10, 16);
                int attackRange = random.Next(1, 6);
                int minimumStrength = random.Next(1, strength + 1);
                int reviveBound = random.Next(1, 6);
                int[] artilleryStrengths = new int[random.Next(1, 4)];
                for (int j = 0; j < artilleryStrengths.Length; j++)
                {
                    artilleryStrengths[j] = random.Next(1, strength + 1);
                }
                fighters.Add(new Turret(row, column, strength, attackRange, minimumStrength, new DefaultArtilleryProvider(artilleryStrengths), reviveBound));
            }

            for (int i = 0; i < 2; i++)
            {
                int row = random.Next(5);
                int column = random.Next(5);
                int strength = random.Next(10, 16);
                int attackRange = random.Next(1, 6);
                int minimumStrength = random.Next(1, strength + 1);
                int[] artilleryStrengths = new int[random.Next(1, 4)];
                for (int j = 0; j < artilleryStrengths.Length; j++)
                {
                    artilleryStrengths[j] = random.Next(1, strength + 1);
                }
                fighters.Add(new Infantry(row, column, strength, attackRange, minimumStrength, new DefaultArtilleryProvider(artilleryStrengths)));
            }

            return fighters;
        }

        // Moving infantry
        static void MoveInfantry(List<Fighter> fighters)
        {
            Console.WriteLine("Moving infantry...");
            Infantry infantry1 = (Infantry)fighters[2];
            infantry1.Move(1, 0);
            Infantry infantry2 = (Infantry)fighters[3];
            infantry2.Move(4, 4);
        }

        // Move turret -- it wouldn't move
        static void AttemptToMoveTurrets(List<Fighter> fighters)
        {
            Console.WriteLine("Attempting to move turrets...");
            Turret turret1 = (Turret)fighters[0];
            turret1.Move(1, 1);
            Turret turret2 = (Turret)fighters[1];
            turret2.Move(2, 3);
        }
        

        // Call shift for each fighter
        static void ChangeAttackRanges(List<Fighter> fighters)
        {
            Console.WriteLine("Changing attack range...");
            Turret turret1 = (Turret)fighters[0];
            turret1.Shift(3);
            Turret turret2 = (Turret)fighters[1];
            turret2.Shift(2);
            Infantry infantry1 = (Infantry)fighters[2];
            infantry1.Shift(3);
            Infantry infantry2 = (Infantry)fighters[3];
            infantry2.Shift(6);
        }

        //Targeting enemies for each fighter
        static void AttemptToTargetEnemies(List<Fighter> fighters)
        {
            Random random = new Random();
            Turret turret1 = (Turret)fighters[0];
            Turret turret2 = (Turret)fighters[1];
            Infantry infantry1 = (Infantry)fighters[2];
            Infantry infantry2 = (Infantry)fighters[3];
            Console.WriteLine("Turret 1 targeting: " + turret1.Target(4, 1, 7));
            Console.WriteLine("Turret 2 targeting: " + turret2.Target(2, 2, 7));
            Console.WriteLine("Infantry 1 targeting: " + infantry1.Target(random.Next(5), random.Next(5), random.Next(1, 11)));
            Console.WriteLine("Infantry 2 targeting: " + infantry2.Target(random.Next(5), random.Next(5), random.Next(1, 11)));
        }

        // Print vanquished for each fighter
        static void PrintTargetsVanquished(List<Fighter> fighters)
        {
            Console.WriteLine("Targets vanquished:");
            for (int i = 0; i < fighters.Count; i++)
            {
                Console.WriteLine("Fighter " + (i + 1) + ": " + fighters[i].Sum());
            }
        }
    }
}
