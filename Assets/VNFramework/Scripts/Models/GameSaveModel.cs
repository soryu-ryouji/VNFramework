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
                sb.Append("<|\n" +
    $"save_index: {i}\n" +
    $"save_date: {_gameSaves[i].SaveDate}\n" +
    $"mermaid_node: {_gameSaves[i].MermaidNode} \n" +
    $"script_index: {_gameSaves[i].VNScriptIndex} \n" +
    $"resume_pic: {_gameSaves[i].ResumePic} \n" +
    $"resume_text: {_gameSaves[i].ResumeText}\n" +
    $"|>\n");
            }

            return sb.ToString();
        }

        protected override void OnInit()
        {
            _gameSaves = this.GetUtility<GameDataStorage>().LoadGameSave();
        }
    }
}