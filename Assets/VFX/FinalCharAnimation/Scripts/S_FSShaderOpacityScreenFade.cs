using UnityEngine;

[ExecuteAlways]
public class S_FSShaderParameterController : MonoBehaviour
{
    [Header("Material Settings")]
    public Material material; // Asigna aqu� tu Material del Shader
    public string parameterName = "_Threshold"; // Nombre del par�metro del Shader

    [Header("Shader Parameter")]
    [SerializeField, Range(0f, 1f)]
    private float parameterValue;

    public float ParameterValue
    {
        get => parameterValue;
        set
        {
            parameterValue = value;
            ApplyToMaterial();
            Debug.Log($"ParameterValue actualizado a: {parameterValue}");
        }
    }

    private void ApplyToMaterial()
    {
        if (material != null && material.HasProperty(parameterName))
        {
            material.SetFloat(parameterName, parameterValue);
        }
        else
        {
            Debug.LogWarning($"Material o par�metro '{parameterName}' no encontrado.");
        }
    }

    private void LateUpdate()
    {
        // Garantiza que el Shader se actualiza en cada frame
        ApplyToMaterial();
    }

    private void OnValidate()
    {
        // Actualiza el Shader tambi�n en el Editor
        ApplyToMaterial();
    }
}
