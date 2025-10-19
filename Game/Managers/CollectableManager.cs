using System.Collections.Generic;
using System.Drawing;

class CollectableManager
{

    private readonly Dictionary<char, Collectable> collectableMap = new()
    {
        { 'T', new Coin() }, // Coin Tokens
        { 'K', new Keycard() }, // Keycard
        { 'J', new DoubleJumpBoots() }, // Double Jump Boots
        { '+', new TripleJumpBoots() }, // Triple Jump Boots
        { 'A', new Armor() }, // Armor
        { '.', new DamageBoots() }, // Damage Boots
        { '-', new Materials()} // Materials
    };

    public Collectable ReturnCollectable(char collectableChar)
    {
        Collectable entity;
        if (collectableMap.TryGetValue(collectableChar, out var mappedCollectable))
        {
            entity = mappedCollectable;
        }
        else
        {
            entity = new Keycard();
        }
        return entity; //
    }
}