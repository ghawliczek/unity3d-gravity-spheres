using System.Collections;
using System.Linq;
using UnityEngine;

public class Sphere : MonoBehaviour, IPoolable
{
    public bool ReversePullForce = false;
    public float GravityAcceleration { get; set; } = 1;
    public int CollidesCounter { get; set; } = 1;
    public float InitialMass { get; set; } = 0.1666667f;
    private Rigidbody Rigidbody { get; set; }
    private Renderer Renderer { get; set; }

    public void TemporarilyDisableCollision()
    {
        StartCoroutine(DisableCollisionRoutine());
    }

    public void DoHighSpeedShoot()
    {
        Rigidbody.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(0.01f, 1f), Random.Range(-1f, 1f)) * 250);
    }

    public void ResetState()
    {
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.mass = InitialMass;
        transform.localScale = Vector3.one * 0.5f;
        GetComponent<SphereCollider>().enabled = true;
    }

    private void OnEnable()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Renderer = GetComponent<MeshRenderer>();
    }

    private void FixedUpdate()
    {
        Pull();
        if (!Renderer.IsVisibleFrom(Camera.main))
        {
            SphereController.Instance.DestroySphere(this, shouldRemoveFromList: false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Rigidbody == null || collision.rigidbody == null) return;

        if (collision.rigidbody.mass == Rigidbody.mass && collision.transform.position.y > transform.position.y
            || collision.rigidbody.mass > Rigidbody.mass)
        {
            SphereController.Instance.DestroySphere(this);
            return;
        }

        var collisionCollidesCounter = collision.transform.GetComponent<Sphere>().CollidesCounter;
        var mass = Rigidbody.mass;
        Rigidbody.mass = 4f / 3f * Mathf.Pow(0.5f * Mathf.Sqrt(CollidesCounter + collisionCollidesCounter), 3);
        if (mass > Rigidbody.mass)
        {
            Debug.LogError("XDD");
        }
        transform.localScale = Vector3.one * (0.5f * Mathf.Sqrt(CollidesCounter + collisionCollidesCounter));

        CollidesCounter++;

        if (Rigidbody.mass >= 50 * InitialMass)
        {
            Debug.Log(Rigidbody.mass);
            Debug.Log(CollidesCounter);
            SphereController.Instance.BlowSphere(this);
        }
    }

    private void Pull()
    {
        foreach (var sphere in SphereController.Instance.Spheres.Where(b => b != this && b.enabled))
        {
            Rigidbody rbToPull = sphere.Rigidbody;

            Vector3 pullDirection = Rigidbody.position - rbToPull.position;

            if (pullDirection.magnitude == 0) return;

            Vector3 pullForce = Rigidbody.mass * rbToPull.mass * pullDirection.normalized / Mathf.Pow(pullDirection.magnitude, 2) * GravityAcceleration;

            if (ReversePullForce) pullForce *= -1;
            rbToPull.AddForce(pullForce);
        }
    }

    private IEnumerator DisableCollisionRoutine()
    {
        var collider = GetComponent<SphereCollider>();
        collider.enabled = false;
        yield return new WaitForSeconds(0.5f);
        collider.enabled = true;
    }
}
