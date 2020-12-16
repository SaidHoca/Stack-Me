using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientBg : MonoBehaviour
{
    public static GradientBg instance;

    public Material gradientMat;
    Renderer mrenderer;
    public Texture[] textures = new Texture[10];

	private void Awake()
	{
        if (instance == null) instance = this;
	}

	public void Start()
    {
        mrenderer = GetComponent<Renderer>();
        mrenderer.material.EnableKeyword("_MainTex");
        SetTextures();
    }


    private void SetTextures()
	{
        int indis = Random.Range(0, 10);
        mrenderer.material.SetTexture("_MainTex", textures[indis]);
        
    }
}
