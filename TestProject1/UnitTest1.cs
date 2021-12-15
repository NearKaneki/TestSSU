using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace TestProject1
{
    public class Tests
    {
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Navigate().GoToUrl("https://www.citilink.ru/");
        }

        [Test]
        public void Login()
        {
            driver.FindElement(By.XPath(".//div[@class='HeaderMenu__buttons  HeaderMenu__buttons_user']")).Click();
            driver.FindElement(By.Name("login")).SendKeys("7917300");
            driver.FindElement(By.Name("pass")).SendKeys("123456789");
            Assert.IsFalse(driver.FindElements(By.XPath("//button[contains(@class, 'SignIn__button js--SignIn__action_sign-in  Button  jsButton Button_theme_primary Button_size_m Button_full-width') and not(@disabled)]")).Any(), "Кнопка не работает, так как введены не верные данные");
        }

        [Test]
        public void ChangeTown()
        {
            driver.FindElement(By.CssSelector("button[class=\"js--CitiesSearch-trigger MainHeader__open-text TextWithIcon\"]")).Click();
            var element = driver.FindElement(By.CssSelector("a[data-search=\"хвалынск\"]"));
            new Actions(driver).MoveToElement(element).Perform();
            driver.FindElement(By.CssSelector("a[data-search=\"хвалынск\"]")).Click();
            Assert.IsTrue(driver.FindElement(By.CssSelector("button[class=\"js--CitiesSearch-trigger MainHeader__open-text TextWithIcon\"]")).Text == "Хвалынск");
        }

        [Test]
        public void Shetki()
        {
            driver.FindElement(By.CssSelector("button[data-label=\"Каталог товаров\"]")).Click();
            var element = driver.FindElement(By.CssSelector("a[data-title=\"Красота и здоровье\"]"));
            new Actions(driver).MoveToElement(element).Perform();
            driver.FindElement(By.CssSelector("a[data-title=\"Зубные щетки\"]")).Click();
            var el1 = driver.FindElement(By.XPath("//*[@id='app-filter']/div/div/div[2]/div[2]/div/div[3]/div[2]/div[2]/input[1]"));
            el1.Clear();
            el1.SendKeys("999");
            var el2 = driver.FindElement(By.XPath("//*[@id='app-filter']/div/div/div[2]/div[2]/div/div[3]/div[2]/div[2]/input[2]"));
            el2.Clear();
            el2.SendKeys("1999");
            el2.SendKeys(Keys.Enter);
            new WebDriverWait(driver, TimeSpan.FromSeconds(5))
                .Until(x => driver.FindElement(By.XPath(".//*[text()='от 999 ₽ до 1 999 ₽']")));
            var vals = driver.FindElements(By.CssSelector("div[class=\"product_data__gtm-js product_data__pageevents-js  ProductCardVertical js--ProductCardInListing " +
                "ProductCardVertical_normal ProductCardVertical_shadow-hover ProductCardVertical_separated\"]"));
            bool good = true;
            foreach(var i in vals)
            {
                var arr=i.GetAttribute("data-params").Split(",");
                int price = int.Parse(arr[2].Substring(8));
                if(price<999 || price > 1999)
                {
                    good = false;
                }
            }
            Assert.IsTrue(good);
        }

        [TearDown]
        public void CleanUp()
        {
            driver.Quit();
        }
    }
}