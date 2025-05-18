using UnityEngine;

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
        if (currentXP >= 1000)
        {

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
}
