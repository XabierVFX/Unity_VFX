using UnityEngine;

[ExecuteInEditMode] // Permite que el script se ejecute en el Editor
public class S_CharacterParameterController : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    private float opacity = 1.0f; // Valor inicial de Opacity

    [SerializeField]
    private float radius = 5.0f; // Valor inicial del Radio

    [SerializeField]
    private Vector3 position = Vector3.zero; // Posición inicial del centro

    private static readonly int OpacityID = Shader.PropertyToID("_Opacity");
    private static readonly int RadiusID = Shader.PropertyToID("_Radius");
    private static readonly int PositionID = Shader.PropertyToID("_Position");

    void OnValidate()
    {
        // Actualiza los valores en el Editor
        UpdateGlobalParameters();
    }

    void Awake()
    {
        // Asegura que los valores estén configurados al cargar la escena
        UpdateGlobalParameters();
    }

    void Update()
    {
        // Actualiza los valores globales en cada frame
        UpdateGlobalParameters();
    }

    private void UpdateGlobalParameters()
    {
        // Establecer los valores globales
        Shader.SetGlobalFloat(OpacityID, opacity);
        Shader.SetGlobalFloat(RadiusID, radius);
        Shader.SetGlobalVector(PositionID, position);
    }

    public void SetOpacity(float newOpacity)
    {
        opacity = newOpacity;
        UpdateGlobalParameters();
    }

    public void SetRadius(float newRadius)
    {
        radius = newRadius;
        UpdateGlobalParameters();
    }

    public void SetPosition(Vector3 newPosition)
    {
        position = newPosition;
        UpdateGlobalParameters();
    }
}
