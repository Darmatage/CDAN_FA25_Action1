using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackShoot : MonoBehaviour
{
    public Animator animator;
    public Animator chargeanimator;
    public Rigidbody2D rb;
    public Camera cam;

    public Vector2 movement;
    public Vector2 mousePos;
    public Transform fireBase;
    public Transform firePoint;

    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    public AudioSource[] shootSounds;
    public float minPitch = 0.95f;
    public float maxPitch = 1.05f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetAxis("Attack") > 0)
            {
                if (GameHandler.gotTokens > 0)
                {
                    playerFire();
                    nextAttackTime = Time.time + 1f / attackRate;
                    GameHandler.SpendTokens(10);
                }
                else
                {
                    Debug.Log("Not enough tokens to shoot!");
                }
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        fireBase.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    void playerFire()
    {
        animator.SetTrigger("Fire");
        chargeanimator.SetTrigger("Fire");

        Vector2 fwd = (firePoint.position - transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(fwd * projectileSpeed, ForceMode2D.Impulse);

        if (shootSounds.Length > 0)
        {
            int index = Random.Range(0, shootSounds.Length);
            AudioSource sfx = shootSounds[index];
            sfx.pitch = Random.Range(minPitch, maxPitch);
            sfx.Play();
        }
    }
}