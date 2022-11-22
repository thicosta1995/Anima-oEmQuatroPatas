using UnityEngine;

namespace _Project.Scripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _walkDirection;

        private Rigidbody _rb;
        private CapsuleCollider _collider;
        private bool _isGrounded = true;
        private float _moveSpeed;

        public void Start()
        {
            Initialize();
        }

        public void Update()
        {
            GroundCheck();
        }

        public void Move(Vector2 moveDirection)
        {
            PlayerMovement(moveDirection);
        }

        public void Jump()
        {
            if (_isGrounded)
            {
                _animator.SetTrigger("Jump");
                _rb.AddForce(Vector3.up * 8, ForceMode.Impulse);
            }
        }

        public void Sit()
        {
            _animator.SetTrigger("Sit");
        }

        private void Initialize()
        {
            _collider = GetComponent<CapsuleCollider>();
            _rb = GetComponent<Rigidbody>();
            _moveSpeed = 8;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void PlayerMovement(Vector2 moveDirection)
        {
            Vector3 direction = new Vector3(moveDirection.x, 0, moveDirection.y).normalized;

            if (direction.magnitude < 0.1f)
            {
                _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
                _animator.SetBool("IsMoving", false);
                return;
            }

            _animator.SetBool("IsMoving", true);
            float turnVelocity = 20;
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.y) * Mathf.Rad2Deg + _walkDirection.eulerAngles.y;
            targetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, 0.03f);
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);

            direction = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            direction *= _moveSpeed;
            direction.y = _rb.velocity.y;
            _rb.velocity = direction;
        }

        private void GrabMovement(float direction)
        {
            _animator.SetBool("IsMoving", direction != 0);

            direction *= _moveSpeed;
            Vector3 right = gameObject.transform.right;
            Vector3 dir = new Vector3(-right.z, 0, right.x);
            Vector3 movementDir = (dir * direction + right * 0);
            _rb.velocity = movementDir;
        }

        private void GroundCheck()
        {
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.3f);
            _animator.SetBool("IsGrounded", _isGrounded);
        }
    }
}
