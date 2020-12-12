using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveRubble : MonoBehaviour
{

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.transform.tag != "stack")
		Destroy(collision.gameObject);
	}
}
