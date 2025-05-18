using UnityEngine;

public class PlayerInventory : MonoBehaviour
{

    string[] powerUps = new string[3];
    int itemCount = 0;

    string[] magicMap = new string[3] {
        "Knockback",
        "Tornado",
        "Magic3"
    };



    void Start()
    {
        powerUps = LoadPowerUps();
        Debug.Log("PowerUps loaded: " + string.Join(", ", powerUps));
    }

    public string[] GetPowerUps() => powerUps;

    public void AddPowerUp(string powerUp)
    {
        for (int i = 0; i < powerUps.Length; i++)
        {
            if (powerUps[i] == null)
            {
                powerUps[i] = powerUp;
                Debug.Log("PowerUp added: " + powerUps[i]);
                showInfo(powerUps[i]);
                SavePowerUps();

                break;
            }
        }
    }

    public void AddItem() => itemCount++;

    public int GetItemCount() => itemCount;

    public bool HasMagic1()
    {
        for (int i = 0; i < powerUps.Length; i++)
            if (powerUps[i] != null && powerUps[i].Equals(magicMap[0], System.StringComparison.OrdinalIgnoreCase))
                return true;
        return false;
    }

    
    //public bool HasPowerUp(string name)
    //{
    //    return powerUps.Contains(name);
    //}

    public void showInfo(string message)
    {
        GameObject uxObject = GameObject.Find("UI");
        if (uxObject != null)
        {
            ShowHabilityMessage messageScript = uxObject.GetComponent<ShowHabilityMessage>();
            if (messageScript != null)
            {
                messageScript.ShowMessage("¡Has obtenido una nueva habilidad!" + message);
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

    public void SavePowerUps()
    {
        string data = string.Join(",", powerUps); 
        PlayerPrefs.SetString("PlayerPowerUps", data);
        PlayerPrefs.Save();
    }

    public string[] LoadPowerUps()
    {
        string data = PlayerPrefs.GetString("PlayerPowerUps", "");

        if (string.IsNullOrEmpty(data))
            return new string[3]; 

        string[] loaded = data.Split(',');

        if (loaded.Length < 3)
        {
            string[] fixedArray = new string[3];
            for (int i = 0; i < loaded.Length; i++)
                fixedArray[i] = loaded[i];
            return fixedArray;
        }

        return loaded;
    }

   

  

  
}
