using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace VNFramework
{
    public class SpriteHandler : MonoBehaviour, ICanGetUtility
    {
        public float fadeDuration = 0.5f;
        private readonly float staticAlpha = 1.0f;
        private readonly float intermediateAlpha = 0.0f;
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
        }

        private Coroutine fadingCoroutine;

        public void OnSpriteChanged(Hashtable hash)
        {
            var action = (string)hash["action"];
            var mode = (string)hash["mode"];
            if (action == "set")
            {
                if (mode == "fading")
                {
                    FadingChangeSprite((string)hash["sprite_name"]);
                }
                else if (mode == "immediate")
                {
                    ShowSprite((string)hash["sprite_name"]);
                }
            }
            else if (action == "hide")
            {
                if (mode == "fading")
                {
                    FadingHideSprite();
                }
                else if (mode == "immediate")
                {
                    HideSprite();
                }
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
        public void ShowSprite(string spriteName)
        {
            var sprite = this.GetUtility<GameDataStorage>().LoadSprite(spriteName);
            if (sprite != null)
            {
                _image.sprite = sprite;
            }

            // 如果图像的 alpha 为透明状态，则重新将其显示
            if (_image.color.a != staticAlpha)
            {
                var color = _image.color;
                color.a = staticAlpha;
                _image.color = color;
            }
        }

        /// <summary>
        /// 将图像设置为隐藏（Alpha设置为0）
        /// </summary>
        public void HideSprite()
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
                Fade(fromAlpha: _image.color.a, toAlpha: intermediateAlpha, fadeDuration);
                yield return new WaitForSeconds(fadeDuration);
            }
            // 将 sprite 赋值给图像框
            if (sprite != null) _image.sprite = sprite;

            if (_image.color.a != staticAlpha)
            {
                Fade(fromAlpha: intermediateAlpha, toAlpha: staticAlpha, fadeDuration);
            }
        }

        private IEnumerator FadingHide()
        {
            // 当 image 的 alpha 不为 0 时，将其渐变归零
            if (_image.color.a != 0)
            {
                Fade(fromAlpha: _image.color.a, toAlpha: intermediateAlpha, fadeDuration);
                yield return new WaitForSeconds(fadeDuration);
            }
        }

        private IEnumerator FadingChange(Sprite sprite)
        {
            if (_image.sprite != null)
            {
                Fade(staticAlpha, intermediateAlpha, fadeDuration);
                yield return new WaitForSeconds(fadeDuration);
            }

            _image.sprite = sprite;

            if (sprite != null)
            {
                Fade(intermediateAlpha, staticAlpha, fadeDuration);
            }
            else
            {
                SetAlpha(staticAlpha);
            }
        }

        public IArchitecture GetArchitecture()
        {
            return VNFrameworkProj.Interface;
        }
    }
}