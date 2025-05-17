using UnityEngine;

public class PlayerInventory : MonoBehaviour {

    string[] powerUps = new string[3];
    int itemCount = 0;

    string[] magicMap = new string[3] {
        "Knockback",
        "Tornado",
        "Magic3"
    };

    void Start() {
        // Get from PlayerPrefs
    }

    public string[] GetPowerUps() => powerUps;

    public void AddPowerUp(string powerUp) {
        for (int i = 0; i < powerUps.Length; i++) {
            if (powerUps[i] == null) {
                powerUps[i] = powerUp;
                Debug.Log("PowerUp added: " + powerUps[i]);
                break;
            }
        }
    }

    public void AddItem() => itemCount++;

    public int GetItemCount() => itemCount;

    public bool HasMagic1() {
        for (int i = 0; i < powerUps.Length; i++)
            if (powerUps[i] != null && powerUps[i].Equals(magicMap[0], System.StringComparison.OrdinalIgnoreCase))
                return true;
        return false;
    }
}
