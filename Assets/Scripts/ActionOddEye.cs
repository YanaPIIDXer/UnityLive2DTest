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
    /// 変化前の色
    /// </summary>
    private Color FromColor = Color.white;

    /// <summary>
    /// 変化後の色
    /// </summary>
    private Color ToColor = Color.white;

    /// <summary>
    /// 変化後の色
    /// </summary>
    private static readonly Color ChangedColor = new Color(0.0f, 0.0f, 1.0f, 1.0f);

    /// <summary>
    /// アニメーションの時間
    /// </summary>
    private static float AnimationTime = 1.5f;

    /// <summary>
    /// 経過時間
    /// </summary>
    private float ElapsedTime = 0.0f;

    /// <summary>
    /// オッドアイ化するか？
    /// </summary>
    private bool ToOddEye = false;

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
    /// 切り替え
    /// </summary>
    public void Toggle()
    {
        ToOddEye = !ToOddEye;
        if (ToOddEye)
        {
            FromColor = Color.white;
            ToColor = ChangedColor;
        }
        else
        {
            FromColor = ChangedColor;
            ToColor = Color.white;
        }
        IsActive = true;
    }

    /// <summary>
    /// アクティブフラグが変更された
    /// </summary>
    protected override void OnActiveFlagChanged()
    {
        ElapsedTime = 0.0f;
    }

    /// <summary>
    /// 更新
    /// </summary>
    protected override void OnUpdate()
    {
        ElapsedTime = Mathf.Min(ElapsedTime + Time.deltaTime, AnimationTime);
        float Ratio = ElapsedTime / AnimationTime;
        Color BlendColor = Color.Lerp(FromColor, ToColor, Ratio);
        EyeMaterial.SetColor("_BlendColor", BlendColor);

        if (ElapsedTime >= AnimationTime)
        {
            IsActive = false;
        }
    }
}
