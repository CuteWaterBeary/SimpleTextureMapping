﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.IO;

public class MainController : MonoBehaviour {

    public SkinnedMeshRenderer TargetRenderer;
    public Material TestShowMaterial;

    private RenderTexture m_RenderTexture;
    private CommandBuffer m_CommandBuffer;
    private Material m_TextureMappingDrawMaterial;

	// Use this for initialization
	void Start () {
        m_RenderTexture = RenderTexture.GetTemporary(1024, 1024, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB, 1);
        m_CommandBuffer = new CommandBuffer();
        m_CommandBuffer.name = "texture mapping commandbuffer";

        Shader textureMapping = Shader.Find("Whacka/TextureMapping");
        m_TextureMappingDrawMaterial = new Material(textureMapping);

        if (TestShowMaterial != null)
        {
            TestShowMaterial.mainTexture = m_RenderTexture;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 0, 200, 200));
        if (GUILayout.Button("Render TextureMapping!"))
        {
            // get info
            Mesh mesh = TargetRenderer.sharedMesh;
            Texture mainTex = TargetRenderer.sharedMaterial.mainTexture;

            // setup command buffer
            m_TextureMappingDrawMaterial.mainTexture = mainTex;
            m_CommandBuffer.DrawMesh(mesh, Matrix4x4.identity, m_TextureMappingDrawMaterial);

            // excute render
            Graphics.SetRenderTarget(m_RenderTexture);
            Graphics.ExecuteCommandBuffer(m_CommandBuffer);

            // save snapshot as result
            Texture2D tex2d = new Texture2D(1024, 1024);
            tex2d.ReadPixels(new Rect(0, 0, 1024, 1024), 0, 0);
            File.WriteAllBytes("Assets/Sample/Out/TextureMapping-Result.png", tex2d.EncodeToPNG());
        }
        GUILayout.EndArea();
    }
}
