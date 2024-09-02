using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    Managers managerMaster;
    public void SetManagerMaster(Managers _managerMaster) { managerMaster = _managerMaster; }


    [SerializeField] PlayerData defaultPlayerData;
    PlayerData initializePlayerData;

    [SerializeField] OptionData defaultOptionData;
    OptionData initializeOptionData;

    string saveDataFilePath;
    string optionDataFilePath;
    string fileExtension;
    struct SaveData
    {
        public PlayerData playerData;
    }
    void Awake()
    {
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
    public void SaveOptionData()
    {
        StreamWriter writer;

        OptionData od;
        od = managerMaster.optionData;

        string jsonstr = JsonUtility.ToJson(od);

        string loadPath = optionDataFilePath + fileExtension;

        writer = new StreamWriter(loadPath, false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }
    public void LoadOptionData()
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
            managerMaster.optionData = od;
        }
        //ファイルが存在しない
        else
        {
            managerMaster.optionData = initializeOptionData;
            SaveOptionData();
        }
    }
}
