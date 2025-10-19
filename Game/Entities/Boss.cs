
using System;
using System.Collections.Generic;
using System.Diagnostics;

class Boss : PhysicsEntity
{
    public BossAnimationController bc { get; internal set; } = new();

    public int stunHealth { get; internal set; } = 5; // Initial stunHealth
    private bool isStunned = false;
    private float stunDuration = 5; // Seconds the enemy is stunned
    private float stunTimer = 0.0f;

    public int trueHealth { get; internal set; } = 10; // Initial trueHealth

    private bool isDamaged = false;
    private float damagedTimer = 0.0f;
    private float damagedDuration = 0.25f;

    private bool trueDamage = false;

    public Boss(float x, float y)
    {
        this.x = x;
        this.y = y;
        isRigid = true;

        drawingWidth = 4;
        drawingHeight = 4;
        hitboxWidth = 12.0f/32*4;
        hitboxHeight = 31.0f/32*4;
    }

    public void UpdatePosition(Player player, List<Entity> allEntities)
    {
        trueDamage = player.bossDamageEnabled;

        if (isDamaged)
        {
            damagedTimer += Engine.TimeDelta;
            if (damagedTimer >= damagedDuration)
            {
                isDamaged = false;
                damagedTimer = 0.0f;
            }
        }

        if (isStunned)
        {
            if (trueHealth > 0)
            {
                stunTimer += Engine.TimeDelta;
                if (stunTimer >= stunDuration)
                {
                    isStunned = false;
                    stunTimer = 0.0f;
                    stunHealth = 3;
                }
            }
        }
        else
        {
            foreach (Entity entity in player.currentlyInContactWith)
            {
                if (entity == this && !player.currentGroundEntities.Contains(this))
                {
                    player.ReduceHealth(1);
                    Game.audioManager.PlayHurtPlayer();
                }
            }

            if (Math.Abs(x - player.x) < 10)
            {
                if (player.x + player.hitboxWidth < x)
                {
                    currentXVelocity = -10;
                }
                else if (player.x > x + hitboxWidth)
                {
                    currentXVelocity = 10;
                }
            }
            else
            {
                currentXVelocity = 0;
            }
            UpdatePosition(true, allEntities);
        }

        AnimateBoss();
    }

    public void AnimateBoss()
    {
        if (currentXVelocity != 0 && bc.currAnimationNum != 1) bc.SetCurrAnim(1);
        if (currentXVelocity == 0 && bc.currAnimationNum != 0) bc.SetCurrAnim(0);
        if (isStunned && bc.currAnimationNum != 2) bc.SetCurrAnim(2);

        if (isDamaged)
        {
            if (!trueDamage && bc.currAnimationNum != 3) bc.SetCurrAnim(3);
            if (trueDamage && bc.currAnimationNum != 4) bc.SetCurrAnim(4);
        }

        bc.AdvanceCurrAnim();
    }

    public override Texture GetSprite() => bc.GetCurrAnimation().GetCurrSprite();

    public bool IsStunned()
    {
        return isStunned;
    }

    public void ReduceStunHealth()
    {
        stunHealth--;
        isDamaged = true;
        if (stunHealth <= 0)
        {
            isStunned = true;
        }
        Debug.WriteLine("Stun health reduced");
    }

    public void ReduceTrueHealth()
    {
        trueHealth--;
        isDamaged = true;
        if (trueHealth <= 0)
        {
            isStunned = true;
        }
    }
}