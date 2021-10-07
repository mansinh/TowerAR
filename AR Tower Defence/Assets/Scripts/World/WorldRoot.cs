using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldRoot : MonoBehaviour
{
    public static WorldRoot instance;

    NavController navController;
    EnemyController enemyController;
  
    public int size = 40;  
    [SerializeField] Block[] blockPrefabs;


    private void Awake()
    {
        instance = this;
        for (int i = 0; i < blockPrefabs.Length;i++) {
            blockPrefabs[i].type = i;
        }
    }

    public void Init()
    {
        navController = FindObjectOfType<NavController>();
       

        //enemyController = FindObjectOfType<EnemyController>();
        //enemyController.Init(levels, height, this);
    }
    public void Refresh() {
        navController.Bake();
        NavMeshAgent[] agents = FindObjectsOfType<UnityEngine.AI.NavMeshAgent>();
        foreach (NavMeshAgent agent in agents)
        {
            //agent.ResetPath();            
        }
    }

    [SerializeField] List<Block> blocks = new List<Block>();
    public void Generate()
    {
        foreach (Block block in blocks)
        {
            if (block != null)
            {
                DestroyImmediate(block.gameObject);
            }
        }
        blocks = new List<Block>();

        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                Block block = Instantiate(blockPrefabs[0], transform);
                block.transform.localPosition = new Vector3(x - size / 2, -block.transform.localScale.y, z - size / 2);
                blocks.Add(block);
            }
        }
    }

    public Block ChangeBlock(Block blockEditing, bool next) {
        int type = blockEditing.type;
        if (next)
        {
            type++;
            if (type >= blockPrefabs.Length)
            {
                type = 0;
            }
        }
        else {
            type--;
            if (type < 0)
            {
                type = blockPrefabs.Length-1;
            }
        }
        int index = blocks.IndexOf(blockEditing);
        Block newBlock = Instantiate(blockPrefabs[type], transform);
        newBlock.transform.position = blockEditing.transform.position;
        newBlock.transform.eulerAngles = blockEditing.transform.eulerAngles;
        newBlock.transform.localScale = blockEditing.transform.localScale;

        blocks[index] = newBlock;
        DestroyImmediate(blockEditing.gameObject);

        newBlock.UpdateBlock();
        return newBlock;
    }
}
