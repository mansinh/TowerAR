using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : Agent
{
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] Sprite standing, spear, spearThrust, handsUp;

    protected override void Init()
    {
        base.Init();
        Perception.CheckLine = false;
    }
    
    protected override void Act()
    {
        if (CurrentTarget)
        {
            if (CurrentTarget.GetComponent<Barracks>())
            {
                
            }
            else
            {
                _view.gameObject.SetActive(true);
                base.Act();
                State = AgentState.Action0;
            }
        }
        Animate();
    }
    protected override void LookAround()
    {
        base.LookAround();
        if (CurrentTarget)
        {
            if (World.Instance.GetTile(transform.position).GetState() < 100)
            {
                SetTarget(DefaultTarget, 0.2f);
            }
        }
    }

 
    float timeSinceLastFrame = 0;
    float frameTime = 0.5f;
    void Animate()
    {
        sprite.flipX = Vector3.Dot(transform.forward, Camera.main.transform.right) >= 0;


        switch (State)
        {
            case AgentState.Running: RunAnimation(); break;
            case AgentState.Action0: BuildingAnimation(); break;
            case AgentState.Idling: IdleAnimation(); break;
        }
        timeSinceLastFrame += Time.deltaTime;
    }

    void RunAnimation()
    {
        _view.transform.localPosition = new Vector3(0, Mathf.Sin(Time.time * 50) / 40, 0);
    }
    void BuildingAnimation()
    {

        if (timeSinceLastFrame < frameTime / 2)
        {
            sprite.sprite = spear;
        }
        else if (timeSinceLastFrame < frameTime)
        {
            sprite.sprite = spearThrust;
        }
        else
        {
            timeSinceLastFrame = 0;
        }
    }
    void IdleAnimation()
    {
        sprite.sprite = standing;
    }
}
