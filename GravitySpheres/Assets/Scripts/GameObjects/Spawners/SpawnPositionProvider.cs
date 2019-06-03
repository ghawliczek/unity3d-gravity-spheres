using UnityEngine;

/// <summary>
/// Provides random spawn position based on the size of the ground
/// </summary>
public class SpawnPositionProvider : MonoBehaviour
{
    [SerializeField]
    private Transform ground;
    public float HeightLimit { get; } = 5.0f;
    private Vector3 GroundSize { get; set; }

    public Vector3 Provide()
    {
        return new Vector3(Random.Range(-GroundSize.x / 2, GroundSize.x / 2),
                           Random.Range(0.25f, HeightLimit),
                           Random.Range(-GroundSize.z / 2, GroundSize.z / 2));
    }

    private void OnEnable()
    {
        GroundSize = ground.GetComponent<MeshRenderer>().bounds.size;
    }
}
