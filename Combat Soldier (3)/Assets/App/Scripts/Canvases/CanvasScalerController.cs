using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class CanvasScalerController : MonoBehaviour
{
    private void Awake()
    {
        if (AppManager.GetDeviceType() == DeviceType.Tablet)
            GetComponent<CanvasScaler>().matchWidthOrHeight = 1f;
    }
}