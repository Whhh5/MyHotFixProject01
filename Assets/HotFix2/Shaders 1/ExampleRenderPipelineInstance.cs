using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ExampleRenderPipelineInstance : RenderPipeline
{
    private ExampleRenderPipelineAsset renderPipelineAsset;
    public ExampleRenderPipelineInstance(ExampleRenderPipelineAsset asset)
    {
        renderPipelineAsset = asset;
    }
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        Debug.Log(renderPipelineAsset.exampleString);
        var cmd = new CommandBuffer();
        cmd.ClearRenderTarget(true, true, Color.black);
        context.ExecuteCommandBuffer(cmd);
        cmd.Release();

        foreach (Camera camera in cameras)
        {
            camera.TryGetCullingParameters(out ScriptableCullingParameters cillingParameters);

            var cullingResults = context.Cull(ref cillingParameters);

            context.SetupCameraProperties(camera);

            ShaderTagId shaderTagId = new ShaderTagId("Example");

            var sortingSettings = new SortingSettings(camera);

            DrawingSettings drawingSettings = new DrawingSettings(shaderTagId, sortingSettings);

            FilteringSettings filteringSettings = FilteringSettings.defaultValue;

            context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

            if (camera.clearFlags == CameraClearFlags.Skybox && RenderSettings.skybox != null)
            {
                context.DrawSkybox(camera);
            }

            context.Submit();
        }
    }
}