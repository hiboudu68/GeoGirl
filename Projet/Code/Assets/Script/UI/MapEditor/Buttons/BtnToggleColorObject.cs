using UnityEngine;
using UnityEngine.EventSystems;

public class BtnToggleColorObject : MonoBehaviour, IPointerClickHandler
{
    private MapEditorManager mapEditor;
    private bool isVisible = false;
    [SerializeField] private FlexibleColorPicker primaryColorPicker;
    [SerializeField] private FlexibleColorPicker secondaryColorPicker;

    public bool IsVisible => isVisible;

    void Start()
    {
        primaryColorPicker.gameObject.SetActive(false);
        secondaryColorPicker.gameObject.SetActive(false);
    }
    void Awake()
    {
        mapEditor = GetComponentInParent<MapEditorManager>(true);
        primaryColorPicker.gameObject.SetActive(false);
        secondaryColorPicker.gameObject.SetActive(false);

        mapEditor.SelectedObjectChanged += OnSelectedObjectChanged;
    }
    private void OnSelectedObjectChanged(BaseObject obj)
    {
        if (isVisible)
            SelectColors(obj);
    }
    public void CloseColors()
    {
        isVisible = false;
        primaryColorPicker.gameObject.SetActive(false);
        secondaryColorPicker.gameObject.SetActive(false);
    }
    private void SelectColors(BaseObject ofObj)
    {
        if (ofObj == null)
            return;

        primaryColorPicker.color = ofObj.LevelObjectInfos.PrimaryColor;
        secondaryColorPicker.color = ofObj.LevelObjectInfos.SecondaryColor;
    }
    public void OnPrimaryColorChanged(Color color)
    {
        if (mapEditor.SelectedObject != null)
        {
            mapEditor.SelectedObject.LevelObjectInfos.PrimaryColor = color;
            mapEditor.Grid.UpdateObject(mapEditor.SelectedObject);
        }
        else
        {
            mapEditor.Grid.PrimaryColor = color;
        }
    }
    public void OnSecondaryColorChanged(Color color)
    {
        if (mapEditor.SelectedObject != null)
        {
            mapEditor.SelectedObject.LevelObjectInfos.SecondaryColor = color;
            mapEditor.Grid.UpdateObject(mapEditor.SelectedObject);
        }
        else
        {
            mapEditor.Grid.SecondaryColor = color;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        isVisible = !isVisible;
        primaryColorPicker.gameObject.SetActive(isVisible);
        secondaryColorPicker.gameObject.SetActive(isVisible);
        SelectColors(mapEditor.SelectedObject);
    }
}
