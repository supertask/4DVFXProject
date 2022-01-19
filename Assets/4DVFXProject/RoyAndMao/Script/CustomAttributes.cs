using UnityEngine;


public class ButtonAttribute : PropertyAttribute
{
	public string methodName;

	public string buttonName;

	public ButtonAttribute(string methodName, string buttonName = null)
	{
		this.methodName = methodName;

		if (buttonName == null)
		{
			this.buttonName = methodName;
		}
		else
		{
			this.buttonName = buttonName;
		}

	}

}