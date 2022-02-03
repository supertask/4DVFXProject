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
	protected Vector3 currentTheta; //radian(0 ~ 2 * PI)

	private float time = 0;
	
	public virtual void Start()
	{
		this.currentTheta = Vector3.zero;
	}

	public virtual void Update()
	{
		this.transform.position = SamplePosition(this.transform.position);
	}

	public virtual Vector3 SamplePosition(Vector3 position)
	{
		Quaternion rotatePosition = Quaternion.Euler(rotationEuler);

		if (this.moveType == MoveType.Circle)
		{
			currentTheta.x = time * freq.x;
			currentTheta.y = 0;
			currentTheta.z = time * freq.y;
			position = new Vector3(Mathf.Cos(currentTheta.x), currentTheta.y, Mathf.Sin(currentTheta.z) );
            position.Scale(radius); // position multiply with radius
			position = rotatePosition * position;
            position += positionOffset;
		}
		else if (this.moveType == MoveType.EightFigure)
		{
			currentTheta.x = time * freq.x * 2;
			currentTheta.y = 0;
			currentTheta.z = time * freq.y;
			position = new Vector3(Mathf.Sin(currentTheta.x), currentTheta.y, - Mathf.Sin(currentTheta.z));
            position.Scale(radius); // position multiply with radius
			position = rotatePosition * position;
            position += positionOffset;
		}
		else if (this.moveType == MoveType.LissajousCurve)
		{
			currentTheta.x = time * freq.x + lissajousOffset.x;
			currentTheta.y = time * freq.y + lissajousOffset.y;
			currentTheta.z = time * freq.z + lissajousOffset.z;
			position = new Vector3(
				Mathf.Sin(currentTheta.x),
                - Mathf.Sin(currentTheta.y),
                Mathf.Sin(currentTheta.z)
            );
            position.Scale(radius); // position multiply with radius
			position = rotatePosition * position;
            position += positionOffset;
		}
		this.time += Time.deltaTime * speed;
        
        return position;
	}

}