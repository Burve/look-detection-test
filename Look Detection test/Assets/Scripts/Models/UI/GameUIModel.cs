using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// In Charge of the references and some logic for Main UI screens
/// </summary>
public class GameUIModel : MonoBehaviour
{
    void Awake()
    {
        _welcomeScreen.SetActive(true);
        Close();
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void CloseWelcomeScreen()
    {
        _welcomeScreen.SetActive(false);
    }

    #region Fields

    [SerializeField, Tooltip("Reference to the welcome screen")]
    private GameObject _welcomeScreen;

    [SerializeField, Tooltip("Reference to the Text that show Screen bounds status")]
    private TextMeshProUGUI _boundsText;

    [SerializeField, Tooltip("Reference to the Text that show Legend status")]
    private TextMeshProUGUI _legendsText;

    #endregion

    #region Properties

    public TextMeshProUGUI BoundsText => _boundsText;

    public TextMeshProUGUI LegendsText => _legendsText;

    #endregion
}
