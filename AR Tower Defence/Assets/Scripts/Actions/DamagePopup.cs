using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    //Creates DamageIndicatorPopup in the game world.
    public static DamagePopup Create(Vector3 position,Damage damage)
    {
        Transform damagePopupTrandform = Instantiate(GameAssets.i.damageIndicatorPopup, position, Quaternion.identity);
        DamagePopup dPopup = damagePopupTrandform.GetComponent<DamagePopup>();
        dPopup.Setup(damage);
        return dPopup;
    }
    private Vector3 moveVector;
    private const float maxDissapearTimer = 1f;
    private TextMeshPro textMesh;
    private Color textColor;
    private static int sortingOrder;
    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    //Prepares TMPro setting it's position, size and position to move to as well as the color of type of the damage.

    public void Setup(Damage damage)
    {
        textMesh.SetText(((int)damage.damage).ToString());
        if (damage.isCritical)
        {
            textMesh.fontSize = 14;
            textColor = new Color32(255,138,85,255);
        }
        else if (damage.isPoison)
        {
            textMesh.fontSize = 12f;
            textColor = new Color32(95, 78, 115, 255);
        }
        else
        {
            textMesh.fontSize = 10;
            textColor = new Color32(255, 255, 255, 255);
        }

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        textMesh.color = textColor;
        dissapearTimer = maxDissapearTimer;
        moveVector = new Vector3(Random.Range(-15,15), (Random.Range(0, 15)), (Random.Range(-15, 15)));
    }
    private float dissapearTimer;
    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8 * Time.deltaTime;

        if (dissapearTimer > maxDissapearTimer*0.5)
        {
            float decreaseScale = 1f;
            transform.localScale -= Vector3.one * decreaseScale * Time.deltaTime;
        }

        //Destroys TMPro upon finishing it's time.

        dissapearTimer -= Time.deltaTime;
        if(dissapearTimer < 0)
        {
            float dissapearSpeed = 3f;
            textColor.a -= dissapearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }

        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
