using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static int numberOfTries = 0;
    //public static Text triesText;

    void Start()
    {
        UpdateTriesDisplay();
    }

    public static void IncrementTries()
    {
        numberOfTries++;
        UpdateTriesDisplay();
    }

    public static void UpdateTriesDisplay()
    {
        /*if (triesText != null)
        {
            triesText.text = "Essais: " + numberOfTries.ToString();
        }*/
    }

}
