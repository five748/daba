using UnityEngine;
public class MoveHelp
{
    private bool isToUp;
    private bool isToRight;
    private bool isXZero = false;
    private bool isYZero = false;
    public Vector2 endPos;
    private Transform endTran;
    public MoveHelp(Vector2 begin, Vector2 target)
    {
        endPos = target;
        isToUp = target.y > begin.y;
        isToRight = target.x > begin.x;
        isYZero = target.y == begin.y;
        isXZero = target.x == begin.x;
    }
    public MoveHelp(Vector2 begin, Transform target)
    {
        endTran = target;
        isToUp = target.position.y > begin.y;
        isToRight = target.position.x > begin.x;
        isYZero = target.position.y == begin.y;
        isXZero = target.position.x == begin.x;
    }
    public bool CheckIsGoToTargetTran(Vector2 movingPos)
    {
        if (isXZero && isYZero)
        {
            return true;
        }
        if (movingPos.x == endTran.position.x && movingPos.y == endTran.position.y)
        {
            return true;
        }
        if (isXZero)
        {
            if (isToUp)
            {
                return movingPos.y > endTran.position.y;
            }
            else
            {
                return movingPos.y < endTran.position.y;
            }
        }
        if (isYZero)
        {
            if (isToRight)
            {
                return movingPos.x > endTran.position.x;
            }
            else
            {
                return movingPos.x < endTran.position.x;
            }
        }
        if (isToRight && isToUp)
        {
            return movingPos.x > endTran.position.x || movingPos.y > endTran.position.y;
        }
        if (isToRight && !isToUp)
        {
            return movingPos.x > endTran.position.x || movingPos.y < endTran.position.y;
        }
        if (!isToRight && !isToUp)
        {
            return movingPos.x < endTran.position.x || movingPos.y < endTran.position.y;
        }
        if (!isToRight && isToUp)
        {
            return movingPos.x < endTran.position.x || movingPos.y > endTran.position.y;
        }
        return true;
    }
    public bool CheckIsGoToTarget(Vector2 movingPos)
    {
        if (isXZero && isYZero)
        {
            return true;
        }
        if (Mathf.Abs(movingPos.x - endPos.x) <= 0.01f && Mathf.Abs(movingPos.y - endPos.y) <= 0.01f)
        {
            return true;
        }
        if (isXZero)
        {
            if (isToUp)
            {
                return movingPos.y > endPos.y;
            }
            else
            {
                return movingPos.y < endPos.y;
            }
        }
        if (isYZero)
        {
            if (isToRight)
            {
                return movingPos.x > endPos.x;
            }
            else
            {
                return movingPos.x < endPos.x;
            }
        }
        if (isToRight && isToUp)
        {
            return movingPos.x > endPos.x || movingPos.y > endPos.y;
        }
        if (isToRight && !isToUp)
        {
            return movingPos.x > endPos.x || movingPos.y < endPos.y;
        }
        if (!isToRight && !isToUp)
        {
            return movingPos.x < endPos.x || movingPos.y < endPos.y;
        }
        if (!isToRight && isToUp)
        {
            return movingPos.x < endPos.x || movingPos.y > endPos.y;
        }
        return true;
    }
}







