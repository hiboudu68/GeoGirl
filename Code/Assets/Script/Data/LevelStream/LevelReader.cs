using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class LevelReader
{
    private FileStream fs;
    public Level levelData;
    public byte[] MusicBytes;
    public List<LevelObject> objects = new();

    public LevelReader(string path)
    {
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            this.fs = fs;
            levelData = new()
            {
                Id = ReadString(),
                Name = ReadString(),
                IsMainLevel = ReadByte() > 0,
            };
            fs.Position += 17; // Ignore old data

            int objCount = ReadInt();
            for (int i = 0; i < objCount; i++)
                ReadObject();

            Debug.Log("Reading music at position " + fs.Position + ", " + (fs.Length - fs.Position) + " bytes");
            if ((fs.Length - fs.Position) > 0)
            {
                MusicBytes = ReadBytes(fs.Length - fs.Position);
            }
            else MusicBytes = Array.Empty<byte>();
        }
    }
    private void ReadObject()
    {
        LevelObject obj = new()
        {
            Id = ReadInt(),
            X = ReadInt(),
            Y = ReadInt(),
            Z = ReadInt(),
            Rotation = ReadByte(),
            TileID = ReadInt(),
            PrimaryColor = new(ReadByte() / 255f, ReadByte() / 255f, ReadByte() / 255f),
            SecondaryColor = new(ReadByte() / 255f, ReadByte() / 255f, ReadByte() / 255f)
        };
        objects.Add(obj);
    }
    private string ReadString()
    {
        short size = ReadShort();
        return Encoding.UTF8.GetString(ReadBytes(size));
    }
    private byte ReadByte()
        => (byte)fs.ReadByte();
    private short ReadShort()
        => BitConverter.ToInt16(ReadBytes(2));
    private int ReadInt()
        => BitConverter.ToInt32(ReadBytes(4));
    private long ReadLong()
        => BitConverter.ToInt64(ReadBytes(8));
    private byte[] ReadBytes(long len)
    {
        byte[] bytes = new byte[len];
        fs.Read(bytes);
        return bytes;
    }
}
