abstract class Collectable : Entity
{
    private Texture sprite;

    public Collectable()
    {
        isRigid = false;
    }

    public void SetSprite(Texture sprite) => this.sprite = sprite;
    public override Texture GetSprite() => sprite;

}