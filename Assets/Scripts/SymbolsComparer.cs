using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SymbolsComparer: EqualityComparer<Symbol[]>
{

    public override bool Equals(Symbol[] s1, Symbol[] s2)
    {
        if (ReferenceEquals(s1, s2))
            return true;

        if (s2 is null || s1 is null)
            return false;

        return ArrayUtility.ArrayEquals(s1, s2);
    }

    public override int GetHashCode(Symbol[] symbols)
    {
        int HashCode = 0;
        foreach(Symbol symbol in symbols){
            HashCode += symbol.GetHashCode();
        }
        return HashCode;
    }
}