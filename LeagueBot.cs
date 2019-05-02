using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueBot
{
    class LeagueBot
    {
        static public IWebDriver driver = new ChromeDriver("Driver");

        public LeagueBot()
        {
            SetupDriver();
            LoginToLeague();
            FarmRoadTwo();
        }

        static void SetupDriver()
        {
            driver.Url = (@"https://game.league17.ru/");
        }

        static void LoginToLeague()
        {
            string UserName = Console.ReadLine();
            string UserPassword = Console.ReadLine();
            IWebElement UName = driver.FindElement(By.Id("txtLogin"));
            IWebElement UPass = driver.FindElement(By.Id("txtPass"));
            IWebElement LoginButton = driver.FindElement(By.Id("btnSignin"));
            UName.SendKeys(UserName);
            UPass.SendKeys(UserPassword);
            LoginButton.Click();
            System.Threading.Thread.Sleep(10000);
            IWebElement StartButton = driver.FindElement(By.Id("aToGame"));
            StartButton.Click();
            System.Threading.Thread.Sleep(1000);
            IWebElement StartButton2 = driver.FindElement(By.ClassName("btnStart"));
            StartButton2.Click();
        }

        static void FarmRoadTwo()
        {
            EnubledWild();
            while (true)
            {
                try
                {
                    IWebElement BattleStatus = driver.FindElement(By.Id("divVisioFight"));
                    if (BattleStatus.GetAttribute("style") == "")
                    {
                        ReadOnlyCollection<IWebElement> Atacks = driver.FindElements(By.ClassName("divMoveTitle"));
                        ReadOnlyCollection<IWebElement> AtacksValues = driver.FindElements(By.ClassName("divMoveParams"));
                        for (int i = 0; i < Atacks.Count; i++)
                        {
                            if (Atacks[i].Text == "Атака крыльями")
                            {
                                System.Threading.Thread.Sleep(1000);
                                if (Int32.Parse(AtacksValues[i].Text.Split('/')[0]) != 0)
                                {
                                    Atacks[i].Click();
                                    System.Threading.Thread.Sleep(1000);
                                    CloseButton();
                                }
                                else
                                {
                                    DisableWild();
                                    Atacks[i - 1].Click();
                                    System.Threading.Thread.Sleep(1000);
                                    CloseButton();
                                    MoveToHealAndRoadTwo();
                                    EnubledWild();
                                }
                            }
                        }
                    }
                } catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private static void CloseButton()
        {
            System.Threading.Thread.Sleep(1000);
            ReadOnlyCollection<IWebElement> Buttons = driver.FindElements(By.ClassName("button"));
            foreach(IWebElement Button in Buttons)
            {
                if (Button.Text == "Закрыть")
                {
                    Button.Click();
                    break;
                }
            }
        }

        private static void MoveToHealAndRoadTwo()
        {
            System.Threading.Thread.Sleep(1000);
            string[] RoadsIds = { "btnGo18", "btnGo9", "btnGo1", "btnGo3" };
            IWebElement Road = driver.FindElement(By.Id("btnGo9"));
            for (int i = 1; i < RoadsIds.Length; i++)
            {
                System.Threading.Thread.Sleep(1000);
                Road = driver.FindElement(By.Id(RoadsIds[i]));
                Road.Click();
            }
            System.Threading.Thread.Sleep(1000);
            IWebElement Heal = driver.FindElement(By.ClassName("btnLocHeal"));
            Heal.Click();
            System.Threading.Thread.Sleep(1000);
            Heal = driver.FindElement(By.ClassName("menuHealAll"));
            Heal.Click();
            for (int i = RoadsIds.Length - 2; i >= 0; i--)
            {
                System.Threading.Thread.Sleep(1000);
                Road = driver.FindElement(By.Id(RoadsIds[i]));
                Road.Click();
            }

        }

        static void EnubledWild()
        {
            System.Threading.Thread.Sleep(1000);
            IWebElement WildMosntersEnuble = driver.FindElement(By.ClassName("btnSwitchWilds"));
            WildMosntersEnuble.Click();
        }

        static void DisableWild()
        {
            System.Threading.Thread.Sleep(1000);
            IWebElement WildMosntersEnuble = driver.FindElement(By.ClassName("btnSwitchWilds"));
            WildMosntersEnuble.Click();
        }

        static void Main(string[] args)
        {
            LeagueBot Bot = new LeagueBot();
        }
    }
}
