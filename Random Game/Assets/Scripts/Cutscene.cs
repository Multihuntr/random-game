using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour {

    public Dialogue[] conversation;
    public Canvas cutsceneView;
    public TextAsset dialogueLog;
    
    private int index = 0;
    private int hasActivated = -1; //-1, not yet activated, 0 running, 1 finished

	// Use this for initialization
	void Start () {
        conversation = Dialogue.loadConversation(dialogueLog);
	}

    public Vector2 getPos()
    {
        return (Vector2)transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && hasActivated == -1)
        {
            //Turn on cutscene ui, disable other ui
            cutsceneView.enabled = true;
            GameObject.Find("HealthBar").GetComponent<Canvas>().enabled = false;

            //Pause game and switch to cutscene state
            GameState.togglePause();
            GameState.inCutscene = true;
            hasActivated = 0;
            index = 0;
            this.advanceText();
        }
    }

    private void advanceText()
    {
        //Check if dialogue is finished or not
        if (index != conversation.Length)
        {
            //Update to next conversation pane
            Text[] temp = cutsceneView.GetComponentsInChildren<Text>();
            temp[0].text = conversation[index].getName();
            temp[1].text = conversation[index].getMessage();

            Image tempImage = cutsceneView.GetComponentInChildren<Image>();
            tempImage.sprite = conversation[index].getImage();

            index++;
        }
        else
        {
            //end cutscene
            cutsceneView.enabled = false;
            GameObject.Find("HealthBar").GetComponent<Canvas>().enabled = true;

            GameState.togglePause();
            GameState.inCutscene = false;
            hasActivated = 1;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (hasActivated == 0)
        {
            //Check if user swaps to next dialogue frame
            if (Input.GetButtonDown("Fire1"))
            {
                this.advanceText();
            }
        }
	
	}
}
