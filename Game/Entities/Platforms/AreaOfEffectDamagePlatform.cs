using System.Collections.Generic;

class AreaOfEffectPlatform : PlatformBlock
{
    private int damage = 1; // Damage per second
    private float timeSinceLastDamage = 1;
    public static bool onAOEPlatform = false;
    public bool canDamage { get; internal set; } = false;

    public AreaOfEffectPlatform(float x, float y, char sprite, int damage) : base(x, y, sprite)
    {
        this.damage = damage;
    }

    public void UpdatePosition(List<Entity> allEntities)
    {
        if (timeSinceLastDamage >= 1.0f) SetSprite('E');
        else SetSprite('E');
        timeSinceLastDamage += Engine.TimeDelta;
        UpdatePosition(false, allEntities);
    }

    public void InContactWithPlayer(Player player, bool inContact, char spriteChar)
    {
        if (inContact && player.OnTopOf(this))
        {
            onAOEPlatform = true;
            if (!sprite.Equals(spriteMap[spriteChar]))
            {
                SetSprite(spriteChar);
                Game.audioManager.PlayFootsteps();
            }
            if (timeSinceLastDamage >= 1.0f)
            {
                player.ReduceHealth(damage);
                timeSinceLastDamage = 0;
                Game.audioManager.PlayHurtPlayer();
            }
        }
        else
        {
            onAOEPlatform = false;
            this.sprite = initialSprite;
        }

        if (onAOEPlatform) player.pc.SetCurrAnim(6);
    }

}