using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player Control script. responsible for movement
/// </summary>
public class PlayerModel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
        DataController.Instance.Input.Player.Movement.performed += ctx => _move = ctx.ReadValue<Vector2>();
        DataController.Instance.Input.Player.Movement.canceled += ctx => _move = Vector2.zero;
        DataController.Instance.Input.Player.Rotation.performed += ctx => _rotate = ctx.ReadValue<Vector2>();
        DataController.Instance.Input.Player.Rotation.canceled += ctx => _rotate = Vector2.zero;
        DataController.Instance.Input.Player.MousePosition.performed += ctx => _mousePos = ctx.ReadValue<Vector2>();
        DataController.Instance.Input.Player.MousePosition.canceled += ctx => _mousePos = Vector2.zero;
        DataController.Instance.Input.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // movement
        _characterController.Move(new Vector3(_move.x, 0, _move.y) * Time.deltaTime * _speed);
        // rotation
        if (_mousePos != _lastMousePosition) _useMouseInput = true;
        if (_rotate != Vector2.zero)    // first checking controller
        {
            var angle = Mathf.Atan2(-_rotate.y, _rotate.x) * Mathf.Rad2Deg + 90f;
            if (angle < 0) angle += 360;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, angle, transform.localEulerAngles.z);
            _useMouseInput = false;
        }
        else if (_useMouseInput) // then checking mouse
        {
            var myPos = DataController.Instance.MainCamera.WorldToScreenPoint(gameObject.transform.position);
            var diff = (new Vector2(myPos.x, myPos.y) - _mousePos).normalized;
            //Debug.Log(diff);
            var angle = Mathf.Atan2(diff.y, -diff.x) * Mathf.Rad2Deg + 90f;
            if (angle < 0) angle += 360;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, angle, transform.localEulerAngles.z);
        }

        _lastMousePosition = _mousePos;
    }


    void OnEnable()
    {
        if (DataController.IsInitialized)
        {
            DataController.Instance.Input.Player.Enable();
        }
    }

    void OnDisable()
    {
        if (DataController.IsInitialized)
        {
            DataController.Instance.Input.Player.Disable();
        }
    }

    #region Fields

    [SerializeField, Tooltip("Movement Speed")]
    private float _speed = 10f;

    private Vector2 _move;
    private Vector2 _rotate;
    private Vector2 _mousePos;

    private CharacterController _characterController;

    private bool _useMouseInput;
    private Vector2 _lastMousePosition;

    #endregion

    #region Properties

    #endregion
}
