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
    }

    public override void FixedUpdate()
    {
        Effect();
    }

    public override void Effect()
    {
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

        //rb.AddForce(spell.transform.rotation * dir * spell.speed, ForceMode.VelocityChange);

        base.Effect();
        Output();
    }
}
