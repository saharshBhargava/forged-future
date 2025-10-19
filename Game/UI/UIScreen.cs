using System.Collections.Generic;

class UIScreen
{
    public List<UIComponent> elements { get; internal set; } = new List<UIComponent>();

    public void AddElement(UIComponent element)
    {
        AddElementRecursively(element);
    }

    private void AddElementRecursively(UIComponent element)
    {
        elements.Add(element);

        foreach (var child in element.GetChildren())
        {
            AddElementRecursively(child);
        }
    }
}
