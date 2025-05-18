using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {
    [Header("Movement")]
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] float _turnSpeedDegs = 740f;

    [Header("Physics")]
    [SerializeField] float _groundDrag = 5f;
    [SerializeField] float _airMultiplier = .5f;

    [Header("Jump")]
    [SerializeField] float _jumpForce = 5f;
    [SerializeField] float _jumpCooldown = .25f;

    [Header("Ground-Check")]
    [SerializeField] float _playerHeight = 2f;
    [SerializeField] LayerMask _groundLayer = ~0;

    [Header("References")]
    [Tooltip("Usually the Main Camera transform. If left empty, will auto-assign.")]
    [SerializeField] Transform _cameraTransform;

    Rigidbody _rb;
    Vector2 _input;
    Vector3 _moveDirection;
    bool _isGrounded;
    bool _readyToJump = true;

    void Awake() {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotationX |
                          RigidbodyConstraints.FreezeRotationZ;

        if (_cameraTransform == null && Camera.main != null)
            _cameraTransform = Camera.main.transform;
    }

    void Update() {
        _isGrounded = Physics.Raycast(transform.position,
                                      Vector3.down,
                                      _playerHeight * .5f + .3f,
                                      _groundLayer);

        _input.x = Input.GetAxisRaw("Horizontal");
        _input.y = Input.GetAxisRaw("Vertical");
        _input = Vector2.ClampMagnitude(_input, 1f);

        if (Input.GetKeyDown(KeyCode.Space) && _readyToJump && _isGrounded) {
            _readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), _jumpCooldown);
        }

        _rb.linearDamping = _isGrounded ? _groundDrag : 0f;
    }

    void FixedUpdate() {
        BuildMoveDirection();
        Move();
        RotateTowardsMoveDirection();
        ClampHorizontalSpeed();
    }

    /// <summary>Builds a camera-relative world-space move vector.</summary>
    void BuildMoveDirection() {
        Vector3 camForward = _cameraTransform.forward;
        camForward.y = 0f;                // keep movement flat
        camForward.Normalize();

        Vector3 camRight = _cameraTransform.right;
        camRight.y = 0f;
        camRight.Normalize();

        _moveDirection = (camForward * _input.y + camRight * _input.x).normalized;
    }

    void Move() {
        if (_moveDirection.sqrMagnitude < .0001f) return;

        float multiplier = _isGrounded ? 1f : _airMultiplier;
        _rb.AddForce(_moveDirection * _moveSpeed * 10f * multiplier, ForceMode.Force);
    }

    /// <summary>Smoothly turns the rigidbody to face the current move direction.</summary>
    void RotateTowardsMoveDirection() {
        if (_moveDirection.sqrMagnitude < .0001f) return;

        Quaternion targetRot = Quaternion.LookRotation(_moveDirection, Vector3.up);
        Quaternion newRot =
            Quaternion.RotateTowards(_rb.rotation, targetRot,
                                     _turnSpeedDegs * Time.fixedDeltaTime);

        _rb.MoveRotation(newRot);
    }

    void ClampHorizontalSpeed() {
        Vector3 flatVel = new(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

        if (flatVel.sqrMagnitude > _moveSpeed * _moveSpeed) {
            Vector3 limited = flatVel.normalized * _moveSpeed;
            _rb.linearVelocity = new(limited.x, _rb.linearVelocity.y, limited.z);
        }
    }

    void Jump() {
        // kill any downward velocity first
        _rb.linearVelocity = new(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    void ResetJump() => _readyToJump = true;
}
