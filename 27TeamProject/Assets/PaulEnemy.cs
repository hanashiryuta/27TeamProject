using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaulEnemy : Enemy {

    RaycastHit[] hitlist;
    Vector3 boxcastScale;

    MoveMode mode;

    public override void Awake()
    {
        mode = MoveMode.PAUL;

    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }
}
