using UnityEngine;

public class LVLSpawnPlayer : MonoBehaviour
{
    public GameObject[] skins;
    public Transform spawn;
    private GameObject camera;
    private void Awake()
    {
        Instantiate(skins[GameManager.playerSkin], spawn.position, spawn.rotation);
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.GetComponent<CameraController>().target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
