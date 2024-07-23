using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;
using DG.Tweening;

public class CarLift : MonoBehaviour, IInteractable
{
    [SerializeField] private Outlinable outlinable;
    
    [SerializeField] private Transform movingPlatform;
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;

    [SerializeField] private bool isLifted = false;
    [SerializeField] private bool isMoving = false;

    public void Interact(PlayerInteract player)
    {   
        //Do nothing if CarLift is already active
        if (isMoving)
            return;
        
        //Type Of Movement Based On CarLift's State
        if (isLifted)
        {
            Down();
        }
        else
        {
            Lift();
        }
    }
    
    private void Lift()
    {
        SetConditionFlags(true, true);
        MovePlatform(maxHeight);
    }
    
    private void Down()
    {
        SetConditionFlags(true, false);
        MovePlatform(minHeight);
    }

    private void MovePlatform(float endValue)
    {
        Tweener tween = movingPlatform.DOLocalMoveY(endValue, 2f);
        tween.Play().OnComplete(() =>
        {
            isMoving = false;
        });
    }

    private void SetConditionFlags(bool movement, bool lifted)
    {
        isMoving = movement;
        isLifted = lifted;
    }

    public void Highlight(bool toHighlight)
    {
        outlinable.enabled = toHighlight;
    }

    public void Drop()
    {
        return;
    }
}