using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character data overlooking visual look (shader changes)
/// </summary>
public class CharacterModel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (_renderer != null)
        {
            _material = _renderer.material;
        }
        _detectedTargets = new List<LookDetectTarget>();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (_detectedTargets != null && _detectedTargets.Count > 0)
        {
            foreach (var target in _detectedTargets)
            {
                Debug.DrawRay(_lookDetection.Eyes.transform.position, target.transform.position - gameObject.transform.position, _rayColor);
            }
        }
#endif
    }

    public void FoundTarget(LookDetectTarget target)
    {
        _detectedTargets.Add(target);
        SetSelection(true);
    }

    public void NotFoundTarget(LookDetectTarget target)
    {
        _detectedTargets.Remove(target);
        if (_detectedTargets.Count == 0)
        {
            SetSelection(false);
        }
    }

    private void SetSelection(bool selection)
    {
        if (_material != null)
        {
            _material.SetFloat("_lookDetection", selection ? 1 : 0);
        }
    }

    // used to override ray color
    public void SetRayColor(Color c)
    {
        _rayColor = c;
    }

    #region Fields

    [SerializeField, Tooltip("Character renderer")]
    private Renderer _renderer;

    private Material _material;

    [SerializeField, Tooltip("Reference to Look Detection")]
    private LookDetection _lookDetection;

    [SerializeField, Tooltip("Reference to Look target")]
    private LookDetectTarget _lookDetectTarget;

    private List<LookDetectTarget> _detectedTargets;

    [SerializeField, Tooltip("Color of the ray when debug is enabled")]
    private Color _rayColor = Color.red;

    #endregion

    #region Propertiews

    public LookDetection LookDetection => _lookDetection;

    public LookDetectTarget LookDetectTarget => _lookDetectTarget;

    #endregion
}
