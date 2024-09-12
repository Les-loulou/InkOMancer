using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SC_LQ_ProcessVolumeChange : MonoBehaviour
{

    public static SC_LQ_ProcessVolumeChange instance;
    public Volume volume;

    [HideInInspector] public float desiredVignIntensity;
    [HideInInspector] public float desiredVignSmooth;

    float speed = 3f;

    Vignette vignette;

    public void SetVignette(float intensity, float smooth)
    {
         desiredVignIntensity = intensity;
         desiredVignSmooth = smooth;
    }

    private void Awake()
    {
        instance = this;

        volume.profile.TryGet(out Vignette vign);
        vignette = vign;

        desiredVignIntensity = vignette.intensity.value;
        desiredVignSmooth = vignette.smoothness.value;
    }

    void Start()
    {
        volume = GetComponent<Volume>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (vignette != null) // for e.g set vignette intensity to .4f
        {
            vignette.intensity.value = Mathf.MoveTowards(vignette.intensity.value, desiredVignIntensity, Time.fixedDeltaTime * speed);
            vignette.smoothness.value = Mathf.MoveTowards(vignette.intensity.value, desiredVignSmooth, Time.fixedDeltaTime * speed);
        }
    }
}
