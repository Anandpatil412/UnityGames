using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
	private Vector2[] adjacentDirections = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
	public int boxState;

	private GameObject GetAdjacent(Vector2 castDir)
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, castDir);

		if (hit.collider != null)
		{
			Sprite a = hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite;
			Sprite b = gameObject.GetComponent<SpriteRenderer>().sprite;

			if(a!=null && b!= null && a == b) //Sprite compare
			{
				GameObject obj = hit.collider.gameObject;
				Box box = obj.GetComponent<Box>();

				if (box.boxState == 0)
					return obj;

				return null;
			}

			return null;
		}

		return null;
	}

	private List<GameObject> GetAllAdjacentBoxes()
	{
		List<GameObject> adjacentTiles = new List<GameObject>();

		for(int i = 0; i < adjacentDirections.Length; i++)
		{
			GameObject obj = GetAdjacent(adjacentDirections[i]);

			if(obj != null)
				adjacentTiles.Add(obj);
			
		}

		return adjacentTiles;
	}

	void FindNeighbours()
    {
		List<GameObject> adjboxes = GetAllAdjacentBoxes();

		foreach (GameObject obj in adjboxes)
		{
			Box box = obj.GetComponent<Box>();

			box.boxState = 1;
			box.FindNeighbours();

			BoxManager.instance.UpdateBoxesToClear(box);
		}
	}

	
	void OnMouseDown()
	{
		if (BoxManager.instance.gameOver) return;

		List<GameObject> adjboxes = GetAllAdjacentBoxes();
		int count = 0;

		if(adjboxes.Count > 0)
		{	
			boxState = 1;
			BoxManager.instance.UpdateBoxesToClear(this);
		}

		foreach (GameObject obj in adjboxes)
        {
			Box box = obj.GetComponent<Box>();

			box.boxState = 1;
			box.FindNeighbours();

			BoxManager.instance.UpdateBoxesToClear(box);

			count++;

			if(count == adjboxes.Count) //Last.. ready to clear boxes
            {
				//Debug.Log(" Last ");
				BoxManager.instance.ClearBoxes();

			}


		}
	}

}