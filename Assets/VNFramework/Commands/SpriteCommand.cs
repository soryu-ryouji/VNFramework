namespace VNFramework
{
    class ShowSpriteCommand : AbstractCommand
    {
        private SpriteObj _spriteObj;
        private string _spriteName;
        private SpriteMode _spriteMode;

        public ShowSpriteCommand(SpriteObj spriteObj, string spriteName, SpriteMode spriteMode)
        {
            _spriteObj = spriteObj;
            _spriteName = spriteName;
            _spriteMode = spriteMode;
        }

        protected override void OnExecute()
        {
            if (_spriteObj == SpriteObj.Bgp)
            {
                GameState.BgpChanged(VNutils.Hash(
                    "action", SpriteAction.Show,
                    "sprite_name", _spriteName,
                    "mode", _spriteMode
                ));
            }

            else if (_spriteObj == SpriteObj.ChLeft)
            {
                GameState.ChlpChanged(VNutils.Hash(
                    "action", SpriteAction.Show,
                    "sprite_name", _spriteName,
                    "mode", _spriteMode
                ));
            }

            else if (_spriteObj == SpriteObj.ChMid)
            {
                GameState.ChmpChanged(VNutils.Hash(
                    "action", SpriteAction.Show,
                    "sprite_name", _spriteName,
                    "mode", _spriteMode
                ));
            }

            else if (_spriteObj == SpriteObj.ChRight)
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
        private SpriteObj _spriteObj;
        private SpriteMode _spriteMode;

        public HideSpriteCommand(SpriteObj spriteObj, SpriteMode spriteMode)
        {
            _spriteObj = spriteObj;
            _spriteMode = spriteMode;
        }

        protected override void OnExecute()
        {
            if (_spriteObj == SpriteObj.Bgp)
            {
                GameState.BgpChanged(VNutils.Hash(
                    "action", SpriteAction.Hide,
                    "mode", _spriteMode
                ));
            }

            else if (_spriteObj == SpriteObj.ChLeft)
            {
                GameState.ChlpChanged(VNutils.Hash(
                    "action", SpriteAction.Hide,
                    "mode", _spriteMode
                ));
            }

            else if (_spriteObj == SpriteObj.ChMid)
            {
                GameState.ChmpChanged(VNutils.Hash(
                    "action", SpriteAction.Hide,
                    "mode", _spriteMode
                ));
            }

            else if (_spriteObj == SpriteObj.ChRight)
            {
                GameState.ChrpChanged(VNutils.Hash(
                    "action", SpriteAction.Hide,
                    "mode", _spriteMode
                ));
            }

        }
    }
}