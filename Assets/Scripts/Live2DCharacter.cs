using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;
using UniRx;
using System;
using UniRx.Operators;

/// <summary>
/// Live2Dのキャラクタ
/// </summary>
public class Live2DCharacter : MonoBehaviour
{
    /// <summary>
    /// パラメータ名とオブジェクトのDictionary
    /// </summary>
    private Dictionary<string, CubismParameter> Parameters = new Dictionary<string, CubismParameter>();

    /// <summary>
    /// アクションリスト
    /// </summary>
    private List<CharacterAction> Actions = new List<CharacterAction>();

    /// <summary>
    /// Model
    /// </summary>
    private CubismModel Model = null;

    void Awake()
    {
        Model = GetComponent<CubismModel>();

        CollectParameters();

        AddAction(new ActionBlink(Parameters["ParamEyeLOpen"], Parameters["ParamEyeROpen"]));
        AddAction(new ActionBreath(Parameters["ParamBreath"]));
        AddAction(new ActionLipSync(gameObject.AddComponent<AudioSource>(), Parameters["ParamMouthOpenY"]));

        var TieParams = new ParameterList();
        for (int i = 1; i <= 7; i++)
        {
            string Name = string.Format("Param_Angle_Rotation_{0}_D_BODY_06", i);
            TieParams.Add(Parameters[Name]);
        }
        AddAction(new ActionPhysics(TieParams, 5.0f, 0.3f));
    }

    void LateUpdate()
    {
        foreach (var Act in Actions)
        {
            Act.Update();
        }
    }

    /// <summary>
    /// アクション追加
    /// </summary>
    /// <param name="Act">アクション</param>
    private void AddAction(CharacterAction Act)
    {
        Actions.Add(Act);
    }

    /// <summary>
    /// パラメータ収集
    /// </summary>
    private void CollectParameters()
    {
        var RootTrans = transform.Find("Parameters");
        for (var i = 0; i < RootTrans.childCount; i++)
        {
            var Obj = RootTrans.GetChild(i).gameObject;
            Parameters.Add(Obj.name, Obj.GetComponent<CubismParameter>());
        }
    }
}
