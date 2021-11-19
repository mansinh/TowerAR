using UnityEngine;

/**
 * Miracle that grows trees and fields
 * Puts out fires
 *@ author Manny Kwong 
 */

public class MiracleWater : Miracle
{
    [SerializeField] float _coolDown = 0.2f;
    float _timeSinceAttack = 0;
    [SerializeField] float extinguishSpeed = 0.25f;
    [SerializeField] float growSpeed = 5;
    [SerializeField] float tileHealAmount = 0.5f;
    [SerializeField] MyTree treePrefab;

    protected override void OnUpdate()
    {
        _timeSinceAttack += Time.deltaTime;

        if (_timeSinceAttack > _coolDown)
        {
            //bool overGrownTree = false;
            _timeSinceAttack = 0;
            Collider[] detected = Physics.OverlapSphere(transform.position, Collider.radius);

            foreach (Collider other in detected)
            {
                //Put out fire if other collider is fire
                MiracleFire fire = other.GetComponent<MiracleFire>();
                if (fire != null)
                {
                    fire.OnWater(extinguishSpeed * _coolDown);
                }

                //Grow trees if other collider is a Tree
                MyTree tree = other.GetComponent<MyTree>();
                if (tree != null)
                {
                    tree.Grow(growSpeed * _coolDown);
                    //overGrownTree = tree.GetIsFullyGrown();
                }

                //Grow field if other collider if field
                Field field = other.GetComponent<Field>();
                if (field != null)
                {
                    field.Grow(growSpeed * _coolDown);
                }

                Tile tile = other.GetComponent<Tile>();
                if (tile != null)
                {
                    if (Mathf.Abs(tile.GetTop().y - MyCursor.Instance.GetCursorHit().point.y) < 0.2)
                    {
                        tile.OnMiracleRain(tileHealAmount);
                    }
                }
            }

            if (Random.value < 1f / 50 && MyCursor.Instance.GetCursorHitting())
            {
                Tile currentTile = MyCursor.Instance.GetCursorHit().collider.GetComponent<Tile>();
                if (currentTile)// && overGrownTree)
                {
                    if (currentTile.GetState() >= 100)
                    {
                        SproutTree();
                    }
                }
            }
        }
    }

    public void SproutTree()
    {
        MyTree newTree = Instantiate(treePrefab, World.Instance.transform);
        if (newTree)
        {
            newTree.transform.position = MyCursor.Instance.GetCursorHit().point;

        }

    }

    public override string GetInfo()
    {
        return "RAIN MIRACLE: restores wasteland, grows trees and puts out fires. Hold use button to cast when activated.";
    }
}
