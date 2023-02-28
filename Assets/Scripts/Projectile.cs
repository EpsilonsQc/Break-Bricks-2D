using UnityEngine;

namespace BreakBricks2D
{
    public class Projectile : MonoBehaviour
    {
        private PhysicalWorld physicalWorld;
        private PhysicalBody physicalBody;
        private GameObject enemy;

        [Header("Movement")]
        [SerializeField] private float projectileSpeed; // the speed of the projectile
        [SerializeField] [Range(0.001f, 0.01f)] private float restitutionCoefficient = 0.003f; // the restitution coefficient of the projectile

        [Header("Attraction")]
        [SerializeField] private bool enemyAttractProjectile; // determine if the projectile is attracted to the enemy
        [SerializeField] [Range(0, 360)] private float attractionForce = 60.0f; // the force of the attraction

        private bool onCollision; // check if the projectile is colliding with something
        private bool onEnemy; // check if the projectile is colliding with the enemy
        private bool enemyIsDead; // check if the enemy is dead
        private bool takenDamage; // check if the object as already taken damage

        private Vector3 velocity;
        private Vector3 direction;

        private void Awake()
        {
            physicalWorld = FindObjectOfType<PhysicalWorld>();
            physicalBody = GetComponent<PhysicalBody>();
            enemy = GameObject.FindGameObjectWithTag("isEnemy");
        }

        private void Update()
        {
            MoveProjectile(); // move the projectile

            if(enemyAttractProjectile)
            {
                AttractProjectile(); // attract the projectile to the enemy
            }
        }

        private void MoveProjectile()
        {
            if(!onCollision && !onEnemy && !enemyIsDead) // not colliding with anything
            {
                velocity = new Vector3(0, projectileSpeed * Time.deltaTime, 0);
                direction = physicalBody.Position + transform.rotation * velocity;
                transform.position = direction;
            }
            else if(onCollision && !onEnemy) // on collision with something, change the direction of the projectile
            {   
                transform.position = new Vector3(transform.position.x - direction.x, transform.position.y - direction.y, transform.position.z);
            }
            else if(onEnemy && !enemyIsDead) // on collision with the enemy
            {
                direction = transform.position; // stop the projectile movement at the enemy position

                if (enemy == null)
                {
                    enemyIsDead = true; // the enemy is dead
                    onEnemy = false; // the projectile is not colliding with the enemy anymore
                    onCollision = false; // the projectile is not colliding with anything anymore
                }
            }
            else if (enemyIsDead)
            {
                velocity = new Vector3(0, -physicalWorld.Gravity * Time.deltaTime, 0); // after the enemy is dead, move the projectile down
                direction = physicalBody.Position + velocity; // set direction to move down from current position
                transform.position = direction; // move the projectile down
            }
        }

        private void AttractProjectile() // attract the projectile to the enemy
        {
            if (tag == "isPlayerMissile" && enemy != null)
            {
                Vector3 direction = (enemy.transform.position - transform.position).normalized;
                float zAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                Quaternion desiredRotation = Quaternion.Euler(0, 0, zAngle);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, attractionForce * Time.deltaTime);
            }
        }

        public void BounceBackProjectile(PhysicalBody body) // called by collision resolution
        {
            onCollision = true; // a collision has occured
            direction = (body.transform.position - transform.position); // get the direction of the collision
            direction.Normalize(); // normalize the direction
            direction = direction * restitutionCoefficient;
        }

        public void StopProjectileMovement() // called by collision resolution
        {
            onEnemy = true; // a collision with the enemy has occured
        }

        public void GiveDamageOnce(PhysicalBody body)
        {
            if(!takenDamage)
            {
                body.GetComponent<DamageHandler>().Health--; // lose health
                takenDamage = true; // stop the projectile from damaging the enemy
            }
        }
    }
}