//
//12月14日
//ビックエネミークラス
//田中　悠斗
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big_Enemy : Enemy
{

    public GameObject origin_Star_Particle;
    GameObject star_Particle;

    public override void Awake()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
            base.Update();

        isSticking = false;
        if (hp < 40)
        {
            isCatch = true;
            if (star_Particle == null)
                star_Particle = Instantiate(origin_Star_Particle, transform.position + new Vector3(0,2,0), Quaternion.identity, transform);
            if (!BlowMode)
            {
            }
            else
            {
                Blow();
            }
        }
        else if (hp > 40)
        {
            isCatch = false;
            if (!waveManager.isWave)
            {
                DeathAction();
            }
            if (!BlowMode)
            {
                if (isHook) Move();
            }
        }
    }

    //public override void DeathAction()
    //{
    //    if (gameObject.layer == 15 && hp == 0)
    //    {
    //        base.DeathAction();
    //    }      

    //}
}
