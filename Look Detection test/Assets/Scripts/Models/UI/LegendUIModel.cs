using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// In Charge of the references and some logic for Legend UI screens
/// </summary>
public class LegendUIModel : MonoBehaviour
{
    void Awake()
    {
        Close();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Open()
    {
        gameObject.SetActive(true);
        if (_singleLegendUiModels == null)
        {
            _singleLegendUiModels = new List<SingleLegendUIModel>();
        }

        foreach (var model in UIController.Instance.UiLegendModels)
        {
            var go = Instantiate(_prefab, Vector3.zero, Quaternion.identity) as SingleLegendUIModel;
            go.gameObject.transform.SetParent(this.gameObject.transform);
            go.gameObject.transform.localScale = Vector3.one;
            go.Init(model);
            _singleLegendUiModels.Add(go);
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
        if (_singleLegendUiModels != null && _singleLegendUiModels.Count > 0)
        {
            foreach (var model in _singleLegendUiModels)
            {
                Destroy(model.gameObject);
            }
            _singleLegendUiModels.Clear();
        }
    }

    #region Fields

    [SerializeField, Tooltip("Single Legend Element Prefab")]
    private SingleLegendUIModel _prefab;

    private List<SingleLegendUIModel> _singleLegendUiModels;

    #endregion

    #region Properties



    #endregion
}
