using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    [Header("Components")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Dropdown resolutionsDropdown;
    [SerializeField] private TextMeshProUGUI fontSizeText;
    [SerializeField] private Toggle englishTranslationToggle;
    [SerializeField] private Slider fontSizeSlider;

    [Header("Settings")]
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab;

    private Resolution[] resolutions;
    private bool isOpen;

    private void Awake()
    {
        // Singleton logic
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        canvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    private void Start()
    {
        // Cache possible resolutions based on hardware
        resolutions = Screen.resolutions;

        // Clear any options
        resolutionsDropdown.ClearOptions();

        // Format resolutions
        List<string> options = new();
        int currentResIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].height == Screen.currentResolution.height && resolutions[i].width == Screen.currentResolution.width)
            {
                currentResIndex = i;
            }
        }

        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = currentResIndex;
        resolutionsDropdown.RefreshShownValue();

        bool enableTranslationDisplay = PlayerPrefs.GetInt(GameConstants.DISPLAY_TRANSLATION_KEY, 0) == 1;
        englishTranslationToggle.isOn = enableTranslationDisplay;

        // Set size
        float size = PlayerPrefs.GetInt(GameConstants.WORD_SIZE_KEY, GameConstants.DEFUALT_WORD_SIZE);
        fontSizeText.text = $"Word Size: {(int)size}";
        fontSizeSlider.value = size;
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey) && isOpen)
            Close();
    }

    public void Open()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        isOpen = true;
    }

    public void Close()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        isOpen = false;
    }

    /// ~~~~~ External Functions ~~~~~

    public void SetMasterVolume(float volume)
    {
        // Set mixer volume 
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        // Set mixer volume 
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        // Set mixer volume 
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SetResolution(int resolutionIndex)
    {
        // Get resolution from our dropdown
        Resolution resolution = resolutions[resolutionIndex];

        // Update resolution
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen); ;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        // Set fullscreen
        Screen.fullScreen = isFullscreen;
    }

    public void SetTextSize(float size)
    {
        // Set size
        PlayerPrefs.SetInt(GameConstants.WORD_SIZE_KEY, (int)size);

        // Set text
        fontSizeText.text = $"Word Size: {(int)size}";
    }

    public void SetDisplayEnglishTranslation(bool display)
    {
        // Set preference
        int val = display ? 1 : 0;
        PlayerPrefs.SetInt(GameConstants.DISPLAY_TRANSLATION_KEY, val);

        englishTranslationToggle.isOn = display;
    }
}
