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
    public class BadooViewModel
    {
       
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


            if (MainViewModel.Instance.Settings.AmountOfBadooLikes <= 0)
            {
                MainViewModel.Instance.Settings.AmountOfBadooLikes = 50;
            }

            driver.Start(() => Work());
        }

        void Stop()
        {
            driver.Stop();
        }

        void Work()
        {
            CheckNotifyPopUp();         
            var likeBtn = driver.GetComponentByClassName(Badoo.likeBtnCSS);
            var disLikeBtn = driver.GetComponentByClassName(Badoo.disLikeBtnCSS);
           

            if (MainViewModel.Instance.Settings.NothingBanned)
            {
                RollForLike(likeBtn, disLikeBtn);
                return;
            }

            var online = driver.GetComponentByClassName(Badoo.onlineCSS);
            if (online == null)
            {
                Dislike(disLikeBtn);
                return;
            }

            var bioElement = driver.GetComponentByClassName(Badoo.bioCSS);
            string bio;
            if (bioElement != null)
            {
                bio = bioElement.Text;
            }
            else
            {
                return;
            }

            var banList = MainViewModel.Instance.Settings.GetBannedWords();

            foreach (var item in banList)
            {
                if (bio.ToLower().Contains(item.ToLower()) || bio.Contains('@') && MainViewModel.Instance.Settings.BanInstaModels)
                {
                    Dislike(disLikeBtn);
                    return;
                }
            }
            RollForLike(likeBtn, disLikeBtn);
        }

        

        void RollForLike(IWebElement likeBtn, IWebElement dislikeBtn)
        {
            var roll = Random.Shared.Next(0, 101);

            if (roll <= MainViewModel.Instance.Settings.LikeChance)
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
                MainViewModel.Instance.Settings.AmountOfBadooLikes--;
                MainViewModel.Instance.About.LikesSentOnBadoo++;
                if (MainViewModel.Instance.Settings.AmountOfBadooLikes <= 0 && !MainViewModel.Instance.Settings.InfiniteLikes)
                {
                    driver.Stop();
                    return;
                }
            }          
            Thread.Sleep(1500);

            CheckForOutOfLikes();
            CheckForMatch();
        }

        void Dislike(IWebElement disLike)
        {
            if (disLike != null)
            {
                driver.ClickElement(disLike);
            }          
            Thread.Sleep(1500);
        }

        void CheckForMatch()
        {
            var match = driver.GetComponentByClassName(Badoo.matchCss);

            if (match != null)
            {
                MainViewModel.Instance.About.MatchesOnBadoo++;
                driver.ClickElement(match);
            }
        }

        void CheckForOutOfLikes()
        {
            var outOfLikes = driver.GetComponentByClassName(Badoo.outOfLikesCss);

            if (outOfLikes != null)
            {
                driver.Stop();
            }
        }

        void CheckNotifyPopUp()
        {
            var pop = driver.GetComponentByClassName(Badoo.notifyPopUpCss);

            if (pop != null)
            {
                driver.ClickElement(pop);
                Thread.Sleep(1000);
            }
        }     

        internal void DesroyDriver()
        {
            driver.Quit();
        }

        public BadooViewModel()
        {
            startCommand = new RelayCommand(f => Start(), e => true);
            stopCommand = new RelayCommand(f => Stop(), e => true);
        }
    }
}
