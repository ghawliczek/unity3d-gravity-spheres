using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField]
    private List<Pool> pools;
    [SerializeField]
    private List<List<GameObject>> pooledObjectsList;
    public static ObjectPooler Instance { get; private set; }
    private List<int> Positions { get; } = new List<int>();

    /// <summary>
    /// Returns next available <see cref="GameObject"/> from <see cref="Pool"/> at given index
    /// </summary>
    public GameObject GetNextPooled(int poolIndex, Vector3 position, Quaternion rotation)
    {
        var obj = GetNextPooled(poolIndex);
        obj.GetComponent<IPoolable>()?.ResetState();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        return obj;
    }

    /// <summary>
    /// Creates new <see cref="Pool"/> at runtime
    /// </summary>
    /// <returns>Created pool index</returns>
    public int CreateNewPool(GameObject prefab, int size = 3, bool expandable = true)
    {
        Pool pool = new Pool
        {
            Prefab = prefab,
            Size = size,
            Expandable = expandable
        };
        int poolIndex = pools.Count;
        pools.Add(pool);
        PreparePool(pool);

        return poolIndex;
    }

    private void Awake()
    {
        Instance = this;
        pooledObjectsList = new List<List<GameObject>>();

        foreach (Pool pool in pools)
        {
            PreparePool(pool);
        }
    }

    private void PreparePool(Pool pool)
    {
        var pooledObjects = new List<GameObject>();
        for (int i = 0; i < pool.Size; i++)
        {
            GameObject obj = Instantiate(pool.Prefab);
            obj.SetActive(false);
            obj.transform.parent = transform;
            pooledObjects.Add(obj);
        }
        pooledObjectsList.Add(pooledObjects);
        Positions.Add(0);
    }

    private GameObject GetNextPooled(int poolIndex)
    {
        int poolSize = pooledObjectsList[poolIndex].Count;
        for (int i = Positions[poolIndex] + 1; i < Positions[poolIndex] + poolSize; i++)
        {
            if (!pooledObjectsList[poolIndex][i % poolSize].activeInHierarchy)
            {
                Positions[poolIndex] = i % poolSize;
                return pooledObjectsList[poolIndex][i % poolSize];
            }
        }

        if (pools[poolIndex].Expandable)
        {
            GameObject obj = Instantiate(pools[poolIndex].Prefab);
            obj.SetActive(false);
            obj.transform.parent = transform;
            pooledObjectsList[poolIndex].Add(obj);
            return obj;
        }

        return null;
    }
}
