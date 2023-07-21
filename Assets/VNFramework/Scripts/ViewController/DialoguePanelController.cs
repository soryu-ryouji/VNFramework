using System.Collections;
using UnityEngine;
using TMPro;

namespace VNFramework
{
    public class DialoguePanelController : MonoBehaviour, IController
    {
        // Name Box
        private GameObject _nameBox;
        private TMP_Text _nameText;
        private string _currentName;

        // Dialogue Box
        private bool _isAnimating = false;
        private GameObject _dialogueBox;
        private TMP_Text _dialogueText;
        private bool _needAnimation = true;
        private Coroutine _animationCoroutine;
        private float _textSpeed;
        private string _currentDialogue;
        private int _currentDialogueIndex;

        // Dialogue Panel
        private bool _dialoguePanelActive = true;

        // Dialogue Model
        private DialogueModel _dialogueModel;

        private void Start()
        {
            _dialogueModel = this.GetModel<DialogueModel>();

            _nameBox = transform.Find("NameBox").gameObject;
            _nameText = transform.Find("NameBox/Text").GetComponent<TMP_Text>();
            _dialogueBox = transform.Find("DialogueBox").gameObject;
            _dialogueText = transform.Find("DialogueBox/Text").GetComponent<TMP_Text>();

            this.RegisterEvent<ShowDialoguePanelEvent>(_ => ShowDialoguePanel())
                .UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<HideDialoguePanelEvent>(_ => HideDialoguePanel())
                .UnRegisterWhenGameObjectDestroyed(gameObject);;
            this.RegisterEvent<ToggleDialoguePanelEvent>(_ => ToggleDialoguePanel())
                .UnRegisterWhenGameObjectDestroyed(gameObject);;

            _textSpeed = this.GetModel<ConfigModel>().TextSpeed;
            this.RegisterEvent<ConfigChangedEvent>(_ => _textSpeed = this.GetModel<ConfigModel>().TextSpeed)
                .UnRegisterWhenGameObjectDestroyed(gameObject);

            // NameBox
            this.RegisterEvent<ChangeNameEvent>(_ =>
            {
                if (_dialogueModel.CurrentName == "")
                {
                    _nameText.text = "";
                    _nameBox.SetActive(false);
                }
                else
                {
                    _nameBox.SetActive(true);
                    _nameText.text = _dialogueModel.CurrentName;
                }
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // Dialogue Box
            this.RegisterEvent<AppendDialogueEvent>(_ =>
            {
                _currentDialogue = _dialogueModel.CurrentDialogue;
                ChangeDisplay();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);;

            this.RegisterEvent<ClearDialogueEvent>(_ =>
            {
                _currentDialogue = "";
                _currentDialogueIndex = 0;
                _dialogueText.text = "";
            }).UnRegisterWhenGameObjectDestroyed(gameObject);;

            this.RegisterEvent<AppendNewLineToDialogueEvent>(_ =>
            {
                _currentDialogue = _dialogueModel.CurrentDialogue;
                _currentDialogueIndex += 4;
                _dialogueText.text = _currentDialogue;
            }).UnRegisterWhenGameObjectDestroyed(gameObject);;

            this.RegisterEvent<StopDialogueAnimEvent>(_ =>
            {
                StopCharacterAnimation();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);;
        }

        private void OnDestroy()
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
                _animationCoroutine = null;
            }
        }

        # region DialoguePanel Active

        private void SetDialoguePanelActive(bool active)
        {
            _dialogueBox.SetActive(active);
            _nameBox.SetActive(active);
        }

        private void ShowDialoguePanel()
        {
            _dialoguePanelActive = true;
            SetDialoguePanelActive(_dialoguePanelActive);
        }

        private void HideDialoguePanel()
        {
            _dialoguePanelActive = false;

            if (_isAnimating) StopCharacterAnimation();

            SetDialoguePanelActive(_dialoguePanelActive);
        }

        private void ToggleDialoguePanel()
        {
            _dialoguePanelActive = !_dialoguePanelActive;

            if (_dialoguePanelActive) ShowDialoguePanel();
            else HideDialoguePanel();
        }

        # endregion

        # region DialogueBox

        public bool IsAnimating
        {
            get { return _isAnimating; }
        }

        private void ChangeDisplay()
        {
            if (!_needAnimation)
            {
                _dialogueText.text = _currentDialogue;
                return;
            }

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
            _dialogueText.text = _currentDialogue;
            _currentDialogueIndex = _currentDialogue.Length;
        }

        private IEnumerator CharacterAnimation()
        {
            while (_currentDialogueIndex < _currentDialogue.Length)
            {
                _dialogueText.text += _currentDialogue[_currentDialogueIndex];
                _currentDialogueIndex++;
                yield return new WaitForSeconds(_textSpeed);
            }

            // Animation stop
            _isAnimating = false;
        }
        # endregion

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}