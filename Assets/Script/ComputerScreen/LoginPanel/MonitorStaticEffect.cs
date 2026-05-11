using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonitorStaticEffect : MonoBehaviour
{
    [Header("UI")]
    public RawImage staticImage;
    public Image blackScreen;
    public GameObject loginPanel;
    public GameObject desktopPanel;

    [Header("Effect Settings")]
    public float staticDuration = 0.7f;
    public float blackDuration = 0.45f;
    public float refreshInterval = 0.04f;
    public float maxStaticAlpha = 0.85f;

    [Header("Noise Texture")]
    public int textureWidth = 256;
    public int textureHeight = 256;

    Texture2D noiseTexture;
    Color32[] pixels;
    Coroutine runningRoutine;

    void Awake()
    {
        CreateNoiseTexture();

        HideStatic();
        HideBlackScreen();

        if (loginPanel != null)
        {
            loginPanel.SetActive(false);
        }

        if (desktopPanel != null)
        {
            desktopPanel.SetActive(false);
        }
    }

    void CreateNoiseTexture()
    {
        noiseTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);
        noiseTexture.filterMode = FilterMode.Point;
        noiseTexture.wrapMode = TextureWrapMode.Clamp;

        pixels = new Color32[textureWidth * textureHeight];

        if (staticImage != null)
        {
            staticImage.texture = noiseTexture;
            staticImage.raycastTarget = false;
        }
    }

    void GenerateNoise()
    {
        for (int i = 0; i < pixels.Length; i++)
        {
            byte value = (byte)Random.Range(0, 256);
            pixels[i] = new Color32(value, value, value, 255);
        }

        noiseTexture.SetPixels32(pixels);
        noiseTexture.Apply(false);
    }

    public void PlayEffect()
    {
        if (!gameObject.activeInHierarchy) return;

        if (runningRoutine != null)
        {
            StopCoroutine(runningRoutine);
        }

        runningRoutine = StartCoroutine(PlayRoutine());
    }

    IEnumerator PlayRoutine()
    {
        if (loginPanel != null)
        {
            loginPanel.SetActive(false);
        }

        if (desktopPanel != null)
        {
            desktopPanel.SetActive(false);
        }

        ShowBlackScreen();

        float timer = 0f;
        float refreshTimer = 0f;

        while (timer < staticDuration)
        {
            timer += Time.deltaTime;
            refreshTimer += Time.deltaTime;

            if (refreshTimer >= refreshInterval)
            {
                GenerateNoise();
                refreshTimer = 0f;
            }

            float fade = 1f - (timer / staticDuration);

            if (staticImage != null)
            {
                Color color = staticImage.color;
                color.a = Random.Range(maxStaticAlpha * 0.65f, maxStaticAlpha) * fade;
                staticImage.color = color;
            }

            yield return null;
        }

        HideStatic();

        yield return new WaitForSeconds(blackDuration);

        if (loginPanel != null)
        {
            loginPanel.SetActive(true);
        }

        HideBlackScreen();

        runningRoutine = null;
    }

    void HideStatic()
    {
        if (staticImage != null)
        {
            Color color = staticImage.color;
            color.a = 0f;
            staticImage.color = color;
            staticImage.raycastTarget = false;
        }
    }

    void ShowBlackScreen()
    {
        if (blackScreen != null)
        {
            blackScreen.gameObject.SetActive(true);

            Color color = blackScreen.color;
            color.a = 1f;
            blackScreen.color = color;
            blackScreen.raycastTarget = true;
        }
    }

    void HideBlackScreen()
    {
        if (blackScreen != null)
        {
            Color color = blackScreen.color;
            color.a = 0f;
            blackScreen.color = color;
            blackScreen.raycastTarget = false;
        }
    }
}