using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Race 
{
    public int race_id = 0;
    public int health  = 0;
    public int damage = 0;
    public int speed = 0;
    public int jump = 0;
    public int firerate = 0;
    public string raceName = "";


    public void SetData(int _id, int _health, int _damage, int _speed, int _jump, int _firerate, string _raceName)
    {
        race_id = _id;
        health = _health;
        damage = _damage;
        speed = _speed;
        jump = _jump;
        firerate = _firerate;
        raceName = _raceName;

    }

}
