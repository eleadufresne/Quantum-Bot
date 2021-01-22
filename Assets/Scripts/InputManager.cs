using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager _inputManagerInstance;

    public static InputManager Instance
    {
        get
        {
            return _inputManagerInstance;
        }
    }

    private PlayerControls _playerControls;

    private void Awake()
    {
        if (_inputManagerInstance != null && _inputManagerInstance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _inputManagerInstance = this;
        }
        _playerControls = new PlayerControls();
        //Cursor.visible = false;
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return _playerControls.Player.Movement.ReadValue<Vector2>();
    }

    public InputAction getMovmentAction()
    {
        return _playerControls.Player.Movement;
    }

    public Vector2 GetMouseDelta()
    {
        return _playerControls.Player.Look.ReadValue<Vector2>();
    }

    public bool IsPlayerJumping()
    {
        return _playerControls.Player.Jump.triggered;
    }

    public bool IsPlayerDashing()
    {
        return _playerControls.Player.Dash.triggered;
    }

}
