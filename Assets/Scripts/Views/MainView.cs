using System;
using Cysharp.Threading.Tasks;
using Holders;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public interface IMainView
    {
        Action<NicknameState> NewNicknameRequestAction { get; set; }
        void StoreNewNickname(string nickname);
    }
    
    public class MainView : IMainView
    {
        public Action<NicknameState> NewNicknameRequestAction { get; set; }

        private readonly Button circleButton;
        private readonly TextMeshProUGUI nicknameText;
        private readonly Button rightArrowButton;
        private readonly Button leftArrowButton;
        private readonly Animator circleButtonSpinAnimator;
        private readonly Animator stateAnimator;
        private readonly GameObject clickMeObject;
        private NicknameState currentNicknameState;
        private string nickname;

        private const string SPIN = "Spin";
        private const string BEST_TO_DEAD_INSIDE = "BestToDeadInside";
        private const string DEAD_INSIDE_TO_ENGLISH = "DeadInsideToEnglish";
        private const string ENGLISH_TO_DEAD_INSIDE = "EnglishToDeadInside";
        private const string DEAD_INSIDE_TO_BEST = "DeadInsideToBest";

        public MainView(MainHolder mainHolder)
        {
            circleButton = mainHolder.circleButton;
            circleButtonSpinAnimator = mainHolder.circleButtonSpinAnimator;
            nicknameText = mainHolder.nicknameText;
            rightArrowButton = mainHolder.rightArrowButton;
            leftArrowButton = mainHolder.leftArrowButton;
            stateAnimator = mainHolder.stateAnimator;
            clickMeObject = mainHolder.clickMeObject;
            Initialize();
        }

        private void Initialize()
        {
            circleButton.onClick.AddListener(OnCircleButtonClick);
            rightArrowButton.onClick.AddListener(OnRightButtonButtonClick);
            leftArrowButton.onClick.AddListener(OnLeftButtonButtonClick);
        }

        private async void OnCircleButtonClick()
        {
            clickMeObject.SetActive(false);
            nicknameText.gameObject.SetActive(false);
            circleButtonSpinAnimator.SetBool(SPIN, true);
            NewNicknameRequestAction.Invoke(currentNicknameState);
            await UniTask.WaitUntil( () => circleButtonSpinAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
            circleButtonSpinAnimator.SetBool(SPIN, false);
            SetNewNickname();
        }

        private void OnRightButtonButtonClick()
        {
            if ((int)currentNicknameState < Enum.GetNames(typeof(NicknameState)).Length)
            {
                currentNicknameState++;
                stateAnimator.SetBool(currentNicknameState == NicknameState.DeadInside ? BEST_TO_DEAD_INSIDE : DEAD_INSIDE_TO_ENGLISH, true);
            }
        }
        
        private void OnLeftButtonButtonClick()
        {
            if ((int)currentNicknameState - 1 >= 0)
            {
                currentNicknameState--;
                stateAnimator.SetBool(currentNicknameState == NicknameState.DeadInside ? ENGLISH_TO_DEAD_INSIDE : DEAD_INSIDE_TO_BEST, true);
            }
        }
        
        public void StoreNewNickname(string nickname)
        {
            this.nickname = nickname;
        }
        
        private void SetNewNickname()
        {
            nicknameText.text = nickname;
            nicknameText.gameObject.SetActive(true);
        }
    }

    public enum NicknameState
    {
        Best = 0,
        DeadInside = 1,
        English = 2
    }
}