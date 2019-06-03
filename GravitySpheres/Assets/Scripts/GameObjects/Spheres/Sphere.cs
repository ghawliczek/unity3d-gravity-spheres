using System.Collections;
using UnityEngine;

public class Sphere : MonoBehaviour, IPoolable
{
    public bool ReversePullForce { get; set; } = false;
    public int CollisionsCounter { get; set; } = 1;
    public float GravityAcceleration { get; } = 1;
    public float InitialMass { get; } = 0.1666667f;
    private Rigidbody Rigidbody { get; set; }
    private Renderer Renderer { get; set; }

    public void TemporarilyDisableCollision()
    {
        StartCoroutine(TemporarilyDisableCollisionRoutine());
    }

    public void DoHighSpeedShoot()
    {
        Rigidbody.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 250);
    }

    public void ResetState()
    {
        CollisionsCounter = 1;
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.mass = 4f / 3f * Mathf.Pow(0.5f, 3);
        transform.localScale = Vector3.one * 0.5f;
        GetComponent<SphereCollider>().enabled = true;
    }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Renderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (!Renderer.IsVisibleFrom(Camera.main))
        {
            SphereController.Instance.DestroySphere(this, shouldRemoveFromList: false);
        }
    }

    private void FixedUpdate()
    {
        DoGravityPull();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Rigidbody == null) return;

        var otherCollisionsCounter = other.transform.GetComponent<Sphere>().CollisionsCounter;

        if (ShouldPreserveOther(other))
        {
            SphereController.Instance.DestroySphere(this);
            return;
        }

        Rigidbody.mass = 4f / 3f * Mathf.Pow(0.5f * Mathf.Sqrt(CollisionsCounter + otherCollisionsCounter), 3);
        transform.localScale = Vector3.one * (0.5f * Mathf.Sqrt(CollisionsCounter + otherCollisionsCounter));
        CollisionsCounter += otherCollisionsCounter;

        if (Rigidbody.mass >= 50 * InitialMass)
        {
            SphereController.Instance.BlowSphere(this);
        }
    }

    private bool ShouldPreserveOther(Collider collider)
    {
        var colliderRigidbody = collider.gameObject.GetComponent<Rigidbody>();
        return colliderRigidbody.mass == Rigidbody.mass && collider.transform.position.y > transform.position.y
            || colliderRigidbody.mass > Rigidbody.mass;
    }

    private void DoGravityPull()
    {
        foreach (var sphere in SphereController.Instance.Spheres)
        {
            Rigidbody rbToPull = sphere.Rigidbody;
            Vector3 pullDirection = Rigidbody.position - rbToPull.position;

            if (pullDirection.magnitude == 0) return;

            Vector3 pullForce = Rigidbody.mass * rbToPull.mass * pullDirection.normalized / Mathf.Pow(pullDirection.magnitude, 2) * GravityAcceleration;

            if (ReversePullForce) pullForce *= -1;
            rbToPull.AddForce(pullForce);
        }
    }

    private IEnumerator TemporarilyDisableCollisionRoutine()
    {
        var collider = GetComponent<SphereCollider>();
        collider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        collider.enabled = true;
    }
}
