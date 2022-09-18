
public class Tinder
{
    static bool premium;

    public static readonly string tLikeButtonCSS = "Bgi($g-ds-background-like):a";
    public static readonly string tDislikeButtonCSS = "Bgi($g-ds-background-nope):a";
    public static string tInfoButton => premium? "/html/body/div[1]/div/div[1]/div/main/div[1]/div/div/div[1]/div[1]/div/div[3]/div[4]/button" : "/html/body/div[1]/div/div[1]/div/main/div[1]/div/div/div[1]/div[1]/div/div[3]/div[3]/button";  

    public static readonly string tOnlineButtonCSS = "Bgc($c-active-indicator-green)";  
    public static readonly string tOnlineButtonXPath = "/html/body/div[1]/div/div[1]/div/main/div[1]/div/div/div[1]/div[1]/div/div[3]/div[4]/div/div[2]/div/div[1]/span";  
    public static readonly string tMatchFoundXPath = "/html/body/div[1]/div/div[1]/div/main/div[2]/main/div/div[1]/div/div[4]/button";  
    public static readonly string tUpgrade = "/html/body/div[2]/main/div/button[2]";  

    public static readonly string tTagsCSS = "Bdrs(100px)";
    public static readonly string tBioCSS = "BreakWord";
    public static readonly string tOutOfLikesBox = "/html/body/div[2]/main/div/div[3]/button[2]";
    public static readonly string tNotInterestedBoxCSS = "l17p5q9z";
    public static string tArrowBioBtn => premium? "/html/body/div[1]/div/div[1]/div/main/div[1]/div/div/div[1]/div[1]/div/div[1]/span/a" : "/html/body/div[1]/div/div[1]/div/main/div[1]/div/div/div[1]/div[1]/div/div[1]/span/a";
    public static readonly string tBoostTime = "/html/body/div[1]/div/div[1]/div/main/div[1]/div/div/div[1]/div[1]/div/div[4]/div";
    public static readonly string tBoostTime2 = "/html/body/div[1]/div/div[1]/div/main/div[1]/div/div/div[1]/div[1]/div/div[4]/div/div/div[1]";

    public static void SetPremiumStatus(bool p)
    {
        premium = p;
    }
}

public class Bumble
{
    public static readonly string bLikeBtn = "encounters-action--like";
    public static readonly string bDislikeBtn = "encounters-action--dislike";
    public static readonly string bBioClass = "encounters-story-about__text";
    public static readonly string bTagClass = "font-weight-medium";
    public static readonly string bScrollBtn = "/html/body/div/div/div[1]/main/div[2]/div/div/span/div[1]/article/div[2]/div[2]";
    public static readonly string bDeactivateBtnCSS = "button--filled";
    public static readonly string bOutOfLikesCSS = "encounters-user__blocker";
}

public class Badoo
{
    public static readonly string likeBtnCSS = "js-profile-header-vote-yes";
    public static readonly string disLikeBtnCSS = "js-profile-header-vote-no";
    public static readonly string bioCSS = "profile-section__txt-line";
    public static readonly string onlineCSS = "online-status--online";
    public static readonly string matchXPath = "/html/body/aside/section/div[1]/div/div[2]";
    public static readonly string matchCss = "ovl__close";
    public static readonly string notifyPopUpCss = "js-chrome-pushes-deny";
    public static readonly string outOfLikesCss = "js-ovl-close";
    public static readonly string outOfLikesXPath = "/html/body/aside/section/div[1]/div/div[2]/i/svg/use";
}
