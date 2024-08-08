using System.Collections;
using UnityEngine;

public class SC_LQ_EFFECT_RuneTeleport : SC_LQ_SpellEffect
{
    public enum Direction { Forward, Backward, Right, Left };

    public Direction direction = Direction.Forward;

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
        transform.position += dir ;

        Output();
    }
}
