using System.Collections;
using UnityEngine;
using TMPro;

namespace VNFramework
{
    public class DialogueBoxController : MonoBehaviour
    {
        public bool needAnimation = true;
        private bool _isAnimating = false;
        private Coroutine _animationCoroutine;

        private float _characterDisplayDuration = 0.1f;

        public TMP_Text dialogueTextBox;
        private string _currentDialogue;
        private int _currentDialogueIndex;

        private void Awake()
        {
            GameState.DialogueChanged += OnDialogueChanged;
            GameState.IsDialogueTyping += IsDialogueTyping;
            GameState.DialogueStop += StopCharacterAnimation;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            GameState.DialogueChanged -= OnDialogueChanged;
            GameState.IsDialogueTyping -= IsDialogueTyping;
            GameState.DialogueStop -= StopCharacterAnimation;
        }

        private bool IsDialogueTyping()
        {
            return _isAnimating;
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
                _currentDialogueIndex += 4;
                dialogueTextBox.text = _currentDialogue;
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