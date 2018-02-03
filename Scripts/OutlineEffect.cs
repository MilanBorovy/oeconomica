using UnityEngine;
using System.Collections;

namespace Oeconomica.Game.Effects
{
    public class OutlineEffect : MonoBehaviour
    {
        private Camera AttachedCamera;
        public Shader Post_Outline;
        public Shader DrawSimple;
        private Camera TempCam;
        private static Material Post_Mat;
        // public RenderTexture TempRT;


        private void Start()
        {
            AttachedCamera = GetComponent<Camera>();
            TempCam = new GameObject().AddComponent<Camera>();
            TempCam.enabled = false;
            Post_Mat = new Material(Post_Outline);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {

            //set up a temporary camera
            TempCam.CopyFrom(AttachedCamera);
            TempCam.clearFlags = CameraClearFlags.Color;
            TempCam.backgroundColor = Color.black;

            //cull any layer that isn't the outline
            TempCam.cullingMask = 1 << LayerMask.NameToLayer("Outline");

            //make the temporary rendertexture
            RenderTexture TempRT = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.R8);

            //put it to video memory
            TempRT.Create();

            //set the camera's target texture when rendering
            TempCam.targetTexture = TempRT;

            //render all objects this camera can render, but with our custom shader.
            TempCam.RenderWithShader(DrawSimple, "");

            //copy the temporary RT to the final image
            Graphics.Blit(TempRT, destination, Post_Mat);

            //release the temporary RT
            TempRT.Release();
        }

        /// <summary>
        /// Sets color of outline
        /// </summary>
        public static void SetColor(Color color)
        {
            Post_Mat.SetColor("_Color", new Color(Mathf.Floor(color.r), Mathf.Floor(color.g), Mathf.Floor(color.b)));
        }

    }
}