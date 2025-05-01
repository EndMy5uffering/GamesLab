using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "debugModifyer", menuName = "ScriptableObjects/debugModifyer")]
public class DebugModifyer : ISpellModifier
{
    public override void ModifyProjectile(ref List<Projectile> projectile)
    {
    }

    public override void ModifyShooter(Shooter shooter)
    {
    }
}
