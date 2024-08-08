using System.Collections;
using UnityEngine;

public class SC_LQ_RuneTeleport : SC_LQ_SpellRune
{
    public enum Direction { Forward, Backward, Right, Left };

    public Direction direction = Direction.Forward;

    private void Awake()
    {
        base.Awake();
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


        base.Effect();
        //spell.transform.position += dir ;

        Output();

        this.enabled = false;
    }
}
