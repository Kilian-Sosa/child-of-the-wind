using UnityEngine;

public class PlayerActions: MonoBehaviour {

    [Header("Movement")]
    [SerializeField] float _moveSpeed = 7f;
    [SerializeField] float _groundDrag = 6f;
    [SerializeField] float _jumpForce = 15f;
    [SerializeField] float _jumpCooldown = 0.25f;
    [SerializeField] float _airMultiplier = 0.4f;

    [Header("Ground Check")]
    [SerializeField] float _playerHeight = 2f;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] Transform _orientation;

    [Header("Magic")]
    [SerializeField] float _magic1Cooldown = 0.5f;

    bool _readyToJump = true;
    bool _isGrounded;
    Rigidbody _rb;

    PlayerInventory _playerInventory;
    bool _readyToUseMagic1 = true;

    void Start() {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;

        _playerInventory = GetComponent<PlayerInventory>();
    }

    public void HandleMovement(float horizontalInput, float verticalInput) {
        GroundCheck();

        _rb.linearDamping = _isGrounded ? _groundDrag : 0f;

        Move(horizontalInput, verticalInput);
        SpeedControl();
    }

    void GroundCheck() {
        _isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            _playerHeight * 0.5f + 0.3f,
            _groundLayer
        );
    }

    void Move(float horizontal, float vertical) {
        Vector3 moveDir = _orientation.forward * vertical + _orientation.right * horizontal;
        if (_isGrounded)
            _rb.AddForce(moveDir.normalized * _moveSpeed * 10f, ForceMode.Force);
        else
            _rb.AddForce(moveDir.normalized * _moveSpeed * 10f * _airMultiplier, ForceMode.Force);
    }

    void SpeedControl() {
        Vector3 flatVel = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        if (flatVel.magnitude > _moveSpeed) {
            Vector3 limitedVel = flatVel.normalized * _moveSpeed;
            _rb.linearVelocity = new Vector3(limitedVel.x, _rb.linearVelocity.y, limitedVel.z);
        }
    }

    public void RequestJump() {
        if (_readyToJump && _isGrounded) {
            _readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }

    void Jump() {
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    void ResetJump() => _readyToJump = true;

    public void RequestMagic1() {
        Debug.Log("asjkdjakd");
        // TODO: Add mana control
        if (_playerInventory.HasMagic1() && _readyToUseMagic1) {
            _readyToUseMagic1 = false;
            Jump();
            Invoke(nameof(ShootMagic1), _magic1Cooldown);
        }
    }

    public void ShootMagic1() {
        Debug.Log("ShootMagic1 called!");
    }
}