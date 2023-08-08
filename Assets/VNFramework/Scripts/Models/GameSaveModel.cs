using System.Collections.Generic;

namespace VNFramework
{
    class GameSaveModel : AbstractModel
    {
        private GameSave[] _gameSaves;

        public GameSave[] GetGameSaves()
        {
            return _gameSaves;
        }

        protected override void OnInit()
        {
            _gameSaves = this.GetUtility<GameDataStorage>().LoadGameSave();
        }
    }
}