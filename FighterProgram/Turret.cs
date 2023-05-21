/*
 * Turret.cs
 * 
 * This is a derived class from Fighter that overrides the functions shift, target and move
 *
 * Class Invariants:
 * - All integer values in the class must be non-negative.
 * - `Strength` must always be greater than or equal to `MinimumStrength`.
 * - `AttackRange` must always be greater than or equal to 0.
 * - Revive bound is passed as param so that each object has its own theshold
 * - Original strength is tracked for reset functionality
 * - Has IsActive status which will be modified only when failed requests goes above reviveBound
 * - Has IsPermanentlyDead which will be modified only when it takes damage and goes below minimum strength to live
 */
using System;
using System.ComponentModel.Design;

namespace FighterProgram
{
    public class Turret : Fighter
    {

        private int ReviveBound;
        private int FailedRequests;
        private bool IsActive;
        private bool IsPermanentlyDead;
        private int OriginalStrength;

        // Used in testing
        public bool getIsPermanentlyDead() { return IsPermanentlyDead; }


        // Pre-condition: In the constructor, `row`, `column`, `strength`, `attackRange`, `minimumStrength`, and `reviveBound` parameters must be non-negative.
        // Set the properties to its assigned parameter value
        public Turret(int row, int column, int strength, int attackRange, int minimumStrength, IArtilleryProvider artilleryProvider, int reviveBound)
            : base(row, column, strength, attackRange, minimumStrength, artilleryProvider)
        {

            if (row < 0 || column < 0 || strength < 0 || attackRange < 0 || minimumStrength < 0 || reviveBound < 0)
            {
                throw new ArgumentException("No values should be negative!");
            }
            ReviveBound = reviveBound;
            FailedRequests = 0;
            IsPermanentlyDead = false;
            IsActive = true;
            OriginalStrength = strength;
        }

        // Post-condition: The `Move` method has no effect on the object.
        public override void Move(int x, int y)
        {
            // Turrets cannot move
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
        // Post-condition: If the provided `x`, `y`, and `q` values are valid (i.e., non-negative), and the turret is not permanently dead,
        // it will return true or false whether or not target is valid.
        public override bool Target(int x, int y, int q)
        {
            if (x < 0 || y < 0 || q < 0)
            {
                throw new ArgumentException("No values should be negative!");
            }
            
            if (IsPermanentlyDead || y != Row || Math.Abs(x - Column) > AttackRange)
            {
                FailedRequests++;
                if (FailedRequests > ReviveBound)
                {
                    IsPermanentlyDead = true;
                }
                Revive();
               
                return false;
            }

            if (Artillery[0] > q)
            {
                
                TargetsVanquished++;
                return true;
            }
            else
            {
                if (IsActive && !IsPermanentlyDead)
                {
                    TakeDamage(q);
                    if (Strength < MinimumStrength)
                    {
                        IsPermanentlyDead = true;
                        IsActive = false;
                    }
                }
                Revive();
                return false;
            }
        }

        // Pre-condition: None
        // Post-condition: If the turret is not permanently dead and its `Strength` is less than `MinimumStrength`, the turret is revived and its `Strength` is set to its original value (`OriginalStrength`).
        private void Revive()
        {
            if (!IsPermanentlyDead && Strength < MinimumStrength)
            {
                Strength = OriginalStrength;
                IsPermanentlyDead = false;
                FailedRequests = 0;
            }
        }
    }
}
// Implementation Invariants:
// - The `FailedRequests` variable represents the number of failed target requests.
// - The `IsPermanentlyDead` variable is `true` if the turret has exceeded the `ReviveBound` number of failed target requests.
// - The `OriginalStrength` variable is equal to the `Strength` value passed in the constructor.
// - The `TargetsVanquished` variable is incremented by 1 each time a target is successfully vanquished.
// - If the turret is permanently dead or the target is out of range, increment the `FailedRequests` variable and check if the turret should be permanently dead.
// - The `OriginalStrength` variable is equal to the `Strength` value passed in the constructor.
// - Turret can only targets in its rows -> rows = column