using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Control All UI components
/// </summary>
public class UIController : MonoSingleton<UIController>
{
    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        DataController.Instance.Input.UI.Legend.performed += ctx => SwitchLegendUI();
        DataController.Instance.Input.UI.ScreenBounds.performed += ctx => SwitchScreenBounds();
        DataController.Instance.Input.UI.Enable();
        _gameUiModel.Open();
    }

    // Update is called once per frame
    void Update()
    {
        if (_displayWelcomeScreen)
        {
            var kb = InputSystem.GetDevice<Keyboard>();
            if (kb.anyKey.wasPressedThisFrame)
            {
                DisabelWelcomeScreen();
            }
        }
    }

    public void AddUIModel(LegendModel m)
    {
        if (_uiLegendModels == null) _uiLegendModels = new List<LegendModel>();
        if (!_uiLegendModels.Contains(m))
        {
            _uiLegendModels.Add(m);
        }
    }

    public void RemoveUIModel(LegendModel m)
    {
        if (_uiLegendModels != null && _uiLegendModels.Contains(m))
        {
            _uiLegendModels.Remove(m);
        }
    }

    private void SwitchLegendUI()
    {
        if (_legendUiModel != null)
        {
            if (_legendUiModel.gameObject.activeSelf)
            {
                _legendUiModel.Close();
                _gameUiModel.LegendsText.text = "Legend Hidden";
            }
            else
            {
                _legendUiModel.Open();
                _gameUiModel.LegendsText.text = "Legend Visible";

            }
        }
    }

    private void DisabelWelcomeScreen()
    {
        if (_gameUiModel != null)
        {
            _gameUiModel.CloseWelcomeScreen();
        }
        _displayWelcomeScreen = false;
    }

    private void SwitchScreenBounds()
    {
        DataController.Instance.DetectOutsideCameraView = !DataController.Instance.DetectOutsideCameraView;
        _gameUiModel.BoundsText.text = !DataController.Instance.DetectOutsideCameraView ? "Bounds Active" : "Bounds Not Active";
    }

    #region Fields

    [SerializeField, Tooltip("Reference to the GameUIModel")]
    private GameUIModel _gameUiModel;

    [SerializeField, Tooltip("Reference to the LegendUIModel")]
    private LegendUIModel _legendUiModel;

    private List<LegendModel> _uiLegendModels;

    private bool _displayWelcomeScreen = true;

    #endregion

    #region Properties

    public List<LegendModel> UiLegendModels => _uiLegendModels;

    #endregion
}
