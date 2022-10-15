using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterControllerBase
{
    PlayerStat _stat;
    bool _stopSkill = false;

    // 이 함수 BaseController.Start() 가 실행
    public override void init()
    {
        WorldObjectType = Define.WorldObject.Player;

        // PlayerStat 도 GetOrAddComponent 해도 될 것 같다.
        _stat = gameObject.GetComponent<PlayerStat>();

        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        if (gameObject.GetComponentInChildren<UI_HpBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HpBar>(transform);
    }

    // 이 함수는 BaseController.Update() 가 실행
    protected override void UpdateMoving()
    {
        if(_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            // magnitude: 벡터3의 길이
            float distance = (_destPos - transform.position).magnitude;
            // 타겟과의 거리가 가까우면 공격 상태로 전환
            if(distance <= 1)
            {
                State = Define.State.Skill;
                return;
            }
        }

        Vector3 dir = _destPos - transform.position;
        // 3D 맵이지만 평평한 맵이기 때문에 높이 차는 아예 무시함
        // 개선되어야 할 코드
        dir.y = 0;
        // 목적지와 아주 가까우면 (도착했으면) 멈춤 상태로 전환
        if(dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
            return;
        }

        Debug.DrawRay(transform.position, dir.normalized, Color.green);
        // 바로 앞에 "Block" 레이어가 있다면 이동하지 않는댜.
        // 더불어 마우스 클릭도 없다면 상태도 멈춤 상태로 전환
        if(Physics.Raycast(
            transform.position + Vector3.up * 0.5f,
            dir,
            1.0f,
            LayerMask.GetMask("Block")))
        {
            if(Input.GetMouseButton(0) == false)
                State = Define.State.Idle;
            return;
        }

        // 타겟과 가깝지도 않, "Block" 레이어와 가깝지도 않으면
        // 타겟 방향으로 이동 및 회전
        float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
        transform.position += dir.normalized * moveDist;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            10 * Time.deltaTime);
    }

    // 이 함수는 BaseController.Update() 가 실행
    protected override void UpdateSkill()
    {
        // 타겟이 없으면 아무것도 하지 않는.
        if(_lockTarget == null)
            return;

        // 타겟 방향으로 회전
        Vector3 dir = _lockTarget.transform.position - transform.position;
        Quaternion quat = Quaternion.LookRotation(dir);
        // 왜 여기는 Lerp 고 UpdateMoving 에서는 Slerp 일까? 맵이 작아서 차이 없을 것 같은데..
        transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
    }

    // 애니메이션 이벤트에서 발생
    void OnHitEvent()
    {
        if(_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);
        }

        // PointerUp 이벤트 발생 시 _stopSkill = true 가 된다.
        if (_stopSkill)
            State = Define.State.Idle;
        else
            State = Define.State.Skill;
    }

    // init() 의 Managers.Input.MouseAction += OnMouseEvent; 에 의해
    // InputManager 에서 실행됨
    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch(State)
        {
            case Define.State.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Skill:
                if(evt == Define.MouseEvent.PointerUp)
                {
                    _stopSkill = true;
                }
                break;
        }
    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int mask = (1 << (int)Define.Layer.Ground | (1 << (int)Define.Layer.Monster));
        // 마우스 위치가 "Ground" 혹은 "Monster" 레이어인지 확인
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, mask);

        switch(evt)
        {
            case Define.MouseEvent.PointerDown:
                if(raycastHit)
                {
                    // 마우스를 눌렀으면 누른 곳을 목적지로, 상태는 Moving 으로 변경
                    _destPos = hit.point;
                    State = Define.State.Moving;
                    _stopSkill = false;

                    // 누른 게 "Monster" 레이어면 타겟을 해당 오브젝트로, 아니면 null 로
                    if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                        _lockTarget = hit.collider.gameObject;
                    else
                        _lockTarget = null;
                }
                break;
            case Define.MouseEvent.Press:
                // 마우스 드래그 시 (클릭 상태 유지 시) 목적지만 변경
                if (_lockTarget == null && raycastHit)
                    _destPos = hit.point;
                break;
            case Define.MouseEvent.PointerUp:
                // 마우스를 땠을 때 공격 중지
                // 이동은 계속 할 수 있으므로 _destPos, _lockTarget 은 따로 처리 안 함
                _stopSkill = true;
                break;
        }
    }
}
