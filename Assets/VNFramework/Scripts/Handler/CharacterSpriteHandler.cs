using System.Collections;
using UnityEngine;

namespace VNFramework
{
    public class CharacterSpriteHandler : MonoBehaviour
    {
        private float moveSpeed = 0.5f; // 平移的速度
        private int loopCount = 4; // 循环次数

        private Vector3 originalPosition; // 初始位置

        private float shakeDistance = 3f; // 抖动的距离
        private float shakeSpeed = 1f; // 抖动的速度
        private int shakeCount = 1; // 抖动的次数
        private int currentShakeCount = 0;

        public float fadeDuration = 0.5f;
        private readonly float staticAlpha = 1.0f;
        private readonly float intermediateAlpha = 0.0f;

        private SpriteRenderer image;

        public string SpriteName
        {
            get { return image.sprite.name; }
        }

        public Color SpriteColor
        {
            get { return image.color; }
        }

        private void Awake()
        {
            image = GetComponent<SpriteRenderer>();
            originalPosition = transform.position;
        }

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
            else if (action == "shake")
            {
                Shake();
            }
            else if (action == "move_lf")
            {
                Move();
            }
        }

        private Coroutine fadingCoroutine;

        /// <summary>
        /// 该函数需要配合Fade函数使用
        /// </summary>
        /// <param name="newAlpha"></param>
        private void SetAlpha(float newAlpha)
        {
            var oldColor = image.color;
            oldColor.a = newAlpha;
            image.color = oldColor;
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
            var sprite = AssetsManager.LoadSprite(spriteName);
            fadingCoroutine = StartCoroutine(FadingDisplay(sprite));
        }

        public void FadingHideSprite()
        {
            fadingCoroutine = StartCoroutine(FadingHide());
        }

        public void FadingChangeSprite(string spriteName)
        {
            var sprite = AssetsManager.LoadSprite(spriteName);
            fadingCoroutine = StartCoroutine(FadingChange(sprite));
        }

        /// <summary>
        /// 直接显示 sprite，若给定的 Sprite 为空，则显示透明图像
        /// </summary>
        /// <param name="spriteName"></param>
        public void ShowSprite(string spriteName)
        {
            var sprite = AssetsManager.LoadSprite(spriteName);
            if (sprite != null)
            {
                image.sprite = sprite;
            }

            // 如果图像的 alpha 为透明状态，则重新将其显示
            if (image.color.a != staticAlpha)
            {
                var color = image.color;
                color.a = staticAlpha;
                image.color = color;
            }
        }

        /// <summary>
        /// 将图像设置为隐藏（Alpha设置为0）
        /// </summary>
        public void HideSprite()
        {
            var color = image.color;
            color.a = 0;
            image.color = color;
        }

        private IEnumerator FadingDisplay(Sprite sprite)
        {
            // 当 sprite 的 alpha 值不为0时，先将alpha值过度到0
            if (image.sprite != null)
            {
                Fade(fromAlpha: image.color.a, toAlpha: intermediateAlpha, fadeDuration);
                yield return new WaitForSeconds(fadeDuration);
            }
            // 将 sprite 赋值给图像框
            if (sprite != null) image.sprite = sprite;

            if (image.color.a != staticAlpha)
            {
                Fade(fromAlpha: intermediateAlpha, toAlpha: staticAlpha, fadeDuration);
            }
        }

        private IEnumerator FadingHide()
        {
            // 当 image 的 alpha 不为 0 时，将其渐变归零
            if (image.color.a != 0)
            {
                Fade(fromAlpha: image.color.a, toAlpha: intermediateAlpha, fadeDuration);
                yield return new WaitForSeconds(fadeDuration);
            }
        }

        private IEnumerator FadingChange(Sprite sprite)
        {
            if (image.sprite != null)
            {
                Fade(staticAlpha, intermediateAlpha, fadeDuration);
                yield return new WaitForSeconds(fadeDuration);
            }

            image.sprite = sprite;

            if (sprite != null)
            {
                Fade(intermediateAlpha, staticAlpha, fadeDuration);
            }
            else
            {
                SetAlpha(staticAlpha);
            }
        }

        //private IEnumerator ShakeImage(int count = 2)
        //{

        //}

        private void PerformShake()
        {
            iTween.ShakeRotation(gameObject, iTween.Hash(
                "amount", new Vector3(0f, 0f, shakeDistance),
                "speed", shakeSpeed,
                "time", 0.5f,
                "oncomplete", "OnShakeComplete",
                "oncompletetarget", gameObject
            ));
        }

        private void OnShakeComplete()
        {
            currentShakeCount++;
            if (currentShakeCount < shakeCount)
            {
                PerformShake();
            }
            else
            {
                currentShakeCount = 0;
            }
        }

        public void Shake()
        {
            PerformShake();
        }

        private IEnumerator MoveLeftAndRight(int count,float moveDistance = 0.5f)
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
                        "time",moveSpeed,
                        "looptype", iTween.LoopType.none,
                        "oncompletetarget", gameObject));
                }
                else
                {
                    // 动画完成后回到原始位置
                    iTween.MoveTo(gameObject, iTween.Hash("position", originalPosition, "time", moveSpeed, "oncomplete", "OnAnimationComplete", "oncompletetarget", gameObject));
                }

                yield return new WaitForSeconds(0.1f);
            }
        }

        public void Move()
        {
            StartCoroutine(MoveLeftAndRight(loopCount));
        }
    }
}