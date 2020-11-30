using GameEngine.Entities;
using GameEngine.Utilities;

namespace GameEngine.Contracts
{
    public interface IGameEngine
    {
        bool IsMoveAllowed(GameRodsState rods, int from, int to, ref string err);
        string UpdateState(GameRodsState rods, int from, int to);

        Game StartGame(int from, int to);
        Game UpdateGame(int gameId, int from, int to);
    }
}
