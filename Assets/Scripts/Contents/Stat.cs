using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    protected int _level;
    [SerializeField]
    protected int _hp;
    [SerializeField]
    protected int _maxHp;
    [SerializeField]
    protected int _attack;
    [SerializeField]
    protected int _defense;
    [SerializeField]
    protected float _moveSpeed;

    public int Level   { get { return _level; }   set { _level = value; } }
    public int Hp      { get { return _hp; }      set { _hp = value; } }
    public int MaxHp   { get { return _maxHp; }   set { _maxHp = value; } }
    public int Attack  { get { return _attack; }  set { _attack = value; } }
    public int Defense { get { return _defense; } set { _defense = value; } }
    public float MoveSpeed {  get { return _moveSpeed; } set { _moveSpeed = value; } }

    void Start()
    {
        Level = 1;
        Hp = 100;
        MaxHp = 100;
        Attack = 10;
        Defense = 5;
        MoveSpeed = 5.0f;
    }

    public virtual void OnAttacked(Stat attacker)
    {
        int damage = Mathf.Max(0, attacker.Attack - Defense);
        Hp -= damage;
        if(Hp <= 0)
        {
            Hp = 0;
            OnDead(attacker);
        }
    }

    protected virtual void OnDead(Stat attacker)
    {
        // TODO
        // - 플레이어에게 경험치 주기
        // - gameObject despawn
    }
}
