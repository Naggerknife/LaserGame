using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    public LineRenderer lineRenderer;

    private Ray2D ray;

    private RaycastHit2D hit;

    private Vector2 inDirection;

    private int reflectPoints;
    private int mirrorsHit;

    [SerializeField]
    private int reflectCount;

    private bool laserFiring;

    private void Start()
    {
        laserFiring = true;
    }

    private void Update()
    {
        if (laserFiring)
            ShootLaser();
    }


    private void ShootLaser()
    {
        reflectPoints = 2;
        lineRenderer.positionCount = reflectPoints;
        lineRenderer.SetPosition(0, transform.position);
        ray = new Ray2D(transform.position, transform.up);
        mirrorsHit = 0;

        for (int i = 0; i <= reflectCount; i++)
        {
            if (i == 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100);

                if (hit.collider != null && hit.collider.name == "Finish")
                {
                    laserFiring = false;
                    GameFinish(mirrorsHit);
                }

                if (hit.collider != null && hit.collider.name == "Mirror")
                {
                    mirrorsHit++;
                    Vector2 inDirection = Vector2.Reflect(ray.direction, hit.normal);
                    ray = new Ray2D(hit.point, inDirection);
                    lineRenderer.SetPosition(i + 1, hit.point);
                }
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100);

                if (hit.collider != null && hit.collider.name == "Finish")
                {
                    laserFiring = false;
                    GameFinish(mirrorsHit);
                }

                if (hit.collider != null && hit.collider.name == "Mirror")
                {
                    mirrorsHit++;
                    Vector3 inDirection = Vector3.Reflect(ray.direction, hit.normal);
                    ray = new Ray2D(hit.point, inDirection);
                    lineRenderer.positionCount = ++reflectPoints;
                    lineRenderer.SetPosition(i + 1, hit.point);
                }
                else
                {
                    if (mirrorsHit < reflectPoints)
                    {
                        lineRenderer.positionCount = ++reflectPoints;
                        mirrorsHit = reflectPoints;
                        lineRenderer.SetPosition(i + 1, ray.origin + (ray.direction * 10));
                    }
                }
            }
        }
    }

    public void GameFinish(int mirrorsHit)
    {
        laserFiring = false;
        Debug.Log(string.Format("congrats you beat the level only using {0} mirrors!", mirrorsHit));
    }
}
