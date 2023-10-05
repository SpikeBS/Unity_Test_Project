using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager 
{
    public static Action Respawn;

    public static void RespawnStart() 
    {
        Respawn.Invoke();
    }
  
}
