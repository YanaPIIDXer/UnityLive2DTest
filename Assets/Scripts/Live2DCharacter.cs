using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;
using UniRx;
using System;
using UniRx.Operators;
using Zenject;

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
    /// 各パーツのTransform
    /// </summary>
    private Dictionary<string, Transform> Transforms = new Dictionary<string, Transform>();

    /// <summary>
    /// アクションリスト
    /// </summary>
    private List<CharacterAction> Actions = new List<CharacterAction>();

    /// <summary>
    /// Model
    /// </summary>
    private CubismModel Model = null;

    /// <summary>
    /// キー入力
    /// </summary>
    [Inject]
    private IKeyInput InputEvents = null;

    void Awake()
    {
        Model = GetComponent<CubismModel>();
        CollectParameters();
        CollectTransforms();

        var Blink = new ActionBlink(Parameters["ParamEyeLOpen"], Parameters["ParamEyeROpen"]);
        var Breath = new ActionBreath(Parameters["ParamBreath"]);
        var LipSync = new ActionLipSync(gameObject.AddComponent<AudioSource>(), Parameters["ParamMouthOpenY"]);

        var TieParams = new ParameterList();
        for (int i = 1; i <= 7; i++)
        {
            string Name = string.Format("Param_Angle_Rotation_{0}_D_BODY_06", i);
            TieParams.Add(Parameters[Name]);
        }
        var Ties = new ActionPhysics(TieParams, 5.0f, 0.3f);

        var LeftHairParams = new ParameterList();
        var RightHairParams = new ParameterList();
        for (var i = 1; i <= 9; i++)
        {
            LeftHairParams.Add(Parameters[string.Format("Param_Angle_Rotation_{0}_D_HAIR_BACK_00", i)]);
            RightHairParams.Add(Parameters[string.Format("Param_Angle_Rotation_{0}_D_HAIR_BACK_10", i)]);
        }
        var LeftHair = new ActionPhysics(LeftHairParams, 7.0f, 0.2f);
        var RightHair = new ActionPhysics(RightHairParams, 7.0f, 0.2f);

        var Wink = new ActionWink(Parameters["ParamEyeLOpen"]);
        Wink.OnComplete
            .Subscribe((_) => Blink.IsActive = true);
        InputEvents.Wink
            .Subscribe((_) =>
            {
                Blink.IsActive = false;
                Wink.IsActive = true;
            });

        List<Transform> Transes = new List<Transform>();
        foreach (var Tr in Transforms.Values)
        {
            Transes.Add(Tr);
        }
        var Getdown = new ActionPromise(Transes);
        InputEvents.Promise
            .Subscribe((_) => Getdown.IsActive = !Getdown.IsActive);

        AddAction(Blink);
        AddAction(Breath);
        AddAction(LipSync);
        AddAction(Ties);
        AddAction(LeftHair);
        AddAction(RightHair);
        AddAction(Wink);
        AddAction(Getdown);
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
            var Param = Obj.GetComponent<CubismParameter>();
            Param.Value = Param.DefaultValue;       // おまじない。これがないとたまにネクタイ揺れが起きなくなる・・・？
            Parameters.Add(Obj.name, Param);
        }
    }

    /// <summary>
    /// Transform収集
    /// </summary>
    private void CollectTransforms()
    {
        var RootTransform = transform.Find("Drawables");
        Transforms.Add("Root", RootTransform);
        for (var i = 0; i < RootTransform.childCount; i++)
        {
            var Trans = RootTransform.GetChild(i);
            Transforms.Add(Trans.gameObject.name, Trans);
        }
    }
}
