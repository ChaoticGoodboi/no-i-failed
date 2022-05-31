//using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;

namespace task3
{


    /*
     * Cтворити програму, в якій описати сценарії та покрити функціональними тестами, 
        блок фільтрації на будь-якому сайті (інтернет магазині) підкатегорії по першому 
        популярному товару та перевірити, що кількість відфільтрованих товарів дорівнює 
        цифрі відображеної біля назви імені товару ДО включення фільтра. Оцінити 
        покриття тестування програми.
        
        Покриття потреб - 100%
        Покриття можливих сценаріїв - 
        сценарій виставлення максимальної ціни та сценарій виставлення фільтру
    */

    [TestFixture]
    public class UnitTest1
    {
        private string url= "https:\\elmir.ua\\vacuum_cleaners";
        private FirefoxDriver driver = new FirefoxDriver("C:\\Program Files\\Mozilla Firefox\\Gecko");
        WebDriverWait wait;
        //SETUP: get to the first popular thingy
        [SetUp]
        public void SetUp()
        {
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(9));
            driver.Navigate().GoToUrl(url);
            wait.Until<IWebElement>((driver) => driver.FindElement(By.Id("logo")));
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }


        //TEST: 

        //TEST:the quantity of filtered things is exactly like the quantity pointed in the checkbox
        [Test]
        public void ChecknTick()
        {
            var filter = driver.FindElement(By.XPath("//ul[@class=\"filter-list\"]/li[not(@class=\"more\")]/a[@class=\"filter-value-link\"]"));// /a[@class=\"filter-value-link\"]
            string tmp = filter.FindElement(By.ClassName("vendor-count")).Text;
            tmp = tmp.Substring(1, tmp.Length - 2);
            int expected = int.Parse(tmp);
            filter.Click();
            wait.Until<IWebElement>((driver) => driver.FindElement(By.Name("gofilter"))).Click();
            int reality = driver.FindElement(By.Id("vitrina-tovars")).FindElements(By.TagName("li")).Count; 
            Assert.AreEqual(expected, reality);
        }

        [Test]
        public void MaxPriceCheck()
        {
            int maxPrice = int.Parse(driver.FindElement(By.Id("maxPrice")).GetAttribute("placeholder"));
            driver.Navigate().GoToUrl(driver.Url+"?orderby=cost&orderdir=desc");
            var a  =driver.FindElement(By.XPath("//*[@id=\"vitrina-tovars\"]/li[1]//*[contains(@class, \"tovar-price\")]/span")).FindElements(By.CssSelector("*:not([style*=\"display: none\"])"));
            string b = "";
            foreach(var z in a) { b += z.Text; }
            int actualmaxprice = int.Parse(b);
            Assert.IsTrue(maxPrice > actualmaxprice);
        }


        //TEST: 
    }
}
