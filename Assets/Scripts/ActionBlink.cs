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
        Observable.Timer(TimeSpan.FromSeconds(UnityEngine.Random.Range(0.1f, 5.0f)))
            .SkipWhile((_) => bIsBlinking)
            .Subscribe((_) =>
            {
                bIsBlinking = true;
                ElapsedTime = 0.0f;
            });
    }
}
