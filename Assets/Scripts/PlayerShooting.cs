using UnityEngine;

namespace BreakBricks2D
{
    public class PlayerShooting : MonoBehaviour
    {
        private GameObject playerHead;
        private GameObject missileParent;
        [SerializeField] private GameObject missile;

        [SerializeField] private float fireRate = 0.5f; // fire rate of the ammo
        private Vector3 missileOffset = new Vector3(0, 0.25f, 0);
        private float cooldownTimer = 0.0f;

        private void Awake()
        {
            playerHead = GameObject.FindWithTag("PlayerHead");
            missileParent = GameObject.Find("Player_Missile");
        }

        private void Update()
        {
            Shoot();
        }

        private void Shoot()
        {
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer > fireRate)
            {
                if (Input.GetMouseButton(0))
                {
                    cooldownTimer = 0f; // reset the timer
                    Vector3 offset = playerHead.transform.rotation * missileOffset;
                    Instantiate(missile, playerHead.transform.position + offset, playerHead.transform.rotation, missileParent.transform);
                }
            }
        }
    }
}