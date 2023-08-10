using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VNFramework
{
    public class ConfigViewController : MonoBehaviour, IController
    {
        private Scrollbar _bgmVolumeScrollbar;
        private Scrollbar _bgsVolumeScrollbar;
        private Scrollbar _chsVolumeScrollbar;
        private Scrollbar _gmsVolumeScrollbar;

        private Button _highBtn;
        private Button _mediumBtn;
        private Button _lowBtn;
        private Button _backConfigViewBtn;

        private ConfigModel _configModel;

        private void Start()
        {
            _configModel = this.GetModel<ConfigModel>();

            _bgmVolumeScrollbar = transform.Find("AudioConfig/BgmVolume/Scrollbar").GetComponent<Scrollbar>();
            _bgsVolumeScrollbar = transform.Find("AudioConfig/BgsVolume/Scrollbar").GetComponent<Scrollbar>();
            _chsVolumeScrollbar = transform.Find("AudioConfig/ChsVolume/Scrollbar").GetComponent<Scrollbar>();
            _gmsVolumeScrollbar = transform.Find("AudioConfig/GmsVolume/Scrollbar").GetComponent<Scrollbar>();

            _highBtn = transform.Find("DialogueConfig/HighSpeedButton").GetComponent<Button>();
            _mediumBtn = transform.Find("DialogueConfig/MediumSpeedButton").GetComponent<Button>();
            _lowBtn = transform.Find("DialogueConfig/LowSpeedButton").GetComponent<Button>();
            _backConfigViewBtn = transform.Find("BackButton").GetComponent<Button>();

            _bgmVolumeScrollbar.value = _configModel.BgmVolume;
            _bgsVolumeScrollbar.value = _configModel.BgsVolume;
            _chsVolumeScrollbar.value = _configModel.ChsVolume;
            _gmsVolumeScrollbar.value = _configModel.GmsVolume;

            _bgmVolumeScrollbar.onValueChanged.AddListener(value => _configModel.BgmVolume = value);
            _bgsVolumeScrollbar.onValueChanged.AddListener(value => _configModel.BgsVolume = value);
            _chsVolumeScrollbar.onValueChanged.AddListener(value => _configModel.ChsVolume = value);
            _gmsVolumeScrollbar.onValueChanged.AddListener(value => _configModel.GmsVolume = value);

            _highBtn.onClick.AddListener(SetHightTextSpeed);
            _mediumBtn.onClick.AddListener(SetMediumTextSpeed);
            _lowBtn.onClick.AddListener(SetLowTextSpeed);
            _backConfigViewBtn.onClick.AddListener(() =>
            {
                this.SendCommand<SaveSystemConfigCommand>();
                this.SendCommand<HideConfigViewCommand>();
            });
        }

        private void OnDestroy()
        {
            _bgmVolumeScrollbar.onValueChanged.RemoveAllListeners();
            _bgsVolumeScrollbar.onValueChanged.RemoveAllListeners();
            _chsVolumeScrollbar.onValueChanged.RemoveAllListeners();
            _gmsVolumeScrollbar.onValueChanged.RemoveAllListeners();

            _highBtn.onClick.RemoveAllListeners();
            _mediumBtn.onClick.RemoveAllListeners();
            _lowBtn.onClick.RemoveAllListeners();
            _backConfigViewBtn.onClick.RemoveAllListeners();
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
        private Coroutine _animationCoroutine;

        private float _characterDisplayDuration = 0.1f;

        public TMP_Text dialogueTextBox;
        private string _currentDialogue = "这是一段测试文本";
        private int _currentDialogueIndex;

        private bool IsDialogueTyping()
        {
            return _isAnimating;
        }

        private void OnCharacterDisplayDurationChanged(float durationTime)
        {
            _characterDisplayDuration = durationTime;
        }

        private void OnDialogueChanged(Hashtable hash)
        {
            var action = (string)hash["action"];
            if (action == "append")
            {
                _currentDialogue += (string)hash["dialogue"];

                ChangeDisplay();
            }
            else if (action == "clear")
            {
                _currentDialogue = "";
                _currentDialogueIndex = 0;
                dialogueTextBox.text = "";
            }
            else if (action == "newline")
            {
                _currentDialogue += "<br>";
                ChangeDisplay();
            }
            else if (action == "text_speed")
            {
                _characterDisplayDuration = (float)hash["value"];
            }
        }

        private void ChangeDisplay()
        {
            if (!needAnimation)
            {
                dialogueTextBox.text = _currentDialogue;
                return;
            }

            // 需要动画
            if (_isAnimating)
            {
                StopCharacterAnimation();
            }

            StartCharacterAnimation();
        }

        public void StartCharacterAnimation()
        {
            _isAnimating = true;
            _animationCoroutine = StartCoroutine(CharacterAnimation());
        }

        private void StopCharacterAnimation()
        {
            if (!_isAnimating)
            {
                return;
            }

            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
                _animationCoroutine = null;
            }

            _isAnimating = false;
            dialogueTextBox.text = _currentDialogue;
            _currentDialogueIndex = _currentDialogue.Length;
        }

        private IEnumerator CharacterAnimation()
        {
            _currentDialogueIndex = 0;
            dialogueTextBox.text = "";

            while (_currentDialogueIndex < _currentDialogue.Length)
            {
                dialogueTextBox.text += _currentDialogue[_currentDialogueIndex];
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