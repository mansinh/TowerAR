using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine : VillageBuilding
{
    protected override void Remove()
    {
        base.Remove();
        GameController.Instance.GameOver();
    }
}
