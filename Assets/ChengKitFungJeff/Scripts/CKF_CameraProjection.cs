using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class CKF_CameraProjection : MonoBehaviour
{
    private Camera selfCamera;
    public float FOV = 90;
    public Transform targetTransform;
    public Image targetImage;
    public Renderer targetRenderer;
    public int targetMaterialIndex;
    public SupportedShader shaderType = SupportedShader.Custom;
    public string targetTextureName = "";
    public RenderDepth renderDepth = RenderDepth.bit_24;
    public Vector2Int bufferResolution = new(256, 256);
    private Material targetMaterial;
    public bool materialMirrorX, materialMirrorY, renderMirrorX, renderMirrorY;
    public Vector3 surfaceWidthDirection = Vector3.right, surfaceHeightDirection = Vector3.up;
    public Vector2 surfaceSize = Vector2.one;
    private int appliedRenderTextureSizeX = -1, appliedRenderTextureSizeY = -1;
    public Vector3 renderOffset;
    private Texture2D renderTexture2D = null;
    private Rect renderTextureRect = default;
    private readonly Vector2 CENTER = new(0.5f, 0.5f);

    public enum SupportedShader
    {
        LitModUVMat,
        Custom
    }

    public enum RenderDepth {
        bit_0,
        bit_16,
        bit_24,
        bit_32
    }

    private static readonly Dictionary<RenderDepth, int> mapRenderDepth
        = new() {
            { RenderDepth.bit_0,0 },
            { RenderDepth.bit_16,16 },
            { RenderDepth.bit_24,24 },
            { RenderDepth.bit_32,32 },
        };

    private void Awake()
    {
        selfCamera = GetComponent<Camera>();
        selfCamera.enabled = true;
        if (targetRenderer)targetMaterial = targetRenderer.materials[targetMaterialIndex];
        if (shaderType == SupportedShader.LitModUVMat) targetTextureName = "_BaseMap";
        UpdateMaterialMirror();
        Update();
    }

    public void SetMaterialMirrorX(bool value) {
        materialMirrorX = value;
        UpdateMaterialMirror();
    }

    public void SetMaterialMirrorY(bool value)
    {
        materialMirrorY = value;
        UpdateMaterialMirror();
    }

    public void SetRenderMirrorX(bool value) => renderMirrorX = value;
    public void SetRenderMirrorY(bool value) => renderMirrorY = value;

    private void UpdateMaterialMirror() {
        if (materialMirrorX || materialMirrorY)
            if(shaderType == SupportedShader.LitModUVMat)
            {
                targetMaterial.SetVector("_Multiplier", new(materialMirrorX ? -1 : 1, materialMirrorY ? -1 : 1));
                targetMaterial.SetVector("_Adder", new(materialMirrorX ? 1 : 0, materialMirrorY ? 1 : 0));
            }
    }

    private void Update()
    {
        int 
            nextRenderTextrueSizeX
            = Mathf.RoundToInt(Vector3.Dot(targetTransform.lossyScale, surfaceWidthDirection.normalized) * surfaceSize.x * bufferResolution.x),
            nextRenderTextrueSizeY
            = Mathf.RoundToInt(Vector3.Dot(targetTransform.lossyScale, surfaceHeightDirection.normalized) * surfaceSize.y * bufferResolution.y);
        bool sizeChanged = false;
        if (appliedRenderTextureSizeX != nextRenderTextrueSizeX || appliedRenderTextureSizeY != nextRenderTextrueSizeY) {
            sizeChanged = true;
            appliedRenderTextureSizeX = nextRenderTextrueSizeX;
            appliedRenderTextureSizeY = nextRenderTextrueSizeY;
            Destroy(selfCamera.targetTexture);
            selfCamera.targetTexture
                = new(appliedRenderTextureSizeX, appliedRenderTextureSizeY,
                mapRenderDepth[renderDepth],
                targetImage ? UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_SRGB :UnityEngine.Experimental.Rendering.GraphicsFormat.R16G16B16A16_SFloat);
            selfCamera.fieldOfView = FOV;
            if (targetRenderer && targetMaterial.HasTexture(targetTextureName))
                targetMaterial.SetTexture(targetTextureName, selfCamera.targetTexture);
        }
        if (targetImage)
        {
            RenderTexture preActive = RenderTexture.active;
            RenderTexture.active = selfCamera.targetTexture;
            selfCamera.Render();
            if (sizeChanged)
            {
                Destroy(renderTexture2D);
                renderTexture2D
                    = new(selfCamera.targetTexture.width, selfCamera.targetTexture.height,
                    UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_SRGB,
                    UnityEngine.Experimental.Rendering.TextureCreationFlags.None);
                renderTextureRect = new(0, 0, selfCamera.targetTexture.width, selfCamera.targetTexture.height);
            }
            renderTexture2D.ReadPixels(renderTextureRect, 0, 0);
            renderTexture2D.Apply();
            RenderTexture.active = preActive;
            targetImage.sprite = Sprite.Create(renderTexture2D, renderTextureRect, CENTER);
        }
    }

    Matrix4x4 preSelfCameraWorldToCameraMatrix = default;
    private bool preInvertCulling = false;
    private void OnPreRender()
    {
        if(renderMirrorX || renderMirrorY)
        {
            preSelfCameraWorldToCameraMatrix = selfCamera.worldToCameraMatrix;
            preInvertCulling = GL.invertCulling;
            GL.invertCulling = renderMirrorX != renderMirrorY;
            selfCamera.worldToCameraMatrix = Matrix4x4.Scale(new(renderMirrorX ? -1 : 1, renderMirrorY ? -1 : 1, 1)) * selfCamera.worldToCameraMatrix * Matrix4x4.Translate(-renderOffset);
        }
        
    }

    void OnPostRender()
    {
        if (renderMirrorX || renderMirrorY)
        {
            selfCamera.worldToCameraMatrix = preSelfCameraWorldToCameraMatrix;
            GL.invertCulling = preInvertCulling;
        }
    }

    private void OnEnable()
    {
        RenderPipelineManager.beginCameraRendering += beginCameraRendering;
        RenderPipelineManager.endCameraRendering += endCameraRendering;
    }
    private void OnDisable()
    {
        RenderPipelineManager.beginCameraRendering -= beginCameraRendering;
        RenderPipelineManager.endCameraRendering -= endCameraRendering;
    }
    private void beginCameraRendering(ScriptableRenderContext src, Camera camera)
    {
        if (camera == selfCamera)
            OnPreRender();
    }
    private void endCameraRendering(ScriptableRenderContext src, Camera camera)
    {
        if (camera == selfCamera)
            OnPostRender();
    }
}
