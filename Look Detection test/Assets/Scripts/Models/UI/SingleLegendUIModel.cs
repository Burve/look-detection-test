using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Container for the Legend UI element
/// </summary>
public class SingleLegendUIModel : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (_model != null)
        {
            gameObject.transform.position = DataController.Instance.MainCamera.WorldToScreenPoint(_model.gameObject.transform.position);
        }
    }

    public void Init(LegendModel model)
    {
        _model = model;
        _text.text = _model.Text;
    }

    #region Fields

    [SerializeField, Tooltip("reference to the text")]
    private TextMeshProUGUI _text;

    private LegendModel _model;

    #endregion

    #region Properties



    #endregion
}
