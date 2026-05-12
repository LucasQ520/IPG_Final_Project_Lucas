using UnityEngine;

public class RecycleDropTarget : MonoBehaviour
{
    public RecycleBinManager recycleBinManager;

    public void DropIcon(DesktopIcon icon)
    {
        if (recycleBinManager != null)
        {
            recycleBinManager.PutInRecycleBin(icon);
        }
    }
}