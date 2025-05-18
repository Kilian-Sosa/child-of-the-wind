using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
   
    public static GameManager instance;


    public int currentXP;
    public int strengthLevel;
    public int magicLevel;
    public int resistanceLevel;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadPlayerStats();
    }

    //PLAYER STATS
    public void LoadPlayerStats()
    {
        strengthLevel = PlayerPrefs.GetInt("StrenghtLevel", 1);
        currentXP = PlayerPrefs.GetInt("CurrentXP", 0);
        magicLevel = PlayerPrefs.GetInt("MagicLevel", 1);
        resistanceLevel = PlayerPrefs.GetInt("ResistanceLevel", 1);

        Debug.Log("Level: " + strengthLevel);
        Debug.Log("Magic Level: " + magicLevel);
        Debug.Log("Resistance Level: " + resistanceLevel);
        Debug.Log("XP: " + currentXP);
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        GameObject.Find("PXs").GetComponent<TextMeshProUGUI>().SetText("PXs:" + currentXP.ToString());
        if (currentXP >= 1000)
        {
            showInfo("Ya puedes subir de nivel\n pulsa ESC para acceder al menú");
        }

        PlayerPrefs.SetInt("CurrentXP", currentXP);
        Debug.Log("XP actual: " + currentXP);
        PlayerPrefs.Save(); ;
    }

    public void LevelUp()
    {

    }

    public int GetStrengthLevel()
    {
        return strengthLevel;
    }


    public void showInfo(string message)
    {
        GameObject uxObject = GameObject.Find("UI");
        if (uxObject != null)
        {
            ShowInfoMessage messageScript = uxObject.GetComponent<ShowInfoMessage>();
            if (messageScript != null)
            {
                messageScript.ShowMessage( message);
            }
            else
            {
                Debug.LogWarning("No se encontró el script ShowHabilityMessage en UX.");
            }
        }
        else
        {
            Debug.LogWarning("No se encontró el GameObject 'UX' en la escena.");
        }

    }

}
