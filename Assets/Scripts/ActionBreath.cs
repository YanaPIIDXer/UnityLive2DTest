using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;
using UniRx;
using UniRx.Operators;
using System;

/// <summary>
/// 息遣い
/// </summary>
public class ActionBreath : CharacterAction
{
    /// <summary>
    /// 息遣いパラメータ
    /// </summary>
    private CubismParameter BreathParam = null;

    /// <summary>
    /// 一周にかかる時間
    /// </summary>
    private static readonly float BreathTime = 1.5f;

    /// <summary>
    /// 経過時間
    /// </summary>
    private float ElapsedTime = 0.0f;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="BreathParam">息遣いパラメータ</param>
    public ActionBreath(CubismParameter BreathParam)
    {
        this.BreathParam = BreathParam;
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void OnUpdate()
    {
        ElapsedTime = Mathf.Min(ElapsedTime + Time.deltaTime, BreathTime);
        float Ratio = 1.0f - (ElapsedTime / BreathTime);
        float Value = Mathf.Abs(1.0f - (Ratio * 2));

        BreathParam.Value = Value;

        if (ElapsedTime >= BreathTime)
        {
            ElapsedTime = 0.0f;
        }
    }
}
