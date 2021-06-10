using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 시스템에서 파일을 생성하기 윈한 DLL
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;
public class DataManager : MonoBehaviour
{
    //싱글톤 선언
    private static DataManager _instance = null;

    public static DataManager instance { get { return _instance; } }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Load();
    }

    public int playerHP = 3;
    public string currentScene = "Level1";
    private void Awake()
    {
        _instance = this;
    }

    public void Save()
    {
        SaveData saveData = new SaveData();
        saveData.sceneName = currentScene;
        saveData.playerHP = playerHP;

        //파일 생성
        FileStream fileStream = File.Create(Application.persistentDataPath + "/svae.dat");

        Debug.Log("저장파일 생성");

        //직렬화
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, saveData);
        fileStream.Close();
    }

    public void Load()
    {
        // 파일이 있는지 확인하기
        if(File.Exists(Application.persistentDataPath + "/svae.dat") == true)
        {
            FileStream fileStream = File.Open(Application.persistentDataPath + "/svae.dat", FileMode.Open);

            if(fileStream != null && fileStream.Length > 0)
            {

                //역직렬화
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                SaveData saveData = (SaveData)binaryFormatter.Deserialize(fileStream);
                playerHP = saveData.playerHP;
                UIManager.instance.PlayerHP();
                currentScene = saveData.sceneName;

                fileStream.Close();
            }
        }
    }
}
