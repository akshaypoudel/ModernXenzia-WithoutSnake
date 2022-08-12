using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using SnakeGame; 

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] GameObject _showAdButton;
    public Button showAdButton;
    private string _androidAdUnitId = "Rewarded_Android";

    [SerializeField] SnakeHandler _snakeHandler;
    [SerializeField] AdManager _adManager;
    [SerializeField] private GameObject continueWithAdsButton;
    string _adUnitId; // This will remain null for unsupported platforms
    private bool adSuccessfullyShown = false;

    public void InitializeRewarded()
    {
        _adUnitId = _androidAdUnitId;
        if(_adManager.isMenuScene)
            _showAdButton.SetActive(false);
    }

    // Load content to the Ad Unit:
    public void LoadAd() { Advertisement.Load(_adUnitId, this); }

    // If the ad successfully loads, add a listener to the button and enable it:
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        if (adUnitId.Equals(_adUnitId) && _adManager.isMenuScene)
        {
            showAdButton.onClick.AddListener(ShowAd);
            _showAdButton.SetActive(true);
        }
    }

    // Implement a method to execute when the user clicks the button:
    public void ShowAd()
    {
        adSuccessfullyShown = false;
        Time.timeScale = 0f;
        if(_adManager.isMenuScene)
            _showAdButton.SetActive(false);
        Advertisement.Show(_androidAdUnitId, this);
    }

    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            if(!adSuccessfullyShown)
            {
                Time.timeScale = 1f;
                if (_adManager.isMenuScene)
                    _adManager.GiveFruitsToUser();
                else
                    _snakeHandler.StartTimerForNewLife();

                adSuccessfullyShown = true;
                Advertisement.Load(_adUnitId, this);
            }
        }
    }

    // Implement Load and Show Listener error callbacks:
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        if (_adManager.isMenuScene)
            _showAdButton.SetActive(false);
        else
            continueWithAdsButton.SetActive(false);
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }

    void OnDestroy()
    {
        if(_adManager.isMenuScene)
            showAdButton.onClick.RemoveAllListeners();
    }
}


