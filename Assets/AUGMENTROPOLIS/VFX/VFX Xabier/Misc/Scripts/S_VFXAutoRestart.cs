using UnityEngine;
using UnityEngine.VFX;
using UnityEditor;

// CUIDADO! SOLO FUNCIONA DE MANERA CONSISTENTE CON 1 VFX EN LA ESCENA!

[ExecuteInEditMode]
public class S_VFXAutoRestart : MonoBehaviour
{
    public VisualEffect vfx;
    public float systemDuration = 2.0f; // Duración de cada ciclo del sistema
    public bool autoRestartEnabled = true;

    private double lastRestartTime;
    private bool isWaitingForNextRestart = true;

    private void OnEnable()
    {
        if (vfx == null)
        {
            vfx = GetComponent<VisualEffect>();
        }

        if (!Application.isPlaying && autoRestartEnabled)
        {
            SubscribeToUpdate();
            lastRestartTime = EditorApplication.timeSinceStartup;
        }
    }

    private void OnDisable()
    {
        UnsubscribeFromUpdate();
    }

    private void SubscribeToUpdate()
    {
        UnsubscribeFromUpdate(); // Asegura que no haya suscripciones duplicadas
        EditorApplication.update += EditorUpdate;
    }

    private void UnsubscribeFromUpdate()
    {
        EditorApplication.update -= EditorUpdate;
    }

    private void EditorUpdate()
    {
        if (!autoRestartEnabled || vfx == null || !isWaitingForNextRestart)
            return;

        if (EditorApplication.timeSinceStartup - lastRestartTime >= systemDuration)
        {
            RestartVFX();
            lastRestartTime = EditorApplication.timeSinceStartup;
            isWaitingForNextRestart = false;
        }
    }

    private void RestartVFX()
    {
        vfx.enabled = false;
        EditorApplication.delayCall += () =>
        {
            vfx.enabled = true;
            isWaitingForNextRestart = true;
        };
    }

    private void OnValidate()
    {
        if (!Application.isPlaying && autoRestartEnabled)
        {
            SubscribeToUpdate();
        }
        else
        {
            UnsubscribeFromUpdate();
        }
    }
}
