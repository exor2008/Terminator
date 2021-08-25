using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Units;
using Game.States;

namespace Game.Healths
{
    public class Health
    {
        Unit unit;
        int hp;
        public Health(Unit _unit)
        {
            unit = _unit;
            hp = 100;
        }
        public int GetHp()
        {
            return hp;
        }
        public int TakeDamage(int damage)
        {
            hp -= damage;
            return hp;
        }
        public void AwareAttacked(Vector3 hitPos)
        {
            unit.SwitchState(new AttackedState(unit, hitPos));
        }
    }
}