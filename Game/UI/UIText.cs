using System;
class UIText : UIComponent
{
    public string text { get; internal set; }
    public Font font { get; internal set; }
    private Alignment alignX;
    private Alignment alignY;
    private float offsetX;
    private float offsetY;

    public UIText(string inputText, int fontSize, float x, float y, Color? color = null)
        : base(x, y, 0, 0, color)
    {
        text = inputText;
        font = Engine.LoadFont("Retro Gaming.ttf", fontSize);
    }

    public UIText(UIComponent parent, string inputText, int fontSize, Alignment alignX = Alignment.LEFT, Alignment alignY = Alignment.TOP, float px = 0, float py = 0, Color? color = null)
        : base(parent, -1, -1, alignX, alignY, px, py, color)
    {
        text = inputText;
        this.alignX = alignX;
        this.alignY = alignY;
        offsetX = px;
        offsetY = py;


        font = Engine.LoadFont("Retro Gaming.ttf", fontSize);

        UpdateDimensions();
    }

    private void UpdateDimensions()
    {
        Bounds2 measuredSize = GetTextboxSize();

        if (parent == null)
        {
            throw new InvalidOperationException();
        }

        float alignedX = CalculateAlignment(parent.dimensions.Position.X, parent.dimensions.Size.X, measuredSize.Size.X, alignX) + offsetX;
        float alignedY = CalculateAlignment(parent.dimensions.Position.Y, parent.dimensions.Size.Y, measuredSize.Size.Y, alignY) + offsetY;

        dimensions = new Bounds2(alignedX, alignedY, measuredSize.Size.X, measuredSize.Size.Y);
    }

    public override void DrawComponent()
    {
        if (!color.Equals(null))
        {
            Engine.DrawString(text, dimensions.Position, color, font, TextAlignment.Left);
        }
    }

    public Bounds2 GetTextboxSize()
    {
        return Engine.DrawString(text, dimensions.Position, color, font, TextAlignment.Left, measureOnly:true);
    }
}