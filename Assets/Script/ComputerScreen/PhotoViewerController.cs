using UnityEngine;
using UnityEngine.UI;

public class ImagePreviewController : MonoBehaviour
{
    [Header("UI")]
    public GameObject previewWindow;
    public Image previewImage;

    public void OpenImage(Sprite sprite)
    {
        if (sprite == null) return;

        previewImage.sprite = sprite;
        previewImage.preserveAspect = true;

        RectTransform rt = previewImage.rectTransform;
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1024);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 683);

        previewWindow.SetActive(true);
    }


    public void Close()
    {
        previewWindow.SetActive(false);
    }
}
