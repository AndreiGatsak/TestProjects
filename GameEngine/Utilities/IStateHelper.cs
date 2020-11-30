
using DBLayer;
using GameEngine.Entities;

namespace GameEngine.Utilities
{
    public interface IStateHelper
    {
        GameRodsState CreateRodsStates(string strDefinition);
        HanoiTower MapModelToTable(Game gameModel);
    }
}
