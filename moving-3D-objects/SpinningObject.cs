using UnityEngine;

/**
 * Rotates an object about the world's y axis (up) at some rate.
 */
namespace Bitzawolf
{
    public class SpinningObject : MonoBehaviour
    {
        [Range(0, 70)]
        [Tooltip("Degrees per second / 10")]
        public float rate = 10;

        private void FixedUpdate()
        {
            float rotation = rate * Time.fixedDeltaTime * 10;
            transform.Rotate(0, rotation, 0, Space.World);
        }
    }
}