using System;
using System.Collections.Generic;
using System.Diagnostics;

class Player : PhysicsEntity
{
    public bool onMovingPlatform { get; internal set; }
    public float platformSpeed { get; internal set; }

    public float enemiesStunned { get; internal set; }


    public bool jumpState { get; internal set; } = false;
    private int jumpCount = 0;
    private int maxJumps = 1;

    private int dashCount = 0;
    private int maxDashes = 1;

    public bool movingRight { get; internal set; } = false;
    public bool movingLeft { get; internal set; } = false;

    public bool facingRight { get; internal set; } = true;

    public PlayerAnimationController pc { get; internal set; } = new();

    public bool isDashing { get; internal set; } = false;
    public bool isDucking { get; internal set; } = false;
    public bool isSliding { get; internal set; } = false;
    private float dashSpeed = 20; // Dash speed
    private float dashDuration = 0.25f; // How long the dash lasts
    private float dashTimer = 0f; // Timer to track dash duration
    private float duckHeight = 0.8f; // Ducking reduces player's height
    private float crawlSpeed = 5; // Speed during the crawl

    private bool isDamaged = false;
    private float damagedTimer = 0.0f;
    private float damagedDuration = 0.25f;

    public int health { set; internal get; } = DataManager.defaultHealth;
    private float timeSinceDamageDealt = 0;
    private bool hasArmor = false;

    public bool bossDamageEnabled { get; internal set; } = false;

    private Inventory inventory;
    public Player(float x, float y)
    {
        drawingWidth = 1.6f;
        drawingHeight = 1.6f;
        hitboxWidth = 10.0f/32*1.6f;
        hitboxHeight = 30.0f/32*1.6f;

        this.x = x;
        this.y = y - (hitboxHeight - 1);
        isRigid = true;

        inventory = new Inventory();
    }
    public void UpdatePosition(List<Entity> allEntities)
    {
        onMovingPlatform = false;
        platformSpeed = 0;
        timeSinceDamageDealt += Engine.TimeDelta;

        if (isDamaged)
        {
            damagedTimer += Engine.TimeDelta;
            if (damagedTimer >= damagedDuration)
            {
                isDamaged = false;
                damagedTimer = 0.0f;
            }
        }

        if (isSliding && Math.Abs(currentXVelocity) == 0)
        {
            StandUp();
            Duck();
        }

        if(!hasArmor)
        {
            foreach (Collectable collectable in inventory.GetInventoryList())
            {
                if (collectable is Armor) hasArmor = true;
                if (collectable is DamageBoots) bossDamageEnabled = true;
            }
            if (hasArmor)
            {
                ResetHealth(DataManager.defaultHealth + 1);
                hasArmor = true;
            }
        }

        // Check if the player is on a moving platform and use only the first one
        foreach (Entity entity in currentGroundEntities)
        {
            if (!onMovingPlatform && entity is MovingPlatform platform)
            {
                onMovingPlatform = true;
                platformSpeed = platform.speed * platform.direction;
            }
            if (entity is Enemy enemy && !enemy.IsStunned() && jumpState)
            {
                enemy.ReduceHealth(this);
                Game.audioManager.PlayDamageEnemy();
            }
            if (entity is Boss boss && !boss.IsStunned() && jumpState)
            {
                if (!bossDamageEnabled) boss.ReduceStunHealth();
                else boss.ReduceTrueHealth();
                Game.audioManager.PlayDamageEnemy();
            }
        }

        // Handle movement logic for the player
        if (movingLeft)
        {
            if (Math.Abs(currentXVelocity) < 1) currentXVelocity = -1;
            if (isDucking) currentXVelocity = -crawlSpeed;
            else currentXAcceleration = -50;
        }
        if (movingRight)
        {
            if (Math.Abs(currentXVelocity) < 1) currentXVelocity = 1;
            if (isDucking) currentXVelocity = crawlSpeed;
            else currentXAcceleration = 50;
        }
        if ((!movingLeft && !movingRight) || (movingLeft && movingRight) && !onMovingPlatform)
        {
            currentXAcceleration = 0;
        }

        // Dash logic
        if (isDashing)
        {
            dashTimer += Engine.TimeDelta; // Increase dash timer based on time delta
            Debug.WriteLine(currentXAcceleration);
            if (dashTimer <= dashDuration)
            {
                // Dash in the direction the player is moving (while dash timer is running)
                currentXVelocity = dashSpeed * (facingRight ? 1 : -1);
                currentXAcceleration = 0;
                gravitationalConstant = 0;
                currentYVelocity = 0;
            }
            else
            {
                // Dash ends after the duration has passed
                isDashing = false;
                gravitationalConstant = 70;
            }
        }

        // Handle jumping logic
        if (jumpState)
        {
            if (isSliding || isDucking)
            {
                if (currentCeilingEntities.Count == 0) StandUp();
            }
            else if ((jumpCount == 0 && currentGroundEntities.Count != 0)
                || (jumpCount > 0 && jumpCount < maxJumps))
            {
                currentYVelocity = -110 / (jumpCount + 5);
                jumpCount++;
                if (onMovingPlatform && !movingRight && !movingLeft) currentXVelocity = platformSpeed;
                UpdatePosition(false, allEntities);
            }
            jumpState = false;
        }
        else
        {
            if (onMovingPlatform && !movingRight && !movingLeft) currentXVelocity = platformSpeed;
            UpdatePosition(true, allEntities);
        }

        if (currentGroundEntities.Count > 0)
        {
            jumpCount = 0;
            if(!isDashing) dashCount = 0;
        }

        AnimatePlayer();
    }

    public void AnimatePlayer()
    {
        if (currentXVelocity == 0 && pc.currAnimationNum != 0) pc.SetCurrAnim(0);
        if ((movingRight || movingLeft) && pc.currAnimationNum != 1) pc.SetCurrAnim(1);
        if (currentGroundEntities.Count == 0 && pc.currAnimationNum != 2) pc.SetCurrAnim(2);
        if (onMovingPlatform && !movingLeft && !movingRight) pc.SetCurrAnim(0);
        if (isDucking)
        {
            if(Math.Abs(currentXVelocity) > 0 && pc.currAnimationNum != 5) pc.SetCurrAnim(5);
            else if (Math.Abs(currentXVelocity) == 0 && pc.currAnimationNum != 4) pc.SetCurrAnim(4);
        }
        if (isDashing && pc.currAnimationNum != 3) pc.SetCurrAnim(3);
        if (isSliding && pc.currAnimationNum != 5) pc.SetCurrAnim(5);
        if (isDamaged && pc.currAnimationNum != 7) pc.SetCurrAnim(7);

        pc.AdvanceCurrAnim();
    }

    public void ReduceHealth(int damageDealt)
    {
        if (timeSinceDamageDealt >= 1)
        {
            timeSinceDamageDealt = 0;
            health -= damageDealt;
            isDamaged = true;
        }
        if (health < 0) health = 0;
    }

    public void ResetHealth(int newHealth)
    {
        health = newHealth;
        timeSinceDamageDealt = 0;
    }

    public void Dash()
    {
        if (!isDashing && !isSliding && !isDucking && dashCount < maxDashes)
        {
            isDashing = true;
            dashTimer = 0f;
            dashCount++;
        }
    }

    public void Duck()
    {
        if (!isDucking)
        {
            isDucking = true;
            hitboxHeight /= 2.0f;
            y += hitboxHeight;
        }
    }

    public void Slide()
    {
        if (!isSliding)
        {
            isSliding = true;
            hitboxHeight /= 2.0f;
            y += hitboxHeight;
        }
    }
    public void StandUp()
    {
        isDucking = false;
        isSliding = false;
        hitboxHeight *= 2.0f;
        y -= hitboxHeight / 2.0f;
    }

    public override Texture GetSprite() => pc.GetCurrAnimation().GetCurrSprite();

    public Inventory GetInventory() => inventory;

    public override TextureMirror GetMirror()
    {
        if (facingRight) return TextureMirror.None;
        else return TextureMirror.Horizontal;
    }

    public bool GetPlayerMoving()
    {
        return (movingLeft || movingRight || jumpState);
    }

    public void SetMaxJumps(int maxJumps) => this.maxJumps = Math.Max(this.maxJumps, maxJumps);
}