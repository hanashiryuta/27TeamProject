﻿//
//10月3日
//作成者：安部崇寛
//移動だけするエネミークラス
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : Enemy
{

    public override void Awake()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        
        if (!BlowMode)
        {
            if (isHook) Move();
        }
        else
        {
            Blow();
        }

        if (!isHook)
        {
            HookSwing();
        }
    }

    public override void AttackAnime()
    {
        base.AttackAnime();
        seAudio.PlayOneShot(seList[0]);
        animator.SetTrigger("isAttack");
    }
}
