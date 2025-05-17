using UnityEngine;
using UnityEngine.EventSystems;

public class BtnDeleteLevel : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        ConfirmPopup.Instance.GetValue((bool result) =>
        {
            if (result)
            {
                LevelsManager.DeleteLevel(GetComponentInParent<EditableLevelListMenu>()
                    .GetComponentInChildren<LevelSlider>()
                    .GetLevel());
            }
        });
    }
}
