using UnityEngine;

public class CubeRotationCorrector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
