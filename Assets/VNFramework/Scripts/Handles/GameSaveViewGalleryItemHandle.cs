using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

namespace VNFramework
{
    public class GameSaveViewGalleryItemHandle : MonoBehaviour, IController
    {
        public int Index { get; set; }
        public Button btn;
        private Image _gameGalleryImage;
        private Image _resumePic;
        private TMP_Text _resumeText;
        private TMP_Text _mermaidName;
        private TMP_Text _dateText;

        private void Awake()
        {
            btn = this.GetComponent<Button>();
            _gameGalleryImage = this.GetComponent<Image>();
            _resumePic = transform.Find("ResumePic").GetComponent<Image>();
            _resumeText = transform.Find("ResumeText").GetComponent<TMP_Text>();
            _mermaidName = transform.Find("MermaidName").GetComponent<TMP_Text>();
            _dateText = transform.Find("SaveDate").GetComponent<TMP_Text>();

            _gameGalleryImage.sprite = this.GetUtility<GameDataStorage>().LoadSprite(this.GetModel<ProjectModel>().GameSaveViewGalleryItemPic);
        }

        public void SetGameSaveItem(GameSave gameSave)
        {
            if (gameSave == null || gameSave.SaveDate == null)
            {
                _resumePic.sprite = null;
                _resumeText.text = "";
                _mermaidName.text = "";
                _dateText.text = "";
            }
            else
            {
                var tool = this.GetUtility<GameDataStorage>();
                if (!string.IsNullOrWhiteSpace(gameSave.ResumePic))
                {
                    _resumePic.sprite = tool.LoadSprite(gameSave.ResumePic);
                }
                _resumeText.text = gameSave.ResumeText;
                _mermaidName.text = gameSave.MermaidNode;
                _dateText.text = gameSave.SaveDate;
            }
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}
