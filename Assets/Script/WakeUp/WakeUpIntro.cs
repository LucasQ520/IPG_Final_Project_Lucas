using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WakeUpIntro : MonoBehaviour
{
    [Header("Player")]
    public FirstPersonController playerController;
    public Transform playerCamera;

    [Header("HUD")]
    public GameObject crosshair;

    [Header("Overlay")]
    public Image wakeUpOverlay;

    [Header("Camera Start")]
    public Vector3 groundCameraLocalPosition = new Vector3(0f, -0.6f, 0f);
    public Vector3 standingCameraLocalPosition = new Vector3(0f, 0.7f, 0f);

    public Vector3 groundCameraLocalRotation = new Vector3(-80f, 0f, 0f);
    public Vector3 standingCameraLocalRotation = new Vector3(0f, 0f, 0f);

    [Header("Blink Timing")]
    public float firstBlackDelay = 0.7f;
    public float firstBlinkOpenDuration = 0.35f;
    public float firstBlinkHoldOpen = 0.25f;
    public float firstBlinkCloseDuration = 0.25f;
    public float secondBlinkClosedHold = 0.35f;
    public float secondBlinkOpenDuration = 0.8f;

    [Header("Dizzy Camera Shake")]
    public float dizzyShakeDuration = 1.4f;
    public float shakeAngle = 5f;
    public float shakeSpeed = 7f;

    [Header("Getting Up")]
    public float getUpDuration = 2.8f;

    void Start()
    {
        StartCoroutine(WakeUpRoutine());
    }

    IEnumerator WakeUpRoutine()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        if (crosshair != null)
        {
            crosshair.SetActive(false);
        }

        if (wakeUpOverlay != null)
        {
            wakeUpOverlay.gameObject.SetActive(true);
        }

        if (playerCamera != null)
        {
            playerCamera.localPosition = groundCameraLocalPosition;
            playerCamera.localRotation = Quaternion.Euler(groundCameraLocalRotation);
        }

        SetOverlayAlpha(1f);

        yield return new WaitForSeconds(firstBlackDelay);

        yield return FadeOverlay(1f, 0f, firstBlinkOpenDuration);
        yield return new WaitForSeconds(firstBlinkHoldOpen);

        yield return FadeOverlay(0f, 1f, firstBlinkCloseDuration);
        yield return new WaitForSeconds(secondBlinkClosedHold);

        yield return FadeOverlay(1f, 0f, secondBlinkOpenDuration);

        yield return DizzyShakeRoutine();

        yield return GetUpCameraRoutine();

        if (crosshair != null)
        {
            crosshair.SetActive(true);
        }

        if (playerController != null)
        {
            playerController.enabled = true;
        }

        if (wakeUpOverlay != null)
        {
            wakeUpOverlay.gameObject.SetActive(false);
        }
    }

    IEnumerator DizzyShakeRoutine()
    {
        if (playerCamera == null) yield break;

        float timer = 0f;

        while (timer < dizzyShakeDuration)
        {
            timer += Time.deltaTime;

            float shake = Mathf.Sin(timer * shakeSpeed) * shakeAngle;
            float fadeOut = 1f - (timer / dizzyShakeDuration);

            Vector3 rotation = groundCameraLocalRotation;
            rotation.z += shake * fadeOut;

            playerCamera.localPosition = groundCameraLocalPosition;
            playerCamera.localRotation = Quaternion.Euler(rotation);

            yield return null;
        }

        playerCamera.localPosition = groundCameraLocalPosition;
        playerCamera.localRotation = Quaternion.Euler(groundCameraLocalRotation);
    }

    IEnumerator GetUpCameraRoutine()
    {
        if (playerCamera == null) yield break;

        Vector3 startPosition = groundCameraLocalPosition;
        Vector3 endPosition = standingCameraLocalPosition;

        Quaternion startRotation = Quaternion.Euler(groundCameraLocalRotation);
        Quaternion endRotation = Quaternion.Euler(standingCameraLocalRotation);

        float timer = 0f;

        while (timer < getUpDuration)
        {
            timer += Time.deltaTime;

            float t = timer / getUpDuration;
            t = Mathf.SmoothStep(0f, 1f, t);

            float smallShake = Mathf.Sin(timer * 8f) * 0.8f * (1f - t);

            playerCamera.localPosition = Vector3.Lerp(startPosition, endPosition, t);

            Quaternion getUpRotation = Quaternion.Slerp(startRotation, endRotation, t);
            Quaternion shakeRotation = Quaternion.Euler(0f, 0f, smallShake);

            playerCamera.localRotation = getUpRotation * shakeRotation;

            yield return null;
        }

        playerCamera.localPosition = endPosition;
        playerCamera.localRotation = endRotation;
    }

    IEnumerator FadeOverlay(float startAlpha, float endAlpha, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, t);

            SetOverlayAlpha(alpha);

            yield return null;
        }

        SetOverlayAlpha(endAlpha);
    }

    void SetOverlayAlpha(float alpha)
    {
        if (wakeUpOverlay == null) return;

        Color color = wakeUpOverlay.color;
        color.a = alpha;
        wakeUpOverlay.color = color;
    }
}