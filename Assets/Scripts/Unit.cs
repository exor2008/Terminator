using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Weapons;
using Game.FieldsOfView;
using Game.Move;

namespace Game.Units
{
    public class Unit
    {
        public Transform transform;
        public Rigidbody rigidbody;
        public Weapon weapon;
        public FieldOfView fieldOfView;
        public Mover mover;

        public Unit(
            Transform _transform,
            Rigidbody _rigidbody,
            Weapon _weapon,
            FieldOfView _fieldOfView,
            Mover _move)
        {
            transform = _transform;
            rigidbody = _rigidbody;
            weapon = _weapon;
            fieldOfView = _fieldOfView;
            mover = _move;
        }

        void ChangeWeapon(Weapon _weapon)
        {
            weapon = _weapon;
        }
    }
}