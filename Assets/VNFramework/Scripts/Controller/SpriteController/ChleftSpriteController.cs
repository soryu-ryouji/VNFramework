using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VNFramework
{
    public class ChleftSpriteController : CharacterSpriteHandler
    {
        private void Start()
        {
            GameState.ChlpChanged += OnSpriteChanged;
        }
        private void OnDestroy()
        {
            GameState.ChlpChanged -= OnSpriteChanged;
        }
    }
}
