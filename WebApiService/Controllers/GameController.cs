using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DBLayer;
using GameEngine.Contracts;

namespace WebApiService.Controllers
{
    public class GameController : ApiController
    {
        private IGameEngine gameEngine;

        public GameController(IGameEngine gameEngine)
        {
            this.gameEngine = gameEngine;
        }

        [HttpPost]
        public GameEngine.Entities.Game Post(int from, int to)
        {
            return this.gameEngine.StartGame(from, to);
        }

        [HttpPut]
        public GameEngine.Entities.Game Put(int gameId, int from, int to)
        {
            return this.gameEngine.UpdateGame(gameId, from, to);
        }
    }
}
