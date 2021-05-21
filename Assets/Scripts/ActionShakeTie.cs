using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ネクタイ揺れ
/// </summary>
public class ActionShakeTie : CharacterAction
{
    /// <summary>
    /// パラメータリスト
    /// </summary>
    private ParameterList ParamList = new ParameterList();

    /// <summary>
    /// パラメータの取り得る範囲の絶対値
    /// </summary>
    private static readonly float ParameterRangeAbs = 15.0f;

    /// <summary>
    /// 経過時間
    /// </summary>
    private float ElapsedTime = 0.0f;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="ParamList">パラメータリスト</param>
    public ActionShakeTie(ParameterList ParamList)
    {
        this.ParamList = ParamList;
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void OnUpdate()
    {
        ElapsedTime += Time.deltaTime;
        float Value = Mathf.Sin(ElapsedTime) * ParameterRangeAbs;
        ParamList.SetValue(Value, 0.3f);
    }
}
