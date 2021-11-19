using UnityEngine.AI;
/*
* Enemies that attack village buildings as they make their way to the shine from the portal
*@ author Manny Kwong 
*/
public class Enemy : Agent
{  
    protected override void Init()
    {
        Name = "Enemy";
        TargetName = "Village";
        if (FindObjectOfType<Shrine>())
        {
            DefaultTarget = FindObjectOfType<Shrine>().transform;
        }
        base.Init();
    }

    protected override void DamageEffects(Damage damage)
    {
        base.DamageEffects(damage);
        if (damage.damage > 0)
        {
            SoundEffects.PlayOneShot(SoundManager.Instance.SoundClips[(int)SoundManager.SoundType.MonsterDamage]);
        }
    }

    protected override void LookAround()
    {
        base.LookAround();
        if (CurrentTarget)
        {
            /*
            if (CurrentTarget.GetComponent<Wall>())
            {
                NavMeshPath path = new NavMeshPath();
                if (NavAgent.CalculatePath(DefaultTarget.transform.position + GetRandomAround(DistanceFromTarget), path))
                {
                    SetTarget(DefaultTarget,DistanceFromTarget);
                }
            }*/

            //If there is no path to shrine set the target to a wall blocking the path to the shrine 
            if (CurrentTarget == DefaultTarget)
            {
                if (NavAgent.pathStatus != NavMeshPathStatus.PathComplete)
                {
                    Wall[] walls = FindObjectsOfType<Wall>();
                    foreach (Wall wall in walls)
                    {
                        NavMeshPath path = new NavMeshPath();
                        NavAgent.CalculatePath(wall.transform.position + GetRandomAround(DistanceFromTarget), path);

                        if (path.status == NavMeshPathStatus.PathComplete)
                        {
                            print("path complete" + wall);
                            SetTarget(wall.transform, 0.1f);
                            break;
                        }
                    }
                }
            }
        }
    }
}


