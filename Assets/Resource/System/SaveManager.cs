using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    
    [SerializeField] PlayerData defaultPlayerData;
    static PlayerData initializePlayerData;

    [SerializeField] OptionData defaultOptionData;
    static OptionData initializeOptionData;

    static string saveDataFilePath;
    static string optionDataFilePath;
    static string fileExtension;
    struct SaveData
    {
        public PlayerData playerData;
    }
    void Awake()
    {
        instance = this;
        saveDataFilePath = Application.persistentDataPath + "/savedata";
        optionDataFilePath = Application.persistentDataPath + "/optiondata";
        fileExtension = ".json";
        initializePlayerData = defaultPlayerData;
        initializeOptionData = defaultOptionData;
    }
    void Start()
    {

    }

    /*
    public static void SavePlayerData(int _num)
    {
        StreamWriter writer;

        SaveData sd;
        sd.playerData = Player.instance.playerData;

        string jsonstr = JsonUtility.ToJson(sd);

        string loadPath = saveDataFilePath + _num.ToString() + fileExtension;

        writer = new StreamWriter(loadPath, false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }

    public static int CheckLoadData(int _num) 
    {
        string loadPath = saveDataFilePath + _num.ToString() + fileExtension;
        //ファイルが存在している
        if (File.Exists(loadPath))
        {
            SaveData sd;

            StreamReader reader = new StreamReader(loadPath);
            string datastr = reader.ReadToEnd();
            reader.Close();

            sd = JsonUtility.FromJson<SaveData>(datastr);

            return 1;
        }
        //ファイルが存在しない
        else
        {
            return 0;
        }
    }

    public static void LoadPlayerData(int _num)
    {
        string loadPath = saveDataFilePath + _num.ToString() + fileExtension;
        //ファイルが存在している
        if (File.Exists(loadPath))
        {
            SaveData sd;

            StreamReader reader = new StreamReader(loadPath);
            string datastr = reader.ReadToEnd();
            reader.Close();

            sd = JsonUtility.FromJson<SaveData>(datastr);
            Player.instance.playerData = sd.playerData;
            //SavePlayerData(_num);
        }
        //ファイルが存在しない
        else
        {
            Player.instance.playerData = initializePlayerData;
            //SavePlayerData(_num);
        }
    }
    */
    public static void SaveOptionData()
    {
        StreamWriter writer;

        OptionData od;
        od = Managers.optionData;

        string jsonstr = JsonUtility.ToJson(od);

        string loadPath = optionDataFilePath + fileExtension;

        writer = new StreamWriter(loadPath, false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }
    public static void LoadOptionData()
    {
        string loadPath = saveDataFilePath + fileExtension;
        //ファイルが存在している
        if (File.Exists(loadPath))
        {
            OptionData od;

            StreamReader reader = new StreamReader(loadPath);
            string datastr = reader.ReadToEnd();
            reader.Close();

            od = JsonUtility.FromJson<OptionData>(datastr);
            Managers.optionData = od;
        }
        //ファイルが存在しない
        else
        {
            Managers.optionData = initializeOptionData;
            SaveOptionData();
        }
    }
}
