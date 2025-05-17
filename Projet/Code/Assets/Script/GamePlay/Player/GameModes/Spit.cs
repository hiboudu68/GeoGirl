using UnityEngine;

public class Spit : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Destroyable"))
            Destroy(collision.gameObject.transform.parent.gameObject);

        Destroy(gameObject);
    }
}
