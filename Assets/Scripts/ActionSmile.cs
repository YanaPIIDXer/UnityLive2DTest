using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;
using UniRx;
using System;

/// <summary>
/// 笑顔
/// </summary>
public class ActionSmile : CharacterAction
{
    /// <summary>
    /// 頭のＺ軸パラメータ
    /// </summary>
    private CubismParameter HeadZParam = null;

    /// <summary>
    /// 体のＺ軸パラメータ
    /// </summary>
    private CubismParameter BodyZParam = null;

    /// <summary>
    /// 目の開き具合のパラメータ群
    /// </summary>
    private List<CubismParameter> EyesParams = null;

    /// <summary>
    /// プラス方向に更新するパラメータ群
    /// </summary>
    private List<CubismParameter> PositiveParams = null;

    /// <summary>
    /// マイナス方向に更新するパラメータ群
    /// </summary>
    private List<CubismParameter> NegativeParams = null;

    /// <summary>
    /// アニメーションにかかる時間
    /// </summary>
    private static readonly float AnimationTime = 0.3f;

    /// <summary>
    /// 経過時間
    /// </summary>
    private float ElapsedTime = 0.0f;

    /// <summary>
    /// 終了中？
    /// </summary>
    private bool bIsClosing = false;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="HeadZParam">頭のＺ軸のパラメータ</param>
    /// <param name="BodyZParam">体のＺ軸のパラメータ</param>
    /// <param name="EyesParams">目の開き具合のパラメータ</param>
    /// <param name="PositiveParams">プラス方向に更新するパラメータ群</param>
    /// <param name="NegativeParams">マイナス方向に更新するパラメータ群</param>
    public ActionSmile(CubismParameter HeadZParam, CubismParameter BodyZParam, List<CubismParameter> EyesParams, List<CubismParameter> PositiveParams, List<CubismParameter> NegativeParams)
    {
        this.HeadZParam = HeadZParam;
        this.BodyZParam = BodyZParam;
        this.EyesParams = EyesParams;
        this.PositiveParams = PositiveParams;
        this.NegativeParams = NegativeParams;
        IsActive = false;
    }

    /// <summary>
    /// アクティブフラグが変更された
    /// </summary>
    protected override void OnActiveFlagChanged()
    {
        if (IsActive)
        {
            ElapsedTime = 0.0f;
            bIsClosing = false;
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void OnUpdate()
    {
        if (ElapsedTime >= AnimationTime)
        {
            SetValue(1.0f);
            return;
        }

        ElapsedTime = Mathf.Min(ElapsedTime + Time.deltaTime, AnimationTime);
        float Value = 1.0f - (ElapsedTime / AnimationTime);
        if (!bIsClosing)
        {
            Value = 1.0f - Value;
        }

        SetValue(Value);

        if (ElapsedTime >= AnimationTime)
        {
            if (!bIsClosing)
            {
                Observable.Timer(TimeSpan.FromSeconds(2.0))
                    .Subscribe((_) =>
                    {
                        bIsClosing = true;
                        ElapsedTime = 0.0f;
                    });
            }
            else
            {
                OnCompleteSubject.OnNext(Unit.Default);
                IsActive = false;
            }
        }
    }

    /// <summary>
    /// 値を設定
    /// </summary>
    /// <param name="Value">値</param>
    private void SetValue(float Value)
    {
        foreach (var Param in PositiveParams)
        {
            Param.Value = Value;
        }
        foreach (var Param in NegativeParams)
        {
            Param.Value = -Value;
        }
        foreach (var Param in EyesParams)
        {
            Param.Value = 1.0f - Value;
        }
        HeadZParam.Value = Value * 30.0f;
        BodyZParam.Value = Value * 10.0f;
    }
}
