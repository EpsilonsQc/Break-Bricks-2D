using UnityEngine;

namespace BreakBricks2D
{
    public class EnemyShooting : MonoBehaviour
    {
        private GameObject missileParent;
        [SerializeField] private GameObject missile;
        [SerializeField] private GameObject player;
        [SerializeField] private float maxShootingDistance = 10.0f;
        [SerializeField] private float fireRate = 3.0f;
        private Vector3 missileOffset = new Vector3(0, 0.4f, 0);
        private float cooldownTimer = 2.0f;

        private void Awake()
        {
            missileParent = GameObject.Find("Enemy_Missile");
            InvokeRepeating("FindPlayer", 0f, 0.5f); // find the player every 0.5 seconds
        }

        private void Update()
        {
            Shoot();
        }

        private void Shoot()
        {
            cooldownTimer -= Time.deltaTime;

            if( cooldownTimer <= 0 && player != null && Vector3.Distance(transform.position, player.transform.position) < maxShootingDistance)
            {
                cooldownTimer = fireRate;
                Vector3 offset = transform.rotation * missileOffset;
                GameObject missileGO = Instantiate(missile, transform.position + offset, transform.rotation, missileParent.transform);
            }
        }

        private void FindPlayer()
        {
            if(player == null)
            {
                player = GameObject.FindWithTag("Player");
            }
        }
    }
}