using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Live2D.Cubism.Core;

/// <summary>
/// パラメータをリスト構造で管理するクラス
/// </summary>
public class ParameterList
{
    /// <summary>
    /// パラメータリスト
    /// </summary>
    private List<CubismParameter> Params = new List<CubismParameter>();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ParameterList()
    {
    }

    /// <summary>
    /// 追加
    /// </summary>
    /// <param name="Param">パラメータ</param>
    public void Add(CubismParameter Param)
    {
        Params.Add(Param);
    }

    /// <summary>
    /// 値を設定
    /// </summary>
    /// <param name="Value">価</param>
    /// <param name="Offset">パラメータとパラメータの間に設けるオフセット値</param>
    public void SetValue(float Value, float Offset = 0.0f)
    {
        float AddValue = Value;
        foreach (var Param in Params)
        {
            Param.Value = AddValue;
            AddValue += Offset;
        }
    }
}
