using UnityEngine;

[ExecuteInEditMode] // Permite que el script se ejecute en el Editor
public class GlobalParameterController : MonoBehaviour
{
    [SerializeField]
    private float radius = 5.0f; // Valor inicial del Radio

    [SerializeField]
    private Vector3 position = Vector3.zero; // Posici�n inicial del centro

    private static readonly int RadiusID = Shader.PropertyToID("_Radius");
    private static readonly int PositionID = Shader.PropertyToID("_Position");

    void OnValidate()
    {
        // Actualiza los valores en el Editor
        UpdateGlobalParameters();
    }

    void Awake()
    {
        // Asegura que los valores est�n configurados al cargar la escena
        UpdateGlobalParameters();
    }

    void Update()
    {
        // Actualiza los valores globales en cada frame
        UpdateGlobalParameters();
    }

    private void UpdateGlobalParameters()
    {
        // Establecer el valor global del Radio
        Shader.SetGlobalFloat(RadiusID, radius);

        // Establecer el valor global de la Posici�n
        Shader.SetGlobalVector(PositionID, position);
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