using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinderBotGUI.Core;

namespace TinderBotGUI.MVVM.ViewModel
{
    public class AboutViewModel : ObservableObject
    {      

        int likesSentOnTinder;
        public int LikesSentOnTinder 
        {
            get => likesSentOnTinder;
            set
            {
                likesSentOnTinder = value;
                OnpropertyChanged();
            }
        }

        int likesSentOnBadoo;
        public int LikesSentOnBadoo
        {
            get => likesSentOnBadoo;
            set
            {
                likesSentOnBadoo = value;
                OnpropertyChanged();
            }
        }

        int matchesOnBadoo;
        public int MatchesOnBadoo
        {
            get => matchesOnBadoo;
            set
            {
                matchesOnBadoo = value;
                OnpropertyChanged();
            }
        }

        int matchesOnTinder;
        public int MatchesOnTinder
        {
            get => matchesOnTinder;
            set
            {
                matchesOnTinder = value;
                OnpropertyChanged();
            }
        }
       

        public string TinderLikeToMatch 
        {
            get
            {
                if (MatchesOnTinder == 0)
                {
                    return "0%";
                }

                return MathF.Round((MatchesOnTinder / LikesSentOnTinder), 2).ToString() + "%";               
            }
            set
            {                
                OnpropertyChanged();
            }
        }
        public string BadooLikeToMatch
        {
            get
            {
                if (MatchesOnBadoo == 0)
                {
                    return "0%";
                }
                return MathF.Round((MatchesOnBadoo / LikesSentOnBadoo), 2).ToString() + "%";            
            }
            set
            {               
                OnpropertyChanged();
            }
        }

        public AboutViewModel(int tLikes, int bLikes, int tMatches, int bMatches)
        {
            LikesSentOnTinder= tLikes;
            LikesSentOnBadoo= bLikes;
            MatchesOnTinder = tMatches;
            MatchesOnBadoo = bMatches;          
        }
    }
}
