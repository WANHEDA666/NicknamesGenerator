using Ads;
using Data;
using Holders;
using Models;
using Presenters;
using UnityEngine;
using Views;

namespace MonoBehaviours
{
    public class GameBoot : MonoBehaviour
    {
        [SerializeField] private MainHolder mainHolderPrefab;

        private void Awake()
        {
            var mainHolderObject = Instantiate(mainHolderPrefab, transform);
            IMainView mainView = new MainView(mainHolderObject.GetComponent<MainHolder>());
            var database = new Database();
            IMainModel mainModel = new MainModel(database);
            var mainPresenter = new MainPresenter(mainView, mainModel);
            var ads = new GoogleAds();
        }
    }
}