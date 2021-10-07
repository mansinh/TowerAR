using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Block : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] Material selectedMaterial;
    [SerializeField] GameObject underBlock;
    MeshRenderer blockRenderer;
    public int type = 0;


    // Start is called before the first frame update
    void Awake()
    {
        blockRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        blockRenderer.material = material;
    }

    public void MouseOver()
    {
        //Debug.Log("Mouse over Block" + transform.position);
        blockRenderer.material = selectedMaterial;
    }

    public void MouseExit()
    {
        //Debug.Log("Mouse leaves Block" + transform.position);
        blockRenderer.material = material;
    }

    public void Raise()
    {
        transform.localPosition += Vector3.up / 2;
        UpdateBlock();
    }
    public void Lower()
    {
        if (transform.localPosition.y > -0.5)
        {
            transform.localPosition -= Vector3.up / 2;
        }
        UpdateBlock();
    }
    public void SetHeight(float height)
    {
        Vector3 position = transform.localPosition;
        position.y = height / 2 - transform.localScale.y;
        transform.localPosition = position;
        UpdateBlock();
    }
    public void UpdateBlock()
    {

        blockRenderer.gameObject.SetActive(transform.localPosition.y >= 0);

        float height = transform.localPosition.y + transform.localScale.y;


        underBlock.transform.localScale = new Vector3(1, Mathf.Min(0, -height / transform.localScale.y), 1);
    }
}
