using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VNFramework
{
    public class SaveFileItemHandle : MonoBehaviour, IController
    {
        private Button _saveFileButton;
        private Image _resumePic;
        private TMP_Text _resumeText;
        private TMP_Text _mermaidName;
        private TMP_Text _dateText;

        private void Awake()
        {
            _saveFileButton = this.GetComponent<Button>();
            _resumePic = transform.Find("ResumePic").GetComponent<Image>();
            _resumeText = transform.Find("ResumeText").GetComponent<TMP_Text>();
            _mermaidName = transform.Find("MermaidName").GetComponent<TMP_Text>();
            _dateText = transform.Find("SaveDate").GetComponent<TMP_Text>();
        }

        public void SetSaveFileItem(SaveFile saveFile)
        {
            if (saveFile.SaveDate == null)
            {
                _resumePic.sprite = null;
                _resumeText.text = "";
                _mermaidName.text = "";
                _dateText.text = "";
            }
            else
            {
                var tool = this.GetUtility<GameDataStorage>();
                _resumePic.sprite = tool.LoadSprite(saveFile.ResumePic);
                _resumeText.text = saveFile.ResumeText;
                _mermaidName.text = saveFile.MermaidNode;
                _dateText.text = saveFile.SaveDate;
            }
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}
