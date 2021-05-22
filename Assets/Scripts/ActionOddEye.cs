using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オッドアイアクション
/// </summary>
public class ActionOddEye : CharacterAction
{
    /// <summary>
    /// 眼のマテリアル
    /// </summary>
    private Material EyeMaterial = null;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="EyeMaterial">眼のマテリアル</param>
    public ActionOddEye(Material EyeMaterial)
    {
        this.EyeMaterial = EyeMaterial;
        IsActive = false;
    }

    /// <summary>
    /// アクティブフラグが変更された
    /// </summary>
    protected override void OnActiveFlagChanged()
    {
        Color BlendColor = Color.white;
        if (IsActive)
        {
            BlendColor = new Color(0, 0, 255, 255);
        }
        EyeMaterial.SetColor("_BlendColor", BlendColor);
    }
}
