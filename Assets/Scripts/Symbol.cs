using System;
using UnityEngine;

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

    // override object.Equals
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Symbol symbol = (Symbol) obj;
        
        if (this.getId() == symbol.getId())
        {
            return true;
        }
        
        return base.Equals (obj);
    }
    
    // override object.GetHashCode
    public override int GetHashCode()
    {
        // TODO: write your implementation of GetHashCode() here
        return HashCode.Combine(id,image);
    }
    public override string ToString()
    {
        return "Symbol " + id.ToString();
    }
}
