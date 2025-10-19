class Container : UIComponent
{
    //Keep width and height here
    public Container(float x, float y, float width, float height, Color? color = null)
        : base(x, y, width, height, color)
    { }

    public Container(UIComponent parent, float width, float height, Alignment alignX = Alignment.LEFT, Alignment alignY = Alignment.TOP, float px = 0, float py = 0, Color? color = null)
        : base(parent, width, height, alignX, alignY, px, py, color)
    { }

    public override void DrawComponent()
    {
        if (!color.Equals(null))
        {
            Engine.DrawRectSolid(dimensions, color);
        }
    }
}
