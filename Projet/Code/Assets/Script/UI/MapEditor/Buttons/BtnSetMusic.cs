using UnityEngine;
using UnityEngine.EventSystems;

public class BtnSetMusic : NativeFileDialog, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        string file = OpenFileDialog("mp3");
        if (!string.IsNullOrEmpty(file))
        {
            GetComponentInParent<MapEditorManager>().DefineMusic(file);
        }
        else Debug.Log("No file selected");
    }
}