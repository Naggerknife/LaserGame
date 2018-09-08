using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour, ILaserInteractable
{
    [SerializeField] private LaserShooter _laserShooterPrefab;
    [SerializeField] private int _maxReflectionsPerMirror = 5;

    // Pass in a caller and get the reflection shooter for this caller.
    // Basically: Get the GameObject that shoots the reflection that corresponds with a shooter (key).
    private readonly Dictionary<LaserShooter, LaserShooter> _reflectionShooters = new Dictionary<LaserShooter, LaserShooter>();

    public void OnRaycastStay2D(LaserShooter caller, Ray2D ray, RaycastHit2D raycastHit)
    {
        HandleReflections(caller, ray, raycastHit);
    }

    public void OnRaycastExit2D(LaserShooter caller)
    {
        HandleLaserDisabling(caller);
    }

    private void HandleReflections(LaserShooter caller, Ray2D ray, RaycastHit2D raycastHit)
    {
        Vector3 reflectedDirection = Vector3.Reflect(ray.direction, raycastHit.normal).normalized;

        // If the dict contains the caller already, the LaserShooter that is shooting this mirror was shooting the mirror before this frame.
        if (_reflectionShooters.ContainsKey(caller))
        {
            _reflectionShooters[caller].transform.position = raycastHit.point; // Update position
            _reflectionShooters[caller].transform.up = reflectedDirection; // Update rotation
            return;
        }

        if (_reflectionShooters.Count > _maxReflectionsPerMirror) return;

        // The LaserShooter that is shooting this mirror was not shooting it last frame. Spawns a new reflection for this LaserShooter.
        var newShooter = Instantiate(_laserShooterPrefab, raycastHit.transform);
        _reflectionShooters.Add(caller, newShooter);
        newShooter.transform.up = reflectedDirection; // Update rotation

        Debug.Log($"{name} was hit by {caller.name}");
    }

    private void HandleLaserDisabling(LaserShooter caller)
    {
        if (!_reflectionShooters.ContainsKey(caller)) return;

        var shooterToDisable = _reflectionShooters[caller];

        // If this reflection is shooting at another interactable, also go to that interactable and disable it's reflections
        if (shooterToDisable.LatestInteractableHit != null)
        {
            shooterToDisable.StartChainDisable();
        }

        Destroy(shooterToDisable.gameObject);
        _reflectionShooters.Remove(caller);

        Debug.Log($"{caller.name} stopped shooting at {name}");
    }
}
