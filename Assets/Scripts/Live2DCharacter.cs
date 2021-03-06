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
    /// カスタム用Material
    /// </summary>
    [SerializeField]
    private Material CustomMaterial = null;

    /// <summary>
    /// セットアップ
    /// </summary>
    /// <param name="InputEvents">入力イベントインタフェース</param>
    [Inject]
    private void Setup(IKeyInput InputEvents)
    {
        Model = GetComponent<CubismModel>();
        CollectParameters();
        SetupTransformss();

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

        var FrontHairParams = new ParameterList();
        FrontHairParams.Add(Parameters["ParamHairFront"]);
        FrontHairParams.Add(Parameters["ParamHairFront2"]);
        var FrontHair = new ActionPhysics(FrontHairParams, 0.3f, 0.1f);

        var BackHairParams = new ParameterList();
        BackHairParams.Add(Parameters["ParamHairBack"]);
        BackHairParams.Add(Parameters["ParamHairBackL8"]);
        BackHairParams.Add(Parameters["ParamHairBackR6"]);
        var BackHair = new ActionPhysics(BackHairParams, 0.3f, 0.1f);

        var Wink = new ActionWink(Parameters["ParamEyeLOpen"]);
        Wink.OnComplete
            .Subscribe((_) => Blink.IsActive = true)
            .AddTo(gameObject);
        InputEvents.Wink
            .Subscribe((_) =>
            {
                Blink.IsActive = false;
                Wink.IsActive = true;
            })
            .AddTo(gameObject);

        List<Transform> Transes = new List<Transform>();
        foreach (var Tr in Transforms.Values)
        {
            Transes.Add(Tr);
        }
        var Getdown = new ActionPromise(Transes);
        InputEvents.Promise
            .Subscribe((_) => Getdown.IsActive = !Getdown.IsActive)
            .AddTo(gameObject);

        List<CubismParameter> EyesParams = new List<CubismParameter>();
        EyesParams.Add(Parameters["ParamEyeLOpen"]);
        EyesParams.Add(Parameters["ParamEyeROpen"]);
        List<CubismParameter> PositiveParams = new List<CubismParameter>();
        PositiveParams.Add(Parameters["ParamEyeLSmile"]);
        PositiveParams.Add(Parameters["ParamEyeRSmile"]);
        PositiveParams.Add(Parameters["ParamMouthOpenY"]);
        PositiveParams.Add(Parameters["ParamMouthForm"]);
        PositiveParams.Add(Parameters["ParamEyeLSmile"]);
        List<CubismParameter> NegativeParams = new List<CubismParameter>();
        NegativeParams.Add(Parameters["ParamBrowLY"]);
        NegativeParams.Add(Parameters["ParamBrowRY"]);
        var Smile = new ActionSmile(Parameters["ParamAngleZ"], Parameters["ParamBodyAngleZ"], EyesParams, PositiveParams, NegativeParams);
        InputEvents.Smile
            .Subscribe((_) =>
            {
                Smile.IsActive = true;
                Blink.IsActive = false;
            })
            .AddTo(gameObject);
        Smile.OnComplete
            .Subscribe((_) => Blink.IsActive = true)
            .AddTo(gameObject);

        var OddEye = new ActionOddEye(Transforms["D_EYE_BALL_01"].GetComponent<MeshRenderer>().material);
        InputEvents.OddEye
            .Subscribe((_) => OddEye.Toggle())
            .AddTo(gameObject);

        AddAction(Blink);
        AddAction(Breath);
        AddAction(LipSync);
        AddAction(Ties);
        AddAction(LeftHair);
        AddAction(RightHair);
        AddAction(FrontHair);
        AddAction(BackHair);
        AddAction(Wink);
        AddAction(Getdown);
        AddAction(Smile);
        AddAction(OddEye);
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
            Parameters.Add(Obj.name, Param);
        }
    }

    /// <summary>
    /// Transformのセットアップ
    /// </summary>
    private void SetupTransformss()
    {
        var RootTransform = transform.Find("Drawables");
        Transforms.Add("Root", RootTransform);
        for (var i = 0; i < RootTransform.childCount; i++)
        {
            var Trans = RootTransform.GetChild(i);
            Transforms.Add(Trans.gameObject.name, Trans);

            var Mat = new Material(CustomMaterial);
            Trans.GetComponent<MeshRenderer>().material = Mat;
            // HACK:ここでは_MainTexがまだ取れないらしい
            //      仕方ないので2048*2048決め打ち（どの道このソースもミクのAssetありきだし）
            Mat.SetFloat("_TexelX", 1.0f / 2048);
            Mat.SetFloat("_TexelY", 1.0f / 2048);
        }
    }
}
