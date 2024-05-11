using UnityEngine;

namespace _Main.Scripts.Grid
{
    public class MyNode
    {
        public int XId { get; private set; }
        public int YId{ get; private set; }
        public bool Walkable{ get; private set; }
        public Vector3 WorldPos{ get; private set; }

        public void Initialize(bool p_walkable, Vector3 p_worldPos, float p_radius, Vector3 gridId)
        {
            Walkable = p_walkable;
            WorldPos = p_worldPos;

            XId = (int)gridId.x;
            YId = (int)gridId.y;
        }
    }
}