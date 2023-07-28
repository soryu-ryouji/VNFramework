using System.Collections;
using UnityEngine;

namespace VNFramework
{
    public class BgpSpriteController : SpriteController
    {
        private void Awake()
        {
            image = GetComponent<SpriteRenderer>();
            GameState.BgpChanged += OnSpriteChanged;
        }

        private void OnDestroy()
        {
            GameState.BgpChanged -= OnSpriteChanged;
        }
    }
}