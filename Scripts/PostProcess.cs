using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Oeconomica
{
    public class PostProcess : MonoBehaviour
    {
        private Camera AttCam;
        private Camera TempCam;
        public Shader PostOutline;
        public Shader DrawSimple;
        private Material PostMaterial;

        // Use this for initialization
        private void Start()
        {
            AttCam = GetComponent<Camera>();
            TempCam = new GameObject().AddComponent<Camera>();
            TempCam.enabled = false;
            PostMaterial = new Material(PostOutline);
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            TempCam.CopyFrom(AttCam);
            TempCam.clearFlags = CameraClearFlags.Color;
            TempCam.backgroundColor = Color.black;

            TempCam.cullingMask = 1 << LayerMask.NameToLayer("Outline");

            RenderTexture render = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.R8);

            render.Create();

            TempCam.targetTexture = render;

            TempCam.RenderWithShader(DrawSimple, "");

            Graphics.Blit(render, destination, PostMaterial);

            render.Release();
        }
    }
}
