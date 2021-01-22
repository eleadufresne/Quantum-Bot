using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{

    public int controlModifier = 1;
    public float _playerSpeed = 2.0f;
    public float _jumpHeight = 1.0f;
    public bool _hasFovChange = false;

    [SerializeField] private float turnSmoothTime;
    [SerializeField] private float _rotationSpeed = 1.0f;
    [SerializeField] private float _dashSpeed = 10f;
    [SerializeField] private float _dashCooldownTime = 5f;
    [SerializeField] private float _dashTime = 0f;
    [SerializeField] float _movementRate = 10f;
    [SerializeField] float _movementTime = 0.05f;

    private Animator _playerAnim;
    private int _dashedInTheAirCnt = 0;
    private float _dashTimeStamp = 0f;
    private bool _isDashing = false;
    private float _movementT = 0.0f;
    private float _slowDownT = 0.0f;
    private float _dashCooldownTimestamp;
    private Vector3 _movDir;
    private Vector2 _input;
    private Vector3 _movement;
    private float _movTimeStamp = 0;
    private InputManager _inputManager;
    private Transform _cameraTransform;
    private CinemachineFreeLook _camera;
    private Rigidbody _playerRigidBody;
    private float turnSmoothVelocity;
    private bool _isMoving;

    // Start is called before the first frame update
    void Start()
    {
        _playerAnim = GetComponentInChildren<Animator>();
        _playerRigidBody = this.GetComponent<Rigidbody>();
        _inputManager = InputManager.Instance;
        _cameraTransform = Camera.main.transform;
        _camera = GameObject.Find("CM FreeLook1").GetComponent<CinemachineFreeLook>();
        Cursor.visible = false;
    }
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _input = _inputManager.GetPlayerMovement();
        _playerAnim.SetBool("isMoving",_isMoving);
        // Make the player rotate when turning the camera
        float targetAngle = Mathf.Atan2(_input.x, _input.y) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * _rotationSpeed);
        if (_input != Vector2.zero)
        {
            _movDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            _movement = new Vector3 (_movDir.normalized.x * controlModifier, _movDir.normalized.y, _movDir.normalized.z * controlModifier) * _playerSpeed;

            _movement.y = _playerRigidBody.velocity.y;

            _isMoving = true;

            if (_inputManager.IsPlayerDashing())
            {
                //Debug.Log("Dashing!");
   
                if (!GroundCheck())
                {
                    if (_dashedInTheAirCnt < 1 && _dashCooldownTimestamp < Time.time)
                    {
                        // This adds the cooldownn to the current time to create a timestamp of when the user is allowed to use the dash again.
                        _dashCooldownTimestamp = Time.time + _dashCooldownTime;
                        _dashTimeStamp = Time.time + _dashTime;
                        _isDashing = true;
                        _dashedInTheAirCnt++;
                        _playerRigidBody.AddForce(new Vector3(_movement.x, 0f, _movement.z) * _dashSpeed, ForceMode.Impulse);
                    }
                }
                else
                {
                    // This test if the dash is still in cooldown.
                    if (_dashCooldownTimestamp < Time.time)
                    {
                        // This adds the cooldownn to the current time to create a timestamp of when the user is allowed to use the dash again.
                        _dashCooldownTimestamp = Time.time + _dashCooldownTime;
                        _dashTimeStamp = Time.time + _dashTime;
                        _isDashing = true;
                        _playerRigidBody.AddForce(new Vector3(_movement.x, 0f, _movement.z) * _dashSpeed, ForceMode.Impulse);
                    }
                 }

            }
        }
        else
        {
            _isMoving = false;
           // if (GroundCheck()) _playerRigidBody.velocity = new Vector3(0f, _playerRigidBody.velocity.y, 0f);
        }

       

        if (_inputManager.IsPlayerJumping())
        {
            if (GroundCheck())
            {
                _playerRigidBody.AddForce(new Vector3(0, _jumpHeight, 0), ForceMode.Impulse);
                _playerAnim.SetTrigger("jump");
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isDashing && _dashTimeStamp < Time.time)
        {
            _isDashing = false;
        }

        if (GroundCheck())
        {
            _dashedInTheAirCnt = 0;
        }
        if (_isMoving && !_isDashing)
        {
            _slowDownT = 0f;
            
            while (_movementT < 1.0f && _movTimeStamp < Time.time)
            {
                    _movementT += _movementRate * Time.deltaTime;
                    _movTimeStamp = Time.time + _movementTime;
            }
            _playerRigidBody.velocity = new Vector3(Mathf.Lerp(0f, _movement.x, _movementT), _playerRigidBody.velocity.y, Mathf.Lerp(0f, _movement.z, _movementT));
        }
        if (!_isMoving)
        {
            _movementT = 0f;

            while (_slowDownT < 1.0f && _movTimeStamp < Time.time)
            {
                _slowDownT += _movementRate * Time.deltaTime;
                _movTimeStamp = Time.time + _movementTime;
            }
            _playerRigidBody.velocity = new Vector3(Mathf.Lerp(0f,_playerRigidBody.velocity.x, _slowDownT), _playerRigidBody.velocity.y, Mathf.Lerp(0f, _playerRigidBody.velocity.z, _slowDownT));
        }
    }

    // Detect if the player is on the ground using a Spherecast
    private bool GroundCheck()
    {
        RaycastHit hit;
        float distance = 1f;
        Vector3 dir = new Vector3(0, -1f, 0);
        //Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), dir * distance, Color.red);

        if (Physics.SphereCast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), 1, dir, out hit, distance))
        {
            //Debug.Log("Hit ground");
            return true;
        }
        return false;
    }

    public void ApplyBuff(GenericBuff buff)
    {
        StartCoroutine(buff.ActivateBuff(this));
    }

    public IEnumerator FovChange(float from, float to)
    {
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += 0.5f * Time.deltaTime;
            _camera.m_Lens.FieldOfView = Mathf.Lerp(from, to, t);
            yield return null;
        }
    }

}
