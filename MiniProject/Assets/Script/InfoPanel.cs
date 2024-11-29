using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Oculus.Interaction.HandGrab; // Ensure to include this
using Oculus.Interaction; // Ensure to include this for IInteractorView
using Oculus.Interaction.Grab;
using Oculus.Interaction.GrabAPI;
using Oculus.Interaction.Input;

public class InfoPanel : MonoBehaviour
{
    [System.Serializable]
    public class PartData
    {
        public Transform Part;
        public string heading;
        public string description;
        public AudioClip narrate;
    }

    public List<PartData> partDataList;
    public AudioSource speaker; 

    public TextMeshProUGUI headingText;
    public TextMeshProUGUI descriptionText;

    private void Start()
    {
    }

    public void SetInfoText(Transform targetTransform)
    {
        foreach (PartData partData in partDataList)
        {
            if (partData.Part == targetTransform)
            {
                headingText.text = partData.heading;
                descriptionText.text = partData.description;
                speaker.Stop();    
                speaker.PlayOneShot(partData.narrate);
                return;
            }
        }

    }

    private void OnDestroy()
    {
    }
}