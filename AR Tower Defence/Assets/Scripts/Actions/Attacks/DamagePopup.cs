using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] Color _normalColor = new Color32(255, 255, 255, 255);
    [SerializeField] Color _criticalColor = new Color32(255, 138, 85, 255);
    [SerializeField] Color _poisonColor = new Color32(95, 78, 115, 255);
    [SerializeField] Color _healColor = new Color32(180, 255,100, 255);

    //Creates DamageIndicatorPopup in the game world.
    public static DamagePopup Create(Transform damaged,Damage damage, bool isPoison)
    {
        Transform damagePopupTransform = Instantiate(GameAssets.Instance.damageIndicatorPopup);
        damagePopupTransform.SetParent(damaged);
        damagePopupTransform.localPosition = Vector3.up/2;

        DamagePopup dPopup = damagePopupTransform.GetComponent<DamagePopup>();
        dPopup.Setup(damage, isPoison);
        return dPopup;
    }
    private Vector3 moveVector;
    private const float maxDisappearTimer = 1f;
    private TextMeshPro textMesh;
    private Color textColor;
    private static int sortingOrder;
    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    //Prepares TMPro setting it's position, size and position to move to as well as the color of type of the damage.

    public void Setup(Damage damage, bool isPoison)
    {
        
        if (damage.isCritical)
        {
            textMesh.fontSize = 3;
            textColor = _criticalColor;
        }
        else if (damage.damage < 0)
        {
            textMesh.fontSize = 2;
            textColor = _healColor;
            damage.damage = -damage.damage;
        }
        else
        {
            textMesh.fontSize = 2;
            textColor = _normalColor;
        }

        textMesh.SetText(((int)damage.damage).ToString());
        if (isPoison)
        {
            textMesh.fontSize = 2f;
            textColor = _poisonColor;
            textMesh.SetText(((int)damage.poisonDamage).ToString());
        }

        sortingOrder++;
        textMesh.sortingOrder = sortingOrder;

        textMesh.color = textColor;
        disappearTimer = maxDisappearTimer;
        //moveVector = new Vector3(Random.Range(-15,15), (Random.Range(0, 15)), (Random.Range(-15, 15)));
        moveVector = new Vector3(0, (Random.Range(2, 5)), 0);
    }
    private float disappearTimer;
    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8 * Time.deltaTime;

        if (disappearTimer > maxDisappearTimer*0.5)
        {
            float decreaseScale = 0.5f;
            transform.localScale -= Vector3.one * decreaseScale * Time.deltaTime;
        }

        //Destroys TMPro upon finishing it's time.

        disappearTimer -= Time.deltaTime;
        if(disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }

        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }

    void OnDisable() {
        Destroy(gameObject);
    }
}
