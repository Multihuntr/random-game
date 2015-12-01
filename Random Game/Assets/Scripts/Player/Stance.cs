using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

public class Stance : MonoBehaviour
{
	static BaseStance currStance;

	public GameObject stanceChangeBurstObj;
	public Text stanceText;

	bool cycled = false;

	void Start ()
	{
		BeserkerStance beserker = new BeserkerStance ();
		KnightStance knight = new KnightStance ();
		TricksterStance trickster = new TricksterStance ();

		beserker.next = knight;
		beserker.prev = trickster;
		knight.next = trickster;
		knight.prev = beserker;
		trickster.next = beserker;
		trickster.prev = knight;

		setStance (beserker);
	}

	void Update ()
	{
		if (!cycled) {
			if (Input.GetAxis ("CycleStance") > 0) {
				setStance (currStance.next);
			} else if (Input.GetAxis ("CycleStance") < 0) {
				setStance (currStance.prev);
			}
		}
		cycled = (Input.GetAxis ("CycleStance") != 0);
	}

	void setStance (BaseStance toSet)
	{
		currStance = toSet;
		stanceText.color = currStance.colour;
		stanceText.text = currStance.text;

		// Look at the pretty explosions
		GameObject aBurst = (GameObject)(Instantiate (stanceChangeBurstObj, transform.position, Quaternion.identity));
		StanceChangeBurst scBurst = aBurst.GetComponent<StanceChangeBurst> ();
		scBurst.burst (currStance.colour);
	}

	class BaseStance
	{
		public Color colour;
		public string text;
		public BaseStance next;
		public BaseStance prev;
	}

	class BeserkerStance:BaseStance
	{
		public BeserkerStance ()
		{
			colour = new Color (0.8823f, 0.1216f, 0);
			text = "Beserker";
		}
	}

	class KnightStance:BaseStance
	{		
		public KnightStance ()
		{
			colour = new Color (0.2549f, 0.6588f, 0.9098f);
			text = "Knight";
		}
	}

	class TricksterStance:BaseStance
	{		
		public TricksterStance ()
		{
			colour = new Color (0.1725f, 0.7216f, 0.2706f);
			text = "Trickster";
		}
	}
}