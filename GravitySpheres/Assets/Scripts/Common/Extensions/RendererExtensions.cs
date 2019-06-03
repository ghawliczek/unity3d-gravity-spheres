using UnityEngine;

public static class RendererExtensions
{
    /// <summary>
    /// Checks if given <see cref="Renderer"/> can be seen from given <see cref="Camera"/>
    /// </summary>
    public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }
}