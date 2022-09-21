using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinderBotGUI.Core;
using TinderAutoLikerLibrary.Helpers;

namespace TinderBotGUI.MVVM.ViewModel
{
    public class SettingsViewModel : ObservableObject
    {       

        Serializer serializer = new Serializer();
        SavedSettings? save;

        public SavedSettings SavedData => save;

        public RelayCommand updateBanDisplayCommand { get; set; }

        int amountOfTinderLikes = 50;
        int amountOfBadooLikes = 50;

        public int AmountOfBadooLikes
        {
            get => amountOfBadooLikes;
            set
            {
                amountOfBadooLikes = value;
                OnpropertyChanged();
            }
        }

        public int AmountOfTinderLikes 
        {
            get => amountOfTinderLikes; 
            set
            {
                amountOfTinderLikes = value;
                OnpropertyChanged();
            }
        }
        public int LikeChance { get; set; } = 50;

        bool isEnglish = true;
        public bool IsEnglish 
        {
            get => isEnglish; 
            set
            {
                isEnglish = value;
                OnpropertyChanged();
            }
        }

        bool online;
        public bool RecentlyOnline 
        {
            get => online;
            set
            {
                online = value;
                OnpropertyChanged();
            }
        }
        bool hasPremium = true;
        public bool HasPremium 
        {
            get => hasPremium;
            set
            {
                hasPremium = value;
                Tinder.SetPremiumStatus(value);
                OnpropertyChanged();
            }
        }

        bool infiniteLikes;
        public bool InfiniteLikes 
        {
            get => infiniteLikes;
            set
            {
                infiniteLikes = value;
                OnpropertyChanged();
            }
        }

        bool banFeminist;
        public bool BanFeminist 
        {
            get => banFeminist;
            set
            {
                banFeminist = value;
                if (value)
                {
                    fullList.AddRange(feminist);
                }
                else
                {
                    feminist.ForEach(x => fullList.Remove(x));
                }
                OnpropertyChanged();
            }
        }

        bool lgbt;
        public bool BanLGBTQ 
        {
            get => lgbt;
            set
            {
                lgbt = value;
                if (value)
                {
                    fullList.Add("LBGTQ");
                }
                else
                {
                    fullList.Remove("LGBTQ");
                }
                OnpropertyChanged();
            }
        }

        bool doglovers;
        public bool BanDogLovers 
        {
            get => doglovers; 
            set
            {
                doglovers = value;
                if (value)
                {
                    fullList.AddRange(dogWords);
                }
                else
                {
                    dogWords.ForEach(x => fullList.Remove(x));
                }
                OnpropertyChanged();
            }
        }

        bool spirituals;
        public bool BanSpirituals 
        {
            get => spirituals;
            set
            {
                spirituals = value;
                if (value)
                {
                    fullList.AddRange(spiritual);
                }
                else
                {
                    spiritual.ForEach(x => fullList.Remove(x));
                }
                OnpropertyChanged();
            }
        }

        bool banInsta;
        public bool BanInstaModels 
        {
            get => banInsta;
            set
            {
                banInsta = value;
                if (value)
                {
                    fullList.AddRange(instagram);
                }
                else
                {
                    instagram.ForEach(x => fullList.Remove(x));
                }
                OnpropertyChanged();
            }
        }

        bool banTikTok;
        public bool BanTikTok
        {
            get => banTikTok;
            set
            {
                banTikTok = value;

                if (value)
                {
                    fullList.Add("TikTok");
                }
                else
                {
                    fullList.Remove("TikTok");
                }

                OnpropertyChanged();
            }
        }

        bool nothingBanned = true;
        public bool NothingBanned
        {
            get => nothingBanned;
            set
            {
                nothingBanned = value;
                OnpropertyChanged();
            }
        }
       
      

        List<string> dogWords = new List<string> { "kutyabarát", "doglover", "dog lover", "dog", "kutya" };
        List<string> spiritual = new List<string> { "spiritual", "spirituális"};
        List<string> feminist = new List<string> { "feminist", "feminista"};
        public List<string> instagram = new List<string> { "ig.:", "ig:", "insta:", "insta", "instagram"};
        List<string> fullList = new List<string>();

        public RelayCommand saveSettingsCommand { get; set; }
        public RelayCommand clearBansCommand { get; set; }

        public SettingsViewModel()
        {
            saveSettingsCommand = new RelayCommand(e => Save(), f => true);
            clearBansCommand = new RelayCommand(e => Clear(), f => true);
            updateBanDisplayCommand = new RelayCommand(e => SetBanned(), f => true);          

            try
            {
                save = serializer.Deserialize<SavedSettings>();

                LikeChance = save.LikeChance;
                RecentlyOnline = save.RecentlyOnline;
                BanDogLovers = save.BanDogLovers;
                BanFeminist = save.BanFeminist;
                BanInstaModels = save.BanInstaModels;
                BanLGBTQ = save.BanLGBTQ;
                BanSpirituals = save.BanSpirituals;
                BanTikTok = save.BanTikToks;
                NothingBanned = save.NothingBanned;
                HasPremium = save.HasPremium;
                IsEnglish = save.IsEnglish;
            }
            catch (Exception e)
            {
                save = new SavedSettings(LikeChance, RecentlyOnline, InfiniteLikes, BanFeminist, BanLGBTQ, BanDogLovers, BanSpirituals, BanInstaModels, BanTikTok, NothingBanned, HasPremium, IsEnglish);
                serializer.Serialize(save);
            }

            Tinder.SetPremiumStatus(HasPremium);
        }

        void SetBanned()
        {
            NothingBanned = false;
        }

        void Save()
        {
            save = new SavedSettings(LikeChance, RecentlyOnline, InfiniteLikes, BanFeminist, BanLGBTQ, BanDogLovers, BanSpirituals,BanInstaModels, BanTikTok ,NothingBanned, HasPremium, IsEnglish);
            serializer.Serialize<SavedSettings>(save); 
        }

        public void SaveStats()
        {
            save.LikesSentOnTinder = MainViewModel.Instance.About.LikesSentOnTinder;
            save.LikesSentOnBadoo = MainViewModel.Instance.About.LikesSentOnBadoo;
            save.MatchesOnTinder = MainViewModel.Instance.About.MatchesOnTinder;
            save.MatchesOnBadoo = MainViewModel.Instance.About.MatchesOnBadoo;
            serializer.Serialize<SavedSettings>(save);
        }

        void Clear()
        {
            RecentlyOnline = false;
            BanDogLovers = false;
            BanFeminist = false;
            BanLGBTQ = false;
            BanSpirituals = false;
            BanInstaModels = false;
            NothingBanned = true;
            BanTikTok = false;
        }

        public List<string> GetBannedWords()
        {          
            return fullList;
        }      
    }

    [System.Serializable]
    public class SavedSettings
    {
        public bool HasPremium;
        public bool IsEnglish;
        public int LikeChance;
        public bool RecentlyOnline;
        public bool InfiniteLikes;
        public bool BanFeminist;
        public bool BanLGBTQ;
        public bool BanDogLovers;
        public bool BanSpirituals;
        public bool BanInstaModels;
        public bool BanTikToks;
        public bool NothingBanned;

        public int LikesSentOnTinder;
        public int LikesSentOnBadoo;
        public int MatchesOnTinder;
        public int MatchesOnBadoo;

        public SavedSettings(int chance, bool online, bool infinite, bool feminist, bool lgbtq, bool dog, bool spiritual, bool models, bool tiktok , bool nothing, bool hasPremium, bool isEnglish)
        {
            LikeChance = chance;
            RecentlyOnline = online;
            InfiniteLikes = infinite;
            BanFeminist = feminist;
            BanLGBTQ = lgbtq;
            BanDogLovers = dog;
            BanSpirituals = spiritual;
            BanInstaModels = models;
            BanTikToks = tiktok;
            NothingBanned = nothing;
            HasPremium = hasPremium;
            IsEnglish = isEnglish;
        }
    }
}
