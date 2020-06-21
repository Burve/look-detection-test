using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

/// <summary>
/// Main script for look Detection logic
/// </summary>
public class LookDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        _collider = gameObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_data != null)
        // start with need to actually check if we are outside camera
        {
            if ((!DataController.Instance.DetectOutsideCameraView && GeometryUtility.TestPlanesAABB(GameController.Instance.CameraFrustumPlanes, _collider.bounds)) || DataController.Instance.DetectOutsideCameraView)
            {// checking all look targets
                foreach (var pair in _data)
                {
                    //Debug.DrawRay(_eyes.transform.position, pair.Key.transform.position - gameObject.transform.position, Color.green);
                    //Debug.DrawRay(_eyes.transform.position, _eyes.transform.position);

                    // checking if our target is outside camera
                    if ((!DataController.Instance.DetectOutsideCameraView && GeometryUtility.TestPlanesAABB(GameController.Instance.CameraFrustumPlanes, pair.Value.ObjCollider.bounds)) || DataController.Instance.DetectOutsideCameraView)
                    {       // if it is time to raycheck and target moved or we moved
                        if (pair.Value.CheckTime <= Time.time && (pair.Key.gameObject.transform.position != pair.Value.LastPos || _lastPos != gameObject.transform.position || _lastRot != gameObject.transform.rotation))
                        {
                            var distance = Vector3.Distance(gameObject.transform.position, pair.Key.gameObject.transform.position);
                            // if distance to target is shorter then our sight, then we good
                            if (distance <= (_overrideMaximumDistance ? _overrideMaximumDistanceValue : DataController.Instance.LookDetectionMaximumDistance))
                            {
                                var dir = pair.Key.transform.position - gameObject.transform.position;
                                var angle = Vector3.Angle(dir, transform.forward);

                                // checking if target is in our eyesight
                                if (angle <= (_overrideHalfAngle ? _overrideHalfAngleValue : DataController.Instance.LookDetectionHalfAngle))
                                {
                                    RaycastHit hit;
                                    var ray = new Ray(_eyes.transform.position, dir);
                                    if (Physics.Raycast(ray, out hit, 1000))
                                    {
                                        var target = hit.transform.gameObject.GetComponent<LookDetectTarget>();
                                        if (target != null && target == pair.Key)
                                        {
                                            // we found our target
                                            if (pair.Value.IsVisible) // continue looking at the target
                                            {
                                                _lookingAtAction?.Invoke(pair.Key);
                                            }
                                            else // just spotted target
                                            {
                                                pair.Value.IsVisible = true;
                                                _foundAction?.Invoke(pair.Key);
                                            }
                                        }
                                        else
                                        {
                                            // we did not found our target
                                            pair.Value.IsVisible = false;
                                            _lostAction?.Invoke(pair.Key);
                                            //Debug.Log(string.Format("{0} {1} {2}", (target != null).ToString(), (target == pair.Key).ToString(), target.Guid));
                                        }
                                        // in either way we need to setup next check

                                        // set targets current position
                                        pair.Value.LastPos = pair.Key.transform.position;
                                        // set time when do next raycast
                                        pair.Value.CheckTime = Time.time + (_overrideRaycastDelay ? _overrideRaycastDelayValue : DataController.Instance.LookDetectionRaycastDelay);
                                    }
                                    else
                                    {
                                        // we did not found our target
                                        pair.Value.IsVisible = false;
                                        _lostAction?.Invoke(pair.Key);
                                    }
                                }
                                else
                                {
                                    // we did not found our target
                                    pair.Value.IsVisible = false;
                                    _lostAction?.Invoke(pair.Key);
                                }
                            }
                            else
                            {
                                // we did not found our target
                                pair.Value.IsVisible = false;
                                _lostAction?.Invoke(pair.Key);
                            }
                        }
                    }
                    else
                    {
                        // our target is out of bounds
                        pair.Value.IsVisible = false;
                        _lostAction?.Invoke(pair.Key);
                    }
                }
            }
            else
            {
                // if we are not in camera view and needed to be ignored, then we did not see anything
                foreach (var pair in _data)
                {
                    pair.Value.IsVisible = false;
                    _lostAction?.Invoke(pair.Key);
                }
            }
        }

        _lastPos = gameObject.transform.position;
        _lastRot = gameObject.transform.rotation;
    }


    /// <summary>
    /// Adding look target, that we will check if we can see
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(LookDetectTarget target)
    {
        if (_data == null)
        {
            _data = new Dictionary<LookDetectTarget, LookDetectData>();
        }

        if (!_data.ContainsKey(target))
        {
            _data.Add(target, new LookDetectData() { IsVisible = false, ObjCollider = target.gameObject.GetComponent<Collider>() });
        }
        else
        {
            Debug.LogError("Trying to add same target second time !!!");
        }

    }

    public void Init(Action<LookDetectTarget> foundTarget, Action<LookDetectTarget> lostTarget, Action<LookDetectTarget> lookingAtTarget)
    {
        _foundAction = foundTarget;
        _lostAction = lostTarget;
        _lookingAtAction = lookingAtTarget;
    }

    #region Fields

    [SerializeField, Tooltip("Override Maximum distance")]
    private bool _overrideMaximumDistance = false;

    [SerializeField, Tooltip("Override Maximum Distance value")]
    private float _overrideMaximumDistanceValue;

    [SerializeField, Tooltip("Override Half Angle")]
    private bool _overrideHalfAngle = false;

    [SerializeField, Tooltip("Override Half Angle value")]
    private float _overrideHalfAngleValue;

    [SerializeField, Tooltip("Override Raycast Delay")]
    private bool _overrideRaycastDelay = false;

    [SerializeField, Tooltip("Override Raycast Delay value")]
    private float _overrideRaycastDelayValue;

    private Dictionary<LookDetectTarget, LookDetectData> _data;

    private Vector3 _lastPos;

    private Quaternion _lastRot;

    [SerializeField, Tooltip("Raycast point or eyes of teh character")]
    private GameObject _eyes;

    /// <summary>
    /// Happens when found target
    /// </summary>
    private Action<LookDetectTarget> _foundAction;
    /// <summary>
    /// Happens when lost target
    /// </summary>
    private Action<LookDetectTarget> _lostAction;
    /// <summary>
    /// Happens while looking at target
    /// </summary>
    private Action<LookDetectTarget> _lookingAtAction;

    private Collider _collider;

    #endregion

    #region Properties

    public GameObject Eyes => _eyes;

    #endregion

}
