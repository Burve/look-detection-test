using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Container to keep track of looked at objects
/// </summary>
public class LookDetectData
{
    #region Fields

    public Vector3 LastPos;

    public float CheckTime;

    public bool IsVisible;

    public Collider ObjCollider;

    #endregion

    #region Properties

    #endregion
}
