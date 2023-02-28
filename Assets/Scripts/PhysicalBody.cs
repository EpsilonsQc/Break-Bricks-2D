using UnityEngine;
using UnityEditor;

namespace BreakBricks2D
{
    public class PhysicalBody : MonoBehaviour
    {
        // References
        private PhysicalWorld physicalWorld;
        private SpriteRenderer spriteRenderer;

        [SerializeField] private float radius = 0.25f;
        [SerializeField] private bool isRectangle = false;
        private Vector3 position;
        private Vector2 AA;
        private Vector2 BB;

        // Properties
        public float Radius { get { return radius; } }
        public bool IsRectangle { get { return isRectangle; } }
        public Vector3 Position { get { return position; } }
        public Vector2 _AA { get { return AA; } }
        public Vector2 _BB { get { return BB; } }

        private void Awake()
        {
            physicalWorld = FindObjectOfType<PhysicalWorld>();
            physicalWorld.AddBody(this);

            spriteRenderer = GetComponent<SpriteRenderer>();
            CalculateBoundingBox();
        }

        private void Update()
        {
            GetPosition();
        }

        private void GetPosition()
        {
            position = transform.position;
        }

        private void CalculateBoundingBox()
        {
            if(spriteRenderer != null)
            {
                AA = new Vector2(spriteRenderer.bounds.min.x, spriteRenderer.bounds.min.y);
                BB = new Vector2(spriteRenderer.bounds.max.x, spriteRenderer.bounds.max.y);
            }
        }

        private void OnDrawGizmos()
        {
            if(EditorApplication.isPlaying && physicalWorld.IsDrawGizmo)
            {
                if(isRectangle && spriteRenderer != null)
                {
                    Handles.color = Color.red;
                    Handles.DrawWireCube(position, new Vector3(BB.x - AA.x, BB.y - AA.y, 0));
                }
                else
                {
                    Handles.color = Color.red;
                    Handles.DrawWireDisc(position, Vector3.forward, radius);
                }
            }
        }
    }
}