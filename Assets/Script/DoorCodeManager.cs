using UnityEngine;

public class DoorCodeManager : MonoBehaviour
{
    public static DoorCodeManager instance;

    string doorCode;

    void Awake()
    {
        instance = this;
        GenerateDoorCode();
    }

    void GenerateDoorCode()
    {
        doorCode = "";

        for (int i = 0; i < 6; i++)
        {
            doorCode += Random.Range(0, 10).ToString();
        }
    }

    public string GetDoorCode()
    {
        return doorCode;
    }

    public string GetDigit(int index)
    {
        if (string.IsNullOrEmpty(doorCode)) return "";
        if (index < 0 || index >= doorCode.Length) return "";

        return doorCode[index].ToString();
    }

    public bool CheckDoorCode(string input)
    {
        return input == doorCode;
    }
}