using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Allows runtime control of skybox intensity, material exposure, and light intensity from the Unity Inspector.
/// </summary>
[ExecuteAlways]
public class LightingController : MonoBehaviour
{
    [Header("Runtime Control")]
    [Tooltip("Enable or disable the lighting modifications at runtime.")]
    public bool enableModifications = true;

    [Header("Lighting Settings")]
    [Tooltip("Modify the Skybox Intensity Multiplier value.")]
    [Range(0, 8)]
    public float skyboxIntensity = 1.0f;

    [Header("Skybox Material Settings")]
    [Tooltip("The skybox material to modify (Exposure property).")]
    public Material skyboxMaterial;

    [Range(0, 5)]
    [Tooltip("Modify the skybox material's exposure.")]
    public float skyboxExposure = 1.0f;

    [Header("Light Settings")]
    [Tooltip("The directional light to modify.")]
    public Light directionalLight;

    [Range(0, 10)]
    [Tooltip("Modify the light intensity.")]
    public float lightIntensity = 1.0f;

    private float previousSkyboxIntensity;
    private float previousSkyboxExposure;
    private float previousLightIntensity;

    void OnValidate()
    {
        // Apply settings in the Inspector
        if (enableModifications)
        {
            UpdateSkyboxIntensity();
            UpdateSkyboxMaterialExposure();
            UpdateLightIntensity();
        }
    }

    void Update()
    {
        // Update properties dynamically in edit mode and runtime if modifications are enabled
        if (enableModifications)
        {
            if (!Application.isPlaying || Mathf.Abs(previousSkyboxIntensity - skyboxIntensity) > Mathf.Epsilon)
                UpdateSkyboxIntensity();

            if (skyboxMaterial && (!Application.isPlaying || Mathf.Abs(previousSkyboxExposure - skyboxExposure) > Mathf.Epsilon))
                UpdateSkyboxMaterialExposure();

            if (directionalLight && (!Application.isPlaying || Mathf.Abs(previousLightIntensity - lightIntensity) > Mathf.Epsilon))
                UpdateLightIntensity();
        }
    }

    private void UpdateSkyboxIntensity()
    {
        RenderSettings.ambientIntensity = skyboxIntensity;
        previousSkyboxIntensity = skyboxIntensity;
    }

    private void UpdateSkyboxMaterialExposure()
    {
        if (skyboxMaterial && skyboxMaterial.HasProperty("_Exposure"))
        {
            skyboxMaterial.SetFloat("_Exposure", skyboxExposure);
            previousSkyboxExposure = skyboxExposure;
        }
    }

    private void UpdateLightIntensity()
    {
        if (directionalLight)
        {
            directionalLight.intensity = lightIntensity;
            previousLightIntensity = lightIntensity;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(LightingController))]
public class LightingControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Open Lighting Settings"))
        {
            SettingsService.OpenProjectSettings("Project/Lighting");
        }
    }
}
#endif
