using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravityForce;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _xRotationLimit;
    [SerializeField] private bool _isCanMove = true;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private CharacterController _characterController;

    private Vector3 _moveDirection = Vector3.zero;
    private float _rotationX = 0;
    private float _currentSpeedX;
    private float _currentSpeedY;
    private float _movementDirectionY;

    private bool _isRunning;

    public float RotationSpeed { set => _rotationSpeed = value; }
    public bool IsCanMove { get => _isCanMove; set => _isCanMove = value; }

    private void OnValidate()
    {
        if (_characterController == null)
            _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _rotationSpeed = PlayerPrefs.GetFloat("Sensitivity");
    }

    private void Update()
    {
        Movement();
        Jumping();
        Rotation();
    }

    private void Movement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        _isRunning = Input.GetKey(KeyCode.LeftShift);
        _currentSpeedX = _isCanMove ? (_isRunning ? _runSpeed : _walkSpeed) * Input.GetAxis("Vertical") : 0;
        _currentSpeedY = _isCanMove ? (_isRunning ? _runSpeed : _walkSpeed) * Input.GetAxis("Horizontal") : 0;
        _movementDirectionY = _moveDirection.y;
        _moveDirection = (forward * _currentSpeedX) + (right * _currentSpeedY);
    }

    private void Jumping()
    {
        if (Input.GetButton("Jump") && _isCanMove && _characterController.isGrounded)
            _moveDirection.y = _jumpForce;
        else
            _moveDirection.y = _movementDirectionY;

        if (!_characterController.isGrounded)
            _moveDirection.y -= _gravityForce * Time.deltaTime;
    }

    private void Rotation()
    {
        _characterController.Move(_moveDirection * Time.deltaTime);
        if (_isCanMove)
        {
            _rotationX += -Input.GetAxis("Mouse Y") * _rotationSpeed;
            _rotationX = Mathf.Clamp(_rotationX, -_xRotationLimit, _xRotationLimit);
            _playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _rotationSpeed, 0);
        }
    }
}
