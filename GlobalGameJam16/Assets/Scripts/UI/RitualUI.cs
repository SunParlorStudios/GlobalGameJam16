using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RitualUI : MonoBehaviour
{
    public Sprite aButtonSprite;
    public Sprite bButtonSprite;
    public Sprite xButtonSprite;
    public Sprite yButtonSprite;
    public Sprite leftButtonSprite;
    public Sprite rightButtonSprite;
    public Sprite upButtonSprite;
    public Sprite downButtonSprite;
    public Sprite rightTriggerButtonSprite;
    public Sprite leftTriggerButtonSprite;
    public Sprite rightBumperButtonSprite;
    public Sprite leftBumperButtonSprite;
    public Sprite lastButtonSprite;

    public GameObject keyPrefab;

    public float keySpacing = 1.0f;

    private GameController gameController_;
    private Ritual currentRitual_;

    private List<GameObject> keys_;
    private Dictionary<KeyCodes, Sprite> spriteMapping_;

    void SetupMapping()
    {
        spriteMapping_ = new Dictionary<KeyCodes, Sprite>()
        {
            { KeyCodes.A, aButtonSprite },
            { KeyCodes.B, bButtonSprite },
            { KeyCodes.X, xButtonSprite },
            { KeyCodes.Y, yButtonSprite },
            { KeyCodes.Left, leftButtonSprite },
            { KeyCodes.Right, rightButtonSprite },
            { KeyCodes.Up, upButtonSprite },
            { KeyCodes.Down, downButtonSprite },
            { KeyCodes.RT, rightTriggerButtonSprite },
            { KeyCodes.LT, leftTriggerButtonSprite },
            { KeyCodes.RB, rightBumperButtonSprite },
            { KeyCodes.LB, leftBumperButtonSprite },
            { KeyCodes.Last, lastButtonSprite }
        };
    }

    void Awake()
    {
        keys_ = new List<GameObject>();
        SetupMapping();

        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");

        if (gameController == null)
        {
            Debug.LogError("Could not find a GameController object tagged with 'GameController'");
            return;
        }

        gameController_ = gameController.GetComponent<GameController>();
        currentRitual_ = gameController_.currentRitual;

        currentRitual_.Changed += CreateKeys;
    }

    void CreateKeys(int count)
    {
        transform.localPosition = new Vector3(-((count - 1) * keySpacing) * 0.5f, transform.localPosition.y, transform.localPosition.z);

        for (int i = 0; i < keys_.Count; ++i)
        {
            Destroy(keys_[i]);
        }

        keys_.Clear();

        GameObject key;
        for (int i = 0; i < count; ++i)
        {
            key = Instantiate(keyPrefab);
            key.transform.parent = transform;
            key.GetComponent<UIKey>().Set(spriteMapping_[currentRitual_.ritual[i].keyCode], i, keySpacing);

            keys_.Add(key);
        }
    }

    void Update()
    {
        for (int i = 0; i < currentRitual_.ritual.Count; ++i)
        {
            for (int j = 0; j < 2; ++j)
            {
                keys_[i].GetComponent<UIKey>().SetCurrent(j, currentRitual_.ritual[i].earnedByJoy[j]);
            }
        }
    }
}
