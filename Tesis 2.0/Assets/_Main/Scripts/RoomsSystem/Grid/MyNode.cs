using UnityEngine;

namespace _Main.Scripts.RoomsSystem.Grid
{
    public class MyNode
    {
        public int XId { get; private set; }
        public int YId{ get; private set; }
        public bool Walkable{ get; private set; }
        public Vector3 WorldPos{ get; private set; }

        public void Initialize(bool p_walkable, Vector3 p_worldPos, Vector3 p_gridId)
        {
            Walkable = p_walkable;
            WorldPos = p_worldPos;

            XId = (int)p_gridId.x;
            YId = (int)p_gridId.y;
        }
    }
}