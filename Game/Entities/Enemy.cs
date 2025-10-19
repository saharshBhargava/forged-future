
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
class Enemy : PhysicsEntity
{
    public EnemyAnimationController ec { get; internal set; } = new();

    public int health { get; internal set; } = 3; // Initial health
    private bool isStunned = false;
    private float stunDuration = 5; // Seconds the enemy is stunned
    private float stunTimer = 0.0f;

    private bool isDamaged = false;
    private float damagedTimer = 0.0f;
    private float damagedDuration = 0.25f;
    private bool scoreAdded = false;
    public Enemy(float x, float y)
    {
        drawingWidth = 1.6f;
        drawingHeight = 1.6f;
        hitboxWidth = 67.0f / 128 * 1.6f;
        hitboxHeight = 119.0f / 128 * 1.6f;

        this.x = x;
        this.y = y - (hitboxHeight - 1);
        isRigid = true;
    }

    public Enemy(float x, float y, int w, int h)
    {
        this.x = x;
        this.y = y;
        isRigid = true;

        hitboxWidth = w;
        hitboxHeight = h;
    }

    public void UpdatePosition(Level currentLevel, Player player, List<Entity> allEntities)
    {
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
            if (!scoreAdded)
            {
                currentLevel.levelScore += 100;
                scoreAdded = true;
            }
            stunTimer += Engine.TimeDelta;
            if (stunTimer >= stunDuration)
            {
                //player.enemiesStunned += 1;
                //Debug.WriteLine(player.enemiesStunned);
                isStunned = false;
                stunTimer = 0.0f;
                health = 3;
                scoreAdded = false;
            }
        }
        else
        {
            if (Math.Abs(x - player.x) < 1 && Math.Abs(y - player.y) < 1)
            {
                player.ReduceHealth(1);
                Game.audioManager.PlayHurtPlayer();
            }
            if (Math.Abs(x - player.x) < 6 && Math.Abs(y - player.y) < 2)
            {
                if (player.x + player.hitboxWidth < x)
                {
                    currentXVelocity = -5;
                }
                else if (player.x > x + hitboxWidth)
                {
                    currentXVelocity = 5;
                }
            }
            else
            {
                currentXVelocity = 0;
            }
            UpdatePosition(true, allEntities);
        }

        AnimateEnemy();
    }

    public void AnimateEnemy()
    {
        if (currentXVelocity == 0 && ec.currAnimationNum != 0) ec.SetCurrAnim(0);
        if (currentXVelocity != 0 && ec.currAnimationNum != 1) ec.SetCurrAnim(1);
        if (isStunned && ec.currAnimationNum != 2) ec.SetCurrAnim(2);
        if (isDamaged && ec.currAnimationNum != 3) ec.SetCurrAnim(3);

        ec.AdvanceCurrAnim();
    }

    public override Texture GetSprite() => ec.GetCurrAnimation().GetCurrSprite();

    public bool IsStunned()
    {
        return isStunned;
    }

    public void ReduceHealth(Player player)
    {
        health--;
        isDamaged = true;
        if (health <= 0)
        {
            isStunned = true;
            player.enemiesStunned += 1;
        }
        Debug.WriteLine("health reduced");
    }
}