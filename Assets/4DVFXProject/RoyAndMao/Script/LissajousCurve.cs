using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LissajousCurve : MonoBehaviour
{   
	public enum MoveType {
		Circle, //円
		EightFigure, //八の字
		LissajousCurve, //リサージュ曲線
	}
	[SerializeField] MoveType moveType = MoveType.Circle;
	[SerializeField] Vector3 freq = new Vector3(1,1,1);
	[SerializeField] Vector3 radius = new Vector3(3,3,3);
	[SerializeField] Vector3 positionOffset;

	[SerializeField, Range(0, 5)] float speed = 1;
    [SerializeField] Vector3 rotationEuler;
	[SerializeField] Vector3 lissajousOffset;

	private float time = 0;

	public virtual void Update()
	{
		this.transform.position = SamplePosition(this.transform.position);
	}

	public Vector3 SamplePosition(Vector3 position)
	{
		Quaternion rotatePosition = Quaternion.Euler(rotationEuler);

		if (this.moveType == MoveType.Circle)
		{
			position = new Vector3( Mathf.Cos(time * freq.x), 0, Mathf.Sin(time * freq.y) );
            position.Scale(radius); // position multiply with radius
			position = rotatePosition * position;
            position += positionOffset;
		}
		else if (this.moveType == MoveType.EightFigure)
		{
			position = new Vector3( Mathf.Sin(time * freq.x * 2), 0, - Mathf.Sin(time * freq.y) );
            position.Scale(radius); // position multiply with radius
			position = rotatePosition * position;
            position += positionOffset;
		}
		else if (this.moveType == MoveType.LissajousCurve)
		{
			position = new Vector3(
				Mathf.Sin(time * freq.x + lissajousOffset.x),
                - Mathf.Sin(time * freq.y + lissajousOffset.y),
                Mathf.Sin(time * freq.z + lissajousOffset.z)
            );
            position.Scale(radius); // position multiply with radius
			position = rotatePosition * position;
            position += positionOffset;
		}
		this.time += Time.deltaTime * speed;
        
        return position;
	}

}