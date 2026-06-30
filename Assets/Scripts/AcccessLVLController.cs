using UnityEngine;
using UnityEngine.UI;

public class AcccessLVLController : MonoBehaviour
{
    public GameObject[] lvlButtons;
    public GameObject[] lockImage;
    void Start()
    {
        for (int i = 0;i < lvlButtons.Length;i++)
        {
            if (GameManager.unlockLevels[i])
            {
                lvlButtons[i].GetComponent<Button>().interactable = true;
                lockImage[i].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
