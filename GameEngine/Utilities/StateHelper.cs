using AutoMapper;
using DBLayer;
using GameEngine.Entities;
using Newtonsoft.Json;

namespace GameEngine.Utilities
{
    public class StateHelper : IStateHelper
    {
        public GameRodsState CreateRodsStates(string strDefinition)
        {
            var GameRodsState = new GameRodsState();

            return JsonConvert.DeserializeObject<GameRodsState>(strDefinition);
        }

        public HanoiTower MapModelToTable(Game gameModel)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Game, HanoiTower>();
            });
            IMapper iMapper = config.CreateMapper();
            return iMapper.Map<Game, HanoiTower>(gameModel);
        }
    }
}