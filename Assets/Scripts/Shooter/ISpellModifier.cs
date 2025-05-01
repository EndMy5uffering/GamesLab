using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ISpellModifier: ScriptableObject
{

    [Header("Projectile Setting Adding")]

    [SerializeField] private float spreadAngle = 0; // on shooter
    [SerializeField] private int multiShot = 0; // on shooter
    [SerializeField] private float fireDelay = 0; // on shooter
    [SerializeField] public float projectileLifeTime = 0; // on projectile


    //private float weaponHeat = 0;


    [Header("Projectile Setting Multiplier")]

    [SerializeField] private float projectileSpeedMultiplier = 1; // on projectile
    [SerializeField] private float gravityMultiplier = 0; // on projectile
    [SerializeField] private float dragMultiplier = 0; // on projectile

    [Header("General Settings")]

    [SerializeField] private int burstCount = 0; // on shooter
    [SerializeField] private int bounceCount = 0; // on projectile


    public abstract void ModifyShooter(Shooter shooter);
    public abstract void ModifyProjectile(ref List<Projectile> projectile);

    public void DefaultModifyShooter(Shooter shooter)
    {
        shooter.AddBurstCount(burstCount);
        shooter.AddFiringDelay(fireDelay);
        shooter.AddMultiShot(multiShot);
        shooter.AddSpreadAngle(spreadAngle);
    }

    public void DefaultModifySpell(ref List<Projectile> projectile)
    {
        foreach(Projectile proj in projectile)
        {
            proj.AddLifeTime(projectileLifeTime);
            proj.MulSpeed(projectileSpeedMultiplier);
            proj.MulGravity(gravityMultiplier);
            proj.MulDrag(dragMultiplier);
            proj.AddBounceCount(bounceCount);
        }
    }

}
