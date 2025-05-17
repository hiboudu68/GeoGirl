using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class LevelWriter
{
    private Level levelData;
    private FileStream fs;

    public LevelWriter(Level levelData, string basePath)
    {
        this.levelData = levelData;

        string path;
        if (basePath.Contains(".geomap") == false)
            path = Path.Join(basePath, levelData.Id + ".geomap");
        else path = basePath;
        if (File.Exists(path))
            File.Delete(path);
        fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
        WriteHeader();
    }
    private void WriteHeader()
    {
        WriteString(levelData.Id);
        WriteString(levelData.Name);

        byte[] buffer = new byte[18];
        buffer[0] = (byte)(levelData.IsMainLevel ? 1 : 0);

        fs.Write(buffer);
    }
    public void Close()
    {
        fs.Flush();
        fs.Dispose();
        fs = null;
        levelData = null;
    }
    private void WriteString(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            fs.Write(new byte[4]);
            return;
        }

        byte[] strBytes = Encoding.UTF8.GetBytes(str);
        byte[] head = new byte[strBytes.Length + 2];
        BitConverter.GetBytes((short)strBytes.Length).CopyTo(head, 0);
        strBytes.CopyTo(head, 2);
        fs.Write(head);
    }
    public void WriteObjs(LevelObject[] objs)
    {
        const int objSize = 27;

        byte[] buffer = new byte[objSize * objs.Length + 4];
        BitConverter.GetBytes(objs.Length).CopyTo(buffer, 0);

        for (int i = 0; i < objs.Length; i++)
            WriteObj(objs[i].TileID, objs[i], buffer, i * objSize + 4);

        fs.Write(buffer);
    }
    public void WriteObjs(BaseObject[] objs)
    {
        List<BaseObject> objList = new(objs);
        objList.Sort((BaseObject a, BaseObject b) => a.LevelObjectInfos.X.CompareTo(b.LevelObjectInfos.X));

        const int objSize = 27;

        byte[] buffer = new byte[objSize * objList.Count + 4];
        BitConverter.GetBytes(objList.Count).CopyTo(buffer, 0);

        for (int i = 0; i < objList.Count; i++)
            WriteObj(objList[i].TileInfos.Id, objList[i].LevelObjectInfos, buffer, i * objSize + 4);

        fs.Write(buffer);
    }
    public void SetMusicData(byte[] data)
    {
        if (data != null)
        {
            Debug.Log("Set music data at position " + fs.Position + ", " + data.Length + " bytes");
            fs.Write(data);
        }
    }
    private void WriteObj(int tileId, LevelObject obj, byte[] buffer, int offset)
    {
        BitConverter.GetBytes(obj.Id).CopyTo(buffer, offset);
        BitConverter.GetBytes(obj.X).CopyTo(buffer, offset + 4);
        BitConverter.GetBytes(obj.Y).CopyTo(buffer, offset + 8);
        BitConverter.GetBytes(obj.Z).CopyTo(buffer, offset + 12);
        buffer[offset + 16] = obj.Rotation;

        BitConverter.GetBytes(tileId).CopyTo(buffer, offset + 17);

        buffer[offset + 21] = (byte)(obj.PrimaryColor.r * 255);
        buffer[offset + 22] = (byte)(obj.PrimaryColor.g * 255);
        buffer[offset + 23] = (byte)(obj.PrimaryColor.b * 255);

        buffer[offset + 24] = (byte)(obj.SecondaryColor.r * 255);
        buffer[offset + 25] = (byte)(obj.SecondaryColor.g * 255);
        buffer[offset + 26] = (byte)(obj.SecondaryColor.b * 255);
    }
}
