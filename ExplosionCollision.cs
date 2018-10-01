using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionCollision : MonoBehaviour {
    // 트리거로 사용한다.
    public float force = 0f;
    public bool reduction = false;
    public float reductionVariable = 1f;
    public float colliderSize = 0f;
    public bool staticDirection = false;
    // 고정 방향은 -180 ~ 180까지있다.
    public Vector2 direction;
    /// <summary>
    ///  세팅 용 함수 들.
    /// </summary>
    public void SetExploion(float force_)
    {
        force = force_;
    }
    public void SetExploion(float force_, float reduciton_)
    {
        SetExploion(force_);
        reduction = true;
        reductionVariable = reduciton_;
    }
    public void SetExploion(float force_, bool dir, Vector2 direction_)
    {
        SetExploion(force_);
        staticDirection = dir;
        direction = direction_;
    }
    public void SetExploion(float force_, float reduciton_, bool dir, Vector2 direction_)
    {

        SetExploion(force_, reduciton_);
        staticDirection = dir;
        direction = direction_;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //unit은 움직일 수 있는 오브젝트를 뜻한다. 또한 2d강체를 포함 하고 있다
        if (collision.gameObject.tag != "unit") return;
        Vector3 myPositipon = transform.position;
        Vector3 youPosition = collision.transform.position;
        Rigidbody2D rb2d = collision.gameObject.GetComponent<Rigidbody2D>();

        if (reduction)
        {
            float newForce = (youPosition - myPositipon).magnitude;
            newForce = colliderSize - newForce;
            newForce = newForce / colliderSize / reductionVariable;
            if (newForce < 0f) newForce = 0f;
            newForce = newForce * force;

            if (staticDirection)
            {
                rb2d.AddForce(direction.normalized * newForce);
            }
            else
            {
                rb2d.AddForce((youPosition - myPositipon).normalized * newForce);
            }
        }
        else
        {
            if (staticDirection)
            {
                rb2d.AddForce(direction.normalized * force);
            }
            else
            {
                rb2d.AddForce((youPosition - myPositipon).normalized * force);
            }
        }

    }

}
