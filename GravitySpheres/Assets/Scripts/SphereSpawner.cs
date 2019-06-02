using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    [SerializeField]
    private SpawnPositionProvider positionProvider;
    [SerializeField]
    private int poolIndex;
    private ObjectPooler pooler;

    public GameObject Spawn()
    {
        return Spawn(positionProvider.Provide());
    }

    public GameObject Spawn(Vector3 position)
    {
        return pooler.GetNextPooled(poolIndex, position, Quaternion.identity);
    }

    private void Start()
    {
        pooler = ObjectPooler.Instance;
    }
}
