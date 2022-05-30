using Models;
using Views;

namespace Presenters
{
    public class MainPresenter
    {
        private readonly IMainView mainView;
        private readonly IMainModel mainModel;

        public MainPresenter(IMainView mainView, IMainModel mainModel)
        {
            this.mainView = mainView;
            this.mainModel = mainModel;
            Initialize();
        }

        private void Initialize()
        {
            mainView.NewNicknameRequestAction += state => mainModel.ActionGenerateNickname.Invoke(state);
            mainModel.ActionNicknameGenerated += state => mainView.StoreNewNickname(state);
        }
    }
}