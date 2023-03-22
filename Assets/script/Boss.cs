using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Character
{
    public override void GetDamage(int dmg, int arrowdmg = 0)
    {
        base.GetDamage(dmg / 2);
    }

    protected override void Death()
    {
        base.Death();
        Debug.Log("BOSS DEAD");

    }

    protected override void SetStats()
    {
        stats.SetDefaltNecromant();
    }
}
