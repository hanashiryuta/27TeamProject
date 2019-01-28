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

    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (!GetComponentInChildren<Renderer>().isVisible && isGround && isEscape && isHook)
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
            HookSwing();
        }
    }

    public override void HookSwing()
    {
        Player playerE = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerE.swingSpeed = playerE.swingSpeedRange;
        
        Vector3 pos = new Vector3(playerE.transform.position.x, transform.position.y, playerE.transform.position.z) - transform.position;
        Vector3 normal = Vector3.Normalize(pos);
        if (normal.z > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
