using UnityEngine;
using UnityEngine.UIElements;

public class Symbol
{
    int id;
    Texture2D image;

    public Symbol(int id, Texture2D image){
        this.id = id;
        this.image = image;
    }

    public int getId(){
        return id;
    }
}
