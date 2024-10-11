using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{

    [SerializeField] OptionData defaultOptionData;

    string optionDataFilePath;
    string fileExtension;
    struct SaveData
    {
        public string playerName;
        public float masterVolume;
        public float bgmVolume;
        public float sfxVolume;
        public float voiceVolume;
        public bool cameraShakeOn;
        public float mortarSensitive;
    }
    void Awake()
    {
        optionDataFilePath = Application.persistentDataPath + "/optiondata";
        fileExtension = ".json";
    }
    void Start()
    {
        //オプションデータのロード
        Managers.instance.optionData = LoadOptionData();
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
    public void SaveOptionData(OptionData _receiveData)
    {
        StreamWriter writer;

        string jsonstr = JsonUtility.ToJson(_receiveData);

        string loadPath = optionDataFilePath + fileExtension;

        writer = new StreamWriter(loadPath, false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();
    }
    public OptionData LoadOptionData()
    {
        OptionData returnData;

        //string loadPath = optionDataFilePath + fileExtension;
        string loadPath = Application.persistentDataPath + "/optiondata" + ".json";

        //ファイルが存在している
        if (File.Exists(loadPath))
        {
            StreamReader reader = new StreamReader(loadPath);
            string datastr = reader.ReadToEnd();
            reader.Close();

            SaveData sd = JsonUtility.FromJson<SaveData>(datastr);
            returnData = ConvertSaveData(sd);
        }
        //ファイルが存在しない
        else
        {
            returnData = new OptionData();
            returnData.Init(defaultOptionData);
        }

        return returnData;
    }

    OptionData ConvertSaveData(SaveData _sd)
    {
        OptionData returndata=new OptionData();
        returndata.playerName = _sd.playerName;
        returndata.masterVolume = _sd.masterVolume;
        returndata.bgmVolume = _sd.bgmVolume;
        returndata.sfxVolume = _sd.sfxVolume;
        returndata.voiceVolume = _sd.voiceVolume;
        returndata.cameraShakeOn = _sd.cameraShakeOn;
        returndata.mortarSensitive = _sd.mortarSensitive;

        return returndata;
    }
}
