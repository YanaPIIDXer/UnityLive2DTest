using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;

/// <summary>
/// リップシンク
/// 所謂「口パク」
/// </summary>
public class ActionLipSync : CharacterAction
{
    /// <summary>
    /// 音声入力ソース
    /// </summary>
    private AudioSource Source = null;

    /// <summary>
    /// 口パク用パラメータ
    /// </summary>
    private CubismParameter LipParam = null;

    /// <summary>
    /// 現在のボリューム
    /// </summary>
    private float CurrentVolume = 0.0f;

    /// <summary>
    /// 速度
    /// </summary>
    private float Velocity = 0.0f;

    /// <summary>
    /// 強さ
    /// </summary>
    private static float Power = 40.0f;

    /// <summary>
    /// 閾値
    /// </summary>
    private static readonly float Threshold = 0.1f;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="Source">音声入力ソース</param>
    /// <param name="LipParam">口パクパラメータ</param>
    public ActionLipSync(AudioSource Source, CubismParameter LipParam)
    {
        this.Source = Source;
        this.LipParam = LipParam;

        this.Source.clip = Microphone.Start(null, true, 1, 44100);
        while (Microphone.GetPosition(null) < 0) { }
        this.Source.Play();
        this.Source.loop = true;
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void OnUpdate()
    {
        float TargetVolume = GetAveragedVolume() * Power;
        TargetVolume = TargetVolume < Threshold ? 0 : TargetVolume;
        CurrentVolume = Mathf.SmoothDamp(CurrentVolume, TargetVolume, ref Velocity, 0.05f);

        LipParam.Value = Mathf.Clamp01(CurrentVolume);
    }

    /// <summary>
    /// 平均ボリューム取得
    /// </summary>
    /// <returns>平均ボリューム</returns>
    private float GetAveragedVolume()
    {
        float[] data = new float[256];
        float a = 0;
        Source.GetOutputData(data, 0);
        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }
        return a / 255.0f;
    }
}
