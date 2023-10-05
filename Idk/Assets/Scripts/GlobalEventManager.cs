using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEventManager
{
    public static event Action OnPlayerStop;

    public static void SendPlayerStop() 
    {
        OnPlayerStop?.Invoke();
    }

    public static event Action OnPlayerActive;

    public static void SendPlayerActive()
    {
        OnPlayerActive?.Invoke();
    }

    public static event Action OnPlayerDied;

    public static void SendPlayerDied()
    {
        OnPlayerDied?.Invoke();
    }

    public static event Action<int> OnGotDamageEnemy;

    public static void SendGotDamageEnemy(int damage)
    {
        OnGotDamageEnemy?.Invoke(damage);
    }


    public static event Action<int> OnGotDamage;

    public static void SendGotDamage(int damage)
    {
        OnGotDamage?.Invoke(damage);
    }

    public static event Action OnFire;

    public static void SendFire()
    {
        OnFire?.Invoke();
    }

    public static event Action<Items> OnItemTake;

    public static void SendItemTake(Items item)
    {
        OnItemTake?.Invoke(item);
    }
}
