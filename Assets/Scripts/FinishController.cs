using UnityEngine;

public class FinishController : MonoBehaviour
{
    public int unlockLevelNum;
    public GameManager gameManager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.unlockLevels[unlockLevelNum] = true;
            gameManager.LevelSelect();
        }
    }

}
