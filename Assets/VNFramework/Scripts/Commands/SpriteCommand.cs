using AssetBundleBrowser.AssetBundleModel;
using UnityEngine;

namespace VNFramework
{
    class ShowSpriteCommand : AbstractCommand
    {
        private AsmObj _spriteObj;
        private string _spriteName;
        private string _spriteMode;

        public ShowSpriteCommand(AsmObj spriteObj, string spriteName, string spriteMode)
        {
            _spriteObj = spriteObj;
            _spriteName = spriteName;
            _spriteMode = spriteMode;
        }

        protected override void OnExecute()
        {
            if (_spriteObj == AsmObj.bgp)
            {
                this.GetModel<PerformanceModel>().BgpName = _spriteName;

                if (_spriteMode == "fading") this.SendEvent<BgpFadingShowEvent>();
                else if (_spriteMode == "immediate") this.SendEvent<BgpImmediateShowEvent>();
                else this.SendEvent<BgpImmediateShowEvent>();
            }

            else if (_spriteObj == AsmObj.ch_left)
            {
                this.GetModel<PerformanceModel>().ChLeft = _spriteName;

                if (_spriteMode == "fading") this.SendEvent<ChLeftFadingShowEvent>();
                else if (_spriteMode == "immediate") this.SendEvent<ChLeftImmediateShowEvent>();
            }

            else if (_spriteObj == AsmObj.ch_mid)
            {
                this.GetModel<PerformanceModel>().ChMid = _spriteName;

                if (_spriteMode == "fading") this.SendEvent<ChMidFadingShowEvent>();
                else if (_spriteMode == "immediate") this.SendEvent<ChMidImmediateShowEvent>();
            }

            else if (_spriteObj == AsmObj.ch_right)
            {
                this.GetModel<PerformanceModel>().ChRight = _spriteName;

                if (_spriteMode == "fading") this.SendEvent<ChRightFadingShowEvent>();
                else if (_spriteMode == "immediate") this.SendEvent<ChRightImmediateShowEvent>();
            }
        }
    }

    class HideSpriteCommand : AbstractCommand
    {
        private AsmObj _spriteObj;
        private string _spriteMode;

        public HideSpriteCommand(AsmObj spriteObj, string spriteMode)
        {
            _spriteObj = spriteObj;
            _spriteMode = spriteMode;
        }

        protected override void OnExecute()
        {
            if (_spriteObj == AsmObj.bgp)
            {
                this.GetModel<PerformanceModel>();

                if (_spriteMode == "fading") this.SendEvent<BgpFadingHideEvent>();
                else if (_spriteMode == "immediate") this.SendEvent<BgpImmediateHideEvent>();
            }

            else if (_spriteObj == AsmObj.ch_left)
            {
                this.GetModel<PerformanceModel>().ChLeft = "";

                if (_spriteMode == "fading") this.SendEvent<ChLeftFadingHideEvent>();
                else if (_spriteMode == "immediate") this.SendEvent<ChLeftImmediateHideEvent>();
            }

            else if (_spriteObj == AsmObj.ch_mid)
            {
                this.GetModel<PerformanceModel>().ChMid = "";

                if (_spriteMode == "fading") this.SendEvent<ChMidFadingHideEvent>();
                else if (_spriteMode == "immediate") this.SendEvent<ChMidImmediateHideEvent>();
            }

            else if (_spriteObj == AsmObj.ch_right)
            {
                this.GetModel<PerformanceModel>().ChRight = "";

                if (_spriteMode == "fading") this.SendEvent<ChRightFadingHideEvent>();
                else if (_spriteMode == "immediate") this.SendEvent<ChRightImmediateHideEvent>();
            }
        }
    }
}