using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("Movement")]
    [SerializeField] float _moveSpeed;

    [SerializeField] float _groundDrag;

    [SerializeField] float _jumpForce;
    [SerializeField] float _jumpCooldown;
    [SerializeField] float _airMultiplier;
    bool _readyToJump;

    [Header("Keybinds")]
    [SerializeField] KeyCode _jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    [SerializeField] float _playerHeight;
    [SerializeField] LayerMask groundLayer;
    bool _isGrounded;

    [SerializeField] Transform _orientation;

    float _horizontalInput;
    float _verticalInput;

    Vector3 _moveDirection;

    Rigidbody _rb;


    void Start() {
        _rb = GetComponent<Rigidbody>();
        //_rb.freezeRotation = true;

        _readyToJump = true;
    }

    void Update() {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.3f, groundLayer);

        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(_jumpKey) && _readyToJump && _isGrounded) {
            _readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), _jumpCooldown);
        }

        _rb.linearDamping = _isGrounded ? _groundDrag : 0f;
    }

    void FixedUpdate() {
        Move();
        SpeedControl();
    }

    void Move() {

        _moveDirection = _orientation.forward * _verticalInput + _orientation.right * _horizontalInput;
        
        if (_isGrounded)
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);
        else if (!_isGrounded)
            _rb.AddForce(_moveDirection.normalized * _moveSpeed * 10f * _airMultiplier, ForceMode.Force);
    }

    void SpeedControl() {

        Vector3 flatVel = new(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

        if (flatVel.magnitude > _moveSpeed) {
            Vector3 limitedVel = flatVel.normalized * _moveSpeed;
            _rb.linearVelocity = new(limitedVel.x, _rb.linearVelocity.y, limitedVel.z);
        }   
    }

    void Jump() {

        _rb.linearVelocity = new(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    void ResetJump() => _readyToJump = true;
}
