using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifEditorHandler : MonoBehaviour
{

    public static bool IsModif;
    
    private static ModifEditorHandler _instance;

    public static ModifEditorHandler Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        IsModif = false;
    }
}
