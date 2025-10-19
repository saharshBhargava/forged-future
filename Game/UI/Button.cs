using System;
using System.Diagnostics;

class Button : Container
{
    public Action onClickAction;
    private Color baseColor;

    public Button(UIComponent parent, float width, float height, Alignment alignX = Alignment.LEFT, Alignment alignY = Alignment.TOP, float px = 0, float py = 0, Color? color = null, Action action = null)
        : base(parent, width, height, alignX, alignY, px, py, color)
    {
        baseColor = this.color;
        onClickAction = action;
    }

    public void SetAction(Action action)
    {
        onClickAction = action;
    }

    public override void OnClick()
    {
        if (onClickAction != null)
        {
            onClickAction.Invoke();
        }
    }

    public void onHover(bool mouseOnButton)
    {
        if (!mouseOnButton)
        {
            if (color.Equals(Color.LightGray))
            {
                color = baseColor;
                DrawComponent();
            }
        }
        else if (color.Equals(baseColor))
        {
            color = Color.LightGray;
            DrawComponent();
        }
    }
}
