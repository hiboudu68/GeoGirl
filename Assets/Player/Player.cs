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
        if(defaultMode == null){
            foreach (Transform childTransform in this.transform)
            {
                defaultMode = childTransform.GetComponent<PlayerController>();
            }
        }
        if(currentMode == null){
            foreach (Transform childTransform in this.transform)
            {
                currentMode = childTransform.GetComponent<PlayerController>();
            }
        }
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

        var newInstance = GameObject.Instantiate(prefabMode.gameObject);

        currentMode = newInstance.GetComponent<PlayerController>();
        currentMode.transform.parent = transform;
        currentMode.transform.localPosition = Vector3.zero;
        
        currentMode.enabled = true;
        newInstance.GetComponent<PlayerDeathHandler>().enabled = true;
        newInstance.GetComponent<BoxCollider2D>().enabled  = true;


    }
}
