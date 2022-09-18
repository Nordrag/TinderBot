using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TinderBotGUI.Core;

namespace TinderBotGUI.MVVM.ViewModel
{
    //https://www.youtube.com/watch?v=5oz2zJF_jM4
    public class BadooViewModel
    {
        bool banConditionFound;

        BotDriver driver = new BotDriver();
        public RelayCommand startCommand { get; set; }
        public RelayCommand stopCommand { get; set; }

        void Start()
        {
            if (!driver.IsDriverReady)
            {
                driver.CreateDriver();
                driver.OpenSite(BotDriver.badooUrl);
                return;
            }


            if (SettingsViewModel.instance.AmountOfTinderLikes <= 0)
            {
                SettingsViewModel.instance.AmountOfTinderLikes = 50;
            }

            driver.Start(() => Work());
        }

        void Stop()
        {
            driver.Stop();
        }

        void Work()
        {
            banConditionFound = false;
            var likeBtn = driver.GetComponentByClassName(Badoo.likeBtnCSS);
            var disLikeBtn = driver.GetComponentByClassName(Badoo.disLikeBtnCSS);
            var bio = driver.GetComponentByClassName(Badoo.bioCSS);

            if (SettingsViewModel.instance.NothingBanned)
            {
                RollForLike(likeBtn, disLikeBtn);
                return;
            }


            if (bio != null)
            {
                CheckBio(bio.Text, disLikeBtn);
            }

            if (!banConditionFound)
            {
                RollForLike(likeBtn, disLikeBtn);
            }
        }

        void CheckBio(string bio, IWebElement dislike)
        {
            var banList = SettingsViewModel.instance.GetBannedWords();

            foreach (var item in banList)
            {
                if (bio.ToLower().Contains(item.ToLower()))
                {
                    banConditionFound = true;
                    Dislike(dislike);
                    return;
                }
            }
        }

        void RollForLike(IWebElement likeBtn, IWebElement dislikeBtn)
        {
            var roll = Random.Shared.Next(0, 101);

            if (roll <= SettingsViewModel.instance.LikeChance)
            {
                Like(likeBtn);
            }
            else
            {
                Dislike(dislikeBtn);
            }
        }

        void Like(IWebElement likeBtn)
        {
            if (likeBtn != null)
            {
                driver.ClickElement(likeBtn);
            }
            else
            {
                driver.Stop();
            }
            Thread.Sleep(1000);
        }

        void Dislike(IWebElement disLike)
        {
            if (disLike != null)
            {
                driver.ClickElement(disLike);
            }
            else
            {
                driver.Stop();
            }
            Thread.Sleep(1000);
        }

        public BadooViewModel()
        {
            startCommand = new RelayCommand(f => Start(), e => true);
            stopCommand = new RelayCommand(f => Stop(), e => true);
        }
    }
}
