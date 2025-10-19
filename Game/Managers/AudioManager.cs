class AudioManager
{
    private readonly Sound footsteps;
    private readonly Sound hurtPlayer;
    private readonly Sound damageEnemy;
    private readonly Sound collectable;
    private readonly Sound music;

    private SoundInstance collisionSound;
    private SoundInstance backgroundMusic;

    public AudioManager()
    {   
        footsteps = Engine.LoadSound("Footsteps.wav");
        hurtPlayer = Engine.LoadSound("PlayerHurt.wav");
        damageEnemy = Engine.LoadSound("DamageEnemy.wav");
        collectable = Engine.LoadSound("Collectable.wav");
        music = Engine.LoadSound("GameMusic.wav");

        collisionSound = new SoundInstance();
        PlayGameMusic();
    }

    public void PlayGameMusic() => backgroundMusic = Engine.PlaySound(music, true);

    public void PlayCollisionSound(Sound sound)
    {
        collisionSound = Engine.PlaySound(sound);
    }
    public void PlayFootsteps() => PlayCollisionSound(footsteps);
    public void PlayHurtPlayer() => PlayCollisionSound(hurtPlayer);
    public void PlayDamageEnemy() => PlayCollisionSound(damageEnemy);
    public void PlayCollectable() => PlayCollisionSound(collectable);
}