using System.Collections.Generic;
using UnityEngine;

public class AppStart : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> InitializeManagerList = new List<MonoBehaviour>();

    private void OnValidate()
    {
        for (int i = 0; i < InitializeManagerList.Count; i++)
        {
            MonoBehaviour currentComponent = InitializeManagerList[i];

            if (currentComponent is not IInitializeManager)
            {
                Debug.LogWarning($"Component {currentComponent.name} doesn't implement IInitializeManager. It will be removed.", this);

                InitializeManagerList.RemoveAt(i);
            }
        }
    }

    private void Awake()
        => InitializeAllManagers();

    private void InitializeAllManagers()
    {
        foreach (IInitializeManager Manager in InitializeManagerList)
            Manager.InitializeManager();
    }
}

public interface IInitializeManager
{
    public void InitializeManager();
}