using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲッダン☆
/// ゆーれるまーわるふーれるせつなーいきもちー
/// </summary>
public class ActionPromise : CharacterAction
{
    /// <summary>
    /// Transformリスト
    /// </summary>
    private List<Transform> Transforms = null;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="Transforms">Transformリスト</param>
    public ActionPromise(List<Transform> Transforms)
    {
        this.Transforms = Transforms;
        IsActive = false;
    }

    /// <summary>
    /// アクティブフラグが変更された
    /// </summary>
    protected override void OnActiveFlagChanged()
    {
        if (!IsActive)
        {
            foreach (var Tr in Transforms)
            {
                Tr.position = Vector3.zero;
                Tr.eulerAngles = Vector3.zero;
            }
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void OnUpdate()
    {
        foreach (var Tr in Transforms)
        {
            Tr.position = new Vector3(0.0f, 0.3f, 0.0f);
            Tr.eulerAngles = new Vector3(UnityEngine.Random.Range(0.0f, 360.0f), 0.0f, UnityEngine.Random.Range(0.0f, 360.0f));
        }
    }
}
