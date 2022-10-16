using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Stat _userStat;
    float _speed;

    float _validRange = 8f;
    float _moved = 0f;

    public void Shoot(Transform shooter, Transform target, float speed = 5f)
    {
        _userStat = shooter.GetComponent<Stat>();
        _speed = speed;
        _moved = 0f;

        Vector3 dist = target.position - shooter.position;
        Quaternion quat = Quaternion.LookRotation(dist);
        transform.rotation = Quaternion.Lerp(transform.rotation, quat, 1);

        Collider shooterCollider = shooter.GetComponent<Collider>();
        transform.position = shooter.position
            + shooter.up * shooterCollider.bounds.size.y * 0.5f
            + transform.forward * shooterCollider.bounds.size.x * 1.1f;

    }

    void Update()
    {
        float delta = _speed * Time.deltaTime;
        _moved += delta;
        if(_moved > _validRange)
        {
            Managers.Game.Despawn(gameObject);
            return;
        }

        transform.position += transform.forward * delta;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != (int)Define.Layer.Monster)
            return;

        float dist = Vector3.Distance(other.transform.position, transform.position);
        if (dist > _validRange)
            return;

        other.gameObject.GetComponent<Stat>().OnAttacked(_userStat);
        Managers.Game.Despawn(gameObject);
    }
}
