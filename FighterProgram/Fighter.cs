/*
 * Fighter.cs
 * 
 * This is the base class that turret and infantry will inherit, it declares the functions that infantry and turret will override
 * and uses dependency injection for its values in the artillery.
 *
 * Class Invariants:
 * - All integer values in the class must be non-negative.
 * - `Strength` must always be greater than or equal to `MinimumStrength`.
 * - Supports Target, Sum, Move and shift has no effect
 * - Used dependency injection for artillery
 * - Each object has a minimum strength it must have before its considered inactive, 
 *   this varies across all objects therefore passed as parameter in constructor
 * - Keep track of number of vanquished targets for each object
 */

using System;

namespace FighterProgram
{
    // Pre-condition: None
    // Post-condition: The method returns an array of integers representing the available artillery.
    public interface IArtilleryProvider
    {
        int[] GetArtillery();
    }

    // Pre-condition: In the constructor, the `artillery` parameter must not be null.
    // Post-condition: The method returns an array of integers representing the available artillery.
    public class DefaultArtilleryProvider : IArtilleryProvider
    {
        private int[] _artillery;

        public DefaultArtilleryProvider(int[] artillery)
        {
            _artillery = artillery;
        }

        public int[] GetArtillery()
        {
            return _artillery;
        }
    }

    public class Fighter
    {
        protected int Row;
        protected int Column;
        protected int Strength;
        protected int AttackRange;
        protected int MinimumStrength;
        protected int[] Artillery;
        protected int TargetsVanquished;
        protected IArtilleryProvider ArtilleryProvider;


        // Return methods used for unit testing.
        public int getRow() { return Row; }
        public int getColumn() { return Column; }
        public int getStrength() {  return Strength; }
        public int getAttackRange() { return AttackRange; }
        public int getMinimumStrength() {  return MinimumStrength; }

        // Pre-condition: In the constructor, `row`, `column`, `strength`, `attackRange`, and `minimumStrength` parameters must be non-negative.
        // Post-condition: Bind the properties to the parameter values
        public Fighter(int row, int column, int strength, int attackRange, int minimumStrength, IArtilleryProvider artilleryProvider)
        {
            if (row < 0 || column < 0 || strength < 0 || attackRange < 0 || minimumStrength < 0)
            {
                throw new ArgumentException("No values should be negative!");
            }
            Row = row;
            Column = column;
            Strength = strength;
            AttackRange = attackRange;
            MinimumStrength = minimumStrength;
            ArtilleryProvider = artilleryProvider;
            Artillery = ArtilleryProvider.GetArtillery();
            TargetsVanquished = 0;
        }
        // Pre-condition: In the `Move`, `Shift`, and `Target` methods, `x`, `y`, and `q` parameters must be non-negative.
        // Post-conditon: changes row to be the new row and column to be the new column
        public virtual void Move(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                throw new ArgumentException("Values must not be negative!");
            }
            Row = x;
            Column = y;
        }

        // No pre- or post-condition
        public virtual void Shift(int p)
        {
            // Base fighter class has no effect
        }

        // Pre-condition: `x`, `y`, and `q` parameters must be non-negative `Artillery[0]` must be greater than `q`.
        // Post-condition: If `Strength` is greater than or equal to `MinimumStrength` and `Artillery[0]` is greater than `q`, `
        // TargetsVanquished` is incremented by 1 and `true` is returned; otherwise, `Strength` is reduced by `q` which is the takeDamage()
        public virtual bool Target(int x, int y, int q)
        {
            if (x < 0 || y < 0 || q < 0)
            {
                throw new ArgumentException("No values should be negative!");
            }
            if (Strength >= MinimumStrength && Artillery[0] > q)
            {
                TargetsVanquished++;
                return true;
            }
            else
            {
                TakeDamage(q);
                return false;
            }
        }
        // Pre-condition: The provided `damage` value must not be negative.
        // Post-condition: The object's `Strength` property is reduced by the provided `damage` value. If `Strength` is less than `MinimumStrength`, it is set to 0.
        protected void TakeDamage(int damage)
        {
            if(damage < 0)
            {
                throw new ArgumentException("Damage should not be negative");
            }
            Strength -= damage;
            if (Strength < MinimumStrength)
            {
                Strength = 0;
            }
        }

        // Pre-condition: None
        // Post-condition: The object's `TargetsVanquished` property is returned.
        public int Sum()
        {
            return TargetsVanquished;
        }
    }
}

// Implementation Invariants:
// - All integer values in the class must be non-negative.
// - The `Artillery` array must be obtained from the `ArtilleryProvider`.
// - The `TargetsVanquished` variable must be incremented by 1 each time a target is successfully vanquished.
// - The object takes damage if target is unsuccessful similar to "yugioh" combat
// - Target, shift and move are virtual to support overriding in derived classes