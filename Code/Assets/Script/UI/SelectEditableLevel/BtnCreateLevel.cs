using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnCreateLevel : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        AskForName.Instance.GetValue((string value) =>
        {
            Level newLevel = new();
            newLevel.Id = Guid.NewGuid().ToString();
            newLevel.Name = value;
            newLevel.TotalBonusCount = 0;

            LevelsManager.CreateLevel(newLevel);
            GetComponentInParent<EditableLevelListMenu>().StartEditLevel(newLevel);
        });
    }
}
