using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UIManager : MonoBehaviour
{
    public Button resetButton;
    public Button exitButton;
    public Button saveButton;
    public GameObject jawModel;
    public bool reset;
    public DraggableTooth draggable;
    public DraggableTooth draggable1;
    public DraggableTooth draggable2;
    private Vector3 initialJawPosition;
    private Quaternion initialJawRotation;

    private JawData loadedJawData;

    void Start()
    {
        initialJawPosition = jawModel.transform.position;
        initialJawRotation = jawModel.transform.rotation;

        resetButton.onClick.AddListener(ResetJaw);
        exitButton.onClick.AddListener(ExitApplication);
        saveButton.onClick.AddListener(SaveJawData);

        LoadJawData();
    }

    public void ResetJaw()
    {
        reset = true;
        jawModel.transform.position = initialJawPosition;
        jawModel.transform.rotation = initialJawRotation;

        foreach (Transform child in jawModel.transform)
        {
            draggable.ResetPosition();
            draggable1.ResetPosition();
            draggable2.ResetPosition();
            DraggableTooth draggableTooth = child.GetComponent<DraggableTooth>();
            if (draggableTooth != null)
            {
                draggableTooth.transform.SetParent(jawModel.transform);
                draggableTooth.ResetPosition();

            }
        }
    }

    void ExitApplication()
    {
        Application.Quit();
    }

    void SaveJawData()
    {
        JawData data = new JawData();
        data.position = jawModel.transform.position;
        data.rotation = jawModel.transform.rotation;

        string jsonData = JsonUtility.ToJson(data);
        string filePath = Application.persistentDataPath + "/jawdata.json";
        File.WriteAllText(filePath, jsonData);

        Debug.Log("Данные челюсти сохранены в: " + filePath);
    }

    void LoadJawData()
    {
        string filePath = Application.persistentDataPath + "/jawdata.json";
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            loadedJawData = JsonUtility.FromJson<JawData>(jsonData);

            if (loadedJawData != null)
            {
                jawModel.transform.position = loadedJawData.position;
                jawModel.transform.rotation = loadedJawData.rotation;
            }
        }
    }
}

[System.Serializable]
public class JawData
{
    public Vector3 position;
    public Quaternion rotation;
}