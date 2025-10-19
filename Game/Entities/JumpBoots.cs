class JumpBoots : Collectable
{
    public int maxJumps { get; internal set; }
    public JumpBoots()
    {
        drawingWidth = 1;
        drawingHeight = 1;
        hitboxWidth = 1;
        hitboxHeight = 1;
    }

    public JumpBoots(int maxJumps): this()
    {
        this.maxJumps = maxJumps;
    }
}