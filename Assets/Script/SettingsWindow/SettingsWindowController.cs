using UnityEngine;
using UnityEngine.UI;

public class SettingsWindowController : MonoBehaviour
{
    [Header("Desktop Background")]
    public Image desktopBackgroundImage;

    [Header("Wallpaper Sprites")]
    public Sprite defaultWallpaper;
    public Sprite nightWallpaper;
    public Sprite patternWallpaper;

    [Header("Selection Box")]
    public RectTransform selectionBox;
    public RectTransform defaultPreview;
    public RectTransform nightPreview;
    public RectTransform patternPreview;

    int selectedWallpaper;
    int appliedWallpaper;

    void OnEnable()
    {
        selectedWallpaper = appliedWallpaper;
        UpdateSelectionBox();
    }

    public void SelectDefault()
    {
        selectedWallpaper = 0;
        UpdateSelectionBox();
    }

    public void SelectNight()
    {
        selectedWallpaper = 1;
        UpdateSelectionBox();
    }

    public void SelectPattern()
    {
        selectedWallpaper = 2;
        UpdateSelectionBox();
    }

    public void ApplyWallpaper()
    {
        appliedWallpaper = selectedWallpaper;

        if (desktopBackgroundImage == null) return;

        if (appliedWallpaper == 0 && defaultWallpaper != null)
        {
            desktopBackgroundImage.sprite = defaultWallpaper;
        }
        else if (appliedWallpaper == 1 && nightWallpaper != null)
        {
            desktopBackgroundImage.sprite = nightWallpaper;
        }
        else if (appliedWallpaper == 2 && patternWallpaper != null)
        {
            desktopBackgroundImage.sprite = patternWallpaper;
        }
    }

    public void ClickOK()
    {
        ApplyWallpaper();

        if (ComputerDesktopManager.instance != null)
        {
            ComputerDesktopManager.instance.CloseSettings();
        }
    }

    public void ClickCancel()
    {
        selectedWallpaper = appliedWallpaper;

        if (ComputerDesktopManager.instance != null)
        {
            ComputerDesktopManager.instance.CloseSettings();
        }
    }

    void UpdateSelectionBox()
    {
        if (selectionBox == null) return;

        RectTransform target = null;

        if (selectedWallpaper == 0)
        {
            target = defaultPreview;
        }
        else if (selectedWallpaper == 1)
        {
            target = nightPreview;
        }
        else if (selectedWallpaper == 2)
        {
            target = patternPreview;
        }

        if (target == null) return;

        selectionBox.gameObject.SetActive(true);
        selectionBox.SetParent(target.parent, false);
        selectionBox.position = target.position;
        selectionBox.sizeDelta = target.sizeDelta + new Vector2(12f, 12f);
        selectionBox.SetSiblingIndex(1);
    }
}