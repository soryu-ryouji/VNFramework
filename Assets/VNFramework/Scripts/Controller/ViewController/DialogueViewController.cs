using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace VNFramework
{
    public class DialogueViewController : MonoBehaviour, IController
    {
        // Dialogue View
        private bool _dialogueViewActive = true;
        private GameObject _curDialogueBox;
        private TMP_Text _curDialogueBoxText;

        private Coroutine _animationCoroutine;
        private float _textSpeed;
        private string _curDialogue;
        private int _curDialogueIndex;

        // Normal Dialogue Box
        private GameObject _normDialogueBox;
        private GameObject _normNameBox;
        private TMP_Text _normDialogueBoxText;
        private Image _normDialogueBoxImage;
        private TMP_Text _normNameBoxText;
        private Image _normNameBoxImage;

        // Full Dialogue Box
        private GameObject _fullDialogueBox;
        private TMP_Text _fullDialogueBoxText;
        private Image _fullDialogueBoxImage;

        // Dialogue Model
        private DialogueModel _dialogueModel;

        private void Awake()
        {
            // 获取 View 组件
            _fullDialogueBox = transform.Find("FullDialogueBox").gameObject;
            _fullDialogueBoxText = _fullDialogueBox.transform.Find("DialogueBoxText").GetComponent<TMP_Text>();
            _fullDialogueBoxImage = _fullDialogueBox.transform.Find("DialogueBoxBgp").GetComponent<Image>();

            _normDialogueBox = transform.Find("NormDialogueBox").gameObject;
            _normNameBox = _normDialogueBox.transform.Find("NameBox").gameObject;
            _normNameBoxText = _normDialogueBox.transform.Find("NameBox/NameBoxText").GetComponent<TMP_Text>();
            _normNameBoxImage = _normDialogueBox.transform.Find("NameBox/NameBoxBgp").GetComponent<Image>();
            _normDialogueBoxText = _normDialogueBox.transform.Find("DialogueBox/DialogueBoxText").GetComponent<TMP_Text>();
            _normDialogueBoxImage = _normDialogueBox.transform.Find("DialogueBox/DialogueBoxBgp").GetComponent<Image>();

            var result = _fullDialogueBox != null && _fullDialogueBoxText != null && _fullDialogueBoxImage != null &&
                         _normDialogueBox != null && _normNameBox != null && _normNameBoxText != null && _normNameBoxImage != null &&
                         _normDialogueBoxText != null && _normDialogueBoxImage != null;

            if (!result) Debug.LogError("Dialogue View Component Init Failed");
            else Debug.Log("<color=green>Dialogue View Component Init Success</color>");

            // 对 DialogueView 进行初始化
            _dialogueModel = this.GetModel<DialogueModel>();
            _textSpeed = this.GetModel<ConfigModel>().TextSpeed;

            var projectModel = this.GetModel<ProjectModel>();
            _normNameBoxImage.sprite = this.GetUtility<GameDataStorage>().LoadSprite(projectModel.NormNameBoxPic);
            _normDialogueBoxImage.sprite = this.GetUtility<GameDataStorage>().LoadSprite(projectModel.NormDialogueBoxPic);
            _fullDialogueBoxImage.sprite = this.GetUtility<GameDataStorage>().LoadSprite(projectModel.FullDialogueBoxPic);

            // 注册 DialogueView 相关事件
            this.RegisterEvent<ConfigChangedEvent>(_ => UpdateTextSpeed()).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<ShowDialoguePanelEvent>(_ => ShowDialogueView()).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<HideDialoguePanelEvent>(_ => HideDialogueView()).UnRegisterWhenGameObjectDestroyed(gameObject); ;
            this.RegisterEvent<ToggleDialoguePanelEvent>(_ => ToggleDialogueView()).UnRegisterWhenGameObjectDestroyed(gameObject); ;
            this.RegisterEvent<ChangeNameEvent>(_ => ChangeNameBox()).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<AppendDialogueEvent>(_ => AppendDialogue()).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<ClearDialogueEvent>(_ => ClearDialogue()).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<AppendNewLineToDialogueEvent>(_ => AppendNewLineToDialogue()).UnRegisterWhenGameObjectDestroyed(gameObject); ;

            this.RegisterEvent<StopDialogueAnimEvent>(_ => StopCharacterAnimation()).UnRegisterWhenGameObjectDestroyed(gameObject);

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

        private void UpdateTextSpeed()
        {
            _textSpeed = this.GetModel<ConfigModel>().TextSpeed;
        }

        private void AppendDialogue()
        {
            _curDialogue = _dialogueModel.CurrentDialogue;
            ChangeDisplay();
        }

        private void ClearDialogue()
        {
            _curDialogue = "";
            _curDialogueIndex = 0;
            if (_curDialogueBoxText != null) _curDialogueBoxText.text = "";
        }

        private void AppendNewLineToDialogue()
        {
            _curDialogue = _dialogueModel.CurrentDialogue;
            _curDialogueIndex += 4;
            _curDialogueBoxText.text = _curDialogue;
        }

        # region Dialogue View Controller

        private void OpenNormDialogueBox()
        {
            Debug.Log("Dialogue Controller: Open Norm Dialogue Box");
            HideFullDialogueBox();
            ShowNormDialogueBox();

            _curDialogueBox = _normDialogueBox;
            _curDialogueBoxText = _normDialogueBoxText;
            _dialogueModel.CurrentName = "";

            ChangeNameBox();
        }

        private void OpenFullDialogueBox()
        {
            Debug.Log("Dialogue Controller: Open Full Dialogue Box");
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
            if (_dialogueModel.IsAnimating) StopCharacterAnimation();

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
                _normNameBoxText.text = "";
                _normNameBox.SetActive(false);
            }
            else
            {
                _normNameBox.SetActive(true);
                _normNameBoxText.text = _dialogueModel.CurrentName;
            }
        }
        # endregion

        # region DialogueBox

        private void ChangeDisplay()
        {
            if (!_dialogueModel.NeedAnimation)
            {
                _curDialogueBoxText.text = _curDialogue;
                return;
            }

            if (_dialogueModel.IsAnimating)
            {
                StopCharacterAnimation();
            }

            StartCharacterAnimation();
        }

        public void StartCharacterAnimation()
        {
            _dialogueModel.IsAnimating = true;
            _animationCoroutine = StartCoroutine(CharacterAnimation());
        }

        private void StopCharacterAnimation()
        {
            if (!_dialogueModel.IsAnimating)
            {
                return;
            }

            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
                _animationCoroutine = null;
            }

            _dialogueModel.IsAnimating = false;
            Debug.Log("Dialogue View is Animating : " + _dialogueModel.IsAnimating);
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
            _dialogueModel.IsAnimating = false;
        }
        # endregion

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}