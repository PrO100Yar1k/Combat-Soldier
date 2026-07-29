using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AppManager : MonoBehaviour // to do
{
    public bool IsInitialized { get; private set; } = false;

    public void Initialize()
    {
        Debug.Log("Welcome to " + Application.productName + " by " + Application.companyName);
        Debug.Log("Build version: " + Application.version);
        Debug.Log("Current app mode: " + (IsProductionVersion() ? "Production" : "Debug"));
        Debug.Log("Target frame rate: " + Application.targetFrameRate);

        IsInitialized = true;
    }

    public static IEnumerator CheckInternetConnection(Action<bool> result)
    {
        Debug.Log("Checking Internet availability...");

        using var request = UnityWebRequest.Get("https://www.google.com");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Internet connection is available");
            result(true);
        }
        else
        {
            Debug.LogError("Internet connection in unavailable");
            result(false);
        }
    }

    public static bool IsProductionVersion()
    {
        return Application.version.Contains(".");
    }

    #region Save Quit Date & Time

    private void OnApplicationPause(bool isApplicationPaused)
    {
        if (!isApplicationPaused)
            return;
    
        //SaveQuitDateTime();
    }

    private void OnApplicationQuit()
    {
        //SaveQuitDateTime();
    }

    #endregion
}