using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.IO;
using UnityEngine.EventSystems;



public class CanvasController : MonoBehaviour
{
    // Default displayed buttons on startup
    public GameObject airConditionerButton; 
    public GameObject GPSButton; 
    public GameObject radioButton;
    public GameObject mediaButton;

   // AC Main Controls
    public GameObject temperatureButton;
    public GameObject fanSpeedButton;
   
   //Temperature Controls
   public GameObject temperatureSlider;
   public GameObject increaseTemperatureButton;
   public GameObject decreaseTemperatureButton;
   public Image temperatureFill;

   //fanSpeed Controls
   public GameObject fanSpeedSlider;
   public GameObject increasefanSpeedButton;
   public GameObject decreasefanSpeedButton;
   public Image fanSpeedFill;
   
   // Radio Controls 
   public GameObject volumeSlider;
   public GameObject increaseVolumeButton;
   public GameObject decreaseVolumeButton;
   public GameObject nextButton;
   public GameObject previousButton;
   public GameObject playButton;
   public Image volumeSliderFill;
   public AudioClip[] audioClips;
   public GameObject[] channelButtons;
   private AudioSource audioSource;
   private int currentClipIndex = -1;
   private bool isPlaying = false;
   public GameObject radioVolumeButton;
   public GameObject channelsSelectionButton;

   // Media Player Controls
   public GameObject mediaVolumeSlider;
   public GameObject mediaIncreaseVolumeButton;
   public GameObject mediaDecreaseVolumeButton;
   public GameObject mediaPlayButton;
   public Image mediaVolumeFill;
   public List<string> mediaFiles;
   private VideoPlayer videoPlayer;
   public RectTransform  mediaButtonsContainer; // the arc slider
   private int currentBatch = 0;
   public GameObject nextBatchButton;
   public GameObject previousBatchButton;

   private List<Button> mediaButtons = new List<Button>();
   private int currentMediaIndex = -1;
   private bool mediaIsPlaying = false;
   private bool buttonsCreated = false;

   // GPS Controls
   public GameObject GPSImage;
   [SerializeField] public Image imageContainer;
   public Sprite[] images;
   private int currentImageIndex = 0;

   // Back Button Controls
   public GameObject backButton;
   private string currentSystem;

    // Communication
    public UDP udpReceive;
    private bool sameData = false;
    private string tempData;
    private string currentlySelected ="AC";
    private int selectedRadioChannel = 0;
    private int selectedMediaFile = 0;

   //selection 
   private bool isButtonSelected = false;


    void Start()
    {
        // Get the audio source component
        audioSource = GetComponent<AudioSource>();
        videoPlayer = GetComponent<VideoPlayer>();
        tempData =  udpReceive.data;
        SetupControls();
        airConditionerButton.GetComponent<Button>().Select();

    }

    void FixedUpdate()
    {
        if (isPlaying)
        {
            // Check if the current audio clip has finished playing
            if (!audioSource.isPlaying)
            {
                // Move to the next audio clip in the list
                currentClipIndex++;
                if (currentClipIndex >= audioClips.Length)
                {
                    currentClipIndex = 0;
                }

                // Set the next audio clip to play
                audioSource.clip = audioClips[currentClipIndex];

                // Play the audio clip
                audioSource.Play();
            }
        }
        //Start Of Communication ..
        string data = udpReceive.data;          
        if(tempData == data)
        {
            sameData = true;
        }
        else
        {
            sameData = false;
        }
        if(!sameData)
        {
        //will handle the logic of gesturs here by calling the functions defined below.. 
        //First Menu Logic

        if(currentlySelected =="AC") 
        {
         
            if(data == "Confirm")
            {
                ShowAcControls();
                temperatureButton.GetComponent<Button>().Select();
                currentlySelected = "Temperature";
            }
            if(data == "2") //swipe right
            {
                GPSButton.GetComponent<Button>().Select();
                currentlySelected = "GPS";
            }
            if (data == "4") //swipe left
            {
                airConditionerButton.GetComponent<Button>().Select();
                currentlySelected = "AC";
            }
 
        }
        else if(currentlySelected == "GPS") 
        {
            if(data == "Confirm")
            {
                StartDisplayingImages();
                currentlySelected = "GPS Container";
            }
            if(data == "2") //swipe right
            {
                mediaButton.GetComponent<Button>().Select();
                currentlySelected = "Media";
            }
            if (data == "4") //swipe left
            {
                airConditionerButton.GetComponent<Button>().Select();
                currentlySelected = "AC";
            }              
        }
        else if(currentlySelected == "Media") 
        {
            if(data == "Confirm")
            {
                showMediaControls();
            }
            if(data == "2") //swipe right
            {
                radioButton.GetComponent<Button>().Select();
                currentlySelected = "Radio";
            }
            if (data == "4") //swipe left
            {
                GPSButton.GetComponent<Button>().Select();
                currentlySelected = "GPS";
            }             
        }
        else if (currentlySelected == "Radio")
        {
            if(data == "Confirm")
            {
                radioVolumeButton.GetComponent<Button>().Select();
                showRadioControls();
                currentlySelected = "RadioVolume";
            }
            if(data == "2") //swipe right
            {
                radioButton.GetComponent<Button>().Select();
                currentlySelected = "Radio";
            }
            if (data == "4") //swipe left
            {
                mediaButton.GetComponent<Button>().Select();
                currentlySelected = "Media";
            }    
        }
        //End of first Menu Logic /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Start of AC Handling ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        else if(currentSystem == "AC")
        {
            if(currentlySelected == "Temperature") 
            {
                if(data == "2") //swipe right
                {
                    currentlySelected = "Fan";
                    fanSpeedButton.GetComponent<Button>().Select();   
                }
                else if (data == "Confirm")
                {
                    ShowTemperatureControls();
                }
            }
            else if (currentlySelected == "Fan")
            {
                if(data == "4") //swipe left
                {
                    currentlySelected = "Temperature";
                    temperatureButton.GetComponent<Button>().Select();   
                }
                else if (data == "Confirm")
                {
                    ShowFanSpeedControls();
                }
            }
        }

        else if(currentSystem == "Temperature")
        {

            if(data == "2") //swipe right 
            {
                increaseTemp();
            }
            else if(data == "4") //swipe left 
            {
                decreaseTemperature();
            }
        }
        else if(currentSystem == "Fan")
        {

            if(data == "2") //swipe right 
            {
                IncreaseFanSpeed();
            }
            else if(data == "4") //swipe left 
            {
                DecreaseFanSpeed();
            }
        }
    // END of AC Handling ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //Start of Radio Handling //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        else if(currentSystem == "Radio")
        {
            if(currentlySelected == "RadioVolume") 
            {
                if(data == "2") //swipe right 
                {
                    currentlySelected="RadioChannels";
                    channelsSelectionButton.GetComponent<Button>().Select(); 
                }
                if(data =="Confirm") 
                {
                    showRadioVolume();
                }
            }
            else if(currentlySelected == "RadioChannels")
            {
                if(data == "4") //swipe right 
                {
                    currentlySelected="RadioVolume";
                    radioVolumeButton.GetComponent<Button>().Select(); 
                }
                if(data =="Confirm") 
                {
                    channelButtons[selectedRadioChannel].GetComponent<Button>().Select();
                    showChannels();
                }
            }
        }
        else if(currentSystem == "RadioVolume")
        {
            if(data == "2") //swipe right
            {
                IncreaseVolume();
            }
            else if(data =="4") //swipe left
            {
                DecreaseVolume();
            }
        }
        else if(currentSystem == "RadioChannels") 
        {
            if(selectedRadioChannel == 0)
            {
                if(data == "2") //swipe right
                {
                    selectedRadioChannel = 1 ;
                    channelButtons[selectedRadioChannel].GetComponent<Button>().Select();
                }
                else if(data == "Confirm")
                {
                    PlayFirstChannel();
                }
            }
            else if(selectedRadioChannel == 1){
                if(data == "2") //swipe right
                {
                    selectedRadioChannel =2;
                    channelButtons[selectedRadioChannel].GetComponent<Button>().Select();
                }
                if(data == "4") //swipe left
                {
                    selectedRadioChannel = 1 ;
                    channelButtons[selectedRadioChannel].GetComponent<Button>().Select();
                }
                if(data == "Confirm")
                {
                    PlaySecondChannel();
                }
            }
            else if(selectedRadioChannel == 2){
                if(data == "2") //swipe right
                {
                    selectedRadioChannel =3;
                    channelButtons[selectedRadioChannel].GetComponent<Button>().Select();
                }
                if(data == "4") //swipe left
                {
                    selectedRadioChannel = 2 ;
                    channelButtons[selectedRadioChannel].GetComponent<Button>().Select();
                }
                if(data == "Confirm")
                {
                    PlayThirdChannel();
                }
            }
            else if(selectedRadioChannel == 3){
                if(data == "4") //swipe left
                {
                    selectedRadioChannel = 2 ;
                    channelButtons[selectedRadioChannel].GetComponent<Button>().Select();
                }
                if(data == "Confirm")
                {
                    PlayFourthChannel();
                }
            }
            
        }
        else if(currentSystem == "RadioChannel")
        {
            if(data == "2") //swipe right
            {
                IncreaseVolume();
            }
            else if(data == "4") //swipe left
            {   
                DecreaseVolume();
            }
            else if (data == "Next")
            {
                            print("here");

                if (selectedRadioChannel == 1)
                {
                    selectedRadioChannel = 2;
                    PlaySecondChannel();
                }
                else if(selectedRadioChannel == 2)
                {
                    selectedRadioChannel =3;
                    PlayThirdChannel();
                }
                else if(selectedRadioChannel == 3)
                {
                    selectedRadioChannel =4;
                    PlayFourthChannel();
                }
                else 
                {
                    selectedRadioChannel = 1;
                    PlayFirstChannel();
                }
            }
            else if (data == "Confirm")
            {
                TogglePlaying();
            }
            
        }
        //END of Radio handling ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Start of Media Handling //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        else if(currentSystem == "Media") 
        {
            if(selectedMediaFile == 0) 
            {
                if(data == "2") //swipe right
                {
                    selectedMediaFile=1;
                    mediaButtons[selectedMediaFile].Select();
                }
                else if(data =="Confirm")
                {
                    OnMediaButtonClick(mediaButtons[selectedMediaFile]);
                }
            }
            else if (selectedMediaFile == 1 ||selectedMediaFile == 2)
            {
                if(data == "2") //swipe right
                {
                    selectedMediaFile +=1;
                    mediaButtons[selectedMediaFile].Select();
                }
                else if(data == "4") //swipe left
                {
                    selectedMediaFile -=1;
                    mediaButtons[selectedMediaFile].Select();
                }
                else if(data =="Confirm")
                {
                    OnMediaButtonClick(mediaButtons[selectedMediaFile]);
                }
            }
            else if (selectedMediaFile == 3)
            {
                if(data == "2") //swipe right
                {
                    selectedMediaFile +=1;
                    mediaButtons[selectedMediaFile].Select();
                    OnNextBatchClick();
                }
                else if(data == "4") //swipe left
                {
                    selectedMediaFile -=1;
                    mediaButtons[selectedMediaFile].Select();
                }
                else if(data =="Confirm")
                {
                    OnMediaButtonClick(mediaButtons[selectedMediaFile]);
                }
            }
            else if (selectedMediaFile == 4)
            {
                if(data == "2") //swipe right
                {
                    selectedMediaFile +=1;
                    mediaButtons[selectedMediaFile].Select();
                }
                else if(data == "4") //swipe left
                {
                    selectedMediaFile -=1;
                    mediaButtons[selectedMediaFile].Select();
                    OnPreviousBatchClick();
                }
                else if(data =="Confirm")
                {
                    OnMediaButtonClick(mediaButtons[selectedMediaFile]);
                }
            }
            else if (selectedMediaFile == 5)
            {
                if(data == "4") //swipe left
                {
                    selectedMediaFile -=1;
                    mediaButtons[selectedMediaFile].Select();
                }
                else if(data =="Confirm")
                {
                    OnMediaButtonClick(mediaButtons[selectedMediaFile]);
                }
            }
        }
        else if (currentSystem == "MediaFile")
        {
            if(data == "2") //swipe right
            {
                OnIncreaseVolumeButtonClick();
            }
            else if(data == "4") //swipe left
            {
                OnDecreaseVolumeButtonClick();
            }
        }
        //End of Media Handling ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if(data == "Back")
        {
            backButtonOnClick();
        }
        tempData = data;
        }
    }
    // onclick functions to traverse those lists

    //Start of the AC Controls..
    public void ShowAcControls()
    {
        SetupControls();
        //display the AC controls 
        temperatureButton.SetActive(true);  
        fanSpeedButton.SetActive(true); 
       

        currentSystem =  "AC";
    }

   public void ShowTemperatureControls()
   {
        SetupControls();
        //show the temperature controls
        temperatureSlider.SetActive(true);
        increaseTemperatureButton.SetActive(true);
        decreaseTemperatureButton.SetActive(true);
        
        currentSystem = "Temperature";
   }

   //onclick functions for the 2 buttons
   public void increaseTemp()
   {
    if(temperatureFill.fillAmount <= 0.9)
    {
        temperatureFill.fillAmount += 0.05f; // increase fill by 10%
    }
   }

   public void decreaseTemperature()
   {
    if(temperatureFill.fillAmount >= 0.15 )
    {
        temperatureFill.fillAmount -= 0.05f; // decrease fill by 10%
    }
   } 
    // END of temperature controls//////////////////////////////////////////////////////

    //Start of the FanSpeed Controls..
   public void ShowFanSpeedControls()
   {
        SetupControls();
        //show the FanSpeed controls
        fanSpeedSlider.SetActive(true);
        increasefanSpeedButton.SetActive(true);
        decreasefanSpeedButton.SetActive(true);

        currentSystem="Fan";
   }
    //onclick functions for the 2 buttons
   public void IncreaseFanSpeed()
   {
    if(fanSpeedFill.fillAmount <= 0.9)
    {
         fanSpeedFill.fillAmount += 0.05f; // increase fill by 10%
    }
   }

   public void DecreaseFanSpeed()
   {
    if(fanSpeedFill.fillAmount >= 0.15 )
    {
         fanSpeedFill.fillAmount -= 0.05f; // decrease fill by 10%
    }
   } 
    // END of FanSpeed controls//////////////////////////////////////////////////////
    // END of AC controls///////////////////////////////////////////////////////////

    //Start of the Radio Controls ..

    public void showRadioControls () 
    {
        SetupControls();
        channelsSelectionButton.SetActive(true);
        radioVolumeButton.SetActive(true);
        currentSystem = "Radio";
    }
    public void showChannels ()
    {
        SetupControls();
        for(int i = 0; i < channelButtons.Length ; i++)
        {
            channelButtons[i].SetActive(true);
        }
       currentSystem = "RadioChannels";
    }

    public void showRadioVolume ()
    {
        SetupControls();
        volumeSlider.SetActive(true);
        increaseVolumeButton.SetActive(true);
        decreaseVolumeButton.SetActive(true);
        nextButton.SetActive(true);
        previousButton.SetActive(true);
        playButton.SetActive(true);

        currentSystem = "RadioVolume";
    }

    public void PlayFirstChannel () 
    {
        SetupControls();
        if(currentClipIndex != 0) 
        {
            isPlaying = true;
            currentClipIndex = 0;
            // Set the audio clip to play
            audioSource.clip = audioClips[currentClipIndex];
            // Play the audio clip
            audioSource.Play();
        }
        //display the Radio controls 
        volumeSlider.SetActive(true);  
        increaseVolumeButton.SetActive(true); 
        decreaseVolumeButton.SetActive(true); 
        nextButton.SetActive(true);
        previousButton.SetActive(true);
        playButton.SetActive(true);

        //hide the channels
        for(int i = 0; i < channelButtons.Length; i++)
        {
            channelButtons[i].SetActive(false);
        }
        currentSystem="RadioChannel";
    }
        public void PlaySecondChannel () 
    {
        SetupControls();
        if(currentClipIndex != 1) 
        {
            isPlaying = true;
            currentClipIndex= 1;
            // Set the audio clip to play
            audioSource.clip = audioClips[currentClipIndex];
            // Play the audio clip
            audioSource.Play();
        }
        //display the Radio controls 
        volumeSlider.SetActive(true);  
        increaseVolumeButton.SetActive(true); 
        decreaseVolumeButton.SetActive(true); 
        nextButton.SetActive(true);
        previousButton.SetActive(true);
        playButton.SetActive(true);

        //hide the channels
        for(int i = 0; i < channelButtons.Length; i++)
        {
            channelButtons[i].SetActive(false);
        }
        currentSystem="RadioChannel";
        
    }
        public void PlayThirdChannel () 
    {
        SetupControls();
        if(currentClipIndex != 2) 
        {
            isPlaying = true;
            currentClipIndex = 2;
            // Set the audio clip to play
            audioSource.clip = audioClips[currentClipIndex];
            // Play the audio clip
            audioSource.Play();
            
        }
       
        //display the Radio controls 
        volumeSlider.SetActive(true);  
        increaseVolumeButton.SetActive(true); 
        decreaseVolumeButton.SetActive(true); 
        nextButton.SetActive(true);
        previousButton.SetActive(true);
        playButton.SetActive(true);

        //hide the channels
        for(int i = 0; i < channelButtons.Length; i++)
        {
            channelButtons[i].SetActive(false);
        }
        currentSystem="RadioChannel";

    }
        public void PlayFourthChannel () 
    {
        SetupControls();
        if(currentClipIndex != 3) 
        {
            isPlaying = true;
            currentClipIndex = 3;
            // Set the audio clip to play
            audioSource.clip = audioClips[currentClipIndex];
            // Play the audio clip
            audioSource.Play();
        }
        //display the Radio controls 
        volumeSlider.SetActive(true);  
        increaseVolumeButton.SetActive(true); 
        decreaseVolumeButton.SetActive(true); 
        nextButton.SetActive(true);
        previousButton.SetActive(true);
        playButton.SetActive(true);

        //hide the channels
        for(int i = 0; i < channelButtons.Length; i++)
        {
            channelButtons[i].SetActive(false);
        }
        currentSystem="RadioChannel";

    }
    //onclick functions for the 2 buttons
    public void IncreaseVolume()
   {
    //only for display purposes
    if(volumeSliderFill.fillAmount <= 0.85)
    {
         volumeSliderFill.fillAmount += 0.09f; // increase fill by 10%
         audioSource.volume += 0.1f;

    }
   }

   public void DecreaseVolume()
   {
    //only for display purposes
    if(volumeSliderFill.fillAmount >= 0.10 )
    {
         volumeSliderFill.fillAmount -= 0.09f; // decrease fill by 10%
         audioSource.volume -= 0.1f;
    }
   } 
  
  public void TogglePlaying()
    {
        if (!isPlaying)
        {
            isPlaying = true;

            // Set the first audio clip to play
            audioSource.clip = audioClips[currentClipIndex];

            // Play the audio clip
            audioSource.Play();
        }
        else
        {
            isPlaying = false;

            // Pause the audio clip
            audioSource.Pause();
        }
    }

    public void NextTrack()
    {
        isPlaying = true;
        currentClipIndex++;
        if (currentClipIndex >= audioClips.Length)
        {
            currentClipIndex = 0;
        }
        audioSource.clip = audioClips[currentClipIndex];
        audioSource.Play();
    }

    public void PreviousTrack()
    {
        isPlaying = true;
        currentClipIndex--;
        if (currentClipIndex < 0)
        {
            currentClipIndex = audioClips.Length - 1;
        }

        audioSource.clip = audioClips[currentClipIndex];
        audioSource.Play();
    }
    //end of of radio controls///////////////////////////////////////////////////////////////////////////////////////////////

    //Start of Media Controls
     public void showMediaControls () 
    {
        SetupControls();
        //show the list media controls..
        nextBatchButton.SetActive(true);
        previousBatchButton.SetActive(true);
    
        if(!buttonsCreated) 
        {
            // Get all audio files in the Resources folder
            var audioClips = Resources.LoadAll<AudioClip>("").ToArray();
            var audioFiles = audioClips.Select(x => x.name).ToList();

            // Get all video files in the Resources folder
            var videoClips = Resources.LoadAll<UnityEngine.Video.VideoClip>("").ToArray();
            var videoFiles = videoClips.Select(x => x.name).ToList();

            // Combine the audio and video files into one list
            mediaFiles = new List<string>();
            mediaFiles.AddRange(audioFiles);
            mediaFiles.AddRange(videoFiles);
            var counter = 0; // will be used to put the buttons in their correct position

            // Create a button for each media file
            for (int i = 0; i < mediaFiles.Count; i++)
            {
            var buttonObject = new GameObject("MediaButton"+i);
            var buttonRectTransform = buttonObject.AddComponent<RectTransform>();
            buttonRectTransform.sizeDelta = new Vector2(110, 100);
            buttonObject.transform.SetParent(mediaButtonsContainer.transform, false);
            //switch case to place the button according to its number
            switch(counter)
            {
                case 0:
                    buttonObject.transform.localPosition = new Vector3(-450, -160, 0);
                    break;
                case 1:
                    buttonObject.transform.localPosition = new Vector3(-256, -40, 0);
                    break;
                case 2:
                    buttonObject.transform.localPosition = new Vector3(83, -40, 0);
                    break;
                default: 
                    buttonObject.transform.localPosition = new Vector3(285f, -160, 0);
                    counter=0;
                    break;
            }
            counter += 1;
            var image = buttonObject.AddComponent<Image>();
            var images = Resources.Load<Sprite>("MediaButton"+i);
            image.sprite = images;
            image.color = Color.white;

            var button = buttonObject.AddComponent<Button>();
            // Get the Selectable component of the button

            // Convert the hex color value to a Color object
            Selectable selectable = button.GetComponent<Selectable>();
            ColorBlock colorBlock = selectable.colors;
            ColorUtility.TryParseHtmlString("#EECC33", out Color selectedColor);
            //colorBlock.highlightedColor = selectedColor;
            //colorBlock.pressedColor = selectedColor;
            colorBlock.selectedColor = selectedColor;
            // Assign the modified ColorBlock back to the Selectable component
            selectable.colors = colorBlock;

                    // Disable keyboard navigation for the button
                    Navigation navigation = button.navigation;
                    navigation.mode = Navigation.Mode.None;
                    button.navigation = navigation;
            //button.onClick.AddListener(() => OnMediaButtonClick(button));

            // var textObject = new GameObject("MediaButtonText");
            // var textRectTransform = textObject.AddComponent<RectTransform>();
            // textRectTransform.sizeDelta = new Vector2(200, 30);
            // textObject.transform.SetParent(buttonObject.transform, false);
            // textRectTransform.anchoredPosition = new Vector2(0, 0);

            // var text = textObject.AddComponent<Text>();
            // text.text = mediaFiles[i];
            // text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            // text.alignment = TextAnchor.MiddleCenter;
            // text.color = Color.black;

            mediaButtons.Add(button);
        }
            for (int i = 4; i < mediaButtons.Count; i++)
            {
                mediaButtons[i].gameObject.SetActive(false);
            }
            mediaButtons[0].Select();
            buttonsCreated = true;
        }
        else
        {
            //show the media buttons 
             for (int i = 0; i < 4; i++)
            {
                mediaButtons[i].gameObject.SetActive(true);
            }
            mediaButtons[0].Select();

        }
      
        currentSystem = "Media";
        currentlySelected = "";
    }

    public void OnNextBatchClick()
    {
        // Hide the current set of buttons
        for (int i = currentBatch; i < currentBatch + 4; i++)
        {
            if (i < mediaButtons.Count)
            {
                mediaButtons[i].gameObject.SetActive(false);
            }
        }

        // Show the next set of buttons
        currentBatch += 4;
        if (currentBatch >= mediaButtons.Count)
        {
            currentBatch = 0;
        }
        for (int i = currentBatch; i < currentBatch + 4 && i < mediaButtons.Count; i++)
        {
            mediaButtons[i].gameObject.SetActive(true);
        }
    }

    public void OnPreviousBatchClick()
    {
        // Hide the current set of buttons
        for (int i = currentBatch; i < currentBatch + 4; i++)
        {
            if (i < mediaButtons.Count)
            {
                mediaButtons[i].gameObject.SetActive(false);
            }
        }

        // Show the previous set of buttons
        currentBatch -= 4;
        if (currentBatch < 0)
        {
            currentBatch = 0;
        }
    
        for (int i = currentBatch; i < currentBatch + 4 && i < mediaButtons.Count; i++)
        {
            mediaButtons[i].gameObject.SetActive(true);
        }
    }

    private void OnMediaButtonClick(Button button)
    {
        SetupControls();
        //show the controls
        mediaVolumeSlider.SetActive(true);
        mediaIncreaseVolumeButton.SetActive(true);
        mediaDecreaseVolumeButton.SetActive(true);
        mediaPlayButton.SetActive(true);

        //hide the media buttons 
        foreach (Button btn in mediaButtons)
        {
            btn.gameObject.SetActive(false);
        }
        int temp = currentMediaIndex;
        // Get the index of the clicked button
      
        currentMediaIndex = mediaButtons.IndexOf(button);
        if(temp != currentMediaIndex)
        {
        audioSource.clip = null;
        videoPlayer.clip = null;    
        // Load the corresponding media file
        var mediaFile = mediaFiles[currentMediaIndex];
        var audioClip = Resources.Load<AudioClip>(mediaFile);
        var videoClip = Resources.Load<UnityEngine.Video.VideoClip>(mediaFile);

        // Play the audio or video file depending on the file type
        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        if (videoClip != null)
        {
            videoPlayer.clip = videoClip;
            videoPlayer.Play();
        }

        mediaIsPlaying = true;
        }
        currentSystem = "MediaFile" ;
    }

    public void OnIncreaseVolumeButtonClick()
    {
        if (audioSource.clip != null)
        {
                if(mediaVolumeFill.fillAmount <= 0.85)
            {
                    mediaVolumeFill.fillAmount += 0.09f; // increase fill by 10%
                    audioSource.volume += 0.1f;
            }
        }
            else if (videoPlayer.clip != null)
        {
                if(mediaVolumeFill.fillAmount <= 0.85)
            {
                    mediaVolumeFill.fillAmount += 0.09f; // increase fill by 10%
                    // Get the audio source of the video player
                    AudioSource audioSource = videoPlayer.GetTargetAudioSource(0);

                    // Decrease the audio volume by 0.1
                    audioSource.volume += 0.1f;
            }
        }
    }

    public void OnDecreaseVolumeButtonClick()
    {
        if(audioSource.clip != null)
        {
            if(mediaVolumeFill.fillAmount >= 0.10 )
            {
                mediaVolumeFill.fillAmount -= 0.09f; // decrease fill by 10%
                audioSource.volume -= 0.1f;
            }
        }
         else if (videoPlayer.clip != null)
        {
            if(mediaVolumeFill.fillAmount >= 0.10)
            {
                mediaVolumeFill.fillAmount -= 0.09f; // decrease fill by 10%
                // Get the audio source of the video player
                AudioSource audioSource = videoPlayer.GetTargetAudioSource(0);

                // Decrease the audio volume by 0.1
                audioSource.volume -= 0.1f;
            }
        }
    }
    public void ToggleMediaPlaying () 
    {
        // Toggle the play mode of the current media file
        var mediaFile = mediaFiles[currentMediaIndex];
        var audioClip = Resources.Load<AudioClip>(mediaFile);
        var videoClip = Resources.Load<UnityEngine.Video.VideoClip>(mediaFile);

        if (audioClip != null)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
            else
            {
                audioSource.Play();
            }
        }
        else if (videoClip != null)
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
            }
            else
            {
                videoPlayer.Play();
            }
        }
    }
    //end of the media functions/////////////////////////////////////////////////////////////////////////////

    // start of gps functions
    IEnumerator DisplayImages(Sprite[] images)
    {
        while (true)
        {
            // Set the image sprite to the current image
            imageContainer.sprite =(Sprite) images[currentImageIndex];

            // Wait for 6 seconds before displaying the next image
            yield return new WaitForSeconds(6f);

            // Increment the image index, and loop back to the beginning if necessary
            currentImageIndex++;
            if (currentImageIndex >= images.Length)
            {
                currentImageIndex = 0;
            }
        }
    }

    public void StartDisplayingImages()
    {
        SetupControls();
        GPSImage.SetActive(true);
        StartCoroutine(DisplayImages(images));
        currentSystem= "GPS";
    }
    // end of gps functions.

    //Helper and Navigation Functions..
    public void backButtonOnClick ()
    {
        SetupControls();
        switch(currentSystem) 
        {
            case "AC" : 
                currentSystem = "";
                currentlySelected= "AC";
                airConditionerButton.GetComponent<Button>().Select();
                break;

            case "Temperature":
                //show the AC main list
                temperatureButton.SetActive(true);
                fanSpeedButton.SetActive(true);

                currentSystem = "AC";
                currentlySelected= "Temperature";
                temperatureButton.GetComponent<Button>().Select();
                break;

            case "Fan" : 
                //show the AC main list
                temperatureButton.SetActive(true);
                fanSpeedButton.SetActive(true);

                currentSystem = "AC";
                currentlySelected= "Fan";
                fanSpeedButton.GetComponent<Button>().Select();
                break;

            case "Radio" :
                currentSystem = "";
                currentlySelected= "Radio";
                radioButton.GetComponent<Button>().Select();
                break;

            case "RadioVolume":
                radioVolumeButton.SetActive(true);
                channelsSelectionButton.SetActive(true);
                currentlySelected = "RadioVolume";
                radioVolumeButton.GetComponent<Button>().Select();
                currentSystem="Radio";
                break;
                
            case "RadioChannels":
                radioVolumeButton.SetActive(true);
                channelsSelectionButton.SetActive(true);
                currentlySelected = "RadioChannels";
                channelsSelectionButton.GetComponent<Button>().Select();
                currentSystem="Radio";
                break;

            case "RadioChannel":
                showChannels();
                break;

            case "Media" :
                currentSystem = "";
                currentlySelected = "Media";
                mediaButton.GetComponent<Button>().Select();
                break;

            case "MediaFile" :
                //show the media buttons 
                //show the media buttons 
                for (int i = 0; i < 4; i++)
                {
                    mediaButtons[i].gameObject.SetActive(true);
                }
                mediaButtons[0].Select();
                selectedMediaFile = 0;
                nextBatchButton.SetActive(true);
                previousBatchButton.SetActive(true);
                currentMediaIndex = -1;
                currentSystem = "Media";
                break;        

            default : //left for the gps .. show the Main List for now.
                currentSystem = "";
                currentlySelected="GPS";
                GPSButton.GetComponent<Button>().Select();
                break;
        }
    }
    private void SetupControls  ()
    {
        //hide the back button
        backButton.SetActive(false);

        //hide the media controls
        mediaButton.SetActive(false);
        mediaVolumeSlider.SetActive(false);
        mediaDecreaseVolumeButton.SetActive(false);
        mediaIncreaseVolumeButton.SetActive(false);
        mediaPlayButton.SetActive(false);
        nextBatchButton.SetActive(false);
        previousBatchButton.SetActive(false);
   
        //hide the nested Radio Controls
        volumeSlider.SetActive(false);
        increaseVolumeButton.SetActive(false);
        decreaseVolumeButton.SetActive(false);
        nextButton.SetActive(false);
        previousButton.SetActive(false);
        playButton.SetActive(false);
        channelsSelectionButton.SetActive(false);
        radioVolumeButton.SetActive(false);
        for(int i = 0; i < channelButtons.Length; i++)
        {
            channelButtons[i].SetActive(false);
        }
        
        //hide the nested AC controls
        temperatureButton.SetActive(false);  
        fanSpeedButton.SetActive(false); 

        //hide the nested temperature controls
        temperatureSlider.SetActive(false); 
        increaseTemperatureButton.SetActive(false);
        decreaseTemperatureButton.SetActive(false);

        //hide the nested fan controls 
        fanSpeedSlider.SetActive(false);
        increasefanSpeedButton.SetActive(false);
        decreasefanSpeedButton.SetActive(false);

        //hide the nested Radio Controls
        volumeSlider.SetActive(false);

        //hide the GPS container
        GPSImage.SetActive(false);

        //hide the media buttons 
        foreach (Button btn in mediaButtons)
        {
            btn.gameObject.SetActive(false);
        }

        //display the default controls 
        airConditionerButton.SetActive(true); 
        GPSButton.SetActive(true);
        radioButton.SetActive(true);
        mediaButton.SetActive(true);
    }
    //END of Helpers and Navigation Functions
 } 