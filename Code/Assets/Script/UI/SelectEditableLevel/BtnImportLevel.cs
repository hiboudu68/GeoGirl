using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnImportLevel : NativeFileDialog, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        string file = OpenFileDialog("geomap");
        if (!string.IsNullOrEmpty(file))
        {
            LevelReader reader = new LevelReader(file);
            reader.levelData.Id = Guid.NewGuid().ToString();
            LevelsManager.CreateLevel(reader.levelData);
            LevelWriter writer = new LevelWriter(reader.levelData, Application.persistentDataPath);
            Debug.Log(reader.objects.Count + " objects imported !");
            writer.WriteObjs(reader.objects.ToArray());
            writer.SetMusicData(reader.MusicBytes);
            writer.Close();
        }
    }
}
