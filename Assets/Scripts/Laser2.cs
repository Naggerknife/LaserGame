using UnityEngine;

public class Laser2 : MonoBehaviour 
{
    public LineRenderer Line;
    public LayerMask layers;
    public RaycastHit2D PublicHit;

    [SerializeField]
    private float _numOfReflections;

	private void Update()
	{
		ShootLaser();
	}

	private void ShootLaser()
	{
        Ray ray = new Ray(transform.position, transform.up);
        for (int i = 0; i <= _numOfReflections; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layers);
            if(hit.collider != null)
            {
                PublicHit = hit;
                Vector3 inDirection = Vector3.Reflect(ray.direction, hit.normal);
                Debug.DrawLine(ray.origin, hit.point);
                ray = new Ray(hit.point, inDirection);
            }
        }
	}
}