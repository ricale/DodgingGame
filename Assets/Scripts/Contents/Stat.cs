using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    protected int _level;
    [SerializeField]
    protected float _hp;
    [SerializeField]
    protected float _maxHp;
    [SerializeField]
    protected float _attack;
    [SerializeField]
    protected float _defense;
    [SerializeField]
    protected float _moveSpeed;
    [SerializeField]
    protected float _collisionDamage;

    public int Level     { get { return _level; }   set { _level = value; } }
    public float Hp      { get { return _hp; }      set { _hp = value; } }
    public float MaxHp   { get { return _maxHp; }   set { _maxHp = value; } }
    public float Attack  { get { return _attack; }  set { _attack = value; } }
    public float Defense { get { return _defense; } set { _defense = value; } }
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }
    public float CollisionDamage
    {
        get { return _collisionDamage; }
        set { _collisionDamage = value; }
    }

    void Start()
    {
    }

    void OnDamanged(Stat attacker, float damage)
    {
        Debug.Log($"OnDamaged Hp: {Hp} damage: {damage}");
        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            OnDead(attacker);
        }
    }

    public virtual void OnAttacked(Stat attacker)
    {
        OnDamanged(attacker, Mathf.Max(0, attacker.Attack - Defense));
    }

    public virtual void OnCollided(Stat attacker)
    {
        OnDamanged(attacker, Mathf.Max(0, attacker.CollisionDamage - Defense));
    }

    protected virtual void OnDead(Stat attacker)
    {
        // TODO
        // - 플레이어에게 경험치 주기
        // - gameObject despawn
    }
}
