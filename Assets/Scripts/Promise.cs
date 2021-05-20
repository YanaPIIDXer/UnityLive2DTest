using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;

/// <summary>
/// ゲッダン☆
/// </summary>
public class Promise : MonoBehaviour
{
    /// <summary>
    /// DrawableのTransform
    /// </summary>
    private Transform DrawablesTransform = null;

    /// <summary>
    /// Model
    /// </summary>
    private CubismModel Model = null;

    void Awake()
    {
        DrawablesTransform = transform.Find("Drawables");
        Model = GetComponent<CubismModel>();

    }

    void LateUpdate()
    {
        DrawablesTransform.eulerAngles = new Vector3(UnityEngine.Random.Range(0.0f, 360.0f), 0.0f, UnityEngine.Random.Range(0.0f, 360.0f));
        for (var i = 0; i < DrawablesTransform.childCount; i++)
        {
            var Tr = DrawablesTransform.GetChild(i);
            Tr.position = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), 0.0f);
            Tr.eulerAngles = new Vector3(UnityEngine.Random.Range(0.0f, 360.0f), 0.0f, UnityEngine.Random.Range(0.0f, 360.0f));
        }
    }
}
