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
    }

    void Update()
    {
        foreach (var Act in Actions)
        {
            Act.Update();
        }
        Model.ForceUpdateNow();
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
