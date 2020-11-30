using System.Collections.Generic;

namespace GameEngine.Utilities
{
    public class GameRodsState
    {
        public List<int>[] Rods { get; set; }

        public GameRodsState()
        {
            Rods = new List<int>[3];

            for (int i = 0; i < 3; i++)
                Rods[i] = new List<int>();
        }
    }
}
