using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物理揺れ
/// </summary>
public class ActionPhysics : CharacterAction
{
    /// <summary>
    /// パラメータリスト
    /// </summary>
    private ParameterList ParamList = new ParameterList();

    /// <summary>
    /// 強さ
    /// </summary>
    private float Power = 0.0f;

    /// <summary>
    /// ジョイント間に発生する強さ
    /// </summary>
    private float JointPower = 0.0f;

    /// <summary>
    /// 経過時間
    /// </summary>
    private float ElapsedTime = 0.0f;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="ParamList">パラメータリスト</param>
    /// <param name="Power">強さ</param>
    /// <param name="JointPower">ジョイント間に発生する強さ</param>
    public ActionPhysics(ParameterList ParamList, float Power, float JointPower = 0.0f)
    {
        this.ParamList = ParamList;
        this.Power = Power;
        this.JointPower = JointPower;
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void OnUpdate()
    {
        ElapsedTime += Time.deltaTime;
        float Value = Mathf.Sin(ElapsedTime) * Power;
        ParamList.SetValue(Value, JointPower);
    }
}
