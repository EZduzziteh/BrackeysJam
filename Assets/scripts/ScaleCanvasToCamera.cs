using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ScaleCanvasToCamera : MonoBehaviour
{
    [SerializeField] PixelPerfectCamera pixelPerfectCamera;

    CanvasScaler canvasScaler;

    void Awake()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
    }

    void Update()
    {
        canvasScaler.scaleFactor =
            Mathf.Max(1, pixelPerfectCamera.pixelRatio);
    }
}