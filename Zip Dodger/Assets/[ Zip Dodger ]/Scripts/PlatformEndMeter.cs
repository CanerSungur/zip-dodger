using UnityEngine;
using DG.Tweening;
using TMPro;

public class PlatformEndMeter : MonoBehaviour
{
    private PlatformEnd platformEnd;
    public PlatformEnd PlatformEnd => platformEnd == null ? platformEnd = GetComponentInParent<PlatformEnd>() : platformEnd;

    int index = 0;
    bool activated = false;
    TextMeshProUGUI indexText;

    private void Start()
    {
        transform.localPosition = new Vector3(0f, PlatformEnd.DefaultYAxis, (index - 1) * PlatformEnd.DistanceBetweenMeters);
        GetComponent<Renderer>().material.color = PlatformEnd.Colors[index - 1];
        
        indexText = GetComponentInChildren<TextMeshProUGUI>();
        indexText.text = index + "   inches";

        BoxCollider boxCol = GetComponent<BoxCollider>();
        boxCol.size = new Vector3(Mathf.Abs(boxCol.size.x), Mathf.Abs(boxCol.size.y), Mathf.Abs(boxCol.size.z));
    }

    private void OnDisable()
    {
        transform.DOKill();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ChildZipper childZipper) && !activated)
        {
            activated = true;

            AudioHandler.PlayAudio(AudioHandler.AudioType.PlatformEndMeter);

            transform.DOLocalMoveY(PlatformEnd.ActivatedYAxis, 0.1f).SetEase(Ease.InOutSine).OnComplete(() => {
                transform.DOShakeScale(0.1f, 0.5f);
            });

            childZipper.Detach();
        }

        if (other.TryGetComponent(out Player player) && !activated)
        {
            activated = true;

            AudioHandler.PlayAudio(AudioHandler.AudioType.PlatformEndMeter);
            GameManager.SetGameState(GameState.PlatformIsOver);

            transform.DOLocalMoveY(PlatformEnd.ActivatedYAxis, 0.1f).SetEase(Ease.InOutSine).OnComplete(() => {
                transform.DOShakeScale(0.1f, 0.5f).OnComplete(() => {
                    // Calculate Reward, play reward animation.
                    PlatformEnd.GameManager.CalculateRewardTrigger(PlatformEnd.GameManager.DefaultRewardForLevelSuccess * index);
                    DataManager.LevelEndMultiplier = index;
                    Debug.Log("Finish Game!");
                    // End The Game!
                });
            });
        }

        //if (other.TryGetComponent(out ZipperPairHandler childZipper) && childZipper._Type == ZipperPairHandler.Type.Child && !activated)
        //{
        //    activated = true;

        //    AudioHandler.PlayAudio(AudioHandler.AudioType.PlatformEndMeter);

        //    transform.DOLocalMoveY(PlatformEnd.ActivatedYAxis, 0.1f).SetEase(Ease.InOutSine).OnComplete(() => {
        //        transform.DOShakeScale(0.1f, 0.5f);
        //    });

        //    childZipper.DetachForPlatformEnd();
        //}

        //if (other.TryGetComponent(out ZipperPairHandler parentZipper) && parentZipper._Type == ZipperPairHandler.Type.Parent && !activated)
        //{
        //    activated = true;

        //    AudioHandler.PlayAudio(AudioHandler.AudioType.PlatformEndMeter);
        //    GameManager.SetGameState(GameState.PlatformIsOver);

        //    transform.DOLocalMoveY(PlatformEnd.ActivatedYAxis, 0.1f).SetEase(Ease.InOutSine).OnComplete(() => {
        //        transform.DOShakeScale(0.1f, 0.5f).OnComplete(() => {
        //            // Calculate Reward, play reward animation.
        //            PlatformEnd.GameManager.CalculateRewardTrigger(PlatformEnd.GameManager.DefaultRewardForLevelSuccess * index);
        //            DataManager.LevelEndMultiplier = index;
        //            Debug.Log("Finish Game!");
        //            // End The Game!
        //        });
        //    });
        //}
    }

    public void SetIndex(int index) => this.index = index;
}
