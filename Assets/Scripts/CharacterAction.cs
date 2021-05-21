using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// キャラクタのアクション基底クラス
/// </summary>
public abstract class CharacterAction
{
    /// <summary>
    /// 有効か？
    /// </summary>
    public bool IsActive
    {
        get { return bIsActive; }
        set
        {
            bIsActive = value;
            OnActiveFlagChanged();
        }
    }

    /// <summary>
    /// 有効か？
    /// </summary>
    private bool bIsActive = true;

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        if (IsActive)
        {
            OnUpdate();
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected virtual void OnUpdate() { }

    /// <summary>
    /// アクティブフラグが変更された
    /// </summary>
    protected virtual void OnActiveFlagChanged() { }

    /// <summary>
    /// アクションが終了した時のSubject
    /// </summary>
    protected Subject<Unit> OnCompleteSubject { get; private set; } = new Subject<Unit>();

    /// <summary>
    /// アクションが終了した
    /// </summary>
    public IObservable<Unit> OnComplete { get { return OnCompleteSubject; } }
}
