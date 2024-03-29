using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

namespace VNFramework
{
    public class ConfigViewController : MonoBehaviour, IController
    {
        public GameObject generalPanel;
        public GameObject volumePanel;
        public GameObject shortcutPanel;
        public GameObject commonPanel;

        public Button generalPanelBtn;
        public Button volumePanelBtn;
        public Button shortcutPanelBtn;

        public Toggle _chineseBtn;
        public Toggle _englishBtn;
        public Toggle _fullScreenBtn;
        public Slider _bgmVolume;
        public Slider _bgsVolume;
        public Slider _chsVolume;
        private Slider _gmsVolume;

        private Slider _textSpeed;

        // private Button _highBtn;
        // private Button _mediumBtn;
        // private Button _lowBtn;
        private Button _backBtn;

        private ConfigModel _configModel;

        private void Start()
        {
            _configModel = this.GetModel<ConfigModel>();
            
            generalPanel = transform.Find("ConfigPanel/Tabs/GeneralPanel").gameObject;
            volumePanel = transform.Find("ConfigPanel/Tabs/VolumePanel").gameObject;
            shortcutPanel = transform.Find("ConfigPanel/Tabs/ShortcutPanel").gameObject;
            commonPanel = transform.Find("ConfigPanel/Common").gameObject;

            generalPanelBtn = commonPanel.transform.Find("LeftPanel/General").GetComponent<Button>();
            volumePanelBtn = commonPanel.transform.Find("LeftPanel/Volume").GetComponent<Button>();
            shortcutPanelBtn = commonPanel.transform.Find("LeftPanel/Shortcut").GetComponent<Button>();
            _backBtn = commonPanel.transform.Find("RightPanel/Back").GetComponent<Button>();

            _chineseBtn = generalPanel.transform.Find("LeftPanel/Display/Options/Language/Chinese").GetComponent<Toggle>();
            _englishBtn = generalPanel.transform.Find("LeftPanel/Display/Options/Language/English").GetComponent<Toggle>();
            _fullScreenBtn = generalPanel.transform.Find("LeftPanel/Display/Options/FullScreen").GetComponent<Toggle>();
            _textSpeed = generalPanel.transform.Find("RightPanel/DialogBox/Options/TextSpeed/Slider").GetComponent<Slider>();
            dialogueTextBox = generalPanel.transform.Find("RightPanel/DialogBox/Options/TextPreviewPanel/Text").GetComponent<TMP_Text>();

            _bgmVolume = volumePanel.transform.Find("LeftPanel/Volume/Options/BgmVolume/Slider").GetComponent<Slider>();
            _bgsVolume = volumePanel.transform.Find("LeftPanel/Volume/Options/BgsVolume/Slider").GetComponent<Slider>();
            _chsVolume = volumePanel.transform.Find("LeftPanel/Volume/Options/ChsVolume/Slider").GetComponent<Slider>();
            _gmsVolume = volumePanel.transform.Find("LeftPanel/Volume/Options/GmsVolume/Slider").GetComponent<Slider>();


            _chineseBtn.onValueChanged.AddListener(_ => SwitchLanguage("Chinese"));
            _englishBtn.onValueChanged.AddListener(_ => SwitchLanguage("English"));
            _fullScreenBtn.onValueChanged.AddListener(_ => {
                if (_fullScreenBtn.isOn) this.SendCommand<SwitchToFullScreenCommand>();
                else this.SendCommand<SwitchToWindowCommand>();
            });

            SwitchLanguage(_configModel.Language);
            _bgmVolume.value = _configModel.BgmVolume;
            _bgsVolume.value = _configModel.BgsVolume;
            _chsVolume.value = _configModel.ChsVolume;
            _gmsVolume.value = _configModel.GmsVolume;
            _textSpeed.value = _configModel.TextSpeed * 10;
            _textSpeed.onValueChanged.AddListener(vaule => {
                var speed = vaule / 10;
                _characterDisplayDuration = speed;
                _configModel.TextSpeed = speed;
                if (!IsDialogueTyping()) StartCharacterAnimation();
            });

            _bgmVolume.onValueChanged.AddListener(value => _configModel.BgmVolume = value);
            _bgsVolume.onValueChanged.AddListener(value => _configModel.BgsVolume = value);
            _chsVolume.onValueChanged.AddListener(value => _configModel.ChsVolume = value);
            _gmsVolume.onValueChanged.AddListener(value => _configModel.GmsVolume = value);

            _backBtn.onClick.AddListener(() =>
            {
                this.SendCommand<SaveSystemConfigCommand>();
                this.SendCommand<HideConfigViewCommand>();
            });

            generalPanelBtn.onClick.AddListener(() => SwitchPanel(1));
            volumePanelBtn.onClick.AddListener(() => SwitchPanel(2));
            shortcutPanelBtn.onClick.AddListener(() => SwitchPanel(3));

            SwitchPanel(1);
            SwitchLanguage(_configModel.Language);
        }

        private void SwitchLanguage(string language)
        {
            if (language == "Chinese")
            {
                _chineseBtn.SetIsOnWithoutNotify(true);
                _configModel.Language = language;
            }
            else if (language == "English")
            {
                _englishBtn.SetIsOnWithoutNotify(true);
                _configModel.Language = language;
            }
        }

        private void SwitchPanel(int panelNum)
        {
            if (panelNum == 1)
            {
                generalPanel.SetActive(true);
                volumePanel.SetActive(false);
                shortcutPanel.SetActive(false);
            }
            else if (panelNum == 2)
            {
                generalPanel.SetActive(false);
                volumePanel.SetActive(true);
                shortcutPanel.SetActive(false);
            }
            else if (panelNum == 3)
            {
                generalPanel.SetActive(false);
                volumePanel.SetActive(false);
                shortcutPanel.SetActive(true);
            }
        }

        private void OnDestroy()
        {
            _bgmVolume.onValueChanged.RemoveAllListeners();
            _bgsVolume.onValueChanged.RemoveAllListeners();
            _chsVolume.onValueChanged.RemoveAllListeners();
            _gmsVolume.onValueChanged.RemoveAllListeners();

            // _highBtn.onClick.RemoveAllListeners();
            // _mediumBtn.onClick.RemoveAllListeners();
            // _lowBtn.onClick.RemoveAllListeners();
            _backBtn.onClick.RemoveAllListeners();
        }

        public void SetHightTextSpeed()
        {
            _characterDisplayDuration = 0.04f;
            _configModel.TextSpeed = 0.04f;
            StartCharacterAnimation();
        }

        public void SetMediumTextSpeed()
        {
            _characterDisplayDuration = 0.08f;
            _configModel.TextSpeed = 0.08f;
            StartCharacterAnimation();
        }

        public void SetLowTextSpeed()
        {
            _characterDisplayDuration = 0.12f;
            _configModel.TextSpeed = 0.12f;
            StartCharacterAnimation();
        }

        #region  TestDialogueBox

        public bool needAnimation = true;
        private bool _isAnimating = false;
        private float _characterDisplayDuration = 0.1f;
        public TMP_Text dialogueTextBox;
        private string _currentText;
        private int _currentDialogueIndex;

        private bool IsDialogueTyping()
        {
            return _isAnimating;
        }

        public void StartCharacterAnimation()
        {
            _currentText = this.GetModel<I18nModel>().__(dialogueTextBox.GetComponent<I18nText>().inflateTextKey);
            _isAnimating = true;
            StartCoroutine(CharacterAnimation());
        }

        private IEnumerator CharacterAnimation()
        {
            _currentDialogueIndex = 0;
            dialogueTextBox.text = "";

            while (_currentDialogueIndex < _currentText.Length)
            {
                dialogueTextBox.text += _currentText[_currentDialogueIndex];
                _currentDialogueIndex++;
                yield return new WaitForSeconds(_characterDisplayDuration);
            }

            // Animation stop
            _isAnimating = false;
        }

        #endregion

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}