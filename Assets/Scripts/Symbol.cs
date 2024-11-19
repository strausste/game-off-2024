using UnityEngine;
using UnityEngine.UIElements;

public class Symbol
{
    int id;
    Sprite image;

    public Symbol(int id, Sprite image)
    {
        this.id = id;
        this.image = image;
    }

    public int getId()
    {
        return id;
    }

    public Sprite getSprite()
    {
        return image;
    }

    public bool Equals(Symbol s)
    {
        if (this == s)
        {
            return true;
        }
        if (this.getId() == s.getId())
        {
            return true;
        }

        return false;
    }
}
