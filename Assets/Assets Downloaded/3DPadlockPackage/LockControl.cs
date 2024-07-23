using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockControl : MonoBehaviour
{
    public int[] result, correctCombination;
    public float moveUp = 0.0f;
    private bool isOpened;

    [SerializeField] Locker locker;

    private void Start()
    {
        result = new int[]{0,0,0,0};
        //correctCombination = new int[] {6,7,8,4};
        isOpened = false;
        Rotate.Rotated += CheckResults;
    }

    private void CheckResults(string wheelName, int number)
    {
        switch (wheelName)
        {
            case "WheelOne":
                result[0] = number;
                break;

            case "WheelTwo":
                result[1] = number;
                break;

            case "WheelThree":
                result[2] = number;
                break;

            case "WheelFour":
                result[3] = number;
                break;
        }

        if (result[0] == correctCombination[0] && result[1] == correctCombination[1]
            && result[2] == correctCombination[2] && result[3] == correctCombination[3] && !isOpened)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + moveUp, transform.localPosition.z);
            isOpened = true;
            locker.lockerOpen = true;
			PlayerManager.Instance.UpdatePlayerState(PlayerState.Default);

		}
    }

    private void OnDestroy()
    {
        Rotate.Rotated -= CheckResults;
    }
}
