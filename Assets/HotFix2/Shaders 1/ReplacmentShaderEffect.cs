using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplacmentShaderEffect : MonoBehaviour
{
    public Color color;
    public float slider;
    public Shader ReplacmentShader;

    private void OnValidate()
    {
        Shader.SetGlobalColor("_Color_1", color);
    }
    private void Update()
    {
        Shader.SetGlobalFloat("_Temp", slider);
    }
    private void OnEnable()
    {
        if (ReplacmentShader != null)
        {
            GetComponent<Camera>().SetReplacementShader(ReplacmentShader, "");
        }
    }
    private void OnDisable()
    {
        GetComponent<Camera>().ResetReplacementShader();
    }
}
