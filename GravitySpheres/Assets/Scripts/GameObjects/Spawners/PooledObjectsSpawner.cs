using UnityEngine;

/// <summary>
/// Instantiates objects from given <see cref="Pool"/> using <see cref="ObjectPooler"/>
/// </summary>
public class PooledObjectsSpawner : MonoBehaviour
{
    [SerializeField]
    private SpawnPositionProvider positionProvider;
    [SerializeField]
    private int poolIndex;
    private ObjectPooler Pooler { get; set; }

    /// <summary>
    /// Instantiates next object from the pool at random position received from spawn position provider
    /// </summary>
    /// <returns></returns>
    public GameObject Spawn()
    {
        return Spawn(positionProvider.Provide());
    }

    /// <summary>
    /// Instantiates next object from the pool at given position
    /// </summary>
    /// <returns></returns>
    public GameObject Spawn(Vector3 position)
    {
        return Pooler.GetNextPooled(poolIndex, position, Quaternion.identity);
    }

    private void OnEnable()
    {
        Pooler = ObjectPooler.Instance;
    }
}
