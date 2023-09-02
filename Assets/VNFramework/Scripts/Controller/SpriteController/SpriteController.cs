using UnityEngine;

namespace VNFramework
{
    class SpriteController: MonoBehaviour, IController
    {
        private SpriteHandler _bgpHandler;
        private CharacterSpriteHandler _chLeftHandler;
        private CharacterSpriteHandler _chRightHandler;
        private CharacterSpriteHandler _chMidHandler;

        private PerformanceModel _performModel;

        private void Awake()
        {
            _bgpHandler = this.transform.Find("Bgp").GetComponent<SpriteHandler>();
            _chLeftHandler = this.transform.Find("ChLeft").GetComponent<CharacterSpriteHandler>();
            _chRightHandler = this.transform.Find("ChRight").GetComponent<CharacterSpriteHandler>();
            _chMidHandler = this.transform.Find("ChMid").GetComponent<CharacterSpriteHandler>();
            
            _performModel = this.GetModel<PerformanceModel>();

            this.RegisterEvent<BgpFadingShowEvent>(_=> _bgpHandler.ShowSprite(_performModel.BgpName, SpriteMode.Fading))
                .UnRegisterWhenGameObjectDestroyed(this);
            this.RegisterEvent<BgpFadingHideEvent>(_=> _bgpHandler.HideSprite(SpriteMode.Fading))
                .UnRegisterWhenGameObjectDestroyed(this);
            this.RegisterEvent<BgpImmediateShowEvent>(_=> _bgpHandler.ShowSprite(_performModel.BgpName, SpriteMode.Immediate))
                .UnRegisterWhenGameObjectDestroyed(this);
            this.RegisterEvent<BgpImmediateHideEvent>(_=> _bgpHandler.HideSprite(SpriteMode.Immediate))
                .UnRegisterWhenGameObjectDestroyed(this);

            this.RegisterEvent<ChLeftFadingShowEvent>(_=> _chLeftHandler.ShowSprite(_performModel.ChLeft, SpriteMode.Fading))
                .UnRegisterWhenGameObjectDestroyed(this);
            this.RegisterEvent<ChLeftFadingHideEvent>(_=> _chLeftHandler.HideSprite(SpriteMode.Fading))
                .UnRegisterWhenGameObjectDestroyed(this);
            this.RegisterEvent<ChLeftImmediateShowEvent>(_=> _chLeftHandler.ShowSprite(_performModel.ChLeft, SpriteMode.Immediate))
                .UnRegisterWhenGameObjectDestroyed(this);
            this.RegisterEvent<ChLeftImmediateHideEvent>(_=> _chLeftHandler.HideSprite(SpriteMode.Immediate))
                .UnRegisterWhenGameObjectDestroyed(this);

            this.RegisterEvent<ChRightFadingShowEvent>(_=> _chRightHandler.ShowSprite(_performModel.ChRight, SpriteMode.Fading))
                .UnRegisterWhenGameObjectDestroyed(this);
            this.RegisterEvent<ChRightFadingHideEvent>(_=> _chRightHandler.HideSprite(SpriteMode.Fading))
                .UnRegisterWhenGameObjectDestroyed(this);
            this.RegisterEvent<ChRightImmediateShowEvent>(_=> _chRightHandler.ShowSprite(_performModel.ChRight, SpriteMode.Immediate))
                .UnRegisterWhenGameObjectDestroyed(this);
            this.RegisterEvent<ChRightImmediateHideEvent>(_=> _chRightHandler.HideSprite(SpriteMode.Immediate))
                .UnRegisterWhenGameObjectDestroyed(this);

            this.RegisterEvent<ChMidFadingShowEvent>(_=> _chMidHandler.ShowSprite(_performModel.ChMid, SpriteMode.Fading))
                .UnRegisterWhenGameObjectDestroyed(this);
            this.RegisterEvent<ChMidFadingHideEvent>(_=> _chMidHandler.HideSprite(SpriteMode.Fading))
                .UnRegisterWhenGameObjectDestroyed(this);
            this.RegisterEvent<ChMidImmediateShowEvent>(_=> _chMidHandler.ShowSprite(_performModel.ChMid, SpriteMode.Immediate))
                .UnRegisterWhenGameObjectDestroyed(this);
            this.RegisterEvent<ChMidImmediateHideEvent>(_=> _chMidHandler.HideSprite(SpriteMode.Immediate))
                .UnRegisterWhenGameObjectDestroyed(this);
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }

    }
}