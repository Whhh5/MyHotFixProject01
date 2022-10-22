using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/ExampleRenderPipelineAsset")]
public class ExampleRenderPipelineAsset : RenderPipelineAsset
{
    public Color exampleColor;
    public string exampleString;
    protected override RenderPipeline CreatePipeline()
    {
        return new ExampleRenderPipelineInstance(this);
    }
}
