
using UnityEngine;
using UnityEngine.AI;
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


