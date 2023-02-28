using UnityEngine;

namespace BreakBricks2D
{
    public class FacePlayer : MonoBehaviour
    {
        private Transform player;
        [SerializeField] private float rotationSpeed = 90.0f;

        private void Update()
        {
            FindPlayer();
            FaceTowardPlayer();
        }

        private void FindPlayer()
        {
            if(player == null)
            {
                GameObject player = GameObject.FindWithTag("Player");

                if(player != null)
                {
                    this.player = player.transform;
                }
            }
        }

        private void FaceTowardPlayer()
        {
            if(player == null)
            {
                return;
            }

            Vector3 dir = player.position - transform.position;
            dir.Normalize();
            float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            Quaternion desiredRot = Quaternion.Euler( 0, 0,zAngle );
            transform.rotation = Quaternion.RotateTowards( transform.rotation, desiredRot, rotationSpeed * Time.deltaTime);
        }
    }
}