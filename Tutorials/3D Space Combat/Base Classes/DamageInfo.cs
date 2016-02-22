using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DamageInfo
{
    private GameObject _sender;
    private int _damage;

    public GameObject Sender
    {
        get
        {
            return _sender;
        }
    }
    public int Damage
    {
        get
        {
            return _damage;
        }
    }

    public DamageInfo(GameObject sender, int damage)
    {
        _sender = sender;
        _damage = damage;
    }
}
