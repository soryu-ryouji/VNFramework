using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VNFramework
{
    public class ChMidSpriteController : CharacterSpriteHandler
    {
        private void Start()
        {
            GameState.ChmpChanged += OnSpriteChanged;
        }

        private void OnDestroy()
        {
            GameState.ChmpChanged -= OnSpriteChanged;
        }
    }
}
