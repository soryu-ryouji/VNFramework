using System.Collections;
using UnityEngine;

namespace VNFramework
{
    public class SpriteHandler : MonoBehaviour, ICanGetUtility
    {
        public float fadeDuration = 0.5f;
        private readonly float _staticAlpha = 1.0f;
        private readonly float _intermediateAlpha = 0.0f;
        protected SpriteRenderer _image;

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
        }
        private Coroutine fadingCoroutine;

        public void OnSpriteChanged(Hashtable hash)
        {
            var action = (SpriteAction)hash["action"];
            var mode = (string)hash["mode"];

            if (action == SpriteAction.Show)
            {
                if (mode == "fading") FadingChangeSprite((string)hash["sprite_name"]);
                else if (mode == "immediate") ImmediatelyShowSprite((string)hash["sprite_name"]);
            }
            else if (action == SpriteAction.Hide)
            {
                if (mode == "fading") FadingHideSprite();
                else if (mode == "immediate") ImmediatelyHideSprite();
            }
        }

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
                "onupdate", nameof(SetAlpha)
            ));
        }

        /// <summary>
        /// 渐变隐藏当前 sprite，并渐变将当前 sprite 显示
        /// </summary>
        /// <param name="spriteName"></param>
        /// 

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

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}