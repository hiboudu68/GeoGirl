using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class UIGridFollowCamera : MonoBehaviour
{
    [Tooltip("Taille d'une cellule en unités Unity")]
    public float cellSize = 1f;

    private RawImage rawImage;
    private Camera cam;
    private Rect initialUV;

    void Awake()
    {
        rawImage = GetComponent<RawImage>();
        cam = Camera.main;

        // On clone le Material pour ne pas impacter les autres RawImage
        rawImage.material = Instantiate(rawImage.material);
        // On sauvegarde le tiling initial défini en Inspector
        initialUV = rawImage.uvRect;
    }

    void Update()
    {
        if (cam == null) return;

        // Calcul de l'offset UV en fonction de la position de la caméra
        Vector3 pos = cam.transform.position;
        // On divise par cellSize pour que 1 UV = 1 cellule
        float offsetX = pos.x / cellSize;
        float offsetY = pos.y / cellSize;

        // On applique sur le RawImage
        rawImage.uvRect = new Rect(
            offsetX % 1f,     // on peut garder seulement la partie fractionnaire
            offsetY % 1f,
            initialUV.width,  // conserve le tiling défini en Inspector
            initialUV.height
        );
    }
}
