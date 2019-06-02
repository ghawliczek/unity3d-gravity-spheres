using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance { get; private set; }
    public List<Pool> Pools;

    public List<List<GameObject>> PooledObjectsList;
    private List<int> Positions { get; } = new List<int>();

    public List<GameObject> GetEntirePool(int index)
    {
        return PooledObjectsList[index];
    }

    public GameObject GetNextPooled(int poolIndex, Vector3 position, Quaternion rotation)
    {
        var obj = GetNextPooled(poolIndex);
        obj.GetComponent<IPoolable>()?.ResetState();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        return obj;
    }

    public int CreateNewPool(GameObject prefab, int size = 3, bool expandable = true)
    {
        Pool pool = new Pool
        {
            Prefab = prefab,
            Size = size,
            Expandable = expandable
        };
        int poolIndex = Pools.Count;
        Pools.Add(pool);
        PreparePool(pool);

        return poolIndex;
    }

    private void Awake()
    {
        Instance = this;

        PooledObjectsList = new List<List<GameObject>>();

        foreach (Pool pool in Pools)
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
        PooledObjectsList.Add(pooledObjects);
        Positions.Add(0);
    }

    private GameObject GetNextPooled(int poolIndex)
    {
        int poolSize = PooledObjectsList[poolIndex].Count;
        for (int i = Positions[poolIndex] + 1; i < Positions[poolIndex] + poolSize; i++)
        {
            if (!PooledObjectsList[poolIndex][i % poolSize].activeInHierarchy)
            {
                Positions[poolIndex] = i % poolSize;
                return PooledObjectsList[poolIndex][i % poolSize];
            }
        }

        if (Pools[poolIndex].Expandable)
        {
            GameObject obj = Instantiate(Pools[poolIndex].Prefab);
            obj.SetActive(false);
            obj.transform.parent = transform;
            PooledObjectsList[poolIndex].Add(obj);
            return obj;
        }

        return null;
    }
}
