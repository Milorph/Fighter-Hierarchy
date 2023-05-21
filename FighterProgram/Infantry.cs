/*
 * Infantry.cs
 * 
 * This is a derived class from Fighter that overrides the functions shift, target and move
 *
 * Class Invariants:
 * - All integer values in the class must be non-negative.
 * - `Strength` must always be greater than or equal to `MinimumStrength`.
 * - `AttackRange` must always be greater than or equal to 0.
 * - `lastMoveDirection` is initally null, till it moves.
 * - `isDead` keeps track of whether infantry has died if it goes below minimum strength
 */
using System;

namespace FighterProgram
{
    public class Infantry : Fighter
    {

        private int? lastMoveDirection;
        private int OriginalStrength;
        private bool isDead;

        // Pre-condition: In the constructor, `row`, `column`, `strength`, `attackRange`, and `minimumStrength` parameters must be non-negative.
        // Post-condition: Set all the properties values to its parameter equivalent
        public Infantry(int row, int column, int strength, int attackRange, int minimumStrength, IArtilleryProvider artilleryProvider)
            : base(row, column, strength, attackRange, minimumStrength, artilleryProvider)
        {
         
            if (row < 0 || column < 0 || strength < 0 || attackRange < 0 || minimumStrength < 0)
            {
                throw new ArgumentException("No values should be negative!");
            }
            lastMoveDirection = null;
            OriginalStrength = strength;
            isDead = false;
        }

        // Pre-condition: In the `Move` method, `x` and `y` parameters must be non-negative.
        // Post-condition: If the provided `x` and `y` values are valid (i.e., non-negative), the object's `Row` and `Column` properties are updated with the new values. If the infantry has made a previous move, the `lastMoveDirection` property is updated accordingly.
        public override void Move(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                throw new ArgumentException("Values must not be negative!");
            }
            if (lastMoveDirection.HasValue)
            {
                int newDirection = x > Row ? 1 : (x < Row ? -1 : 0);
                if (newDirection != 0 && newDirection != lastMoveDirection.Value)
                    return;
            }

            base.Move(x, y);
            lastMoveDirection = x > Row ? 1 : (x < Row ? -1 : (lastMoveDirection.HasValue ? lastMoveDirection.Value : 0));
        }

        // Pre-condition: In the `Shift` method, the `p` parameter must be non-negative.
        // Post-condition: If the provided `p` value is valid (i.e., non-negative), the object's `AttackRange` property is updated with the new value.
        public override void Shift(int p)
        {
            if (p < 0)
            {
                throw new ArgumentException("Shift value must be non-negative.");
            }
            AttackRange = p;
        }

        // Pre-condition: In the `Target` method, `x`, `y`, and `q` parameters must be non-negative.
        // Post-condition: If the provided `x`, `y`, and `q` values are valid (i.e., non-negative), the object's `Targets
        public override bool Target(int x, int y, int q)
        {
            if (x < 0 || y < 0 || q < 0)
            {
                throw new ArgumentException("No values should be negative!");
            }
            int distance = Math.Abs(x - Row) + Math.Abs(y - Column);
            if (isDead)
            {
                Reset();
            }

            if (distance > AttackRange)
            {
                return false;
            }

            if (Artillery[0] > q)
            {
                
                TargetsVanquished++;
                return true;
            }
            else
            {
                TakeDamage(q);
                if (Strength < MinimumStrength)
                {
                    isDead = true;
                }
            }

            return false;
        }

        // Pre-condition: None
        // Post-condition: set strength back to original, set it back to alive
        private void Reset()
        {
           
            Strength = OriginalStrength;
            isDead = false;
        }
    }
}

// Implementation Invariants:
// - The `lastMoveDirection` variable is either `null` or has a value of `-1`, `0`, or `1`.
// - The `OriginalStrength` variable is equal to the `Strength` value passed in the constructor.
// - The `isDead` variable is `true` if the infantry's `Strength` is less than `MinimumStrength`.
// - The `OriginalStrength` variable is equal to the `Strength` value passed in the constructor.
// - If the infantry's `Strength` is less than `MinimumStrength`, the `isDead` variable is set to `true`.
// - The `OriginalStrength` variable is equal to the `Strength` value passed in the constructor.