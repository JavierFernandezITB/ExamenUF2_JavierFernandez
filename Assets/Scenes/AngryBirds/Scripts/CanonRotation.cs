using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonRotation : MonoBehaviour
{
    // Base variables
    public Vector3 _maxRotation;
    public Vector3 _minRotation;
    private float offset = -51.6f;
    public GameObject ShootPoint;
    public GameObject Bullet;
    public float ProjectileSpeed = 0;
    public float MaxSpeed;
    public float MinSpeed;
    public GameObject PotencyBar;
    private float initialScaleX;    

    // New variables
    private bool isChargingSpeed = false;
    private Vector3 mousePos;
    private Vector3 direction;
    private Quaternion rotation;
    private float angle;

    private void Awake()
    {
        initialScaleX = PotencyBar.transform.localScale.x;
    }
    void Update()
    {
        mousePos = Input.mousePosition;
        direction = mousePos - Camera.main.WorldToScreenPoint(transform.position);
        angle = (Mathf.Atan2(direction.y, direction.x) * 180f / Mathf.PI + offset);
        rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        if (angle < _maxRotation.z && angle > _minRotation.z)
            transform.rotation = rotation;

        if (Input.GetMouseButton(0) && !isChargingSpeed)
        {
            StartCoroutine(ProjectileSpeedUpdater());
        }

        if(!Input.GetMouseButton(0) && !isChargingSpeed && ProjectileSpeed >= MinSpeed)
        {
            GameObject projectile = Instantiate(Bullet, ShootPoint.transform.position, ShootPoint.transform.rotation);
            direction = -(transform.position - ShootPoint.transform.position);
            projectile.GetComponent<Rigidbody2D>().AddForce(direction.normalized * ProjectileSpeed, ForceMode2D.Impulse);
            ProjectileSpeed = 0;
        }
        CalculateBarScale();
    }

    public IEnumerator ProjectileSpeedUpdater()
    {
        isChargingSpeed = true;
        while (Input.GetMouseButton(0) && ProjectileSpeed < MaxSpeed)
        {
            yield return new WaitForSeconds(0.0625f);
            ProjectileSpeed += .25f;
        }
        isChargingSpeed = false;
    }

    public void CalculateBarScale()
    {
        PotencyBar.transform.localScale = new Vector3(Mathf.Lerp(0, initialScaleX, ProjectileSpeed / MaxSpeed),
            PotencyBar.transform.localScale.y,
            PotencyBar.transform.localScale.z);
    }
}
