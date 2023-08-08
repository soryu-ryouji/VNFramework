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
                GameState.BgpChanged(VNutils.Hash(
                    "action", SpriteAction.Show,
                    "sprite_name", _spriteName,
                    "mode", _spriteMode
                ));
            }

            else if (_spriteObj == AsmObj.ch_left)
            {
                GameState.ChlpChanged(VNutils.Hash(
                    "action", SpriteAction.Show,
                    "sprite_name", _spriteName,
                    "mode", _spriteMode
                ));
            }

            else if (_spriteObj == AsmObj.ch_mid)
            {
                GameState.ChmpChanged(VNutils.Hash(
                    "action", SpriteAction.Show,
                    "sprite_name", _spriteName,
                    "mode", _spriteMode
                ));
            }

            else if (_spriteObj == AsmObj.ch_right)
            {
                GameState.ChrpChanged(VNutils.Hash(
                    "action", SpriteAction.Show,
                    "sprite_name", _spriteName,
                    "mode", _spriteMode
                ));
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
                GameState.BgpChanged(VNutils.Hash(
                    "action", SpriteAction.Hide,
                    "mode", _spriteMode
                ));
            }

            else if (_spriteObj == AsmObj.ch_left)
            {
                GameState.ChlpChanged(VNutils.Hash(
                    "action", SpriteAction.Hide,
                    "mode", _spriteMode
                ));
            }

            else if (_spriteObj == AsmObj.ch_mid)
            {
                GameState.ChmpChanged(VNutils.Hash(
                    "action", SpriteAction.Hide,
                    "mode", _spriteMode
                ));
            }

            else if (_spriteObj == AsmObj.ch_right)
            {
                GameState.ChrpChanged(VNutils.Hash(
                    "action", SpriteAction.Hide,
                    "mode", _spriteMode
                ));
            }

        }
    }
}