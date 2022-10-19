using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    // 이름/타입은 Action 이지만 하는 일은 리스너
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;

    bool _pressed = false;
    float _pressedTime = 0;

    public void OnUpdate()
    {
        // 포인터(마우스)가 게임 오브젝트 위에 있었으면 False, UI 오브젝트 위에 있었으면 True
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if(Input.anyKey && KeyAction != null)
        {
            KeyAction.Invoke();
        }

        if (MouseAction != null)
        {
            // Input.GetMouseButton(0)
            // - 결과가 True 면 눌린 거
            // - 결과가 False 면 놓은 거
            // - 인자 0 은 왼쪽 버튼, 1은 오른쪽 버튼, 2는 중간 버튼
            if (Input.GetMouseButton(0))
            {
                // 이전부터 늘린 상태였는가 아닌가 판단
                if (!_pressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    _pressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;
            }
            else
            {
                // 눌려있던 상태에서 발생한 건지 판단
                if(_pressed)
                {
                    // 누르고 있던 시간이 짧으면 클릭으로 간주
                    if(Time.time < _pressedTime + 0.2f)
                    {
                        MouseAction.Invoke(Define.MouseEvent.Click);
                    }
                    MouseAction.Invoke(Define.MouseEvent.PointerUp);
                }

                _pressed = false;
                _pressedTime = 0;
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
