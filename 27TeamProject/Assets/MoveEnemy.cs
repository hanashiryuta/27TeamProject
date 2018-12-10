//
//10月3日
//作成者：安部崇寛
//移動だけするエネミークラス
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : Enemy {
    
	// Update is called once per frame
	public override void Update () {
        base.Update();
        if (!BlowMode)
        {
            if(isHook) Move();
            
        }
        else
        {
            Blow();
        }
	}
    
}
