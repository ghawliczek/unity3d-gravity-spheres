using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    public UiTextRefresher uiTextRefresher;
    public int sphereLimit = 250;
    public static SphereController Instance { get; private set; }
    public List<Sphere> Spheres { get; } = new List<Sphere>();
    private WaitForSeconds QuarterSecond { get; } = new WaitForSeconds(0.25f);
    private SphereSpawner SphereSpawner { get; set; }


    public void DestroySphere(Sphere sphere, bool shouldRemoveFromList = true)
    {
        if (shouldRemoveFromList)
        {
            if (Spheres.Contains(sphere))
            {
                Spheres.Remove(sphere);
            }
            uiTextRefresher.Refresh($"{Spheres.Count:D3}");
        }
        sphere.gameObject.SetActive(false);
    }

    public void AddSphere(Sphere sphere)
    {
        if (!Spheres.Contains(sphere))
        {
            Spheres.Add(sphere);
        }
        uiTextRefresher.Refresh($"{Spheres.Count:D3}");
    }

    public void BlowSphere(Sphere sphere)
    {
        for (int i = 0; i < sphere.CollidesCounter; i++)
        {
            var spawnedSphere = SphereSpawner.Spawn(sphere.transform.position).GetComponent<Sphere>();
            spawnedSphere.TemporarilyDisableCollision();
            spawnedSphere.DoHighSpeedShoot();
            AddSphere(spawnedSphere);
        }
        DestroySphere(sphere);
    }

    private void OnEnable()
    {
        Instance = this;
        SphereSpawner = GetComponent<SphereSpawner>();
    }

    private void Start()
    {
        StartCoroutine(SphereRoutine());
    }

    private IEnumerator SphereRoutine()
    {
        while (Spheres.Count < sphereLimit)
        {
            var newSphere = SphereSpawner.Spawn().GetComponent<Sphere>();
            Spheres.Add(newSphere);
            uiTextRefresher.Refresh($"{Spheres.Count:D3}");

            yield return QuarterSecond;
        }

        foreach (var sphere in Spheres)
        {
            sphere.ReversePullForce = true;
        }
    }
}
