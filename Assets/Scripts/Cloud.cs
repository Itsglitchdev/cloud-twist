using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Cloud : MonoBehaviour
{
    [Header("CloudData")]
    [SerializeField] private CloudBack cloudBackName;
    [SerializeField] private Image cloudBackImage;
    [SerializeField] private CloudFront cloudFrontName;
    [SerializeField] private Image cloudFrontImage;

    private Button button;
    private bool isFlipped;
    private bool isMatched;

    public CloudBack BackName => cloudBackName;
    public bool IsFlipped => isFlipped;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClicked);
        cloudFrontImage.gameObject.SetActive(true);
        cloudBackImage.gameObject.SetActive(false);
        ResetCloud();
    }

    #region Public Methods

    public void Setup(CloudData data)
    {
        cloudBackName = data.back.cloudBack;
        cloudBackImage.sprite = data.back.imageBack;

        cloudFrontName = data.front.cloudFront;
        cloudFrontImage.sprite = data.front.imageFront;
    }

    public void OnClicked()
    {
        if (isFlipped || isMatched) return;

        StartCoroutine(Flip(true));
        GameManager.Instance.OnCloudSelected(this);
    }

    public IEnumerator Flip(bool showBack)
    {
        isFlipped = showBack;

        float rotationDuration = GameConstants.FLOAT_ZERO_POINT_TWENTYFIVE;
        float elapsedTime = GameConstants.FLOAT_ZERO;

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(GameConstants.FLOAT_ZERO, showBack ? GameConstants.FLOAT_ONE_EIGHT_ZERO : GameConstants.FLOAT_ZERO, GameConstants.FLOAT_ZERO);

        bool imageSwitched = false;

        while (elapsedTime < rotationDuration)
        {
            if (this == null || gameObject == null)
                yield break;

            float t = elapsedTime / rotationDuration;

            float smoothT = Mathf.SmoothStep(GameConstants.FLOAT_ZERO, GameConstants.FLOAT_ONE, t);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, smoothT);

            if (!imageSwitched && t > GameConstants.FLOAT_ZERO_POINT_FIVE)
            {
                cloudFrontImage.gameObject.SetActive(!showBack);
                cloudBackImage.gameObject.SetActive(showBack);
                imageSwitched = true;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }


    public void MarkAsMatched()
    {
        isMatched = true;
        StartCoroutine(DestroyAfterDelay());
    }

    #endregion

    private void ResetCloud()
    {
        isFlipped = false;
        isMatched = false;
        cloudFrontImage.gameObject.SetActive(true);
        cloudBackImage.gameObject.SetActive(false);
        transform.rotation = Quaternion.identity;
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(GameConstants.FLOAT_ZERO_POINT_FIVE);
        Destroy(gameObject);
    }


}