using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class WalkerScript : MonoBehaviour
{

	public Action OnFinishWalk;
	public Action OnBetweenWalk;
	private BaseItemScript _baseItem;
	private float _speed = 5f;
	private Vector3 _targetPosition;

	private bool _isWalking = false;

	private GroundManager.Path _path;
	private int _currentNodeIndex;

	public void SetData(BaseItemScript baseItem)
	{
		this._baseItem = baseItem;
		//this._speed = baseItem.itemData.configuration.speed;
	}

    private void Update()
    {
		if (!_isWalking)
		{
			return;
		}

		WalkUpdate();


    }

	public void WalkToPosition(Vector3 position)
	{
		CancelWalk();
		WalkThePath(GroundManager.instance.GetPath(transform.localPosition, position, false));
	}

	public void CancelWalk()
	{
		this._isWalking = false;
		this._targetPosition = transform.position;
		this.OnFinishWalk = null;
		this.OnBetweenWalk = null;
	}

	private void WalkUpdate()
	{
		float frameDistance = Time.deltaTime * _speed;
		float interpolationValue = frameDistance / (_targetPosition - transform.localPosition).magnitude;
		transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPosition, interpolationValue);

		if (transform.localPosition == _targetPosition)
		{
			WalkNextNode();
		}
	}

	private void WalkNextNode()
	{
		if (_path != null && _path.nodes != null && _currentNodeIndex < _path.nodes.Length - 1)
		{
			_currentNodeIndex++;
			MoveToPosition(_path.nodes[_currentNodeIndex]);
            if (this.OnBetweenWalk != null)
            {
                this.OnBetweenWalk.Invoke();
            }
        }
		else
		{
			FinishWalk();
		}
	}

	private void FinishWalk()
	{
		_baseItem.SetState(GameData.State.IDLE);
		_isWalking = false;
		_targetPosition = transform.position;
		if (OnFinishWalk != null)
		{
			OnFinishWalk.Invoke();
		}

		OnBetweenWalk = null;
	}

	private void MoveToPosition(Vector3 position)
	{
		_isWalking = true;
		_targetPosition = position;
		this._baseItem.LookAt(position);
	}

	private void WalkThePath(GroundManager.Path path)
	{
		if (path.nodes == null || path.nodes.Length == 0)
		{
			FinishWalk();
			return;
		}

		_baseItem.SetState(GameData.State.WALK);
		_path = path;
		_currentNodeIndex = 0;
		if (path != null || path.nodes != null && path.nodes.Length > 0)
		{
			MoveToPosition(_path.nodes[0]);
		}
	}
}
