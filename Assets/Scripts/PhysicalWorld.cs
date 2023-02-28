using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BreakBricks2D
{
    public class PhysicalWorld : MonoBehaviour
    {
        // References
        private List<PhysicalBody> bodyList = new List<PhysicalBody>();
        private DamageHandler damageHandlerA;
        private DamageHandler damageHandlerB;
        private Projectile projectileA;
        private Projectile projectileB;

        private GameObject player;
        private PlayerMovement playerMovement;

        [SerializeField] private float gravity = 2.0f; // the gravity of the world
        [SerializeField] private bool isDrawGizmo;

        // Properties
        public List<PhysicalBody> BodyList { get { return bodyList; } }
        public float Gravity { get { return gravity; } }
        public bool IsDrawGizmo { get { return isDrawGizmo; } }

        private void Awake()
        {
            InvokeRepeating("CollisionDetection", 0.0f, 0.02f); // call the collision detection every 0.02 seconds
        }

        private void PlayerManager()
        {
            player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                playerMovement = player.GetComponent<PlayerMovement>();
                playerMovement.XAxisCollision = false; // reset the player collision with wall
                playerMovement.YAxisCollision = false; // reset the player collision with the ground
            }
        }

        private void CollisionDetection()
        {
            PlayerManager();

            for (int bodyA = 0; bodyA < bodyList.Count; bodyA++)
            {
                for (int bodyB = bodyA + 1; bodyB < bodyList.Count; bodyB++)
                {
                    if(!bodyList[bodyA].IsRectangle && !bodyList[bodyB].IsRectangle)
                    {
                        SphereSphereCollision(bodyA, bodyB);
                    }
                    else
                    {
                        SphereRectangleCollision(bodyA, bodyB);
                    }
                }
            }
        }

        private void SphereSphereCollision(int bodyA, int bodyB)
        {
            float distance = Vector3.Distance(bodyList[bodyA].Position, bodyList[bodyB].Position); // distance between two bodies
            float radiusSum = bodyList[bodyA].Radius + bodyList[bodyB].Radius; // sum of two bodies' radius

            // sphere-sphere collision
            if(distance <= radiusSum)
            {
                GetComponents(bodyList[bodyA], bodyList[bodyB]);
                CollisionResolution(bodyList[bodyA], bodyList[bodyB]);
            }
        }

        private void SphereRectangleCollision(int bodyA, int bodyB)
        {
            // get rectangle corners
            Vector2 AA = bodyList[bodyA]._AA;
            Vector2 BB = bodyList[bodyA]._BB;

            // get closest point on rectangle A to rectangle B
            float closestX = Mathf.Clamp(bodyList[bodyB].Position.x, AA.x, BB.x);
            float closestY = Mathf.Clamp(bodyList[bodyB].Position.y, AA.y, BB.y);
            Vector2 closestPoint = new Vector2(closestX, closestY);

            // get distance between closest point and rectangle B
            float distanceX = bodyList[bodyB].Position.x - closestPoint.x;
            float distanceY = bodyList[bodyB].Position.y - closestPoint.y;
            float distanceBetween = Mathf.Sqrt((distanceX * distanceX) + (distanceY * distanceY));

            // sphere-rectangle collision
            if (distanceBetween < bodyList[bodyB].Radius)
            {
                GetComponents(bodyList[bodyA], bodyList[bodyB]);
                CollisionResolution(bodyList[bodyA], bodyList[bodyB]);
            }
        }

        private void GetComponents(PhysicalBody bodyA, PhysicalBody bodyB)
        {
            damageHandlerA = bodyA.GetComponent<DamageHandler>();
            damageHandlerB = bodyB.GetComponent<DamageHandler>();
            projectileA = bodyA.GetComponent<Projectile>();
            projectileB = bodyB.GetComponent<Projectile>();
        }

        private void CollisionResolution(PhysicalBody bodyA, PhysicalBody bodyB)
        {
            if(bodyB.gameObject.tag == "isPlayerMissile")
            {
                PlayerMissile(bodyA, bodyB);
            }
            else if(bodyB.gameObject.tag == "isEnemyMissile")
            {
                EnemyMissile(bodyA, bodyB);
            }
            else if(bodyB.gameObject.tag == "Player")
            {
                PlayerMovement(bodyA, bodyB);
            }
        }

        private void PlayerMissile(PhysicalBody bodyA, PhysicalBody bodyB)
        {
            if(bodyA.gameObject.tag == "isDestructible")
            {
                damageHandlerA.TakeDamage();
                projectileB.BounceBackProjectile(bodyA);

                if(bodyA.GetComponent<SpecialAbility>() != null)
                {
                    bodyA.GetComponent<SpecialAbility>().ActivateSpecialAbility();
                }
            }
            else if(bodyA.gameObject.tag == "isEnemy")
            {
                projectileB.GiveDamageOnce(bodyA);
                projectileB.StopProjectileMovement();
            }
            else if(bodyA.gameObject.tag == "isEnemyMissile")
            {
                projectileA.BounceBackProjectile(bodyB);
                projectileB.BounceBackProjectile(bodyA);
            }
            else if(bodyA.gameObject.tag == "isPlayerMissile")
            {
                projectileA.BounceBackProjectile(bodyB);
                projectileB.BounceBackProjectile(bodyA);
            }
            else if(bodyA.gameObject.tag == "isStatic")
            {
                projectileB.BounceBackProjectile(bodyA);
            }
        }

        private void EnemyMissile(PhysicalBody bodyA, PhysicalBody bodyB)
        {
            if(bodyA.gameObject.tag == "Player")
            {
                damageHandlerA.TakeDamage(); // player takes damage
                projectileB.BounceBackProjectile(bodyA); // enemy missile bounces back
            }
            else if(bodyA.gameObject.tag == "isEnemyMissile")
            {
                projectileA.BounceBackProjectile(bodyB);
                projectileB.BounceBackProjectile(bodyA);
            }
            else if(bodyA.gameObject.tag == "isPlayerMissile")
            {
                projectileA.BounceBackProjectile(bodyB);
                projectileB.BounceBackProjectile(bodyA);
            }
            else if(bodyA.gameObject.tag == "isStatic")
            {
                projectileB.BounceBackProjectile(bodyA);
            }
            else if(bodyA.gameObject.tag == "isDestructible")
            {
                projectileB.BounceBackProjectile(bodyA);
            }
        }
        
        private void PlayerMovement(PhysicalBody bodyA, PhysicalBody bodyB)
        {
            if(bodyA.gameObject.tag == "isStatic")
            {
                if(player.transform.position.y >= -4.5f)
                {
                    playerMovement.YAxisCollision = true; // player is colliding with the ground / platforms
                }

                if(player.transform.position.x >= 6.66f)
                {
                    playerMovement.XAxisCollision = true; // player is colliding with right wall
                }
                else if(player.transform.position.x <= -6.66f)
                {
                    playerMovement.XAxisCollision = true; // player is colliding with left wall
                }
            }
        }

        public void AddBody(PhysicalBody newBody)
        {
            bodyList.Add(newBody);
        }
    }
}