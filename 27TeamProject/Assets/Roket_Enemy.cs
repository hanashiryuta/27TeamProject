//
//12月10日
//ロケットエネミークラス
//田中　悠斗
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roket_Enemy : Enemy
{

    //public void OnTriggerEnter(Collider other)
    //{
    //    Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    //    player.swingSpeed = player.swingSpeedRange;
    //}

    //public override void MaxSpeedEnemy(Collider other)
    //{
    //    base.MaxSpeedEnemy(other);
    //    Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    //    player.swingSpeed = player.swingSpeedRange;

    //}

    public override void Awake()
    {
        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (!GetComponentInChildren<Renderer>().isVisible && isGround && isEscape)
        {
            enemySpawnManager.enemyCount--;
            Destroy(this.gameObject);
        }


        if (!BlowMode)
        {
            if (isHook) Move();

        }
        else
        {
            Blow();
        }

        if(!isHook)
        {
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.swingSpeed = player.swingSpeedRange;
        }
    }
}
