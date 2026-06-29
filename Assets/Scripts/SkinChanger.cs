using UnityEngine;

public class SkinChanger : MonoBehaviour
{
    public GameObject[] skins;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.playerSkin == 0)
        {
            skins[0].SetActive(true);
            skins[1].SetActive(false);
        }
        else if(GameManager.playerSkin == 1)
        {
            skins[1].SetActive(true);
            skins[0].SetActive(false);
        }
    
    }

}
