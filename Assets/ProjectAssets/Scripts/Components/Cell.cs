using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

namespace Project.Components
{
    public struct Cell
    {
        public Vector2Int Position;
        
        public EcsPackedEntityWithWorld Entity;
        
        public List<EcsPackedEntityWithWorld> Links;
        public HashSet<EcsPackedEntityWithWorld> Neighbors;

        public EcsPool<Cell> CellPool;
        
        public int GCost;
        public int HCost;
        public int FCost => GCost + HCost;

        public EcsPackedEntityWithWorld PreviousCell;

        public void Link(EcsPackedEntityWithWorld cellEntity, bool bothDirections = true)
        {
            if (Links.Contains(cellEntity)) 
                return;

            Links.Add(cellEntity);
            
            if (bothDirections)
                if (cellEntity.Unpack(out var world, out var entity))
                    CellPool.Get(entity).Link(Entity, false);
        }

        public void UnLink(EcsPackedEntityWithWorld cellEntity, bool bothDirections = true)
        {
            if (Links.Contains(cellEntity) == false)
                return;

            Links.Remove(cellEntity);
            
            if (bothDirections)
                if (cellEntity.Unpack(out var world, out var entity))
                    CellPool.Get(entity).UnLink(Entity, false);
        }
    }
}