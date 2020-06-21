using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

/// <summary>
/// Controls Game related data in the game
/// </summary>
public class GameController : MonoSingleton<GameController>
{
    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        // first get all enemies from they root
        var enemy = _enemyRoot.GetComponentsInChildren<CharacterModel>().ToList();
        // setting up Player to look for the Enemies
        if (_player != null && _player.LookDetection != null)
        {
            foreach (var model in enemy)
            {
                if (model.LookDetectTarget != null)
                {
                    _player.LookDetection.SetTarget(model.LookDetectTarget);
                }
            }
            _player.LookDetection.Init(_player.FoundTarget, _player.NotFoundTarget, null);
        }
        _player.SetRayColor(Color.green); // setting player color to green
        // setting up enemies to look for the Player
        if (_player.LookDetectTarget != null)
        {
            foreach (var model in enemy)
            {
                if (model.LookDetection != null)
                {
                    model.LookDetection.SetTarget(_player.LookDetectTarget);
                    model.LookDetection.Init(model.FoundTarget, model.NotFoundTarget, null);
                }
            }
        }

    
    }


    // Update is called once per frame
    void Update()
    {
        // if we need to disable detecting outside camera bounds, then we need to update our camera bounds if camera moves
        if (!DataController.Instance.DetectOutsideCameraView && DataController.Instance.MainCamera != null && 
            _cameraLastPosition != DataController.Instance.MainCamera.gameObject.transform.position)
        {
            _cameraLastPosition = DataController.Instance.MainCamera.transform.position;
            _cameraFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(DataController.Instance.MainCamera);
        }
    }



    #region Fields

    [SerializeField, Tooltip("Reference to the player")]
    private CharacterModel _player;

    [SerializeField, Tooltip("References to the enemies root")]
    private GameObject _enemyRoot;
    

    private Vector3 _cameraLastPosition;
    private Plane[] _cameraFrustumPlanes;

    #endregion

    #region Properties

    public Plane[] CameraFrustumPlanes => _cameraFrustumPlanes;

    #endregion
}
