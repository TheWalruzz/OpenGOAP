using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berries : SmartObject
{
    protected override void Start()
    {
        base.Start();
        value = 5f;
        typeName = "Berries";
        
    }
}
