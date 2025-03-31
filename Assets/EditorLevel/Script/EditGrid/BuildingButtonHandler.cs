using UnityEngine;
using UnityEngine.UI;

public class BuildingButtonHandler : MonoBehaviour
{
    [SerializeField] BuildingObjectsBase item;
    Button button;
    BuildingCreator buildingCreator;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
        buildingCreator = BuildingCreator.GetInstance();
    }

    private void ButtonClicked()
    {
        buildingCreator.ObjectSelected(item);
    }
}
