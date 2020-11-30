using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameEngine.Utilities;
using Moq;
using DBLayer;
using GameEngine.Entities;

namespace HanoiTowerGame.Test
{
    [TestClass]
    public class GameEngineTest
    {
        private Mock<IStateHelper> mockStateHelper;
        private Mock<IGameRepository> mockGameRepository;
        private GameEngine.Contracts.GameEngine gameEngine;

        [TestInitialize]
        public void TestInit()
        {
            mockStateHelper = new Mock<IStateHelper>();
            mockGameRepository = new Mock<IGameRepository>();
            gameEngine = new GameEngine.Contracts.GameEngine(mockGameRepository.Object, mockStateHelper.Object);
        }

        [TestMethod]
        public void UpdateState_UpdateSuccessfully()
        {
           //Arrange
           var gameState = new GameRodsState()
            {
                Rods = new List<int>[]
                    {
                         new List<int>{ 1,2,3,4},
                         new List<int>(),
                         new List<int>(),
                    },
            };

            //Act
            var result = gameEngine.UpdateState(gameState, 0, 1);

            // Assert
            Assert.AreEqual("{\"Rods\":[[1,2,3],[4],[]]}", result);
        }

        [TestMethod]
        public void IsMoveAllowed_MovingToNotExistinRod_ReturnFalse()
        {
            //Arrange
            var gameState = new GameRodsState()
            {
                Rods = new List<int>[]
                     {
                         new List<int>{ 1,2,3,4},
                         new List<int>(),
                         new List<int>(),
                     },
            };

            //Act
            string errorMsg = string.Empty;
            var result = gameEngine.IsMoveAllowed(gameState, 3, 1, ref errorMsg);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("There are 3 rods in game.", errorMsg);
        }

        [TestMethod]
        public void IsMoveAllowed_ValidMove_ReturnTrue()
        {
            //Arrange
            var gameState = new GameRodsState()
            {
                Rods = new List<int>[]
                     {
                         new List<int>{ 1,2,3,4},
                         new List<int>(),
                         new List<int>(),
                     },
            };

            //Act
            string errorMsg = string.Empty;
            var result = gameEngine.IsMoveAllowed(gameState, 0, 1, ref errorMsg);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(string.Empty, errorMsg);
        }

        [TestMethod]
        public void IsMoveAllowed_ValidMoveToEmptyRod_ReturnTrue()
        {
            //Arrange
            var gameState = new GameRodsState()
            {
                Rods = new List<int>[]
                     {
                         new List<int>{ 1,2,3},
                         new List<int>{4},
                         new List<int>(),
                     },
            };

            //Act
            string errorMsg = string.Empty;
            var result = gameEngine.IsMoveAllowed(gameState, 1, 2, ref errorMsg);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(string.Empty, errorMsg);
        }

        [TestMethod]
        public void IsMoveAllowed_MoveToTheSameRod_ReturnFalse()
        {
            //Arrange
            var gameState = new GameRodsState()
            {
                Rods = new List<int>[]
                     {
                         new List<int>{ 1,2,3,4},
                         new List<int>(),
                         new List<int>(),
                     },
            };

            //Act
            string errorMsg = string.Empty;
            var result = gameEngine.IsMoveAllowed(gameState, 0, 0, ref errorMsg);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("You cannot move disk to the same rod.", errorMsg);
        }

        [TestMethod]
        public void IsMoveAllowed_MoveFromEmptyRod_ReturnFalse()
        {
            //Arrange
            var gameState = new GameRodsState()
            {
                Rods = new List<int>[]
                     {
                         new List<int>{ 1,2,3,4},
                         new List<int>(),
                         new List<int>(),
                     },
            };

            //Act
            string errorMsg = string.Empty;
            var result = gameEngine.IsMoveAllowed(gameState, 1, 2, ref errorMsg);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("You cannot move disk from empty rod.", errorMsg);
        }

        [TestMethod]
        public void IsMoveAllowed_MoveLargerDiskToSmaller_ReturnFalse()
        {
            //Arrange
            var gameState = new GameRodsState()
            {
                Rods = new List<int>[]
                     {
                         new List<int>{ 3,4},
                         new List<int>{1},
                         new List<int>{2},
                     },
            };

            //Act
            string errorMsg = string.Empty;
            var result = gameEngine.IsMoveAllowed(gameState, 1, 2, ref errorMsg);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("You attempt to place larger disk on top of a smaller disk.", errorMsg);
        }

        [TestMethod]
        public void StartGame_InsertSuccessfully()
        {
            // Arrange
            mockStateHelper.
                Setup(mock => mock.CreateRodsStates(It.IsAny<string>())).
                Returns(new GameRodsState()
                {
                    Rods = new List<int>[]
                    {
                         new List<int>{ 1,2,3,4},
                         new List<int>(),
                         new List<int>(),
                    },
                });

            mockStateHelper.
               Setup(mock => mock.MapModelToTable(It.IsAny<Game>())).
               Returns(new HanoiTower()
               {
                   Id = 1, Definition = "{\"Rods\":[[1,2,3,4],[],[]]}",Status = 0
               });

            mockGameRepository.
                Setup(mock => mock.InsertGame(It.IsAny<HanoiTower>())).
                Returns(true);

            // Act
            var result = gameEngine.StartGame( 0, 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Status);
        }

        [TestMethod]
        public void StartGame_InsertFailed()
        {
            // Arrange
            mockStateHelper.
                Setup(mock => mock.CreateRodsStates(It.IsAny<string>())).
                Returns(new GameRodsState()
                {
                    Rods = new List<int>[]
                    {
                         new List<int>{ 1,2,3,4},
                         new List<int>(),
                         new List<int>(),
                    },
                });

            mockGameRepository.
                Setup(mock => mock.InsertGame(It.IsAny<HanoiTower>())).
                Returns(false);

            // Act
            var result = gameEngine.StartGame(0, 1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Status);
            Assert.AreEqual("DB error: game record was not created.", result.Message);
        }

        [TestMethod]
        public void UpdateGame_UpdateSuccessfully()
        {
            // Arrange
            mockStateHelper.
                Setup(mock => mock.CreateRodsStates(It.IsAny<string>())).
                Returns(new GameRodsState()
                {
                    Rods = new List<int>[]
                    {
                         new List<int>{ 1,2,3},
                         new List<int>{ 4 },
                         new List<int>(),
                    },
                });

            mockStateHelper.
               Setup(mock => mock.MapModelToTable(It.IsAny<Game>())).
               Returns(new HanoiTower()
               {
                   Id = 1,
                   Definition = "{\"Rods\":[[1,2,3,4],[],[]]}",
                   Status = 0
               });

            mockGameRepository.
                 Setup(mock => mock.GetGame(It.IsAny<int>())).
                 Returns(new HanoiTower { Id =1, Definition = "{\"Rods\":[[1,2,3],[4],[]]}", Status = 0 });

            mockGameRepository.
                Setup(mock => mock.UpdateGame(It.IsAny<HanoiTower>())).
                Returns(true);

            // Act
            var result = gameEngine.UpdateGame(1, 0, 2);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Status);
            Assert.AreEqual("{\"Rods\":[[1,2],[4],[3]]}", result.Definition);
        }

        [TestMethod]
        public void UpdateGame_UpdateFailed()
        {
            // Arrange
            mockStateHelper.
               Setup(mock => mock.CreateRodsStates(It.IsAny<string>())).
               Returns(new GameRodsState()
               {
                   Rods = new List<int>[]
                   {
                         new List<int>{ 1,2,3},
                         new List<int>{ 4 },
                         new List<int>(),
                   },
               });

            mockGameRepository.
                 Setup(mock => mock.GetGame(It.IsAny<int>())).
                 Returns(new HanoiTower { Id = 1, Definition = "{\"Rods\":[[1,2,3],[4],[]]}", Status = 0 });

            mockGameRepository.
                Setup(mock => mock.UpdateGame(It.IsAny<HanoiTower>())).
                Returns(false);

            // Act
            var result = gameEngine.UpdateGame(1, 0, 2);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Status);
            Assert.AreEqual("DB error: game record was not updated.", result.Message);
        }

        [TestMethod]
        public void UpdateGame_NotExistingGame_UpdateFailed()
        {
            // Arrange
            mockStateHelper.
               Setup(mock => mock.CreateRodsStates(It.IsAny<string>())).
               Returns(new GameRodsState()
               {
                   Rods = new List<int>[]
                   {
                         new List<int>{ 1,2,3},
                         new List<int>{ 4 },
                         new List<int>(),
                   },
               });

            mockGameRepository.
                 Setup(mock => mock.GetGame(It.IsAny<int>())).
                 Returns((HanoiTower)null);

            // Act
            var result = gameEngine.UpdateGame(1, 0, 2);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Status);
            Assert.AreEqual("Game 1 was not found.", result.Message);
        }
    }
}
