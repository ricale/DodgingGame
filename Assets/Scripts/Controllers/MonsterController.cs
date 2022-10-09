using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    Stat _stat;

    [SerializeField]
    float _scanRange = 100f;

    [SerializeField]
    float _attackRange = 0.5f;

    public override void init()
    {
        WorldObjectType = Define.WorldObject.Monster;
        _stat = gameObject.GetComponent<Stat>();
        State = Define.State.Idle;

        // TODO
        // HP 바
    }

    protected override void UpdateIdle()
    {
        GameObject player = Managers.Game.GetPlayer();
        if (player == null)
            return;

        // 플레이어와 거리가 _scanRange 미만이면 플레이어를 쫓는다
        Vector3 dist = player.transform.position - transform.position;
        Debug.Log($"dist.magnitude {dist.magnitude} _scanRange {_scanRange}");
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
        //if (dir.magnitude < 0.1f)
        //{
        //    State = Define.State.Idle;
        //    return;
        //}

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
        // TODO
        Debug.Log("OnHitEvent");
    }
}
