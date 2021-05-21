using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Webカメラ表示
/// </summary>
public class WebCameraDisplay : MonoBehaviour
{
    void Awake()
    {
        var Device = WebCamTexture.devices[0];
        var Tex = new WebCamTexture(Device.name, 1920, 1080, 60);
        var TextureRenderer = GetComponent<Renderer>();
        TextureRenderer.material.mainTexture = Tex;
        Tex.Play();
    }
}
