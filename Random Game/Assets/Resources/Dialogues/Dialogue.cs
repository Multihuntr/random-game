using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class Dialogue
{

    private string name, message;
    private Sprite image;

    public Dialogue(string name, Sprite image, string message)
    {
        this.name = name;
        this.image = image;
        this.message = message;
    }

    public string getName()
    {
        return name;
    }

    public Sprite getImage()
    {
        return image;
    }

    public string getMessage()
    {
        return message;
    }


    /*Conversation format in text files is a line for each 'pane' of dialogue. It contains the character's
     * name, image filename and what they say, separated by ':' as shown below.
     * character name:character image:dialogue   
     */
    public static Dialogue[] loadConversation(TextAsset dialogueLog)
    {
        Dialogue[] conversation = null;

        string[] lines = dialogueLog.text.Split('\n');
        conversation = new Dialogue[lines.Length];

       for (int i = 0; i < lines.Length; i++)
       {
           string[] message = lines[i].Split(':');
           Sprite s = Resources.Load<Sprite>("Sprites/" + message[1]);
           conversation[i] = new Dialogue(message[0], s, message[2]);
       }

       return conversation;
        
    }
}
