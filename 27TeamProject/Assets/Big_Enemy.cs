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

    // Update is called once per frame
    public override void Update()
    {
        isSticking = false;
        if (hp < 40)
        {
            isCatch = true;
            base.Update();
        }
        else if (hp > 40)
        {
            isCatch = false;
            if (!BlowMode)
            {
                if (isHook) Move();
            }
        }
    }

    public override void DeathAction()
    {

        if (gameObject.layer == 15 && hp == 0)
        {
            base.DeathAction();
        }      

    }
}
