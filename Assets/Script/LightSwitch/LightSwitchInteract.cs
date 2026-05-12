using UnityEngine;

public class LightSwitchInteract : MonoBehaviour
{
    public Light roomLight;
    public GameObject hiddenObject;
    public Transform switchLever;
    public Vector3 offRotation = new Vector3(-20f, 0f, 0f);
    public Vector3 onRotation = new Vector3(20f, 0f, 0f);
    public Material offMaterial;
    public Material onMaterial;
    public Renderer switchRenderer;

    bool lightOn;

    void Start()
    {
        SetLight(false);
    }

    public void ToggleLight()
    {
        SetLight(!lightOn);
    }

    void SetLight(bool on)
    {
        lightOn = on;

        if (roomLight != null)
        {
            roomLight.enabled = lightOn;
        }

        if (hiddenObject != null)
        {
            hiddenObject.SetActive(lightOn);
        }

        if (switchLever != null)
        {
            switchLever.localRotation = Quaternion.Euler(lightOn ? onRotation : offRotation);
        }

        if (switchRenderer != null)
        {
            if (lightOn && onMaterial != null)
            {
                switchRenderer.material = onMaterial;
            }
            else if (!lightOn && offMaterial != null)
            {
                switchRenderer.material = offMaterial;
            }
        }
    }

    public bool IsLightOn()
    {
        return lightOn;
    }
}