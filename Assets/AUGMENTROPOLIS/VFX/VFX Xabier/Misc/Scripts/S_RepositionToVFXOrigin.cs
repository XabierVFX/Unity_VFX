using UnityEngine;
using UnityEngine.VFX;

[ExecuteInEditMode]
public class RepositionVFXGraphGizmoToOrigin : MonoBehaviour
{
    void Update()
    {
        // Obtener el componente Visual Effect
        VisualEffect visualEffect = GetComponent<VisualEffect>();

        // Asegurarnos de que hemos encontrado un Visual Effect
        if (visualEffect != null)
        {
            // Obtener la posición del Transform del Visual Effect
            Vector3 vfxOrigin = transform.position;

            // Ajustar la posición del Visual Effect para que el Gizmo esté en el origen
            transform.localPosition = Vector3.zero;

            // O puedes ajustar el offset manualmente si tu Visual Effect tiene un emisor que no está en el centro
            // Por ejemplo, si necesitas un offset basado en algún parámetro del Visual Effect Graph
            // transform.localPosition = someCustomOffset; 
        }
        else
        {
            Debug.LogWarning("No se encontró un Visual Effect en este GameObject.");
        }
    }
}
