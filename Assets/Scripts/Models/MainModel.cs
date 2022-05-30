using System;
using Data;
using Views;

namespace Models
{
    public interface IMainModel
    {
        Action<NicknameState> ActionGenerateNickname { get; set; }
        Action<string> ActionNicknameGenerated { get; set; }
    }
    
    public class MainModel : IMainModel
    {
        private readonly IDatabase database;
        public Action<NicknameState> ActionGenerateNickname { get; set; }
        public Action<string> ActionNicknameGenerated { get; set; }
        
        public MainModel(IDatabase database)
        {
            this.database = database;
            Initialize();
        }

        private void Initialize()
        {
            ActionGenerateNickname += state => database.ActionGenerateNickname.Invoke(state);
            database.ActionGeneratedNickname += state => ActionNicknameGenerated.Invoke(state);
        }
    }
}