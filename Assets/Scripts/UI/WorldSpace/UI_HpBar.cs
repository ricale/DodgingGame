using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpBar : UI_Base
{
    enum GameObjects
    {
        HpBar
    }

    Stat _stat;

    public override void init()
    {
        Bind<GameObject>(typeof(GameObjects));
        _stat = transform.parent.GetComponent<Stat>();
    }

    private void Update()
    {
        Transform parent = transform.parent;
        transform.position = parent.position
            + Vector3.up * parent.GetComponent<Collider>().bounds.size.y * 1.1f;
        transform.rotation = Camera.main.transform.rotation;

        float ratio = _stat.Hp / (float)_stat.MaxHp;
        setHpRatio(ratio);
    }

    public void setHpRatio(float ratio)
    {
        GetObject((int)GameObjects.HpBar).GetComponent<Slider>().value = ratio;
    }
}
