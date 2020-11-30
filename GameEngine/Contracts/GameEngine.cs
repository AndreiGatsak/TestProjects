using DBLayer;
using GameEngine.Entities;
using GameEngine.Utilities;
using System.Linq;
using Newtonsoft.Json;
using AutoMapper;

namespace GameEngine.Contracts
{
    public class GameEngine : IGameEngine
    {
        private IGameRepository gameRepository { get; set; }
        private IStateHelper stateHelper { get; set; }

        public GameEngine(IGameRepository gameRepository, IStateHelper stateHelper)
        {
            this.gameRepository = gameRepository;
            this.stateHelper = stateHelper;
        }
        public bool IsMoveAllowed(GameRodsState rods, int from, int to, ref string err)
        {

            if (from > 2 || to > 2)
            {
                err = "There are 3 rods in game.";
                return false;
            }

            if (from == to)
            {
                err = "You cannot move disk to the same rod.";
                return false;
            }

            if( rods.Rods[from].Count == 0)
            {
                err = "You cannot move disk from empty rod.";
                return false;
            }

            // any disk can go to empty rod
            if (rods.Rods[to].Count == 0)
                return true;

            var topFrom = rods.Rods[from].ElementAt(rods.Rods[from].Count - 1);
            var topTo = rods.Rods[to].ElementAt(rods.Rods[to].Count - 1);
            if (topFrom < topTo)
            {
                err = "You attempt to place larger disk on top of a smaller disk.";
                return false;
            }

            return true;
        }

        public string UpdateState(GameRodsState rods, int from, int to)
        {
            rods.Rods[to].Add(rods.Rods[from].ElementAt(rods.Rods[from].Count - 1));
            rods.Rods[from].RemoveAt(rods.Rods[from].Count - 1);

            return JsonConvert.SerializeObject(rods);
        }

        public Game StartGame(int from, int to)
        {
            string errString = "OK";
            var gameModel = new Game();

            // default game position: all disks are on 1st rod
            gameModel.Definition = "{\"Rods\":[[1,2,3,4],[],[]]}";
            var rods = this.stateHelper.CreateRodsStates(gameModel.Definition);

            if (!IsMoveAllowed(rods, from, to, ref errString))
            {
                gameModel.Message = errString;
                gameModel.Status = 1;

                return gameModel;
            }
            else
            {
                gameModel.Definition = UpdateState(rods, from, to);
            }

            var newRow = this.stateHelper.MapModelToTable(gameModel);
            if (this.gameRepository.InsertGame(newRow))
            {
                gameModel.Id = newRow.Id;
            }
            else
            {
                gameModel.Message = "DB error: game record was not created.";
                gameModel.Status = 1;
            }

            return gameModel;
        }

        public Game UpdateGame(int gameId, int from, int to)
        {
            string errString = "OK";
            var gameModel = new Game();
            gameModel.Id = gameId;

            var gameRow = this.gameRepository.GetGame(gameId);
            if (gameRow == null)
            {
                gameModel.Message = $"Game {gameId} was not found.";
                gameModel.Status = 1;

                return gameModel;
            }

            var rods = this.stateHelper.CreateRodsStates(gameRow.Definition);

            if (!IsMoveAllowed(rods, from, to, ref errString))
            {
                gameModel.Message = errString;
                gameModel.Status = 1;

                return gameModel;
            }
            else
            {
                gameModel.Definition = UpdateState(rods, from, to);

                if(rods.Rods[1].Count == 4 || rods.Rods[2].Count == 4)
                {
                    gameModel.Message = "Game is completed.";
                    gameModel.Status = 2;

                    this.gameRepository.DeleteGame(gameId);

                    return gameModel;
                }
            }

            var updateRow = this.stateHelper.MapModelToTable(gameModel);
            if (!this.gameRepository.UpdateGame(updateRow))
            {
                gameModel.Definition = gameRow.Definition;
                gameModel.Message = "DB error: game record was not updated.";
                gameModel.Status = 1;
            }

            return gameModel;
        }
    }
}
