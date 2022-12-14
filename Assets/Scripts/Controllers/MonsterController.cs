using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : CharacterControllerBase
{
    MonsterStat _stat;

    [SerializeField]
    float _scanRange = 100f;

    [SerializeField]
    float _attackRange = 0.8f;

    [SerializeField]
    float _collisionRange = 1.0f;

    public override void init()
    {
        WorldObjectType = Define.WorldObject.Monster;
        _stat = gameObject.GetComponent<MonsterStat>();
        State = Define.State.Idle;

        // TODO
        // HP 바
    }

    public void Dead()
    {
        Managers.Game.Despawn(gameObject);
    }

    protected override void UpdateIdle()
    {
        GameObject player = Managers.Game.GetPlayer();
        if (player == null)
            return;

        // 플레이어와 거리가 _scanRange 미만이면 플레이어를 쫓는다
        Vector3 dist = player.transform.position - transform.position;
        if(dist.magnitude <= _scanRange)
        {
            _lockTarget = player;
            State = Define.State.Moving;
            return;
        }
    }

    protected override void UpdateMoving()
    {
        if (_lockTarget == null)
        {
            State = Define.State.Idle;
            return;
        }

        _destPos = _lockTarget.transform.position;
        Vector3 dir = _destPos - transform.position;

        if (dir.magnitude <= _collisionRange)
        {
            GameObject player = Managers.Game.GetPlayer();
            if (player)
                player.GetComponent<Stat>().OnCollided(_stat);
        }

        NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();

        if (dir.magnitude <= _attackRange)
        {
            nma.SetDestination(transform.position);
            State = Define.State.Skill;
            return;
        }

        nma.SetDestination(_destPos);
        nma.speed = _stat.MoveSpeed;

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            10 * Time.deltaTime);
    }

    protected override void UpdateSkill()
    {
        if (_lockTarget == null)
            return;

        Vector3 dir = _lockTarget.transform.position - transform.position;

        if(dir.magnitude > _attackRange)
        {
            State = Define.State.Moving;
            return;
        }

        Quaternion quat = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            quat,
            20 * Time.deltaTime);
    }

    void OnHitEvent()
    {
        if(_lockTarget == null)
        {
            State = Define.State.Idle;
            return;
        }

        Stat targetStat = _lockTarget.GetComponent<Stat>();
        targetStat.OnAttacked(_stat);

        if(targetStat.Hp <= 0)
        {
            State = Define.State.Idle;
            return;
        }

        Vector3 dist = (_lockTarget.transform.position - transform.position);
        if (dist.magnitude <= _attackRange)
            State = Define.State.Skill;
        else
            State = Define.State.Moving;
    }
}
