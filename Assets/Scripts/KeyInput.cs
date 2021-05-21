using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UniRx.Triggers;

/// <summary>
/// キー入力インタフェース
/// </summary>
public interface IKeyInput
{
    /// <summary>
    /// ウインク
    /// </summary>
    IObservable<Unit> Wink { get; }
}

/// <summary>
/// キー入力
/// </summary>
public class KeyInput : MonoBehaviour, IKeyInput
{
    /// <summary>
    /// ウインク
    /// </summary>
    public IObservable<Unit> Wink => Observable.EveryUpdate()
                                        .Where((_) => Input.GetKey(KeyCode.W))
                                        .Select((_) => Unit.Default);
}
