using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QuarterView;

    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 6.0f, -5.0f);

    [SerializeField]
    GameObject _player = null;

    public void SetPlayer(GameObject player)
    {
        _player = player;
    }

    void Start()
    {
    }

    void LateUpdate()
    {
        switch(_mode)
        {
            case Define.CameraMode.QuarterView:
                UpdateForQuarterView();
                break;
            case Define.CameraMode.TopView:
                UpdateForTopView();
                break;
        }
        
    }

    void UpdateForQuarterView()
    {
        if (_player.IsValid() == false)
            return;

        int mask = 1 << (int)Define.Layer.Block;
        RaycastHit hit;
        if (Physics.Raycast(
            _player.transform.position,
            _delta,
            out hit,
            _delta.magnitude,
            mask))
        {
            Vector3 dir = hit.point - _player.transform.position;
            float dist = dir.magnitude * 0.8f;
            transform.position = _player.transform.position
                + _delta.normalized * dist;
        }
        else
        {
            transform.position = _player.transform.position + _delta;
            transform.LookAt(_player.transform);
        }
    }

    void UpdateForTopView()
    {
        if (_player.IsValid() == false)
            return;

        transform.position = _player.transform.position
            + new Vector3(0.0f, _delta.y, 0.0f);
        transform.LookAt(_player.transform);
    }

    public void SetQuaterView(Vector3 delta)
    {
        //_mode = Define.CameraMode.QuarterView;
        //_delta = delta;
    }
}
