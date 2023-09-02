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

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int i = 0; i <  _gameSaves.Length; i++)
            {
                sb.Append(
@$"<|\
    save_index: {i}
    save_date: {_gameSaves[i].SaveDate}
    mermaid_node: {_gameSaves[i].MermaidNode}
    script_index: {_gameSaves[i].VNScriptIndex}
    resume_pic: {_gameSaves[i].ResumePic}
    resume_text: {_gameSaves[i].ResumeText}
|>");
            }

            return sb.ToString();
        }

        protected override void OnInit()
        {
            _gameSaves = this.GetUtility<GameDataStorage>().LoadGameSave();
            Debug.Log(ToString());
        }
    }
}