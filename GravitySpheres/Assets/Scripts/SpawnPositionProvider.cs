using UnityEngine;

public class SpawnPositionProvider : MonoBehaviour
{
    public float HeightLimit = 5.0f;
    [SerializeField]
    private Transform ground;
    private Vector3 GroundSize { get; set; }

    public Vector3 Provide()
    {
        return new Vector3(Random.Range(-GroundSize.x / 2, GroundSize.x / 2),
                           Random.Range(0.25f, HeightLimit),
                           Random.Range(-GroundSize.z / 2, GroundSize.z / 2));
    }

    private void Start()
    {
        GroundSize = ground.GetComponent<MeshRenderer>().bounds.size;
    }
}
