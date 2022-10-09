using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    Texture2D _attackIcon;
    Texture2D _handIcon;

    enum CursorType
    {
        None,
        Attack,
        Hand,
    }

    CursorType _cursorType = CursorType.None;

    void Start()
    {
        _attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        _handIcon= Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");
    }

    void Update()
    {
        // 마우스를 누르는 중이면 아무것도 하지 않음
        if (Input.GetMouseButton(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        int mask = (1 << (int)Define.Layer.Ground | (1 << (int)Define.Layer.Monster));
        RaycastHit hit;
        // 카메라에서 마우스 방향으로 raycast (Ground, Monster 대상)
        if(Physics.Raycast(ray, out hit, 100.0f, mask))
        {
            // raycast 에 몬스터가 맞았다면
            if(hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                // 아직 커서가 공격 커서가 아니라면, 커서를 바꾼다.
                if(_cursorType != CursorType.Attack)
                {
                    Cursor.SetCursor(
                        _attackIcon,
                        new Vector2(_attackIcon.width / 5, 0),
                        CursorMode.Auto);
                    _cursorType = CursorType.Attack;
                }
            }
            // raycast 에 지면이 맞았다면
            else
            {
                // 아직 커서가 손 모양이 아니라면, 커서를 바꾼다.
                if(_cursorType != CursorType.Hand)
                {
                    Cursor.SetCursor(
                        _handIcon,
                        new Vector2(_attackIcon.width / 3, 0),
                        CursorMode.Auto);
                    _cursorType = CursorType.Hand;
                }
            }
        }
    }
}
