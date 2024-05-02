using System;
using System.IO;
using UnityEngine;
using TMPro;

public class VRActionLogger : MonoBehaviour
{
    public TextMeshProUGUI vrLoggerDisplay;
    StreamWriter file;
    public LayerMask layerMask;
    string logData = "";

    void Start()
    {
        string fname = DateTime.Now.ToString("HH-mm-ss") + " Log.csv";
        string path = Path.Combine(Application.persistentDataPath, fname);
        file = new StreamWriter(path);
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Vector3 controllerPosition = transform.position;
            Vector3 controllerForward = transform.forward;
            Ray ray = new Ray(controllerPosition, controllerForward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                string objectName = hit.collider.gameObject.name;
                string currentTime = GetCurrentTime();
                AddActionToLog(objectName, currentTime);
            }
        }
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            SaveVRActionLog();
        }
    }

    string GetCurrentTime()
    {
        DateTime currentTime = DateTime.Now;
        return currentTime.ToString("HH:mm:ss");
    }

    void AddActionToLog(string gameObjectClicked, string currentTime)
    {
        logData += $"Clicked on: {gameObjectClicked} at {currentTime}\n";
        Debug.Log("The gameobject you clicked was added to the log.");
        Debug.Log($"Clicked on: {gameObjectClicked} at {currentTime}");
        vrLoggerDisplay.text = gameObjectClicked;
    }

    void SaveVRActionLog()
    {
        if (!string.IsNullOrEmpty(logData))
        {
            file.Write(logData);
            vrLoggerDisplay.text = "Log Saved";
        }
        else
        {
            Debug.Log("Log is empty. No data to save.");
        }

        file.Close();
    }

    void OnDestroy()
    {
        if (file != null)
        {
            file.Close();
        }
    }
}
