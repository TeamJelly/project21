﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile
{
    public int number;
    public string name;

    public Vector2Int position;

    public Effect tileEffect;

    Unit unit = null;

    public bool IsUsable()
    {
        return unit == null;
    }

    public bool IsUsable(Unit unit)
    {
        if (this.unit == unit || this.unit == null)
            return true;
        else
            return false;
    }

    public void SetUnit(Unit newUnit)
    {
        unit = newUnit;
    }
}
