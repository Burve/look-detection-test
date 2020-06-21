using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls ar the global data in the game
/// </summary>
public class DataController : MonoSingleton<DataController>
{
    void Awake()
    {
        Instance = this;
        _input = new InputMaster();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Fields

    [Header("Look Detection General Parameters")]

    [SerializeField, Tooltip("Look Detection maximum distance")]
    private float _lookDetectionMaximumDistance = 100f;

    [SerializeField, Tooltip("Look Detection Half Angle")]
    private float _lookDetectionHalfAngle = 30f;

    [SerializeField, Tooltip("Look Detection Raycast delay")]
    private float _lookDetectionRaycastDelay = 0.1f;

    [Header("Camera View check")]

    [SerializeField, Tooltip("Detect outside Camera view")]
    private bool _detectOutsideCameraView = true;

    [SerializeField, Tooltip("Main Camera")]
    private Camera _mainCamera;

    private InputMaster _input;

    #endregion

    #region Properties

    public float LookDetectionMaximumDistance => _lookDetectionMaximumDistance;

    public float LookDetectionHalfAngle => _lookDetectionHalfAngle;

    public float LookDetectionRaycastDelay => _lookDetectionRaycastDelay;

    public bool DetectOutsideCameraView
    {
        get => _detectOutsideCameraView;
        set => _detectOutsideCameraView = value;
    }

    public Camera MainCamera => _mainCamera;

    public InputMaster Input => _input;

    #endregion

}
