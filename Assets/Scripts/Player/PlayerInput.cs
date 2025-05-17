using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerActions))]
public class PlayerInput : MonoBehaviour {
    [Header("Keybinds")]
    [SerializeField] InputActionAsset _actions;

    private InputActionMap _playerMap;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _interactAction;

    private PlayerActions _movement;
    private Vector2 _moveInput;

    void Awake() {
        _movement = GetComponent<PlayerActions>();
        _playerMap = _actions.FindActionMap("Player", throwIfNotFound: true);

        _moveAction = _playerMap.FindAction("Move", throwIfNotFound: true);
        _jumpAction = _playerMap.FindAction("Jump", throwIfNotFound: true);
        _interactAction = _playerMap.FindAction("Interact", throwIfNotFound: true);

        _moveAction.performed += OnMove;
        _moveAction.canceled += OnMove;
        _jumpAction.performed += ctx => _movement.RequestJump();
        _interactAction.performed += ctx => _movement.ShootMagic1();
    }

    void OnEnable() {
        _playerMap.Enable();
    }

    void OnDisable() {
        _playerMap.Disable();
    }

    private void OnMove(InputAction.CallbackContext ctx) {
        _moveInput = ctx.ReadValue<Vector2>();
    }

    void FixedUpdate() {
        _movement.HandleMovement(_moveInput.x, _moveInput.y);
    }
}