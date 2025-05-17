using UnityEngine.EventSystems;

public class BtnExport : NativeFileDialog, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        MapEditorManager mapEditor = GetComponentInParent<MapEditorManager>();
        string file = SaveFileDialog(mapEditor.Grid.CurrentLevel.Name, "geomap");
        LevelWriter levelWriter = new(mapEditor.Grid.CurrentLevel, file);
        levelWriter.WriteObjs(mapEditor.Grid.Objects);
        levelWriter.SetMusicData(mapEditor.Grid.musicBytes);
        levelWriter.Close();
    }
}
