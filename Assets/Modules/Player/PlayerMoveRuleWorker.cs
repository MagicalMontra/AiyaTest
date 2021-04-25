using UnityEngine;

public class PlayerMoveRuleWorker
{
    public bool IsMovable(Vector2 currentDir, Vector2 nextDir, Avatar head, Avatar[] tails)
    {
        var left = Vector2.left;
        var right = Vector2.right;
        var up = Vector2.up;
        var down = Vector2.down;

        if (currentDir == left && nextDir == right)
            return false;
            
        if (currentDir == right && nextDir == left)
            return false;

        if (currentDir == up && nextDir == down)
            return false;
            
        if (currentDir == down && nextDir == up)
            return false;

        return true;
    }
}