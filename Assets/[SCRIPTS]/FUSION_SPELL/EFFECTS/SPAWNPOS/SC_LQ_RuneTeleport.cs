using UnityEngine;

public class SC_LQ_RuneTeleport : SC_LQ_SpellRune
{
    public enum Direction { Forward, Backward, Right, Left };

    public Direction direction = Direction.Forward;

    float power = 5;

    public override void Effect(GameObject nothing)
    {
        base.Effect(null);
        //Le faire en continue et mettre un wait ???

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

        spell.transform.position += (spell.transform.rotation * dir) * power;

        Output();
    }

    public override void SetRuneState(bool state)
    {
        base.SetRuneState(state);

        if (state == true)
        {
            Effect(null);
        }
    }

}
