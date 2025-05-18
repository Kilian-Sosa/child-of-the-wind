using UnityEngine;

public class PlayerMenu : MonoBehaviour
{
   public GameObject playerMenu;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (playerMenu.activeSelf)
            {
                playerMenu.SetActive(false);
                Time.timeScale = 1f;
            }
            else
            {
                playerMenu.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }
}
