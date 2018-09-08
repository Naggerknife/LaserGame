using UnityEngine;

public class LaserShooter : MonoBehaviour
{
    internal ILaserInteractable LatestInteractableHit { get; private set; }

    [SerializeField] private LayerMask _layerMask = ~0;

	private void Update ()
	{
        // Could be refactored to only shoot raycast if an enemy crosses the line renderer / if this shooter moves
	    ShootRaycast();
	}

    /// <summary>Returns true if the <see cref="newInteractable"/> does not match the latest hit <see cref="ILaserInteractable"/>.</summary>
    private bool RaycastHitHasChanged(ILaserInteractable newInteractable)
    {
        return LatestInteractableHit != newInteractable;
    }

    // Sets off a recursion that starts disabling lasershooters when it finds one with a LatestInteractableHit that is null.
    internal void StartChainDisable()
    {
        LatestInteractableHit?.OnRaycastExit2D(this);
        LatestInteractableHit = null;
    }

    private void ShootRaycast()
    {
        var ray = new Ray2D(transform.position, transform.up);
        var hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, _layerMask);

        if (!hit)
        {
            StartChainDisable();

            Debug.DrawRay(transform.position, transform.up * 100000);
            return;
        }

        var interactable = hit.collider.GetComponentInParent<ILaserInteractable>();

        if (RaycastHitHasChanged(interactable))
        {
            LatestInteractableHit?.OnRaycastExit2D(this);
        }

        interactable?.OnRaycastStay2D(this, ray, hit);

        Debug.DrawLine(transform.position, hit.point);
        LatestInteractableHit = interactable;
    }
}
