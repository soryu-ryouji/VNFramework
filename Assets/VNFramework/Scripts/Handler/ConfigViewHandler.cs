using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VNFramework
{
    public class ConfigViewHandler : MonoBehaviour
    {
        public Scrollbar _bgmVolume;
        public Scrollbar _bgsVolume;
        public Scrollbar _chsVolume;
        public Scrollbar _gmsVolume;

        public Button highButton;
        public Button mediumButton;
        public Button lowButton;

        private void Awake()
        {
            Debug.Log("Config View Awake");
            _bgmVolume.value = ConfigController.BgmVolume;
            _bgsVolume.value = ConfigController.BgsVolume;
            _chsVolume.value = ConfigController.ChsVolume;
            _gmsVolume.value = ConfigController.GmsVolume;

            _bgmVolume.onValueChanged.AddListener(OnBgmScrollbarValueChanged);
            _bgsVolume.onValueChanged.AddListener(OnBgsScrollbarValueChanged);
            _chsVolume.onValueChanged.AddListener(OnChsScrollbarValueChanged);
            _gmsVolume.onValueChanged.AddListener(OnGmsScrollbarValueChanged);

            highButton.onClick.AddListener(SetHightTextSpeed);
            mediumButton.onClick.AddListener(SetMediumTextSpeed);
            lowButton.onClick.AddListener(SetLowTextSpeed);

        }

        private void Ondestory()
        {

            _bgmVolume.onValueChanged.RemoveListener(OnBgmScrollbarValueChanged);
            _bgsVolume.onValueChanged.RemoveListener(OnBgsScrollbarValueChanged);
            _chsVolume.onValueChanged.RemoveListener(OnChsScrollbarValueChanged);
            _gmsVolume.onValueChanged.RemoveListener(OnGmsScrollbarValueChanged);

            highButton.onClick.RemoveListener(SetHightTextSpeed);
            mediumButton.onClick.RemoveListener(SetMediumTextSpeed);
            lowButton.onClick.RemoveListener(SetLowTextSpeed);
        }
        private void OnBgmScrollbarValueChanged(float value)
        {
            ConfigController.SetBgmVolume(value);
        }
        private void OnBgsScrollbarValueChanged(float value)
        {
            ConfigController.SetBgsVolume(value);
        }
        private void OnChsScrollbarValueChanged(float value)
        {
            ConfigController.SetChsVolume(value);
        }
        private void OnGmsScrollbarValueChanged(float value)
        {
            ConfigController.SetGmsVolume(value);
        }

        public void SetHightTextSpeed()
        {
            _characterDisplayDuration = 0.04f;
            StartCharacterAnimation();
        }

        public void SetMediumTextSpeed()
        {
            _characterDisplayDuration = 0.08f;
            StartCharacterAnimation();
        }

        public void SetLowTextSpeed()
        {
            _characterDisplayDuration = 0.12f;
            StartCharacterAnimation();
        }

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
    }
}