using UnityEngine;
using System.Collections.Generic;

public static class ObjectPooler
{
    public static Dictionary<string, Component> poolLookup = new Dictionary<string, Component>();
    public static Dictionary<string, Queue<Component>> poolDictionary = new Dictionary<string, Queue<Component>>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init()
    {
        poolLookup = new Dictionary<string, Component>();
        poolDictionary = new Dictionary<string, Queue<Component>>();
    }

    public static void EnqueueObject<T>(T item, string name) where T : Component
    {
        if (!item.gameObject.activeSelf)
            return;

        item.transform.position = Vector3.zero;
        item.gameObject.SetActive(false);

        poolDictionary[name].Enqueue(item);
    }

    public static T DequeueObject<T>(string key) where T : Component
    {
        if (poolDictionary[key].TryDequeue(out var item))
            return (T) item;

        return (T) EnqueueNewInstances(poolLookup[key], key);
    }

    public static T EnqueueNewInstances<T>(T item, string key) where T : Component
    {
        T newInstance = Object.Instantiate(item);

        newInstance.gameObject.SetActive(false);
        newInstance.transform.position = Vector3.zero;

        poolDictionary[key].Enqueue(newInstance);

        return newInstance;
    }

    public static void SetupPool<T>(Transform poolParent, T pooledItemPrefab, int poolSize, string dictionaryEntry) where T : Component
    {
        poolDictionary.Add(dictionaryEntry, new Queue<Component>());

        poolLookup.Add(dictionaryEntry, pooledItemPrefab);

        for (int i = 0; i < poolSize; i++)
        {
            T pooledInstance = Object.Instantiate(pooledItemPrefab);

            pooledInstance.gameObject.SetActive(false);
            pooledInstance.transform.SetParent(poolParent);

            poolDictionary[dictionaryEntry].Enqueue((T) pooledInstance);
        }
    }
}
