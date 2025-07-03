using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AppManager : MonoBehaviour // to do
{
    public bool IsInitialized { get; private set; } = false;

    public void Initialize()
    {
        // Manually set target FPS to 30
        Application.targetFrameRate = 30;

        Debug.Log("Welcome to " + Application.productName + " by " + Application.companyName);
        Debug.Log("Build version: " + Application.version);
        Debug.Log("Current app mode: " + (IsProductionVersion() ? "Production" : "Debug"));
        Debug.Log("Target frame rate: " + Application.targetFrameRate);

        // Mark that plugin is initialized
        IsInitialized = true;
    }

    public static IEnumerator CheckInternetConnection(Action<bool> result)
    {
        Debug.Log("Checking Internet availability...");

        // Create web request to google.com
        using var request = UnityWebRequest.Get("https://www.google.com");

        // Waiting for result
        yield return request.SendWebRequest();

        // Check if result is successful
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

    #region Save Quit Date & Time
    //private void OnApplicationPause(bool pause)
    //{
    //    if (!pause)
    //        return;
    //
    //    SaveQuitDateTime();
    //}

    //private void OnApplicationQuit()
    //{
    //    SaveQuitDateTime();
    //}

    //private void SaveQuitDateTime()
    //{
    //    PlayerData.General.Offline = DateTime.UtcNow;
    //}
    #endregion

    #region Runtime Platform
    public static ApplicationPlatform platform
    {
        get
        {
            if (!Application.isEditor)
            {
#if UNITY_ANDROID
                return ApplicationPlatform.Android;
#elif UNITY_IOS
                return ApplicationPlatform.iOS;
#elif UNITY_WEBGL
                return ApplicationPlatform.Web;
#else
                return ApplicationPlatform.Unknown;
#endif
            }
            else
            {
                return ApplicationPlatform.Editor;
            }
        }
    }
    #endregion

    #region Device Type
    public static DeviceType GetDeviceType()
    {
#if UNITY_ANDROID
        bool isTablet = GetDeviceDiagonalSizeInInches() > 6.5f && GetDeviceAspectRatio() < 2f;
        return isTablet ? DeviceType.Tablet : DeviceType.Phone;
#elif UNITY_IOS
        bool isTablet = UnityEngine.iOS.Device.generation.ToString().Contains("iPad");
        return isTablet ? DeviceType.Tablet : DeviceType.Phone;
#else
    return DeviceType.Unknown; // to do
#endif
    }

    public static float GetDeviceAspectRatio()
    {
        return Mathf.Max(Screen.width, Screen.height) / Mathf.Min(Screen.width, Screen.height);
    }

    public static float GetDeviceDiagonalSizeInInches()
    {
        float screenWidth = Screen.width / Screen.dpi;
        float screenHeight = Screen.height / Screen.dpi;
        float diagonalSizeInInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
        return diagonalSizeInInches;
    }
    #endregion

    public static bool IsProductionVersion()
    {
        return Application.version.Contains(".");
    }
}

public enum ApplicationPlatform
{
    Unknown = 0,
    Editor = 1,
    Android = 2,
    iOS = 3,
    Web = 4
}

public enum DeviceType
{
    Unknown = 0,
    Desktop = 1,
    Phone = 2,
    Tablet = 3
}

// © Cobra Games Studio - 2023