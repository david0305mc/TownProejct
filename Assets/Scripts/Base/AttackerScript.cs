using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerScript : MonoBehaviour
{
    private BaseItemScript _baseItem;
    private BaseItemScript _currentTarget;
    private Vector3 _currentTargetPoint;

    public void SetData(BaseItemScript baseItem)
    {
        _baseItem = baseItem;
    }

    public void AttackNearestTarget()
    {
        BaseItemScript target = GetNearestTargetItem();
        if (target != null)
        {
            Attack(target);
        }
        else
        { 
        
        }
    }

    private void Attack(BaseItemScript target)
    {
        var outCells = target.getOutCells();
        float nearestDist = 9999f;
        foreach (var item in outCells)
        {
            float dist = Vector3.Distance(item, target.GetPosition());
            if (dist < nearestDist)
            {
                dist = nearestDist;
                _currentTargetPoint = item;
            }
        }

        _baseItem.Walker.WalkToPosition(_currentTargetPoint);
        if (_baseItem.itemData.configuration.attackRange > 0)
        {
            //_baseItem.Walker.OnBetweenWalk += checkt
        }
        else
        {
            Debug.LogError("_baseItem.itemData.configuration.attackRange > 0");
        }
        
    }

    private void CheckTargetBeyondRange()
    { 
    
    }
    

    private BaseItemScript GetNearestTargetItem()
    {
        BaseItemScript nearestTarget = null;
        float nearestDist = 9999f;
        foreach (var item in Game.SceneManager.instance.GetItemInstances())
        {
            if (item.Value == this)
                continue;

            var d = Vector3.Distance(transform.localPosition, item.Value.transform.localPosition);
            if (nearestDist < d)
            {
                nearestDist = d;
                nearestTarget = item.Value;
            }
        }

        return nearestTarget;

    }
}
