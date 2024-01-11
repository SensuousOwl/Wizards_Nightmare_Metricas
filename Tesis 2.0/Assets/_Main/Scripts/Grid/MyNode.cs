using UnityEngine;

namespace _Main.Scripts.Grid
{
    public class MyNode
    {
        public int XId;
        public int YId;
        public bool Walkable;
        public Vector3 WorldPos;

        public void Initialize(bool p_walkable, Vector3 p_worldPos, float p_radius, Vector3 gridId)
        {
            Walkable = p_walkable;
            WorldPos = p_worldPos;

            XId = (int)gridId.x;
            YId = (int)gridId.y;
        }
    }
}