/* InfantryTests.cs
 * 
 * In this file, I have tested my public methods for my derived Infantry class
 * 
*/

using FighterProgram;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FighterProgram.Tests
{
    [TestClass]
    public class InfantryTests
    {

        // Test Infantry reset when inital strength is less than minimum strength
        [TestMethod]
        public void TestReset()
        {
            Infantry infantry = new Infantry(0, 0, 3, 3, 4, new DefaultArtilleryProvider(new int[] { 10 }));
           
            Assert.AreEqual(3, infantry.getStrength());
        }


        // Testing the move of the Infantry
        [TestMethod]
        public void TestMove()
        {
            Infantry infantry = new Infantry(0, 0, 10, 5, 2, new DefaultArtilleryProvider(new int[] { 10 }));
            infantry.Move(2, 2);
            Assert.AreEqual(2, infantry.getRow());
            Assert.AreEqual(2, infantry.getColumn());
        }


        // Testing the shift for the Infantry
        [TestMethod]
        public void TestShift()
        {
            Infantry infantry = new Infantry(0, 0, 10, 5, 2, new DefaultArtilleryProvider(new int[] { 10 }));
            infantry.Shift(2);
            Assert.AreEqual(2, infantry.getAttackRange());
        }


        // Testing Target for infantry
        [TestMethod]
        public void TestTarget()
        {
            Infantry infantry = new Infantry(0, 0, 10, 3, 2, new DefaultArtilleryProvider(new int[] { 10 }));
            Assert.IsTrue(infantry.Target(0, 3, 5));
        }
    }
}
