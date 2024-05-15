using System.IO;
using UnityEngine;

public class ClearCache : MonoBehaviour
{
    public void Clear()
    {
        PlayerPrefs.DeleteAll();
        DeleteAllFilesInFolder();
  
    }

    void DeleteAllFilesInFolder()
    {
        var path = Application.persistentDataPath;
        if (!Directory.Exists(path)) return;
        var files = Directory.GetFiles(path);
        foreach (var file in files)
        {
            File.Delete(file);
            Debug.Log("File deleted: " + file);
        }
    }
}