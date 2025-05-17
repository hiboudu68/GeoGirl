using UnityEngine;
using UnityEngine.UI;

public class RayCastToggler : MonoBehaviour
{
    public void SetValue(bool raycastTarget)
    {
        MaskableGraphic self = GetComponent<MaskableGraphic>();
        if (self != null)
            self.raycastTarget = raycastTarget;

        foreach (MaskableGraphic raycaster in GetComponentsInChildren<MaskableGraphic>())
            raycaster.raycastTarget = raycastTarget;
    }
}
