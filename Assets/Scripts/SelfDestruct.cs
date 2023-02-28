using UnityEngine;

namespace BreakBricks2D
{
    public class SelfDestruct : MonoBehaviour
    {
        private PhysicalWorld physicalWorld;
        private PhysicalBody physicalBody;

        [SerializeField] private float timer = 7.5f; // seconds

        private void Awake()
        {
            physicalWorld = FindObjectOfType<PhysicalWorld>();
            physicalBody = GetComponent<PhysicalBody>();
        }

        void Update()
        {
            SelfDestructs();
        }

        private void SelfDestructs()
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                Destroy(gameObject); // destroy the object
                physicalWorld.BodyList.Remove(physicalBody); // remove the object from the list
            }
        }
    }
}