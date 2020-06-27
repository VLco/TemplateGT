using System.Collections;
using SimpleFileBrowser;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.Android;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

static class RScript
{
    public static int countPoint { get; set; } = 2;
    public static int curPoint { get; set; } = 0;
    public static int curTask { get; set; } = 0;
    public static ResForPoint[] resources { get; set; }

    private static string nameArchive { get; set; } = null;

    private static string pathArchive { get; set; }



    static public IEnumerator ResourceFromArchive()
    {

        FileBrowser.SetFilters(true, new FileBrowser.Filter("ZipArchive", ".zip"));
        FileBrowser.SetDefaultFilter(".zip");
        FileBrowser.SetExcludedExtensions(".exe", ".lnk", ".tmp");

        yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");

        Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);

        if (FileBrowser.Success)
        {
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result);
            try
            {
                

                MemoryStream ms = new MemoryStream(bytes);
                
                using (ZipArchive zip = new ZipArchive(ms))
                {
                    if(FileBrowserHelpers.GetFilename(FileBrowser.Result) != nameArchive)
                    {
                        ResourceAllocation(zip.GetEntry("DataFile.xml"));
                        nameArchive = FileBrowserHelpers.GetFilename(FileBrowser.Result);
                    }
                    foreach(ZipArchiveEntry entry in zip.Entries)
                    {
                        if(entry.FullName != "DataFile.xml")
                            ResourceAllocation(entry);
                    }
                }

                 
            }
            catch(Exception e)
            {
                Debug.LogError("Error: " + e.Message);
                nameArchive = null;
            }


        }
    }


    static void ResourceAllocation(ZipArchiveEntry entry)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            string[] fullName = entry.FullName.Split('.')[0].Split(new char[] {'/', '\\'}, StringSplitOptions.RemoveEmptyEntries);
            Debug.Log(entry.FullName);
            entry.Open().CopyTo(ms);
            if (fullName[0] == "DataFile")
            {
                try
                {
                    ms.Position = 0;
                    XmlSerializer formatter = new XmlSerializer(typeof(ResForPoint[]));
                    resources = (ResForPoint[])formatter.Deserialize(ms);
                    Debug.Log(resources[0].infoTitle);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                    throw;
                }
                finally
                {
                    ms.Close();
                }
            }
            else if (fullName.Length >= 2)
            {
                if (fullName[0].IndexOf("Point_") != -1)
                {
                    if (fullName.Length >= 3)
                    {
                        if (fullName.Length >= 4)
                        {
                            if (fullName[1].IndexOf("Task_") != -1)
                            {
                                if (fullName[2].IndexOf("Target") != -1)
                                {
                                    if (fullName[3].IndexOf("Texture_") != -1)
                                    {
                                        Texture2D temp = new Texture2D(2, 2);
                                        temp.LoadImage(ms.ToArray());
                                        int numPoint = Int32.Parse(fullName[0].Substring("Point_".Length));
                                        int numTask = Int32.Parse(fullName[1].Substring("Task_".Length));
                                        int numTexture = Int32.Parse(fullName[3].Substring("Texture_".Length));
                                        ResForPoint t1 = resources[numPoint];
                                        Task t2 = t1.tasks[numTask];
                                        Texture2D[] t3 = t2.textures;
                                        if(t3.Length < numTexture+1)
                                            Array.Resize<Texture2D>(ref t3, numTexture + 1);
                                        t3[numTexture] = temp;
                                        t2.textures = t3;
                                    }
                                    else
                                    {
                                        Debug.LogWarning("Resource allocation error: texture not found LVL3");

                                    }
                                }
                                else
                                {
                                    Debug.LogWarning("Resource allocation error: task not found LVL2");

                                }

                            }

                        }
                    }
                   
                } 
                else
                {
                    Debug.LogWarning("Resource allocation error: unknown type LVL1");
                }

            }
            else
            {
                Debug.LogWarning("Resource allocation error: unknown type LVL0");
            }
        }

    }


    static public void ReqestPermission()
    {
        #if PLATFORM_ANDROID
                if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead) || !Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
                {
                    Permission.RequestUserPermission(Permission.ExternalStorageRead);
                    Permission.RequestUserPermission(Permission.ExternalStorageWrite);

                }
        #endif
    }


    static public string getNameResourcesArchive()
    {
        return nameArchive;
    }


}


public class RScriptHelper: MonoBehaviour
{

}


[Serializable]
public class Task
{
    public typeTask typeTask { get; set; }
    public string desc { get; set; }
    public string title { get; set; }

    public string[] descObj { get; set; } = new string[] { };

    [XmlIgnore]
    public bool isDone { get; set; } = false;
    [XmlIgnore]
    public Texture2D[] textures { get; set; } = new Texture2D[0] { };


    public Task(typeTask _type, string _desc, string _title, string[] _descObj)
    {
        this.title = _title;
        this.typeTask = _type;
        this.desc = _desc;
        if (typeTask.Tour == _type)
        {

            this.descObj = _descObj;
        }
        else
        {
            Debug.LogError("Error: typeTask is not tour");
        }
    }

    public Task(typeTask _type, string _desc, string _title)
    {
        this.title = _title;
        this.typeTask = _type;
        this.desc = _desc;
    }

    public Task() { }

}

[Serializable]
public class ResForPoint
{
    public int countTasks { get; set; }
    public Task[] tasks { get; set; }
    public string infoText { get; set; }
    public string infoTitle { get; set; }

    public ResForPoint(int countTasks, Task[] tasks, string infoText, string infoTitle)
    {
        this.countTasks = countTasks;
        this.tasks = tasks;
        this.infoText = infoText;
        this.infoTitle = infoTitle;
    }

    public ResForPoint()
    {

    }

}


public enum typeTask
{
    Find = 1,
    MultiFind,
    Tour,

}


