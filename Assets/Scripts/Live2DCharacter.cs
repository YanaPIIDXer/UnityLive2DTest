using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;
using UniRx;
using System;
using UniRx.Operators;

/// <summary>
/// Live2Dのキャラクタ
/// </summary>
public class Live2DCharacter : MonoBehaviour
{
    /// <summary>
    /// 左目の開き具合
    /// </summary>
    [SerializeField]
    private CubismParameter LeftEyeOpen = null;

    /// <summary>
    /// 右目の開き具合
    /// </summary>
    [SerializeField]
    private CubismParameter RightEyeOpen = null;

    void Awake()
    {
        // これでウインクになるはず
        LeftEyeOpen.Value = 0.0f;
        RightEyeOpen.Value = 1.0f;
    }
}
