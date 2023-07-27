using System.Collections;
using UnityEngine;
using TMPro;

namespace VNFramework
{
    public class DialogueViewController : MonoBehaviour, IController
    {
        // Dialogue View
        private bool _dialogueViewActive = true;
        private GameObject _curDialogueBox;
        private TMP_Text _curDialogueBoxText;

        private bool _isAnimating = false;
        private bool _needAnimation = true;
        private Coroutine _animationCoroutine;
        private float _textSpeed;
        private string _curDialogue;
        private int _curDialogueIndex;

        // Name Box
        private GameObject _normNameBox;
        private TMP_Text _normNameText;

        // Normal Dialogue Box
        private GameObject _normDialogueBox;
        private TMP_Text _normDialogueBoxText;

        // Full Dialogue Box
        private GameObject _fullDialogueBox;
        private TMP_Text _fullDialogueBoxText;

        // Dialogue Model
        private DialogueModel _dialogueModel;

        private void Start()
        {
            _normDialogueBox = transform.Find("NormDialogueBox").gameObject;
            _fullDialogueBox = transform.Find("FullDialogueBox").gameObject;
            _normDialogueBoxText = _normDialogueBox.transform.Find("DialogueBox/DialogueBoxText").GetComponent<TMP_Text>();
            _fullDialogueBoxText = _fullDialogueBox.transform.Find("DialogueBoxText").GetComponent<TMP_Text>();

            _normNameBox = _normDialogueBox.transform.Find("NameBox").gameObject;
            _normNameText = _normNameBox.transform.Find("NameBoxText").GetComponent<TMP_Text>();

            _dialogueModel = this.GetModel<DialogueModel>();
            _textSpeed = this.GetModel<ConfigModel>().TextSpeed;

            this.RegisterEvent<ConfigChangedEvent>(_ => _textSpeed = this.GetModel<ConfigModel>().TextSpeed).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<ShowDialoguePanelEvent>(_ => ShowDialogueView()).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<HideDialoguePanelEvent>(_ => HideDialogueView()).UnRegisterWhenGameObjectDestroyed(gameObject); ;
            this.RegisterEvent<ToggleDialoguePanelEvent>(_ => ToggleDialogueView()).UnRegisterWhenGameObjectDestroyed(gameObject); ;
            this.RegisterEvent<ChangeNameEvent>(_ => ChangeNameBox()).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<AppendDialogueEvent>(_ =>
            {
                _curDialogue = _dialogueModel.CurrentDialogue;
                ChangeDisplay();
            }).UnRegisterWhenGameObjectDestroyed(gameObject); ;

            this.RegisterEvent<ClearDialogueEvent>(_ =>
            {
                _curDialogue = "";
                _curDialogueIndex = 0;
                if (_curDialogueBoxText != null) _curDialogueBoxText.text = "";
            }).UnRegisterWhenGameObjectDestroyed(gameObject); ;

            this.RegisterEvent<AppendNewLineToDialogueEvent>(_ =>
            {
                _curDialogue = _dialogueModel.CurrentDialogue;
                _curDialogueIndex += 4;
                _curDialogueBoxText.text = _curDialogue;
            }).UnRegisterWhenGameObjectDestroyed(gameObject); ;

            this.RegisterEvent<StopDialogueAnimEvent>(_ =>
            {
                StopCharacterAnimation();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<OpenFullDialogueBoxEvent>(_ => OpenFullDialogueBox()).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<OpenNormDialogueBoxEvent>(_ => OpenNormDialogueBox()).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void OnDestroy()
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
                _animationCoroutine = null;
            }
        }

        # region Dialogue View Controller

        private void OpenNormDialogueBox()
        {
            HideFullDialogueBox();
            ShowNormDialogueBox();

            _curDialogueBox = _normDialogueBox;
            _curDialogueBoxText = _normDialogueBoxText;
            _dialogueModel.CurrentName = "";

            ChangeNameBox();
        }

        private void OpenFullDialogueBox()
        {
            HideNormDialogueBox();
            ShowFullDialogueBox();
            
            _curDialogueBox = _fullDialogueBox;
            _curDialogueBoxText = _fullDialogueBoxText;
            _dialogueModel.CurrentName = "";

            ChangeNameBox();
        }

        private void ShowNormDialogueBox()
        {
            _normDialogueBox.SetActive(true);
        }

        private void HideNormDialogueBox()
        {
            _normDialogueBox.SetActive(false);
        }
        private void ShowFullDialogueBox()
        {
            _fullDialogueBox.SetActive(true);
        }

        private void HideFullDialogueBox()
        {
            _fullDialogueBox.SetActive(false);
        }


        private void SetDialogueViewActive(bool active)
        {
            _curDialogueBox.SetActive(active);
        }

        private void ShowDialogueView()
        {
            _dialogueViewActive = true;
            SetDialogueViewActive(_dialogueViewActive);
        }

        private void HideDialogueView()
        {
            _dialogueViewActive = false;
            if (_isAnimating) StopCharacterAnimation();

            SetDialogueViewActive(_dialogueViewActive);
        }

        private void ToggleDialogueView()
        {
            _dialogueViewActive = !_dialogueViewActive;

            if (_dialogueViewActive) ShowDialogueView();
            else HideDialogueView();
        }

        # endregion

        # region NameBox

        private void ChangeNameBox()
        {
            if (_dialogueModel.CurrentName == "")
            {
                _normNameText.text = "";
                _normNameBox.SetActive(false);
            }
            else
            {
                _normNameBox.SetActive(true);
                _normNameText.text = _dialogueModel.CurrentName;
            }
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
                _curDialogueBoxText.text = _curDialogue;
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
            _curDialogueBoxText.text = _curDialogue;
            _curDialogueIndex = _curDialogue.Length;
        }

        private IEnumerator CharacterAnimation()
        {
            while (_curDialogueIndex < _curDialogue.Length)
            {
                _curDialogueBoxText.text += _curDialogue[_curDialogueIndex];
                _curDialogueIndex++;
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