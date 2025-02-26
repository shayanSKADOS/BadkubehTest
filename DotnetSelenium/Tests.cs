using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;

namespace DotnetSelenium
{
    public class Tests : IDisposable
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(7));
            _driver.Manage().Window.Maximize();
        }

        [Test]
        public void Test1()
        {
            NavigateToUrl("https://sandbox.kowork.work");
            ClickLoginButton();
            EnterPhoneNumber("09114437522");
            SelectEmployer();
            EnterPassword("shayan1382");
            ClosePopUpsIfNeeded();
            CreateNewOrder();
            FillBusinessName("انبوه سازان پارس");
            SelectCategory();
            FillSubject("طراحی لوگو برای شرکت ساختمانی");
            ValidateSubjectAndCategory();
            GoToNextPage(By.Id("generated-id-30"));
            GoToNextPage(By.Id("generated-id-28"), 300);
            UploadFile("D:\\images.png");
            GoToNextPage(By.Id("generated-id-28"));
            ValidateNextPage();
            GoToNextPage(By.Id("generated-id-32"));
            Check();
            NavigateToHomePageIfNeeded();
            ValidateFinalPage();
        }

        private void Check()
        {
            var understood = _driver.FindElements(By.Id("generated-id-33"));
            if (understood.ToArray().Length != 0) understood.ToArray()[0].Click();
            else _driver.FindElement(By.XPath("/html/body/nav/div/a")).Click();
        }

        private void GoToNextPage(By locator, int time = 10)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(time));
            wait.Until(ExpectedConditions.ElementToBeClickable(locator)).Click();
        }
        private void NavigateToUrl(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        private void ClickLoginButton()
        {
            ClickElement(By.ClassName("black"));
        }

        private void EnterPhoneNumber(string phoneNumber)
        {
            EnterText(By.Id("phone"), phoneNumber);
            SubmitForm(By.Id("phone"));
        }

        private void SelectEmployer()
        {
            ClickElement(By.CssSelector("#generated-id-3"));
        }

        private void EnterPassword(string password)
        {
            WaitForElement(By.Id("generated-id-5")).Click();
            EnterText(By.Name("password"), password);
            SubmitForm(By.Name("password"));
        }

        private void ClosePopUpsIfNeeded()
        {
            if (IsElementVisible(By.ClassName("swal2-popup")))
            {
                ClickElement(By.CssSelector(".swal2-cancel"));
                CloseToastIfVisible();
                ClickElement(By.XPath("/html/body/div[4]/button"));
            }
        }

        private void CreateNewOrder()
        {
            ClickElement(By.Id("generated-id-8"));
            if (IsElementVisible(By.Id("generated-id-3")))
            {
                ClickElement(By.Id("generated-id-3"));
            }
            if (IsElementVisible(By.Id("AI_fill_form")))
            {
                ClickElement(By.Id("AI_fill_form"));
            }
        }

        private void FillBusinessName(string businessName)
        {
            EnterText(By.Id("business_name"), businessName);
            ClickElement(By.Id("generated-id-29"));
            ClearText(By.Id("business_name"));
            if (!IsElementVisible(By.Id("business_name-error")))
            {
                Assert.Fail("Business name error not shown.");
            }
            EnterText(By.Id("business_name"), businessName);
        }

        private void SelectCategory()
        {
            ClickElement(By.CssSelector(".selectize-control.form-control-proj.categories.public_searchable_dropdown"));
            ClickElement(By.CssSelector("div[data-value=\"1\"]"));
        }

        private void FillSubject(string subject)
        {
            EnterText(By.Id("subject-ai"), subject);
            ClickElement(By.Id("generated-id-29"));
        }

        private void ValidateSubjectAndCategory()
        {
            var subjectText = GetElementInnerText(By.Id("subject"));
            string[] wordsToCheck = { "لوگو", "طراحی", "انبوه سازان پارس" };
            foreach (var word in wordsToCheck)
            {
                ClassicAssert.IsTrue(subjectText.Contains(word), $"Textarea doesn't contain: {word}");
            }

            var categoryText = GetElementText(By.XPath("//*[@id=\"tour-category\"]/div/div[1]/div"));
            ClassicAssert.IsTrue(categoryText.Contains("طراحی گرافیک و بصری"), $"Category doesn't contain: {categoryText}");

            var subCategoryText = GetElementText(By.XPath("//*[@id=\"tour-sub_category\"]/div/div[1]/div"));
            ClassicAssert.IsTrue(subCategoryText.Contains("طراحی لوگو"), $"Subcategory doesn't contain: {subCategoryText}");
        }

        private void UploadFile(string filePath)
         {
            var fileInput = _driver.FindElement(By.ClassName("dz-hidden-input"));
            fileInput.SendKeys(filePath);
            WaitForElement(By.XPath("//*[@id=\"createP\"]/form/main/div/div[5]/div[2]/div[2]/div[2]/div[1]/div[2]/div[5]"));
            ClickElement(By.Id("generated-id-28"));
        }

        private void ValidateNextPage()
        {
            ClassicAssert.AreEqual("انبوه سازان پارس", GetElementText(By.Id("business-name")), "Business name is not correct.");
            ClassicAssert.AreEqual("مارکتینگ", GetElementText(By.Id("field-of-activity")), "Field of activity is not correct.");
            ClassicAssert.AreEqual("طراحی لوگو برای شرکت ساختمانی انبوه سازان پارس", GetElementText(By.Id("title")), "Title is not correct.");

            var image = _driver.FindElement(By.XPath("//*[@id=\"show_files\"]/div/div[1]/img"));
            ClassicAssert.AreEqual("images.png", image.GetAttribute("alt"), "Image is not correct.");
        }

        private void NavigateToHomePageIfNeeded()
        {
            var closeButtons = _driver.FindElements(By.XPath("/html/body/nav/div/a"));
            if (closeButtons.Count > 0)
            {
                closeButtons[0].Click();
            }
        }

        private void ValidateFinalPage()
        {
            if (IsElementVisible(By.ClassName("swal2-popup")))
            {
                ClickElement(By.CssSelector(".swal2-cancel"));
                CloseToastIfVisible();
                ClickElement(By.XPath("/html/body/div[4]/button"));
            }

            ClassicAssert.AreEqual("طراحی لوگو برای شرکت ساختمانی انبوه سازان پارس", GetElementText(By.XPath("//*[@id=\"generated-id-17\"]")), "Title is not correct.");
        }

        private void ClickElement(By locator)
        {
            WaitForElement(locator).Click();
        }

        private void EnterText(By locator, string text)
        {
            WaitForElement(locator).SendKeys(text);
        }

        private void SubmitForm(By locator)
        {
            WaitForElement(locator).Submit();
        }

        private void ClearText(By locator)
        {
            WaitForElement(locator).SendKeys(Keys.Control + "a" + Keys.Backspace);
        }

        private IWebElement WaitForElement(By locator)
        {
            return _wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        private bool IsElementVisible(By locator)
        {
            try
            {
                return _wait.Until(ExpectedConditions.ElementIsVisible(locator)) != null;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        private string GetElementText(By locator)
        {
            return WaitForElement(locator).Text;
        }

        private string GetElementInnerText(By locator)
        {
            return WaitForElement(locator).GetAttribute("value");
        }

        private void CloseToastIfVisible()
        {
            var closeButtons = _driver.FindElements(By.ClassName("toast-close-button"));
            if (closeButtons.Count > 0)
            {
                closeButtons[0].Click();
            }
        }

        public void Dispose()
        {
            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}