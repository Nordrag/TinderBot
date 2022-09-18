using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;
using TinderBotGUI.Core;

namespace TinderBotGUI.MVVM.ViewModel
{
    public class TinderViewModel
    {
        BotDriver driver = new BotDriver();
                 
        bool homeScreenPopUp, boostPopUp;

        public RelayCommand openSiteCommand { get; set; }
        public RelayCommand startCommand { get; set; }
        public RelayCommand stopCommand { get; set; }

        public TinderViewModel()
        {
            openSiteCommand = new RelayCommand(e => Setup(), f => true);
            startCommand = new RelayCommand(e => Start(), f => true);
            stopCommand = new RelayCommand(e => driver.Stop(), f => true);                 
        }

        void Setup()
        {
            driver.CreateDriver();
            driver.OpenSite(BotDriver.tinderUrl);
        }

        public void DestroyDriver()
        {
            driver.Quit();
        }

        void Start()
        {
            if (!driver.IsDriverReady)
            {
                Setup();
                return;
            }

            if (SettingsViewModel.instance.AmountOfTinderLikes <=0)
            {
                SettingsViewModel.instance.AmountOfTinderLikes = 50;
            }

            driver.Start(() => Work());          
        }

        void Work()
        {

            CheckBoostTimePopUp();

            if (driver.GetComponentByXPath(Tinder.tOutOfLikesBox) != null)
            {
                driver.Stop();
            }

            var likeBtn = driver.GetComponentByClassName(Tinder.tLikeButtonCSS);

            if (SettingsViewModel.instance.NothingBanned)
            {
                if (Random.Shared.Next(0, 101) <= SettingsViewModel.instance.LikeChance)
                {
                    SendLike(likeBtn);
                }
                else
                {
                    var dLike = driver.GetComponentByClassName(Tinder.tDislikeButtonCSS);
                    SendDisliked(dLike);
                }
                return;
            }
         
            var infoBtn = driver.GetComponentByXPath(Tinder.tInfoButton);
            var onlineIcon = driver.GetComponentByXPath(Tinder.tOnlineButtonXPath);
            string online = onlineIcon == null ? "" : onlineIcon.Text;

            if (SettingsViewModel.instance.RecentlyOnline)
            {
                string compare = SettingsViewModel.instance.IsEnglish ? "Recently active" : "Nemrég aktív";
                bool bOnline = online == compare;
                if (!bOnline)
                {
                    var dLike = driver.GetComponentByClassName(Tinder.tDislikeButtonCSS);
                    SendDisliked(dLike);
                    return;
                }


            }        

            ClickInfoBtn(infoBtn);
            var tags = driver.GetComponentsByClassName(Tinder.tTagsCSS);
            var bio = driver.GetComponentByClassName(Tinder.tBioCSS);
            string bioText = bio == null ? "" : bio.Text;

            List<string> tagStrings = new List<string>();
            tagStrings.Add(bioText);
            tags.ForEach(tag => tagStrings.Add(tag.Text));

            var banned = SettingsViewModel.instance.GetBannedWords();
            foreach (var item in banned)
            {
                foreach (var tag in tagStrings)
                {
                    if (tag.ToLower().Contains(item) || tag.Contains('@') && SettingsViewModel.instance.BanInstaModels)
                    {
                        CloseBio();
                        var dLike = driver.GetComponentByClassName(Tinder.tDislikeButtonCSS);
                        SendDisliked(dLike);
                        return;
                    }
                }             
            }
           
            CloseBio();

            var dislikeBtn = driver.GetComponentByClassName(Tinder.tDislikeButtonCSS);
            var newLikeBtn = driver.GetComponentByClassName(Tinder.tLikeButtonCSS);
            RollForLike(newLikeBtn, dislikeBtn);
            CheckPopUp();
        }

        void SendLike(IWebElement likeBtn)
        {
            if (likeBtn != null)
            {
                AboutViewModel.Instance.LikesSentOnTinder++;
                driver.ClickElement(likeBtn);
                Thread.Sleep(1500);

                if (!SettingsViewModel.instance.InfiniteLikes)
                {
                    SettingsViewModel.instance.AmountOfTinderLikes--;

                    if (SettingsViewModel.instance.AmountOfTinderLikes <= 0)
                    {
                        driver.Stop();
                    }
                }
              
            }
            else
            {
                driver.Stop();
            }
            CheckForAddHomeScreenPopUp();
            CheckForOutOfLikes();
            CheckForMatch();
        }

        void SendDisliked(IWebElement btn)
        {
            if (btn != null)
            {
                driver.ClickElement(btn);
                Thread.Sleep(1500);
            }
            else
            {
                driver.Stop();
            }
            CheckForAddHomeScreenPopUp();
        }

        void CheckPopUp()
        {
            var popUp = driver.GetComponentByXPath(Tinder.tUpgrade);

            if (popUp != null)
            {
                driver.ClickElement(popUp);
            }
        }

        void ClickInfoBtn(IWebElement infoBtn)
        {
            if (infoBtn != null)
            {
                driver.ClickElement(infoBtn);
                Thread.Sleep(1500);
            }
        }
        void CloseBio()
        {
            var res = driver.GetComponentByXPath(Tinder.tArrowBioBtn);

            if (res != null)
            {
                driver.ClickElement(res);
                Thread.Sleep(500);
            }
        }

        void RollForLike(IWebElement likeBtn, IWebElement dislikeBtn)
        {
            if (Random.Shared.Next(0, 101) <= SettingsViewModel.instance.LikeChance)
            {
                SendLike(likeBtn);              
            }
            else
            {
                SendDisliked(dislikeBtn);
            }
        }       
       
        void CheckForAddHomeScreenPopUp()
        {
            if (!homeScreenPopUp)
            {
                var popup = driver.GetComponentByClassName(Tinder.tNotInterestedBoxCSS);

                if (popup != null)
                {
                    driver.ClickElement(popup);
                    homeScreenPopUp = true;
                    Thread.Sleep(1000);
                }
            }
           
        }

        void CheckBoostTimePopUp()
        {
            if (boostPopUp)
            {
                var popup = driver.GetComponentByXPath(Tinder.tBoostTime);

                if (popup != null)
                {
                    var dLike = driver.GetComponentByClassName(Tinder.tDislikeButtonCSS);
                    driver.ClickElement(dLike);
                    boostPopUp = true;
                    Thread.Sleep(1000);
                }
            }          
        }

        void CheckForOutOfLikes()
        {
            if (!SettingsViewModel.instance.HasPremium)
            {
                var outOflikes = driver.GetComponentByXPath(Tinder.tOutOfLikesBox);

                if (outOflikes != null)
                {
                    driver.Stop();
                }
            }        
        }

        void CheckForMatch()
        {
            var matchBox = driver.GetComponentByXPath(Tinder.tMatchFoundXPath);

            if (matchBox != null)
            {
                AboutViewModel.Instance.MatchesOnTinder++;
                driver.ClickElement(matchBox);
                Thread.Sleep(1000);
            }
        }
    }
}
