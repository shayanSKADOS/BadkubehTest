using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V130.Autofill;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace DotnetSelenium
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            IWebDriver driver = new ChromeDriver();

            driver.Navigate().GoToUrl("https://sandbox.kowork.work");

            driver.Manage().Window.Maximize();

            var loginbtn = driver.FindElement(By.ClassName("black"));
            loginbtn.Click();

            //insert phone number
            var phoneNum = driver.FindElement(By.Id("phone"));
            phoneNum.SendKeys("09114437522");
            phoneNum.Submit(); //or we can use driver.FindElement(By.Id("submit_phone")).Click();

            //selecting employer
            driver.FindElement(By.CssSelector("#generated-id-3")).Click();

            //using password
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("generated-id-5"))).Click();

            //fill password
            var passFill = driver.FindElement(By.Name("password"));
            passFill.SendKeys("shayan1382");
            passFill.Submit(); //or we can use driver.FindElement(By.Id("generated-id-2")).Click();

            //close pup ups if needed
            if(CheckForPupUp(driver))
            {
                driver.FindElement(By.Id("generated-id-50")).Click();
                driver.FindElement(By.Id("generated-id-55")).Click();
            }

            //making new order
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("generated-id-8"))).Click();

            //check if there is draft order
            if (NewOrder(driver))
            {
                driver.FindElement(By.Id("generated-id-3")).Click();
            }

            //clicking on AI assistant fill form if existing
            if (AI(driver))
            {
                driver.FindElement(By.Id("AI_fill_form")).Click();
            }

            //filling the 1st form
            driver.FindElement(By.Id("business_name")).SendKeys("test");
            driver.FindElement(By.Id("generated-id-29")).Click(); // I did these tow lines because if we don't use the submit btn when the BN is filled, we won't see the business name error (this is a website bug i believe)
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("business_name"))).SendKeys(Keys.Control + "a" + Keys.Backspace);
            if(!BNError(driver))
            {
                Console.WriteLine("business name error");
            }

            //filling the B.Name
            driver.FindElement(By.Id("business_name")).SendKeys("انبوه سازان پارس");
            
            //selecting B
            driver.FindElement(By.CssSelector(".selectize-control.form-control-proj.categories.public_searchable_dropdown")).Click();
            driver.FindElement(By.CssSelector("div[data-value=\"1\"]")).Click(); //or using the keyboard to type then enter
            
            //checking the toast container
            driver.FindElement(By.Id("generated-id-29")).Click();
            if (!ToastContainer(driver))
            {
                Console.WriteLine("toast-container didn't show up!");
                driver.Quit();
            }
            
            //filling the subject
            driver.FindElement(By.Id("subject-ai")).SendKeys("طراحی لوگو برای شرکت ساختمانی");
            driver.FindElement(By.Id("generated-id-29")).Click();

            //checking needed words in subject
            var subject = driver.FindElement(By.Id("title"));
            string subjectText = subject.GetAttribute("innerText");
            string[] wordsToCheck = {"لوگو", "طراحی", "انبوه سازان پارس"};
            if (!subjectText.Contains(wordsToCheck[0])
                && !subjectText.Contains(wordsToCheck[1])
                && !subjectText.Contains(wordsToCheck[2]))
            {
                Console.WriteLine("Textarea doesn't contains: " + subjectText);
                driver.Quit();
            }

            //checking needed words in category
            var category = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"tour-category\"]/div/div[1]/div")));
            string categoryText = category.GetAttribute("innerText");
            string check = "طراحی گرافیک و بصری";
            if (!categoryText.Contains(check))
            {
                Console.WriteLine("Category doesn't contains: " + categoryText);
                driver.Quit();
            }

            //checking needed words in subCategory
            var subCategory = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"tour-sub_category\"]/div/div[1]/div")));
            string subCategoryText = subCategory.GetAttribute("innerText");
            string check2 = "طراحی لوگو";
            if (!subCategoryText.Contains(check2))
            {
                Console.WriteLine("Subcategory doesn't contains: " + subCategoryText);
                driver.Quit();
            }

            //clicking the next btn
            driver.FindElement(By.Id("generated-id-30")).Click();

            var waitForAI = new WebDriverWait(driver, TimeSpan.FromSeconds(300));
            waitForAI.Until(ExpectedConditions.ElementToBeClickable(By.Id("generated-id-28"))).Click();

            //3.14
        }

        static bool CheckForPupUp(IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("swal2-popup")));
                return true;
            }
            catch (WebDriverTimeoutException) { return false; }
        }
        static bool NewOrder(IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("generated-id-3")));
                return true;
            }
            catch (WebDriverTimeoutException) { return false; }
        }
        static bool AI(IWebDriver driver)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("AI_fill_form")));
                return true;
            }
            catch (WebDriverTimeoutException) { return false; }
        }
        static bool BNError(IWebDriver driver)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("business_name-error")));
                return true;
            }
            catch (WebDriverTimeoutException) { return false; }
        }
        static bool ToastContainer(IWebDriver driver)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("toast-container")));
                return true;
            }
            catch (WebDriverTimeoutException) { return false; }
        }


    }
}