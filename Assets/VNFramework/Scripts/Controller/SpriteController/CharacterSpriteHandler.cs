using System.Collections;
using UnityEngine;

namespace VNFramework
{
    public class CharacterSpriteHandler : MonoBehaviour, ICanGetUtility
    {
        private float _moveSpeed = 0.5f; // 平移的速度
        private int _loopCount = 4; // 循环次数

        private Vector3 _originalPosition; // 初始位置

        private float _shakeDistance = 3f; // 抖动的距离
        private float _shakeSpeed = 1f; // 抖动的速度
        private int _shakeCount = 1; // 抖动的次数
        private int _currentShakeCount = 0;

        public float fadeDuration = 0.5f;
        private readonly float _staticAlpha = 1.0f;
        private readonly float _intermediateAlpha = 0.0f;

        private SpriteRenderer _image;

        public string SpriteName
        {
            get { return _image.sprite.name; }
        }

        public Color SpriteColor
        {
            get { return _image.color; }
        }

        private void Awake()
        {
            _image = GetComponent<SpriteRenderer>();
            _originalPosition = transform.position;
        }

        public void ShowSprite(string spriteName, SpriteMode mode)
        {
            if (mode == SpriteMode.Fading) FadingChangeSprite(spriteName);
            else if (mode == SpriteMode.Immediate) ImmediatelyShowSprite(spriteName);
        }

        public void HideSprite(SpriteMode mode)
        {
            if (mode == SpriteMode.Fading) FadingHideSprite();
            else if (mode == SpriteMode.Immediate) ImmediatelyHideSprite();
        }

        public void ActSprite(SpriteAction action)
        {
            if (action == SpriteAction.Shake) Shake();
            else if (action == SpriteAction.MoveLR) Move();
        }


        private Coroutine fadingCoroutine;

        /// <summary>
        /// 该函数需要配合Fade函数使用
        /// </summary>
        /// <param name="newAlpha"></param>
        private void SetAlpha(float newAlpha)
        {
            var oldColor = _image.color;
            oldColor.a = newAlpha;
            _image.color = oldColor;
        }

        private void Fade(float fromAlpha, float toAlpha, float time)
        {
            iTween.ValueTo(gameObject, iTween.Hash(
                "from", fromAlpha,
                "to", toAlpha,
                "time", time,
                "easetype", "linear",
                "onupdate", "SetAlpha"
            ));
        }

        /// <summary>
        /// 渐变隐藏当前 sprite，并渐变将当前 sprite 显示
        /// </summary>
        /// <param name="spriteName"></param>
        public void FadingDisplaySprite(string spriteName)
        {
            var sprite = this.GetUtility<GameDataStorage>().LoadSprite(spriteName);
            fadingCoroutine = StartCoroutine(FadingDisplay(sprite));
        }

        public void FadingHideSprite()
        {
            fadingCoroutine = StartCoroutine(FadingHide());
        }

        public void FadingChangeSprite(string spriteName)
        {
            var sprite = this.GetUtility<GameDataStorage>().LoadSprite(spriteName);
            fadingCoroutine = StartCoroutine(FadingChange(sprite));
        }

        /// <summary>
        /// 直接显示 sprite，若给定的 Sprite 为空，则显示透明图像
        /// </summary>
        /// <param name="spriteName"></param>
        public void ImmediatelyShowSprite(string spriteName)
        {
            var sprite = this.GetUtility<GameDataStorage>().LoadSprite(spriteName);
            if (sprite != null)
            {
                _image.sprite = sprite;
            }

            // 如果图像的 alpha 为透明状态，则重新将其显示
            if (_image.color.a != _staticAlpha)
            {
                var color = _image.color;
                color.a = _staticAlpha;
                _image.color = color;
            }
        }

        /// <summary>
        /// 将图像设置为隐藏（Alpha设置为0）
        /// </summary>
        public void ImmediatelyHideSprite()
        {
            var color = _image.color;
            color.a = 0;
            _image.color = color;
        }

        private IEnumerator FadingDisplay(Sprite sprite)
        {
            // 当 sprite 的 alpha 值不为0时，先将alpha值过度到0
            if (_image.sprite != null)
            {
                Fade(fromAlpha: _image.color.a, toAlpha: _intermediateAlpha, fadeDuration);
                yield return new WaitForSeconds(fadeDuration);
            }
            // 将 sprite 赋值给图像框
            if (sprite != null) _image.sprite = sprite;

            if (_image.color.a != _staticAlpha)
            {
                Fade(fromAlpha: _intermediateAlpha, toAlpha: _staticAlpha, fadeDuration);
            }
        }

        private IEnumerator FadingHide()
        {
            // 当 image 的 alpha 不为 0 时，将其渐变归零
            if (_image.color.a != 0)
            {
                Fade(fromAlpha: _image.color.a, toAlpha: _intermediateAlpha, fadeDuration);
                yield return new WaitForSeconds(fadeDuration);
            }
        }

        private IEnumerator FadingChange(Sprite sprite)
        {
            if (_image.sprite != null)
            {
                Fade(_staticAlpha, _intermediateAlpha, fadeDuration);
                yield return new WaitForSeconds(fadeDuration);
            }

            _image.sprite = sprite;

            if (sprite != null)
            {
                Fade(_intermediateAlpha, _staticAlpha, fadeDuration);
            }
            else
            {
                SetAlpha(_staticAlpha);
            }
        }

        //private IEnumerator ShakeImage(int count = 2)
        //{

        //}

        private void PerformShake()
        {
            iTween.ShakeRotation(gameObject, iTween.Hash(
                "amount", new Vector3(0f, 0f, _shakeDistance),
                "speed", _shakeSpeed,
                "time", 0.5f,
                "oncomplete", "OnShakeComplete",
                "oncompletetarget", gameObject
            ));
        }

        private void OnShakeComplete()
        {
            _currentShakeCount++;
            if (_currentShakeCount < _shakeCount)
            {
                PerformShake();
            }
            else
            {
                _currentShakeCount = 0;
            }
        }

        public void Shake()
        {
            PerformShake();
        }

        private IEnumerator MoveLeftAndRight(int count, float moveDistance = 0.5f)
        {
            while (count > 0)
            {
                count--;
                if (count > 0)
                {
                    // 左右移动
                    moveDistance *= -1;
                    iTween.MoveBy(gameObject, iTween.Hash(
                        "x", moveDistance,
                        "time", _moveSpeed,
                        "looptype", iTween.LoopType.none,
                        "oncompletetarget", gameObject));
                }
                else
                {
                    // 动画完成后回到原始位置
                    iTween.MoveTo(gameObject, iTween.Hash("position", _originalPosition, "time", _moveSpeed, "oncomplete", "OnAnimationComplete", "oncompletetarget", gameObject));
                }

                yield return new WaitForSeconds(0.1f);
            }
        }

        public void Move()
        {
            StartCoroutine(MoveLeftAndRight(_loopCount));
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}