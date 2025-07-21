using UnityEngine;
using UnityEngine.VFX;

public class S_TriggerVFX : MonoBehaviour
{
    public VisualEffectAsset vfxAsset;         // VFX Graph desde biblioteca
    public KeyCode triggerKey = KeyCode.E;     // Tecla para disparar el VFX
    public float destroyDelay = 5f;            // Tiempo antes de destruir el VFX

    void Update()
    {
        if (Input.GetKeyDown(triggerKey) && vfxAsset != null)
        {
            GameObject go = new GameObject("Spawned VFX");
            go.transform.position = transform.position;
            go.transform.rotation = transform.rotation;

            VisualEffect vfx = go.AddComponent<VisualEffect>();
            vfx.visualEffectAsset = vfxAsset;

            Destroy(go, destroyDelay);
        }
    }
}
