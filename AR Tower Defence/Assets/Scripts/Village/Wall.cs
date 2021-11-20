/**
 * Blocks enemies by carving into nav mesh
 * TODO add villagers repairing the wall
 *@ author Manny Kwong 
 */

public class Wall : VillageBuilding
{  
    public override string GetGameInfo(bool showState)
    {
        return "WALL: Blocks enemies and villagers. " + base.GetGameInfo(showState); ;
    }
}
