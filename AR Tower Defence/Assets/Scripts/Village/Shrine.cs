

using UnityEngine;
/**
* Represents the player
* Gameover if it dies
* TODO add dancing worshippers, gives points over time during the day
*@ author Manny Kwong 
*/
public class Shrine : Destroyable
{


    protected override void Remove()
    {
        GameController.Instance.GameOver();
        base.Remove();
       
    }
}
