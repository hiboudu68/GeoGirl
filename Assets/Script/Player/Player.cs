using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    private Vector3 startPosition;
    private PlayerController currentPrefabMode;
    public PlayerController defaultMode;
    public PlayerController currentMode;

    public void Start()
    {
        Instance = this;
        startPosition = transform.position;
        Restart();
    }
    public void Restart(){
        transform.position = startPosition;
        SetControlMode(defaultMode);
    }

    public void SetControlMode(PlayerController prefabMode)
    {
        Debug.Log("Setting controlMode to " + prefabMode.name);
        if(currentPrefabMode == prefabMode)
            return;
        
        currentPrefabMode = prefabMode;
        if (currentMode != null){
            Destroy(currentMode.gameObject);
        }
        
        if(prefabMode == null) return;
        
        currentMode = GameObject.Instantiate(prefabMode.gameObject).GetComponent<PlayerController>();
        currentMode.transform.parent = transform;
        currentMode.transform.localPosition = Vector3.zero;
    }
}
