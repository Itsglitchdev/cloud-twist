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

    public void Setup(CloudData data)
    {
        cloudBackName = data.back.cloudBack;
        cloudBackImage.sprite = data.back.imageBack;

        cloudFrontName = data.front.cloudFront;
        cloudFrontImage.sprite = data.front.imageFront;
    }

    private void OnClicked()
    {
        if (isFlipped || isMatched || GameManager.Instance.IsBusy) return;

        StartCoroutine(Flip(true));
        GameManager.Instance.OnCloudSelected(this);
    }

    public IEnumerator Flip(bool showBack)
    {
        isFlipped = showBack;

        float rotationDuration = 0.25f;
        float elapsedTime = 0f;

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, showBack ? 180f : 0f, 0f);

        bool imageSwitched = false;

        while (elapsedTime < rotationDuration)
        {
            float t = elapsedTime / rotationDuration;

            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, smoothT);

            if (!imageSwitched && t > 0.5f)
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

    private void ResetCloud()
    {
        isFlipped = false;
        isMatched = false;
        cloudFrontImage.gameObject.SetActive(true);
        cloudBackImage.gameObject.SetActive(false);
        transform.rotation = Quaternion.identity;
    }

    public void MarkAsMatched()
    {
        isMatched = true;
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.15f);
        Destroy(gameObject);
    }


}