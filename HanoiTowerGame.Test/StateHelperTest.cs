using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameEngine.Utilities;
using GameEngine.Entities;

namespace HanoiTowerGame.Test
{
    
    [TestClass]
    public class StateHelperTest
    {
        [TestMethod]
        public void CreateRodsStates_CreatedSuccessfully()
        {
            // Arrange
            var strDefinition = "{\"Rods\":[[1,2,3,4],[],[]]}";
            var expectedRod1Items = new [] {1,2,3,4};
            var target = new StateHelper();
            // Act
            var result = target.CreateRodsStates(strDefinition);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Rods.Length);
            Assert.AreEqual(0, result.Rods[1].Count);
            Assert.AreEqual(0, result.Rods[2].Count);
            CollectionAssert.AreEqual(
               expectedRod1Items,
               result.Rods[0].ToArray());
         }

        [TestMethod]
        public void MapModelToTable_MapSuccessfully()
        {
            // Arrange
            var game = new Game { Id=1, Definition = "{\"Rods\":[[1,2,3,4],[],[]]}" };
            var target = new StateHelper();

            // Act
            var result = target.MapModelToTable(game);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("{\"Rods\":[[1,2,3,4],[],[]]}", result.Definition);
        }
    }
}
