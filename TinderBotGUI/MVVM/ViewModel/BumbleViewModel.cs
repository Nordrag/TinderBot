using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;
using TinderBotGUI.Core;

namespace TinderBotGUI.MVVM.ViewModel
{
    public class BumbleViewModel
    {
        BotDriver driver = new BotDriver();  
        bool banConditionFound = false;
        int likes = 50;

        public RelayCommand openSiteCommand { get; set; }
        public RelayCommand startCommand { get; set; }
        public RelayCommand stopCommand { get; set; }

        void Setup()
        {
            driver.CreateDriver();
            driver.OpenSite(BotDriver.bumbleUrl);
            likes = SettingsViewModel.instance.AmountOfTinderLikes;
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

            likes = likes <= 0 ? SettingsViewModel.instance.AmountOfTinderLikes : likes;

            driver.Start(() => Work());
        }   
        
        void Work()
        {
            var outOfLikes = driver.GetComponentByClassName(Bumble.bOutOfLikesCSS);

            if (outOfLikes != null)
            {
                driver.Stop();
            }

            var offScreenMsg = driver.GetComponentByClassName(Bumble.bDeactivateBtnCSS);

            driver.ClickElement(offScreenMsg);

            var likeBtn = driver.GetComponentByClassName(Bumble.bLikeBtn);
            var dislikeBtn = driver.GetComponentByClassName(Bumble.bDislikeBtn);
            if (SettingsViewModel.instance.NothingBanned)
            {
                RollForLike(likeBtn, dislikeBtn);
                return;
            }

            banConditionFound = false;
            var tags = driver.GetComponentsByClassName(Bumble.bTagClass);
            var scrollBtn = driver.GetComponentByXPath(Bumble.bScrollBtn);

            driver.ClickElement(scrollBtn);
            Thread.Sleep(1000);

            var bio = driver.GetComponentByClassName(Bumble.bBioClass);
            string bioText = bio == null ? "" : bio.Text;
            List<string> tagText = new List<string>();

            tags.ForEach(t => tagText.Add(t.Text));
            CheckBanned(tagText, bioText);

            var newLikeBtn = driver.GetComponentByClassName(Bumble.bLikeBtn);
            var newDisLikeBtn = driver.GetComponentByClassName(Bumble.bDislikeBtn);

            if (banConditionFound)
            {
                SendDislike(newDisLikeBtn);
                return;
            }
            RollForLike(newLikeBtn, newDisLikeBtn);
        }

        void Stop()
        {
            driver.Stop();
        }

        void CheckBanned(List<string> tags, string bio)
        {
            foreach (var item in SettingsViewModel.instance.instagram)
            {
                if (bio.ToLower().Contains(item) || bio.ToLower().Contains('@'))
                {
                    banConditionFound = true;
                    return;
                }
            }

            var bans = SettingsViewModel.instance.GetBannedWords();

            foreach (var tag in tags)
            {
                foreach (var ban in bans)
                {
                    bool found = tag.ToLower().Contains(ban);

                    if (found)
                    {
                        banConditionFound = true;
                        return;
                    }
                }
            }
        }

        void SendDislike(IWebElement btn)
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
        }

        void SendLike(IWebElement likeBtn)
        {
            if (likeBtn != null)
            {
                driver.ClickElement(likeBtn);
                Thread.Sleep(1500);
            }
            else
            {
                driver.Stop();
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
                SendDislike(dislikeBtn);
            }
        }    

        public BumbleViewModel()
        {
            openSiteCommand = new RelayCommand(e => Setup(), f => true);
            startCommand = new RelayCommand(e => Start(), f => true);
            stopCommand = new RelayCommand(e => Stop(), f => true);
        }
    }
}
