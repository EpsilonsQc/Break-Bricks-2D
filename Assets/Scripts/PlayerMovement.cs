using UnityEngine;

namespace BreakBricks2D
{
    public class PlayerMovement : MonoBehaviour
    {
        // References
        private PhysicalWorld physicalWorld;
        private SpriteRenderer spriteRenderer;
        private GameObject player;
        private GameObject playerBody;
        private GameObject playerHead;

        [Header("Movement")]
        [SerializeField] private float walkSpeed = 2.0f;
        [SerializeField] private float runSpeed = 6.0f;
        [SerializeField] private float jumpHeight = 2.0f;
        private float headRotationSpeed = 180.0f;
        private float bodyRotationSpeed = 0.2f;

        private float playerSpeed;
        private float xAxisMovement;

        private Vector3 mousePosition;
        private Vector3 direction;
        
        private bool xAxisCollision; // check if the player is on the wall
        private bool yAxisCollision; // check if the player is on the ground

        // Properties
        public bool XAxisCollision
        {
            get { return xAxisCollision; }
            set { xAxisCollision = value; }
        }

        public bool YAxisCollision
        {
            get { return yAxisCollision; }
            set { yAxisCollision = value; }
        }

        private void Start()
        {
            physicalWorld = FindObjectOfType<PhysicalWorld>();
            player = GameObject.FindWithTag("Player");
            playerBody = GameObject.FindWithTag("PlayerBody");
            playerHead = GameObject.FindWithTag("PlayerHead");
            spriteRenderer = GameObject.FindWithTag("PlayerBody").GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            LeftRightMovement();
            RotateBody();
            RotateHead();
            Jump();
        }

        private void LeftRightMovement()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                playerSpeed = runSpeed;
            }
            else
            {
                playerSpeed = walkSpeed;
            }

            xAxisMovement = Input.GetAxis("Horizontal");

            if(!XAxisCollision) // prevent the player from moving if he hit a wall
            {
                transform.Translate(Vector2.right * xAxisMovement * playerSpeed * Time.deltaTime); // player can move left and right
            }
            else if(player.transform.position.x >= -6.66f)
            {
                transform.Translate(Vector2.right * -0.10f * playerSpeed * Time.deltaTime); // player hit left wall and is slightly forced right
            }
            else if(player.transform.position.x <= 6.66f)
            {
                transform.Translate(Vector2.right * 0.10f * playerSpeed * Time.deltaTime); // player hit right wall and is slightly forced left
            }

        }

        private void RotateBody()
        {
            float bodyRadius;
            float angularSpeed;

            bodyRadius = spriteRenderer.bounds.max.x - spriteRenderer.bounds.min.x;
            angularSpeed = playerSpeed / bodyRadius;
            
            playerBody.transform.Rotate(0.0f, 0.0f, -angularSpeed * xAxisMovement * bodyRotationSpeed);
        }

        private void RotateHead()
        {
            if (Input.GetKey(KeyCode.Q))
            {
                if (playerHead.transform.rotation.z < 0.4f)
                {
                    playerHead.transform.Rotate(Vector3.forward * headRotationSpeed * Time.deltaTime);
                    playerHead.transform.Translate(Vector2.left * Time.deltaTime);
                }
            }

            if (Input.GetKey(KeyCode.E))
            {
                if (playerHead.transform.rotation.z > -0.4f)
                {
                    playerHead.transform.Rotate(Vector3.back * headRotationSpeed * Time.deltaTime);
                    playerHead.transform.Translate(Vector2.right * Time.deltaTime);
                }
            }
        }

        private bool IsGrounded()
        {
            if(yAxisCollision)
            {
                return true;
            }
            else if(player.transform.position.y <= -4.57f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            {
                player.transform.Translate(Vector2.up * jumpHeight); // jump
            }

            if (!IsGrounded())
            {
                player.transform.Translate(Vector2.down * physicalWorld.Gravity * Time.deltaTime); // apply gravity
            }
        }
    }
}