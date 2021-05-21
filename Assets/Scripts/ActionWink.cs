using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;
using UniRx;
using System;
using UniRx.Triggers;

/// <summary>
/// ウインク
/// </summary>
public class ActionWink : CharacterAction
{
    /// <summary>
    /// 目の開き具合
    /// </summary>
    private CubismParameter EyeParam = null;

    /// <summary>
    /// 経過時間
    /// </summary>
    private float ElapsedTime = 0.0f;

    /// <summary>
    /// 目を閉じきるまでの時間
    /// </summary>
    private float EyeCloseTime = 0.15f;

    /// <summary>
    /// 目を開いているか？
    /// </summary>
    private bool bIsOpening = false;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="EyeParam">目のパラメータ</param>
    public ActionWink(CubismParameter EyeParam)
    {
        this.EyeParam = EyeParam;
        IsActive = false;
    }

    /// <summary>
    /// アクティブ状態が変化した
    /// </summary>
    protected override void OnActiveFlagChanged()
    {
        if (IsActive)
        {
            bIsOpening = false;
            ElapsedTime = 0.0f;
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void OnUpdate()
    {
        if (ElapsedTime >= EyeCloseTime)
        {
            EyeParam.Value = 0.0f;      // 動いていない間にデフォルト値に戻っているのか、ここで維持する必要がある。
            return;
        }

        ElapsedTime = Mathf.Min(ElapsedTime + Time.deltaTime, EyeCloseTime);
        float Value = (ElapsedTime / EyeCloseTime);
        if (!bIsOpening)
        {
            Value = 1.0f - Value;
        }
        EyeParam.Value = Value;

        if (ElapsedTime >= EyeCloseTime)
        {
            if (!bIsOpening)
            {
                Observable.Timer(TimeSpan.FromSeconds(2.0))
                    .Subscribe((_) =>
                    {
                        bIsOpening = true;
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
}
