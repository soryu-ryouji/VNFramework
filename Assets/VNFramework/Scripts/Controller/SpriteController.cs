using System.Collections;
using System.ComponentModel;
using UnityEngine;

namespace VNFramework
{
    public class SpriteController : MonoBehaviour
    {
        private SpriteHandler _bgp;
        private CharacterSpriteHandler _chLeft;
        private CharacterSpriteHandler _chMid;
        private CharacterSpriteHandler _chRight;

        private void Awake()
        {
            _chMid = transform.Find("ChMid").GetComponent<CharacterSpriteHandler>();
            _chLeft = transform.Find("ChLeft").GetComponent<CharacterSpriteHandler>();
            _chRight = transform.Find("ChRight").GetComponent<CharacterSpriteHandler>();
            _bgp = transform.Find("Bgp").GetComponent<SpriteHandler>();

            GameState.BgpChanged += OnBgpChanged;
            GameState.ChmpChanged += OnChMidChanged;
            GameState.ChlpChanged += OnChLeftChanged;
            GameState.ChrpChanged += OnChRightChanged;
        }

        private void OnDestroy()
        {
            GameState.BgpChanged -= OnBgpChanged;
            GameState.ChmpChanged -= OnChMidChanged;
            GameState.ChlpChanged -= OnChLeftChanged;
            GameState.ChrpChanged -= OnChRightChanged;
        }

        private void OnChMidChanged(Hashtable hash)
        {
            _chMid.OnSpriteChanged(hash);
        }

        private void OnChLeftChanged(Hashtable hash)
        {
            _chLeft.OnSpriteChanged(hash);
        }

        private void OnChRightChanged(Hashtable hash)
        {
            _chRight.OnSpriteChanged(hash);
        }

        private void OnBgpChanged(Hashtable hash)
        {
            _bgp.OnSpriteChanged(hash);
        }
    }
}