using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Model of the Legend data. It is used to mark element that need to have Legend and set what to say in it
/// </summary>
public class LegendModel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (UIController.IsInitialized) UIController.Instance.AddUIModel(this);
    }

    void OnEnable()
    {
        if (UIController.IsInitialized) UIController.Instance.AddUIModel(this);
    }

    void OnDisable()
    {
        if (UIController.IsInitialized) UIController.Instance.RemoveUIModel(this);
    }

    #region Fields

    [SerializeField, Tooltip("Legend Text")]
    private string _text;

    #endregion

    #region Properties

    public string Text => _text;

    #endregion
}
