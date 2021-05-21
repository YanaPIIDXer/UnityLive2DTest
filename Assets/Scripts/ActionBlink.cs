using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;
using UniRx;
using UniRx.Operators;
using System;

/// <summary>
/// 瞬き
/// </summary>
public class ActionBlink : CharacterAction
{
    /// <summary>
    /// 左目
    /// </summary>
    private CubismParameter LeftEyeParam = null;

    /// <summary>
    /// 右目
    /// </summary>
    private CubismParameter RightEyeParam = null;

    /// <summary>
    /// 瞬きをしているか？
    /// </summary>
    private bool bIsBlinking = false;

    /// <summary>
    /// 瞬きにかかる時間
    /// </summary>
    private static readonly float BlinkTime = 0.3f;

    /// <summary>
    /// 経過時間
    /// </summary>
    private float ElapsedTime = 0.0f;
    /// <summary>
    /// インターバルキャンセルオブジェクト
    /// </summary>
    private IDisposable IntervalCancel = null;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="LeftEyeParam">左目</param>
    /// <param name="RightEyeParam">右目</param>
    public ActionBlink(CubismParameter LeftEyeParam, CubismParameter RightEyeParam)
    {
        this.LeftEyeParam = LeftEyeParam;
        this.RightEyeParam = RightEyeParam;

        WaitInterval();
    }

    /// <summary>
    /// アクティブフラグが切り替わった
    /// </summary>
    protected override void OnActiveFlagChanged()
    {
        if (!IsActive)
        {
            LeftEyeParam.Value = 1.0f;
            RightEyeParam.Value = 1.0f;
            if (IntervalCancel != null)
            {
                IntervalCancel.Dispose();
                IntervalCancel = null;
            }
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void OnUpdate()
    {
        if (!bIsBlinking) { return; }

        ElapsedTime = Mathf.Min(ElapsedTime + Time.deltaTime, BlinkTime);
        float Ratio = 1.0f - (ElapsedTime / BlinkTime);
        float Value = Mathf.Abs(1.0f - (Ratio * 2));

        LeftEyeParam.Value = Value;
        RightEyeParam.Value = Value;

        if (ElapsedTime >= BlinkTime)
        {
            bIsBlinking = false;
            WaitInterval();
        }
    }

    /// <summary>
    /// 数秒間待機
    /// </summary>
    private void WaitInterval()
    {
        IntervalCancel = Observable.Timer(TimeSpan.FromSeconds(UnityEngine.Random.Range(0.1f, 5.0f)))
            .SkipWhile((_) => bIsBlinking)
            .Subscribe((_) =>
            {
                bIsBlinking = true;
                ElapsedTime = 0.0f;
            });
    }
}
