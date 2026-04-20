using UnityEngine;

public class GlowPulse : MonoBehaviour
{
    public Color glowColor = Color.cyan;
    public float minIntensity = 0.5f;
    public float maxIntensity = 3f;
    public float pulseSpeed = 2f;

    private Material mat;
    private bool isGlowing = true;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        if (isGlowing)
        {
            float intensity = Mathf.Lerp(minIntensity, maxIntensity,
                              (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
            mat.SetColor("_EmissionColor", glowColor * intensity);
        }
    }

    public void SetGlow(bool active) => isGlowing = active;
}