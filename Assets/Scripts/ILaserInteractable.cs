using UnityEngine;

public interface ILaserInteractable
{
    void OnRaycastStay2D(LaserShooter caller, Ray2D ray, RaycastHit2D raycastHit);
    void OnRaycastExit2D(LaserShooter caller);
}
