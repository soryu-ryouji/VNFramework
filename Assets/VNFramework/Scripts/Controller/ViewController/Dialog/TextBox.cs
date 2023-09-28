using System.Collections;
using UnityEngine;
using TMPro;

namespace VNFramework
{
    public class TextBox : MonoBehaviour
    {
        private TMP_Text _textBox;
        private string _currentText = "";
        private int _currentTextIndex;
        private Coroutine _animationCoroutine;

        public string Text
        {
            get => _textBox.text;
            set
            {
                if (_animationCoroutine != null) StopCharAnimation();
                _currentText = value;
                _textBox.text = _currentText;
                _currentTextIndex = _currentText.Length;
            }
        }

        public float TextSpeed { get; set; } = 0.05f;
        public bool IsAnimating { get; private set; } = false;

        private void Awake()
        {
            _textBox = GetComponent<TMP_Text>();
            if (_textBox == null) Debug.LogError("DialogBox TMP Text is null");
        }

        public void AppendText(string text)
        {
            _currentText += text;
            StartCharAnimation();
        }

        public void ReType()
        {
            _currentTextIndex = 0;
            _textBox.text = "";
            StartCharAnimation();
        }

        private void StartCharAnimation()
        {
            if (_animationCoroutine != null) StopCharAnimation();

            _animationCoroutine = StartCoroutine(CharAnimation());
        }

        public void StopCharAnimation()
        {
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
                _animationCoroutine = null;
            }

            _textBox.text = _currentText;
            _currentTextIndex = _currentText.Length;

            IsAnimating = false;
        }

        private IEnumerator CharAnimation()
        {
            IsAnimating = true;

            while (_currentTextIndex < _currentText.Length)
            {
                _textBox.text += _currentText[_currentTextIndex];
                _currentTextIndex++;
                yield return new WaitForSeconds(TextSpeed);
            }

            IsAnimating = false;
        }

        private void OnDestroy()
        {
            StopCharAnimation();
            _animationCoroutine = null;
        }
    }
}