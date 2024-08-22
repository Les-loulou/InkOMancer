using UnityEngine;

public class SC_LQ_RuneMove : SC_LQ_SpellRune
{

    public enum Direction { Forward, Backward, Right, Left };

    public Direction direction = Direction.Forward;

    Rigidbody rb;


    public override void Start()
    {
        base.Start();

        rb = spell.rb;

        Output();
    }

    public override void FixedUpdate()
    {
        Effect(null);
    }

    public override void Effect(GameObject nothing)
    {
        base.Effect(null);

        Vector3 dir = Vector3.zero;
        switch (direction)
        {
            case Direction.Forward:
                dir = Vector3.forward;
                break;
            case Direction.Backward:
                dir = Vector3.back;
                break;
            case Direction.Right:
                dir = Vector3.right;
                break;
            case Direction.Left:
                dir = Vector3.left;
                break;
            default:
                break;
        }


        
        rb.linearVelocity = spell.transform.rotation * dir * spell.speed;
    }
}