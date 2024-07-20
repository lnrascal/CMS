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
        if (isMoving)
            return;

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
        isMoving = true;
        isLifted = true;

        Tweener tween = movingPlatform.DOLocalMoveY(maxHeight, 2f);
        tween.Play().OnComplete(() =>
        {
            isMoving = false;
        });
    }

    private void Down()
    {
        isMoving = true;
        isLifted = false;
        
        Tweener tween = movingPlatform.DOLocalMoveY(minHeight, 2f);
        tween.Play().OnComplete(() =>
        {
            isMoving = false;
        });
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