using UnityEngine;

namespace VNFramework
{
    public class DialogueViewController : MonoBehaviour, IController
    {
        private bool _viewActive = false;
        private bool _isFullView = false;

        private TextBox _currentDialogTextBox;
        private TextBoxHandler _normNameTextBoxHandler;
        private TextBoxHandler _normDialogTextBoxHandler;
        private TextBoxHandler _fullDialogTextBoxHandler;

        private DialogModel _dialogModel;

        private void Start()
        {
            InitDialogView();
            RegisterEvent();
            _dialogModel = this.GetModel<DialogModel>();
            var projectModel = this.GetModel<ProjectModel>();

            _normNameTextBoxHandler.img.sprite = this.GetUtility<GameDataStorage>().LoadSprite(projectModel.NormNameBoxPic);
            _normDialogTextBoxHandler.img.sprite = this.GetUtility<GameDataStorage>().LoadSprite(projectModel.NormDialogueBoxPic);
            _fullDialogTextBoxHandler.img.sprite = this.GetUtility<GameDataStorage>().LoadSprite(projectModel.FullDialogueBoxPic);

            ShowNormView();
        }

        private void InitDialogView()
        {
            _normDialogTextBoxHandler = transform.Find("NormView/DialogBox").GetComponent<TextBoxHandler>();
            _normNameTextBoxHandler = transform.Find("NormView/NameBox").GetComponent<TextBoxHandler>();
            _fullDialogTextBoxHandler = transform.Find("FullView/DialogBox").GetComponent<TextBoxHandler>();

            _fullDialogTextBoxHandler.Hide();
            _normDialogTextBoxHandler.Hide();
            _normNameTextBoxHandler.Hide();
        }

        private void RegisterEvent()
        {
            this.RegisterEvent<ShowDialogPanelEvent>(_ => ShowView()).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<HideDialogPanelEvent>(_ => HideView()).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<ToggleDialogPanelEvent>(_ => ToggleView()).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<ChangeNameEvent>(_ => ChangeNameBox()).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<AppendDialogEvent>(_ => _currentDialogTextBox.AppendText(_dialogModel.CurrentDialogue)).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<ClearDialogEvent>(_ => _currentDialogTextBox.Text = "").UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<AppendNewLineToDialogEvent>(_ => _currentDialogTextBox.Text += "<br>").UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<StopDialogueAnimEvent>(_ => _currentDialogTextBox.StopCharAnimation()).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<OpenFullDialogViewEvent>(_ =>
            {
                Debug.Log("Dialogue Controller: Open Full Dialogue Box");
                ShowFullView();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<OpenNormDialogViewEvent>(_ =>
            {
                Debug.Log("Dialogue Controller: Open Norm Dialogue Box");
                ShowNormView();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void ShowNormView()
        {
            _isFullView = false;
            _viewActive = true;

            _fullDialogTextBoxHandler.Hide();
            _normDialogTextBoxHandler.Show();
            _normNameTextBoxHandler.Show();

            _currentDialogTextBox = _normDialogTextBoxHandler.textBox;
            _dialogModel.CurrentName = "";

            ChangeNameBox();
        }

        private void HideNormView()
        {
            _viewActive = false;
            _normDialogTextBoxHandler.Hide();
            _normNameTextBoxHandler.Hide();
        }

        private void ShowFullView()
        {
            _isFullView = true;
            _viewActive = true;

            _normDialogTextBoxHandler.Hide();
            _normNameTextBoxHandler.Hide();
            _fullDialogTextBoxHandler.Show();

            _dialogModel.CurrentName = "";

            ChangeNameBox();
        }

        private void HideFullView()
        {
            _viewActive = false;
            _fullDialogTextBoxHandler.Hide();
        }

        private void ShowView()
        {
            _viewActive = true;

            if (_isFullView) ShowFullView();
            else ShowNormView();
        }

        private void HideView()
        {
            _viewActive = false;

            if (_isFullView) HideFullView();
            else HideNormView();
        }

        private void ToggleView()
        {
            if (_viewActive) ShowView();
            else HideView();
        }

        private void ChangeNameBox()
        {
            if (_dialogModel.CurrentName == "")
            {
                _normNameTextBoxHandler.textBox.Text = "";
                _normNameTextBoxHandler.Hide();
            }
            else
            {
                _normNameTextBoxHandler.Show();
                _normNameTextBoxHandler.textBox.Text = _dialogModel.CurrentName;
            }
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}