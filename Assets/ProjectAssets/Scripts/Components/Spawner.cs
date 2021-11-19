using System.Collections.Generic;

namespace Project.Components
{
    public struct Spawner
    {
        public Cell Parent;
        public int TurnsToSpawn;
        public List<Cell>[] Path;
    }
}