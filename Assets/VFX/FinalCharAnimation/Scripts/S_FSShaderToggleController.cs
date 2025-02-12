using UnityEngine;
using UnityEngine.Rendering.Universal;

public class S_FSShaderToggleController : MonoBehaviour
{
    public Material material; // Material del Shader
    public FullScreenPassRendererFeature rendererFeature; // Asigna aquí tu Renderer Feature

    public void EnableShader()
    {
        if (rendererFeature != null)
        {
            rendererFeature.SetActive(true);
            Debug.Log("Shader activado.");
        }
    }

    public void DisableShader()
    {
        if (rendererFeature != null)
        {
            rendererFeature.SetActive(false);
            Debug.Log("Shader desactivado.");
        }
    }

    public void SetShaderParameter(string parameterName, float value)
    {
        if (material != null && material.HasProperty(parameterName))
        {
            material.SetFloat(parameterName, value);
            Debug.Log($"Parámetro {parameterName} establecido a {value}");
        }
    }
}