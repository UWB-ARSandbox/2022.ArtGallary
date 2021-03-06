using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class SendAndPlayAudio : MonoBehaviour
{
    // Start is called before the first frame update
    AudioClip clip;
    AudioClip clip2;
    AudioSource audioPlayer;

    bool audioInput = false;

    int count = 0;
    int previousCount = 0;

    int hrz = 39000;

    int audioPerSecond = 40; //If changing, will need to make sure stuff in update gets called less as well;

    bool audioEnabled = false;
    GameObject microphoneIcon;
    

    // hrz/audiopersecond must be less than 1000
    void Start()
    {
        if(GameObject.Find("GameLiftManager").GetComponent<GameLiftManager>().m_PeerId == 1)
        {
            if(this.transform.parent.GetComponent<TeacherEnable>() != null)
            {
                microphoneIcon = GameObject.Find("AudioIcon");
                audioInput = true;
                clip = Microphone.Start(Microphone.devices[0], true, 1, hrz);
            }
            
        }
        else{
            if(this.transform.parent.GetComponent<StudentEnable>() != null && this.transform.parent.GetComponent<StudentEnable>().studentID == GameObject.Find("GameLiftManager").GetComponent<GameLiftManager>().m_PeerId)
            {
                microphoneIcon = GameObject.Find("AudioIcon");
                audioInput = true;
                clip = Microphone.Start(Microphone.devices[0], true, 1, hrz);
            }
        }
        audioPlayer = GetComponent<AudioSource>();
        
        clip2 = AudioClip.Create("MyMic", hrz, 1, hrz, false);
        this.GetComponent<ASL.ASLObject>()._LocallySetFloatCallback(UserDefinedFunction);
        
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(audioEnabled)
        {
            if(audioInput == true)
            {
                
                if(count < 40)
                {

            
                this.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                {
                    //Debug.Log("Sending " + count);
                    float[] x = new float[hrz / audioPerSecond];
                    clip.GetData(x, (count * hrz) / audioPerSecond);
                    this.GetComponent<ASL.ASLObject>().SendFloatArray(x); 
                });
                }
                else{
                    //Debug.Log("count is greater than 40");
                    count = 0;
                }
                
                
            }
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            audioEnabled = !audioEnabled;
            if(microphoneIcon != null)
            {
                microphoneIcon.SetActive(!audioEnabled);
            }
        }
        
        
        
        
        
    }

    public void UserDefinedFunction(string _id, float[] _f) 
    {
        
        if(count == 0)
        {
            
        }
        if(count < 40)
        {
            count++;
            if(this.transform.parent.GetComponent<TeacherEnable>() != null && !GameObject.Find("GameLiftManager").GetComponent<GameLiftManager>().AmLowestPeer())
            {
                
                clip2.SetData(_f, ((count - 1) * hrz) / audioPerSecond);
                if(count == 3)
                {
                    audioPlayer.clip = clip2;
                    audioPlayer.Play();
                    //Debug.Log("Playing");
                
                }
            }
            else if(this.transform.parent.GetComponent<StudentEnable>() != null && this.transform.parent.GetComponent<StudentEnable>().studentID != GameObject.Find("GameLiftManager").GetComponent<GameLiftManager>().m_PeerId)
            {
                
                clip2.SetData(_f, ((count - 1) * hrz) / audioPerSecond);
                if(count == 3)
                {
                    audioPlayer.clip = clip2;
                    audioPlayer.Play();
                    //Debug.Log("Playing");
                
                }
            }
            
            
        }
        else{
            
            count = 0;
            if(audioInput)
            {
                //Debug.Log("Microphone starting");
                //clip = Microphone.Start(Microphone.devices[0], true, 1, hrz);
            }
        }
        
        
    }
    /*
    IEnumerator MyCoroutine()
    {
       
        yield return new WaitForSeconds(1);
        Debug.Log(clip.samples);
    

        audioPlayer.clip = clip2;
        audioPlayer.Play();
        float[] x;
        for(int i = 0; i < 40; i++)
        {
            Debug.Log(i);
            x = new float[clip.samples / 40];
            clip.GetData(x, (i * clip.samples) / 40);
            clip2.SetData(x, (i * clip.samples) / 40);
        
            yield return new WaitForSeconds(1/40);
        }
        //audioPlayer.clip = clip;
        yield return new WaitForSeconds(2);
        
        //audioPlayer.clip = clip;
        audioPlayer.Play();
        


        
        

    }
    */
    
}
