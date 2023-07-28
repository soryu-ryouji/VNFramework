using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VNFramework
{
    public class ChRightSpriteController : CharacterSpriteHandler
    {
        private void Start()
        {
            GameState.ChrpChanged += OnSpriteChanged;
        }

        private void OnDestroy()
        {
            GameState.ChrpChanged -= OnSpriteChanged;
        }
    }
}
