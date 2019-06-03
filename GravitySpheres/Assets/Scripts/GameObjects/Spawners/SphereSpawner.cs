using UnityEngine;

public class SphereSpawner : MonoBehaviour
{
    [SerializeField]
    private SpawnPositionProvider positionProvider;
    [SerializeField]
    private int poolIndex;
    private ObjectPooler Pooler { get; set; }

    public GameObject Spawn()
    {
        return Spawn(positionProvider.Provide());
    }

    public GameObject Spawn(Vector3 position)
    {
        return Pooler.GetNextPooled(poolIndex, position, Quaternion.identity);
    }

    private void OnEnable()
    {
        Pooler = ObjectPooler.Instance;
    }
}
