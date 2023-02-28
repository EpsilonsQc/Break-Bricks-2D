using UnityEngine;

namespace BreakBricks2D
{
    public class DamageHandler : MonoBehaviour
    {
        // References
        private PhysicalWorld physicalWorld;
        private PhysicalBody physicalBody;

        [SerializeField] private int health = 1; // number of HP the object has
        private bool isTakingDamage = false;

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        private void Awake()
        {
            physicalWorld = FindObjectOfType<PhysicalWorld>();
            physicalBody = GetComponent<PhysicalBody>();
        }

        private void Update()
        {
            Die();
        }

        private void Die()
        {
            if(health <= 0)
            {
                physicalWorld.BodyList.Remove(physicalBody); // remove the object from the list
                Destroy(gameObject); // destroy the object
            }
        }

        private void ResetTakeDamage()
        {
            isTakingDamage = false;
        }

        public void TakeDamage()
        {
            if(!isTakingDamage)
            {
                health--;
                isTakingDamage = true;

                Invoke("ResetTakeDamage", 0.5f);
            }
        }
    }
}