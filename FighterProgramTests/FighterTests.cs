/* FighterTests.cs
 * 
 * In this file, I have tested my public methods for Fighter class
 * 
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using FighterProgram;

namespace FighterProgram.Tests
{
    [TestClass]
    public class FighterTests
    {

        // Testing Move
        [TestMethod]
        public void TestMove()
        {
            Fighter fighter = new Fighter(0, 0, 10, 5, 5, new DefaultArtilleryProvider(new int[] { 10 }));
            fighter.Move(2, 2);
            Assert.AreEqual(2, fighter.getRow());
            Assert.AreEqual(2, fighter.getColumn());
        }

        //Testing Sum
        [TestMethod]
        public void TestSum()
        {
            Fighter fighter = new Fighter(0, 0, 10, 5, 5, new DefaultArtilleryProvider(new int[] { 10 }));
            Assert.AreEqual(0, fighter.Sum());
        }


        //Testing Target
        [TestMethod]
        public void TestTarget()
        {
            Fighter fighter = new Fighter(0, 0, 10, 3, 5, new DefaultArtilleryProvider(new int[] { 10, 5 }));
            int targetStrength = 5;

            // Simulate fighter targeting an object with strength 5
            bool targetResult = fighter.Target(0, 3, targetStrength);
            Assert.IsTrue(targetResult);
        }


        //Testing if reset is internally called when it goes below minimum strength
        [TestMethod]
        public void TestMinimumStrength()
        {
            // Create a fighter with minimum strength of 5
            Fighter fighter = new Fighter(0, 0, 10, 3, 5, new DefaultArtilleryProvider(new int[] { 10, 5 }));

            // Reduce fighter's strength below minimum
            fighter.Target(0, 3, 8);

            // Ensure that the fighter's strength is reset to minimum
            Assert.AreEqual(10, fighter.getStrength());
        }
    }
}
