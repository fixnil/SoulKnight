using UnityEngine;

public class Character : MonoBehaviour
{
    public int speed = 100;
    public CharacterController characterController;
    public Animator characterAnimator;

    private void Update()
    {
        this.Move();
        this.Rotate();
        this.Atk();
    }

    private void Move()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            characterAnimator.SetBool("walk", true);

            var direction = new Vector3(horizontal, 0, vertical);
            characterController.SimpleMove(direction * speed);
        }
        else
        {
            characterAnimator.SetBool("walk", false);
        }
    }

    private void Rotate()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit, 1000, 1 << 6))
        {
            var target = hit.point;
            target.y = this.transform.position.y;
            this.transform.LookAt(target);
        }
    }

    private void Atk()
    {
        if (Input.GetMouseButtonDown(0))
        {
            characterAnimator.SetBool("atk", true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            characterAnimator.SetBool("atk", false);
        }
    }
}
