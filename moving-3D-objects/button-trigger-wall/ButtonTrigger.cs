using UnityEngine;

/**
 * Represents a space where a player can occupy and it will cause some 3D object(s)
 * to open/close depending on if the button is triggered.
 * 
 * Multiple buttons can be linked together where only one of the buttons is needed
 * to trigger the opening of the target objects.
 */
namespace Bitzawolf
{
    public class ButtonTrigger : MonoBehaviour
    {
        public ButtonTarget[] targets = null;
        public ButtonTrigger[] linkedButtons = null;

        private bool isPressed = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                isPressed = true;
                foreach(ButtonTarget target in targets)
                {
                    target.open();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                isPressed = false;
                bool doCloseDoors = true;
                foreach (ButtonTrigger butt in linkedButtons)
                {
                    if (butt.isPressed)
                    {
                        doCloseDoors = false;
                        break;
                    }
                }
                if (doCloseDoors)
                {
                    foreach (ButtonTarget target in targets)
                    {
                        target.close();
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.4f, 0.4f, 1.0f, 0.4f);
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }
}