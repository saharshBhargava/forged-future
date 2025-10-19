using System.Collections.Generic;

abstract class UIComponent
{
    public Bounds2 dimensions { get; internal set; }
    public Color color { get; internal set; }
    public UIComponent parent { get; internal set; }
    private List<UIComponent> childrenObjects;

    public UIComponent(float x, float y, float width, float height, Color? color)
    {
        childrenObjects = new List<UIComponent>();

        if (width == -1 || height == -1) dimensions = new Bounds2(x, y, width, height);
        else dimensions = new Bounds2(0, 0, Game.Resolution.X, Game.Resolution.Y);

        dimensions = new Bounds2(x, y, width, height);

        if (color.HasValue)
        {
            this.color = color.Value;
        }
    }

    public UIComponent(UIComponent parent, float width, float height, Alignment alignX = Alignment.LEFT, Alignment alignY = Alignment.TOP, float px = 0, float py = 0, Color? color = null)
    {
        this.parent = parent;
        childrenObjects = new List<UIComponent>();

        float parentX = parent.dimensions.Position.X;
        float parentY = parent.dimensions.Position.Y;
        float parentWidth = parent.dimensions.Size.X;
        float parentHeight = parent.dimensions.Size.Y;

        float childX = CalculateAlignment(parentX, parentWidth, width, alignX) + px;
        float childY = CalculateAlignment(parentY, parentHeight, height, alignY) + py;

        dimensions = new Bounds2(childX, childY, width, height);

        if (color.HasValue)
        {
            this.color = color.Value;
        }

        parent.AddChild(this);
    }

    public float CalculateAlignment(float parentPos, float parentSize, float childSize, Alignment alignment)
    {
        return alignment switch
        {
            Alignment.LEFT or Alignment.TOP => parentPos,
            Alignment.MIDDLE => parentPos + (parentSize - childSize) / 2.0f,
            Alignment.RIGHT or Alignment.BOTTOM => parentPos + parentSize - childSize,
            _ => parentPos
        };
    }

    public void AddChild(UIComponent child)
    {
        childrenObjects.Add(child);
    }

    public List<UIComponent> GetChildren() => childrenObjects;

    public virtual void DrawComponent() { }

    public virtual void OnClick() { }

    public bool IsHover(float x, float y)
    {
        return x >= dimensions.Position.X && x <= dimensions.Position.X + dimensions.Size.X &&
               y >= dimensions.Position.Y && y <= dimensions.Position.Y + dimensions.Size.Y;
    }
}