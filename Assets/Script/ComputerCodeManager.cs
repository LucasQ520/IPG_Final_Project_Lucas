using UnityEngine;

public class ComputerCodeManager : MonoBehaviour
{
    public static ComputerCodeManager instance;

    string computerCode;

    void Awake()
    {
        instance = this;
        GenerateComputerCode();
    }

    void GenerateComputerCode()
    {
        computerCode = "";

        for (int i = 0; i < 6; i++)
        {
            computerCode += Random.Range(0, 10).ToString();
        }
    }

    public string GetComputerCode()
    {
        return computerCode;
    }

    public bool CheckComputerCode(string input)
    {
        return input == computerCode;
    }
}