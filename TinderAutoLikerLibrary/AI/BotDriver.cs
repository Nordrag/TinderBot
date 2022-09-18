using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


#pragma warning disable CS8602
#pragma warning disable CS8603
public class BotDriver
{
    WebDriver? driver;
    public bool isWorking { get; private set; }
    public static readonly string tinderUrl = "https://tinder.onelink.me/9K8a/3d4abb81";
    public static readonly string bumbleUrl = "https://bumble.com/get-started";
    public static readonly string badooUrl = "https://badoo.com/";
    string driverPath => Environment.CurrentDirectory + @"\ChromiumDriver";


    public bool IsDriverReady { get; private set; }

    public void OpenSite(string Url)
    {
        driver.Navigate().GoToUrl(Url);
    }
   
    public void CreateDriver()
    {
        var options = new ChromeOptions();
        driver = new ChromeDriver(driverPath, options);
        IsDriverReady = true;
    }

    public void Quit()
    {
        driver?.Quit();
        driver?.Dispose();      
    }

    public IWebElement GetComponentByClassName(string css)
    {      
        try
        {
            return driver.FindElement(By.ClassName(css));
        }
        catch 
        {
            return null;
        }       
    }


    public IWebElement GetComponentByXPath(string xPath)
    {
        try
        {
            return driver.FindElement(By.XPath(xPath));
        }
        catch 
        {
            return null;
        }
    }

    public List<IWebElement> GetComponentsByClassName(string css)
    {
        try
        {
            return driver.FindElements(By.ClassName(css)).ToList();
        }
        catch 
        {
            return new List<IWebElement>();
        }
    }


    public ICollection<IWebElement> GetComponentsByXPath(string xPath)
    {
        try
        {
            return driver.FindElements(By.XPath(xPath));
        }
        catch 
        {
            return null;
        }
    }

    public void ClickElement(IWebElement element)
    {     
        try
        {
            element?.Click();
        }
        catch
        {           
            //isWorking = false;
        }
        
    }

    public void Start(Action a)
    {
        isWorking = true;
        Task.Run(() =>
        {
            while (isWorking)
            {
                a.Invoke();
            }
        });
    }

    public void Stop()
    {
        isWorking = false;
    }
}
