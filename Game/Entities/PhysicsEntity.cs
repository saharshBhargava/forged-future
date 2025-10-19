using System.Collections.Generic;

abstract class PhysicsEntity: Entity
{
    // Physics related fields
    public float currentXVelocity { get; internal set; } = 0;
    public float currentXAcceleration { get; internal set; }
    public float terminalXVelocity { get; internal set; } = 10;

    public float currentYVelocity { get; internal set; } = 0;

    public float frictionalCoefficient { get; internal set; } = 100;

    public float gravitationalConstant { get; internal set; } = 70;

    public List<Entity> currentlyInContactWith { get; internal set; } = new();
    public List<Entity> currentGroundEntities { get; internal set; } = new();
    public List<Entity> currentCeilingEntities { get; internal set; } = new();

    public bool entityOnLeftSide { get; private set; } = false;
    public bool entityOnRightSide { get; private set; } = false;
    public bool collisionOverride { get; internal set; }

    private float prevY;

    public virtual void UpdatePosition(bool applyGravity, List<Entity> allEntities)
    {
        float deltaTime = Engine.TimeDelta;
        prevY = y;

        if (currentXAcceleration != 0)
        {
            currentXVelocity += currentXAcceleration * deltaTime;
            if (currentXVelocity > terminalXVelocity) currentXVelocity = terminalXVelocity;
            else if (currentXVelocity < -1 * terminalXVelocity) currentXVelocity = -1 * terminalXVelocity;
        }
        else
        {
            if (currentXVelocity > 0)
            {
                currentXVelocity -= frictionalCoefficient * deltaTime;
                if (currentXVelocity < 0) currentXVelocity = 0;
            }
            if (currentXVelocity < 0)
            {
                currentXVelocity += frictionalCoefficient * deltaTime;
                if (currentXVelocity > 0) currentXVelocity = 0;
            }
        }

        if (applyGravity) currentYVelocity += gravitationalConstant * deltaTime;

        if (currentXVelocity > 0 && entityOnRightSide)
        {
            currentXVelocity = 0;
        }

        if (currentXVelocity < 0 && entityOnLeftSide)
        {
            currentXVelocity = 0;
        }

        y = y + currentYVelocity * deltaTime;
        x = x + currentXVelocity * deltaTime;

        currentlyInContactWith.Clear();
        currentCeilingEntities.Clear();
        currentGroundEntities.Clear();

        entityOnRightSide = false;
        entityOnLeftSide = false;
        foreach (Entity e in allEntities)
        {
            if (e!=this && e.isRigid)
            {
                if (CollidesWith(e))
                {
                    if (e is PhysicsEntity pe && pe.collisionOverride)
                    {
                        y = e.y - hitboxHeight;
                        currentYVelocity = 0;
                    }
                    else SnapTo(e);
                }
                if (InContactWith(e)) currentlyInContactWith.Add(e);
                if (OnTopOf(e) && !OnRightSideOf(e) && !OnLeftSideOf(e)) currentGroundEntities.Add(e);
                if (Under(e)) currentCeilingEntities.Add(e);

                if (OnLeftSideOf(e)) entityOnRightSide = true;
                if (OnRightSideOf(e)) entityOnLeftSide = true;
            }
        }
    }

    public override bool CollidesWith(Entity e)
    {
        if (e is PhysicsEntity pe && pe.collisionOverride)
        {
            return (base.CollidesWith(e) && currentYVelocity > 0 && prevY + hitboxHeight <= e.y);
        }
        return base.CollidesWith(e);
    }

    public void SnapTo(Entity e)
    {
        float moveToLeft = e.x - hitboxWidth;
        float moveToRight = e.x + e.hitboxWidth;
        float moveToUp = e.y - hitboxHeight;
        float moveToDown = e.y + e.hitboxHeight;

        float dxLeft = x - moveToLeft;
        float dxRight = moveToRight - x;
        float dxUp = y - moveToUp;
        float dxDown = moveToDown - y;

        float xChoice = 0;
        float yChoice = 0;
        if (currentXVelocity > 0)
        {
            xChoice = dxLeft;
        }
        else if (currentXVelocity < 0)
        {
            xChoice = dxRight;
        }
        if (currentYVelocity > 0)
        {
            yChoice = dxUp;
        }
        else if (currentYVelocity < 0)
        {
            yChoice = dxDown;
        }
        if ((xChoice < yChoice || yChoice == 0) && xChoice != 0)
        {
            if (currentXVelocity > 0) x = moveToLeft;
            else if (currentXVelocity < 0) x = moveToRight;
            currentXVelocity = 0;
        }
        else if (yChoice != 0)
        {
            if (currentYVelocity > 0) y = moveToUp;
            else if (currentYVelocity < 0) y = moveToDown;
            currentYVelocity = 0;
        }
    }
}