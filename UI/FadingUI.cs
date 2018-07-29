using UnityEngine;
using UnityEngine.UI;

/**
 * Facilitates hiding/showing UI by fading out the component's color value
 */

namespace Bitzawolf
{
    public class FadingUI : MonoBehaviour
    {
        [Tooltip("This easing curve will be reversed so it's easier to work with")]
        public AnimationCurve easing;
        public float lengthTime = 5;
        public bool targetDescendents = true;
        public bool startFadedOut = false;

        private float timeElapsed = 0;
        private bool fadingOut = false;
        private bool fadingIn = false;

        private void Start()
        {
            if (startFadedOut)
            {
                timeElapsed = lengthTime;
                setAlpha(0);
            }
        }

        public bool isFadedOut()
        {
            return timeElapsed == lengthTime;
        }

        public void fadeOut()
        {
            if (isFadedOut())
            {
                fadingIn = true;
                fadingOut = false;
            }
        }

        public void fadeIn()
        {
            if (!isFadedOut())
            {
                fadingIn = false;
                fadingOut = true;
            }
        }

        void Update()
        {
            if (fadingIn)
            {
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= lengthTime)
                {
                    timeElapsed = lengthTime;
                    fadingIn = false;
                }
                float easingPos = timeElapsed / lengthTime;
                float alpha = 1 - easing.Evaluate(easingPos);
                setAlpha(alpha);
            }
            else if (fadingOut)
            {
                timeElapsed -= Time.deltaTime;
                if (timeElapsed <= 0)
                {
                    timeElapsed = 0;
                    fadingOut = false;
                }
                float easingPos = timeElapsed / lengthTime;
                float alpha = 1 - easing.Evaluate(easingPos);
                setAlpha(alpha);
            }
        }

        private void setAlpha(float alpha)
        {
            Image[] images = GetComponents<Image>();
            Toggle[] toggles = GetComponents<Toggle>();
            Text[] texts = GetComponents<Text>();

            foreach (Image img in images)
            {
                Color c = img.color;
                c.a = alpha;
                img.color = c;
            }

            foreach (Toggle tog in toggles)
            {
                ColorBlock cb = tog.colors;
                Color c = cb.normalColor;
                c.a = alpha;
                cb.normalColor = c;
                tog.colors = cb;
            }

            foreach (Text txt in texts)
            {
                Color c = txt.color;
                c.a = alpha;
                txt.color = c;
            }

            if (targetDescendents)
            {
                images = GetComponentsInChildren<Image>();
                toggles = GetComponentsInChildren<Toggle>();
                texts = GetComponentsInChildren<Text>();

                foreach (Image img in images)
                {
                    Color c = img.color;
                    c.a = alpha;
                    img.color = c;
                }

                foreach (Toggle tog in toggles)
                {
                    ColorBlock cb = tog.colors;
                    Color c = cb.normalColor;
                    c.a = alpha;
                    cb.normalColor = c;
                    tog.colors = cb;
                }

                foreach (Text txt in texts)
                {
                    Color c = txt.color;
                    c.a = alpha;
                    txt.color = c;
                }
            }
        }
    }
}