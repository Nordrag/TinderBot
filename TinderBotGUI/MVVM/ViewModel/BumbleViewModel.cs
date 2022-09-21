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
        int likes = 50;

        public RelayCommand openSiteCommand { get; set; }
        public RelayCommand startCommand { get; set; }
        public RelayCommand stopCommand { get; set; }

        void Setup()
        {
            driver.CreateDriver();
            driver.OpenSite(BotDriver.bumbleUrl);
            likes = MainViewModel.Instance.Settings.AmountOfTinderLikes;
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

            likes = likes <= 0 ? MainViewModel.Instance.Settings.AmountOfTinderLikes : likes;

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
            if (MainViewModel.Instance.Settings.NothingBanned)
            {
                RollForLike(likeBtn, dislikeBtn);
                return;
            }
          
            var tags = driver.GetComponentsByClassName(Bumble.bTagClass);
            var scrollBtn = driver.GetComponentByXPath(Bumble.bScrollBtn);

            driver.ClickElement(scrollBtn);
            Thread.Sleep(1000);

            var bio = driver.GetComponentByClassName(Bumble.bBioClass);
            string bioText = bio == null ? "" : bio.Text;
            List<string> tagText = new List<string>();          
            tags.ForEach(t => tagText.Add(t.Text));
            var banned = MainViewModel.Instance.Settings.GetBannedWords();
            foreach (var item in banned)
            {
                if (bioText.ToLower().Contains(item) || bioText.ToLower().Contains('@') && MainViewModel.Instance.Settings.BanInstaModels)
                {
                    SendDislike(dislikeBtn);
                    return;
                }
            }

            var bans = MainViewModel.Instance.Settings.GetBannedWords();

            foreach (var tag in tagText)
            {
                foreach (var ban in bans)
                {
                    bool found = tag.ToLower().Contains(ban);

                    if (found)
                    {
                        SendDislike(dislikeBtn);
                        return;
                    }
                }
            }

            var newLikeBtn = driver.GetComponentByClassName(Bumble.bLikeBtn);
            var newDisLikeBtn = driver.GetComponentByClassName(Bumble.bDislikeBtn);
          
            RollForLike(newLikeBtn, newDisLikeBtn);
        }

        void Stop()
        {
            driver.Stop();
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
            if (Random.Shared.Next(0, 101) <= MainViewModel.Instance.Settings.LikeChance)
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
