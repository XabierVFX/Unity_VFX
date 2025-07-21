using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderGlobalController : MonoBehaviour
{
    public string parameterName = "_Controller"; // Asegúrate de que coincide con el nombre en tu Shader Graph
    public float transitionTime = 1f;

    private float targetValue = 1f;
    private float currentValue = 0f;
    private bool isTransitioning = false;
    private List<Material> affectedMaterials = new List<Material>();

    // Singleton para que otros scripts puedan acceder fácilmente
    public static ShaderGlobalController Instance { get; private set; }

    void Awake()
    {
        // Asegurar que solo haya una instancia
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Buscar materiales ya presentes en escena
        Renderer[] renderers = FindObjectsOfType<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            foreach (Material mat in renderer.sharedMaterials)
            {
                TryRegisterMaterial(mat);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isTransitioning)
        {
            StartCoroutine(SwitchControllerValue());
        }
    }

    IEnumerator SwitchControllerValue()
    {
        isTransitioning = true;
        float startValue = currentValue;
        float endValue = currentValue == 0 ? 1 : 0;
        float timer = 0f;

        while (timer < transitionTime)
        {
            float t = timer / transitionTime;
            currentValue = Mathf.Lerp(startValue, endValue, t);
            foreach (Material mat in affectedMaterials)
            {
                mat.SetFloat(parameterName, currentValue);
            }
            timer += Time.deltaTime;
            yield return null;
        }

        currentValue = endValue;
        foreach (Material mat in affectedMaterials)
        {
            mat.SetFloat(parameterName, currentValue);
        }

        isTransitioning = false;
    }

    public void RegisterMaterial(Material mat)
    {
        TryRegisterMaterial(mat);
    }

    private void TryRegisterMaterial(Material mat)
    {
        if (mat != null && mat.HasFloat(parameterName) && !affectedMaterials.Contains(mat))
        {
            mat.SetFloat(parameterName, currentValue); // sincroniza valor actual
            affectedMaterials.Add(mat);
        }
    }
}
