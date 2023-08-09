using System.Collections.Generic;
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

        public void SetGameSave(int index, GameSave gameSave)
        {
            _gameSaves[index] = gameSave;
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

        protected override void OnInit()
        {
            _gameSaves = this.GetUtility<GameDataStorage>().LoadGameSave();
            Debug.Log("Game Save Model Loaded");
        }
    }
}