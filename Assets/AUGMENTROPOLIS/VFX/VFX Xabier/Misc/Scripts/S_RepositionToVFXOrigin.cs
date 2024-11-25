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
            // Obtener la posici�n del Transform del Visual Effect
            Vector3 vfxOrigin = transform.position;

            // Ajustar la posici�n del Visual Effect para que el Gizmo est� en el origen
            transform.localPosition = Vector3.zero;

            // O puedes ajustar el offset manualmente si tu Visual Effect tiene un emisor que no est� en el centro
            // Por ejemplo, si necesitas un offset basado en alg�n par�metro del Visual Effect Graph
            // transform.localPosition = someCustomOffset; 
        }
        else
        {
            Debug.LogWarning("No se encontr� un Visual Effect en este GameObject.");
        }
    }
}
