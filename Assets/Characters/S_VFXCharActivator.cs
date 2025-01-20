using UnityEngine;
using UnityEngine.VFX;

public class S_VFXCharActivator : MonoBehaviour
{
    [System.Serializable]
    public class VFXData
    {
        public string vfxName; // Nombre del VFX para identificarlo en el Inspector
        public VisualEffectAsset visualEffectAsset; // Referencia al VFX Graph Asset
        public Transform joint; // El Joint al que seguir� este VFX
        [HideInInspector] public VisualEffect instantiatedVFX; // La instancia del VFX
    }

    public VFXData[] vfxList; // Lista de VFX configurables desde el Inspector

    // M�todos espec�ficos
    public void PlayVFX0() => PlayVFX(0);
    public void StopVFX0() => StopVFX(0);
    public void PlayVFX1() => PlayVFX(1);
    public void StopVFX1() => StopVFX(1);
    public void PlayVFX2() => PlayVFX(2);
    public void StopVFX2() => StopVFX(2);

    private void PlayVFX(int index)
    {
        if (!IsValidIndex(index)) return;

        VFXData vfxData = vfxList[index];

        // Crear el VFX si a�n no existe
        if (vfxData.instantiatedVFX == null)
        {
            GameObject vfxObject = new GameObject($"{vfxData.vfxName}_Instance");
            vfxObject.transform.SetParent(vfxData.joint, false);
            vfxObject.transform.localPosition = Vector3.zero;
            vfxObject.transform.localRotation = Quaternion.identity;

            vfxData.instantiatedVFX = vfxObject.AddComponent<VisualEffect>();
            vfxData.instantiatedVFX.visualEffectAsset = vfxData.visualEffectAsset;
        }

        // Activar el sistema de part�culas
        vfxData.instantiatedVFX.Play();
        //Debug.Log($"VFX '{vfxData.vfxName}' iniciado en �ndice {index}.");
    }

    private void StopVFX(int index)
    {
        if (!IsValidIndex(index)) return;

        VFXData vfxData = vfxList[index];

        if (vfxData.instantiatedVFX == null)
        {
            Debug.LogWarning($"No hay un VFX activo para detener en '{vfxData.vfxName}'.");
            return;
        }

        // Detener el sistema de part�culas
        vfxData.instantiatedVFX.Stop();
        Debug.Log($"VFX '{vfxData.vfxName}' detenido en �ndice {index}.");
    }

    private bool IsValidIndex(int index)
    {
        if (index < 0 || index >= vfxList.Length)
        {
            Debug.LogError($"�ndice '{index}' fuera de rango. Aseg�rate de que el �ndice sea v�lido.");
            return false;
        }

        VFXData vfxData = vfxList[index];
        if (vfxData.joint == null || vfxData.visualEffectAsset == null)
        {
            Debug.LogError($"Faltan datos en el �ndice '{index}'. Aseg�rate de asignar el Joint y el VisualEffectAsset.");
            return false;
        }

        return true;
    }
}
