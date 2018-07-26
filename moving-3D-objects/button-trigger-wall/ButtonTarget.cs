using UnityEngine;

/**
 * Represents some 3D object that will translate to some other location when it
 * is open versus closed.
 */
namespace Bitzawolf
{
    public class ButtonTarget : MonoBehaviour
    {
        [Tooltip("The translation cannot be changed dynamically. It is set on load")]
        public Vector3 translation;

        [Tooltip("In Seconds")]
        public float timeToOpen = 1;

        public AnimationCurve easing;

        private Vector3 orignalPos;
        private Vector3 finalPos;
        private Vector3 openDir;
        private float distance;
        private float timeElapsed;
        private bool opening = false;
        private bool closing = false;

        private void Start()
        {
            orignalPos = transform.position;
            finalPos = transform.position + translation;
            openDir = finalPos - orignalPos;
            distance = openDir.magnitude;
            timeElapsed = 0;
        }

        public void open()
        {
            if (opening)
                return;

            Debug.Log("Opening");
            opening = true;
            closing = false;
        }

        public void close()
        {
            if (closing)
                return;

            Debug.Log("Closing");
            closing = true;
            opening = false;
        }

        private void FixedUpdate()
        {
            if (opening)
            {
                timeElapsed += Time.fixedDeltaTime;
                float percentage = timeElapsed / timeToOpen;
                if (percentage > 1)
                {
                    percentage = 1;
                    opening = false;
                }
                float easingAmt = easing.Evaluate(percentage);
                transform.position = orignalPos + openDir * (distance * easingAmt);
            }
            else if (closing)
            {
                timeElapsed -= Time.fixedDeltaTime;
                float percentage = timeElapsed / timeToOpen;
                if (percentage < 0)
                {
                    percentage = 0;
                    closing = false;
                }
                float easingAmt = easing.Evaluate(percentage);
                transform.position = orignalPos + openDir * (distance * easingAmt);
            }
        }
    }
}