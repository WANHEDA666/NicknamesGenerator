using System;
using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using Views;
using Random = UnityEngine.Random;

namespace Data
{
    public interface IDatabase
    {
        Action<NicknameState> ActionGenerateNickname { get; set; }
        Action<string> ActionGeneratedNickname { get; set; }
    }
    
    public class Database : IDatabase
    {
        public Action<NicknameState> ActionGenerateNickname { get; set; }
        public Action<string> ActionGeneratedNickname { get; set; }
        
        private const string IDIOTIC_NICKNAMES = "idiotic_nicknames";
        private const string DEAD_INSIDE_NICKNAMES = "dead_inside_nicknames";
        private const string ENGLISH_NICKNAMES = "english_nicknames";
        private const string FULL_NAME = "full_name";
        private const string PARTLY_NAME = "partly_name";
        private const string FIRST_PART = "first_part";
        private const string SECOND_PART = "second_part";
        private Nickname[] nicknamesHolders;
        private static DatabaseReference reference;

        public Database()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            nicknamesHolders = new Nickname[] {new BestNickname(this), new DeadInsideNickname(this), new EnglishNickname(this)};
            reference = FirebaseDatabase.DefaultInstance.RootReference;
            ActionGenerateNickname += RandomNickname;
        }
        
        private void RandomNickname(NicknameState nicknameState)
        {
            foreach (var nicknamesHolder in nicknamesHolders)
            {
                if (nicknamesHolder.NicknameState == nicknameState)
                {
                    nicknamesHolder.NewNickname();
                }
            }
        }
        
        private void GeneratedNickname(string path, string type)
        {
            var nickname = "";
            reference.Database.GetReference(path).GetValueAsync().ContinueWithOnMainThread(task => 
            {
                if (task.IsCompleted) 
                {
                    var snapshot = task.Result.Value as Dictionary<string, object>;
                    switch (type)
                    {
                        case FULL_NAME:
                            var nicknames = snapshot?[type] as List<object>;
                            nickname = nicknames?[RealRandomNumber(nicknames.Count)].ToString();
                            break;
                        case PARTLY_NAME:
                            var nicknamesArray = snapshot?[type] as Dictionary<string, object>;
                            var firstPartNicknamesArray = nicknamesArray?[FIRST_PART] as List<object>;
                            var secondPartNicknamesArray = nicknamesArray?[SECOND_PART] as List<object>;
                            nickname = firstPartNicknamesArray?[RealRandomNumber(firstPartNicknamesArray.Count)] + " " + secondPartNicknamesArray?[RealRandomNumber(secondPartNicknamesArray.Count)];
                            break;
                    }
                    ActionGeneratedNickname.Invoke(nickname);
                }
            });
        }
        
        private class BestNickname : Nickname
        {
            public BestNickname(Database database) : base(database)
            {
            }
            
            public override NicknameState NicknameState => NicknameState.Best;

            public override void NewNickname()
            {
                Database.GeneratedNickname(IDIOTIC_NICKNAMES, RealRandomNumber(5) == 4 ? PARTLY_NAME : FULL_NAME);
            }
        }
        
        private class DeadInsideNickname : Nickname
        {
            public DeadInsideNickname(Database database) : base(database)
            {
            }
            
            public override NicknameState NicknameState => NicknameState.DeadInside;

            public override void NewNickname()
            {
                Database.GeneratedNickname(DEAD_INSIDE_NICKNAMES, FULL_NAME);
            }
        }
        
        private class EnglishNickname : Nickname
        {
            public EnglishNickname(Database database) : base(database)
            {
            }
            
            public override NicknameState NicknameState => NicknameState.English;

            public override void NewNickname()
            {
                Database.GeneratedNickname(ENGLISH_NICKNAMES, FULL_NAME);
            }
        }

        private static int RealRandomNumber(int count)
        {
            var localResult = Random.Range(0, count);
            localResult += DateTime.Now.Second + DateTime.Now.Millisecond;
            return localResult < count ? localResult : localResult % count;
        }
    }

    public abstract class Nickname
    {
        protected readonly Database Database;

        protected Nickname(Database database)
        {
            Database = database;
        }
        
        public abstract NicknameState NicknameState { get; }
        public abstract void NewNickname();
    }
}