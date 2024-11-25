using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode] // Asegura que el script se ejecute en el editor
public class S_LockTransform : MonoBehaviour
{
    public Vector3 lockedPosition;
    private bool hasInitialized = false; // Variable para evitar la inicialización no deseada

    void Start()
    {
        // Solo guarda la posición inicial si no se ha hecho antes
        if (!hasInitialized)
        {
            lockedPosition = transform.position;
            hasInitialized = true;
        }
    }

    void Update()
    {
        // Bloquea la posición durante el tiempo de ejecución
        transform.position = lockedPosition;
    }

    // Este método es llamado cada vez que se cambian valores en el editor
    void OnValidate()
    {
        // Bloquea la posición también en el editor
        if (hasInitialized)
        {
            transform.position = lockedPosition;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(S_LockTransform))]
    public class S_LockTransformEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Obtén la referencia al script
            S_LockTransform script = (S_LockTransform)target;

            // Desactiva la edición del transform en el inspector
            EditorGUI.BeginDisabledGroup(true);
            base.OnInspectorGUI();
            EditorGUI.EndDisabledGroup();
        }
    }
#endif
}
