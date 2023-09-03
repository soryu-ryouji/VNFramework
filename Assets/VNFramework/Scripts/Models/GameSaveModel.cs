using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace VNFramework
{
    class GameSaveModel : AbstractModel
    {
        private GameSave[] _gameSaves;

        public GameSave[] GameSaves
        {
            get { return _gameSaves; }
        }

        public void SetGameSave(GameSave gameSave)
        {
            _gameSaves[gameSave.SaveIndex] = gameSave;
            this.GetUtility<GameDataStorage>().SaveGameSave();
        }

        public GameSave GetGameSave(int index)
        {
            return _gameSaves[index];
        }

        public void RemoveGameSave(int index)
        {
            _gameSaves[index] = new GameSave();
            this.GetUtility<GameDataStorage>().SaveGameSave();
        }

        public void InitModel()
        {
            _gameSaves = this.GetUtility<GameDataStorage>().LoadGameSave();
        }

        protected override void OnInit()
        {
        }
    }
}