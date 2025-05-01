using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Base Bullet")]

    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform bulletShootingOrigin;
    [SerializeField] private Vector3 shootingDirection = Vector3.forward;

    [Header("Input settings")]

    [SerializeField] private InputActionReference fireInput;
    private Boolean isFeiring = false;
    private Boolean canModifyShooter = true;

    [Header("Shooting Settings")]

    [SerializeField] private double firingDelay = 2;
    [SerializeField] private float spreadAngle = 0;
    [SerializeField] private int multiShot = 0;
    [SerializeField] private int burstCount = 0;
    private double fieringTimer = 0;

    /*------------------------*/

    [SerializeField] private ISpellModifier[] projectileMods;




    void Awake()
    {
        if (projectilePrefab == null) 
            throw new NullReferenceException("Bullet prefab needed!!!!");

            
        if (bulletShootingOrigin == null) 
            throw new NullReferenceException("Bullet origin needed!!!!");
    }

    void Start()
    {
        if (fireInput != null) fireInput.action.Enable();
    }

    private void OnDisable()
    {
        if (fireInput != null) fireInput.action.Disable();
    }

    void Update()
    {
        UpdateShooter();
        if (isFeiring) FireProjectile();
    }

    private void UpdateShooter()
    {
        if(fireInput.action.IsPressed() && fieringTimer <= 0)
        {
            isFeiring = true;
            fieringTimer = firingDelay;
        }
        else
        {
            fieringTimer -= Time.deltaTime;
            isFeiring = false;
        }
    }

    private void FireProjectile()
    {

        if(canModifyShooter)
        {
            foreach(ISpellModifier mod in projectileMods)
            {
                mod.DefaultModifyShooter(this);
                mod.ModifyShooter(this);
            }
        }

        List<Projectile> projectiles = new();
        for(int i = 0; i < this.multiShot; ++i)
        {
            GameObject projectile = GameObject.Instantiate(projectilePrefab.gameObject, bulletShootingOrigin.position, bulletShootingOrigin.rotation);
            Projectile projectileScr = projectile.GetComponent<Projectile>();

            projectileScr.dir = RotateVector3(bulletShootingOrigin.forward);
            projectiles.Add(projectileScr);
        }

        foreach(ISpellModifier mod in projectileMods)
        {
            mod.DefaultModifySpell(ref projectiles);
            mod.ModifyProjectile(ref projectiles);
        }

        ResetShooterValues();

    }

    Vector3 RotateVector3(Vector3 vector)
    {
        float randomAngle = UnityEngine.Random.Range(0f, this.spreadAngle); // Random rotation angle
        Vector3 randomAxis = UnityEngine.Random.onUnitSphere; // Random normalized direction
        
        return Quaternion.AngleAxis(randomAngle, randomAxis) * vector;
    }


    public double GetFiringDelay() => firingDelay;
    public void SetFiringDelay(double value) => firingDelay = value;
    public void AddFiringDelay(double value) => firingDelay += value;

    public float GetSpreadAngle() => spreadAngle;
    public void SetSpreadAngle(float value) => spreadAngle = value;
    public void AddSpreadAngle(float value) => spreadAngle += value;

    public int GetMultiShot() => multiShot;
    public void SetMultiShot(int value) => multiShot = value;
    public void AddMultiShot(int value) => multiShot += value;

    public int GetBurstCount() => burstCount;
    public void SetBurstCount(int value) => burstCount = value;
    public void AddBurstCount(int value) => burstCount += value;

    public void ResetShooterValues()
    {
        firingDelay = 2;
        spreadAngle = 0;
        multiShot = 0;
        burstCount = 0;
    }

}
