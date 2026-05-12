using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecycleBinManager : MonoBehaviour
{
    public Transform recycleContentRoot;
    public GameObject recycleBinItemPrefab;

    public void PutInRecycleBin(DesktopIcon icon)
    {
        if (icon == null) return;
        if (!icon.canRecycle) return;

        CreateRecycleItem(icon);
        icon.gameObject.SetActive(false);
    }

    void CreateRecycleItem(DesktopIcon icon)
    {
        if (recycleContentRoot == null) return;
        if (recycleBinItemPrefab == null) return;

        GameObject item = Instantiate(recycleBinItemPrefab, recycleContentRoot, false);

        RectTransform itemRect = item.GetComponent<RectTransform>();
        if (itemRect != null)
        {
            itemRect.localScale = Vector3.one;
            itemRect.anchoredPosition = Vector2.zero;
        }

        RecycledIcon recycledIcon = item.GetComponent<RecycledIcon>();
        if (recycledIcon != null)
        {
            recycledIcon.Setup(icon, this);
        }

        Transform imageTransform = item.transform.Find("IconImage");
        Transform labelTransform = item.transform.Find("IconLabel");

        if (imageTransform != null)
        {
            Image iconImage = imageTransform.GetComponent<Image>();

            if (iconImage != null)
            {
                iconImage.sprite = icon.iconSprite;
                iconImage.preserveAspect = true;
                iconImage.enabled = true;
            }
        }

        if (labelTransform != null)
        {
            TMP_Text iconLabel = labelTransform.GetComponent<TMP_Text>();

            if (iconLabel != null)
            {
                iconLabel.text = icon.iconName;
            }
        }
    }

    public void RestoreIcon(DesktopIcon originalIcon, GameObject recycledItem)
    {
        if (originalIcon != null)
        {
            originalIcon.RestoreToDesktop();
        }

        if (recycledItem != null)
        {
            Destroy(recycledItem);
        }
    }
}