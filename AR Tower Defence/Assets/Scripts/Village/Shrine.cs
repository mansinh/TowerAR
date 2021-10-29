/**
 * Represents the player
 * Gameover if it dies
 * TODO add dancing worshippers, gives points over time during the day
 *@ author Manny Kwong 
 */

public class Shrine : VillageBuilding
{
    protected override void Remove()
    {
        base.Remove();
        GameController.Instance.GameOver();
    }
}
