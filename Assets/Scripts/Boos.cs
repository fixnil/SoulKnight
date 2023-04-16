using UnityEngine;

public class Boos : MonoBehaviour
{
    public enum State { Walk, Atk, Die }

    public Transform character;
    public Animator boosAnimator;
    public int speed = 50;
    public int range = 30;
    public int time = 2;

    private int _hp = 50;
    private float _atkTime;
    private State _state;

    private void Update()
    {
        if (_hp <= 0)
        {
            _state = State.Die;
        }

        switch (_state)
        {
            case State.Walk:
            this.Walk();
            break;
            case State.Atk:
            this.Atk();
            break;
            case State.Die:
            this.Die();
            break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_state != State.Die)
        {
            boosAnimator.SetTrigger("hit");

            _hp--;

            if (_hp <= 0)
            {
                _state = State.Die;
            }
        }
    }

    private void Walk()
    {
        this.transform.Rotate(speed * Time.deltaTime * Vector3.up);

        var target = character.position;
        target.y = this.transform.position.y;

        var angle = Vector3.Angle(this.transform.forward, target - this.transform.position);

        if (angle < range / 2)
        {
            _atkTime = time;
            _state = State.Atk;
        }
    }

    private void Atk()
    {
        boosAnimator.SetBool("atk", true);

        _atkTime -= Time.deltaTime;

        if (_atkTime <= 0)
        {
            _state = State.Walk;
            boosAnimator.SetBool("atk", false);
        }
    }

    private void Die()
    {
        boosAnimator.SetBool("die", true);
    }
}
