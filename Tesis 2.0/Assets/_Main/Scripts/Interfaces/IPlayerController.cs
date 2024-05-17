using System;
using UnityEngine;

namespace _Main.Scripts.Interfaces
{
    public interface IPlayerController
    {
        event Action OnUseItem;
        event Action OnInteract;
        event Action OnShoot;
        event Action<Vector2> OnMove;
        event Action<Vector2> OnUpdateCrosshair;
    }
}