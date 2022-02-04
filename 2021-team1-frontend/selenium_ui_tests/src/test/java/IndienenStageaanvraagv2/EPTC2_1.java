package IndienenStageaanvraagv2;

import org.junit.jupiter.api.Test;
import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.chrome.ChromeDriver;
import org.openqa.selenium.interactions.Actions;
import java.util.concurrent.TimeUnit;
import static org.junit.jupiter.api.Assertions.assertEquals;

public class EPTC2_1 {

    @Test
    public void UCT1punt1() throws InterruptedException {
        System.setProperty("webdriver.chrome.driver","C:\\Drivers\\chromedriver.exe");
        WebDriver driver = new ChromeDriver();
        driver.get("http://localhost:4200/login");
        driver.manage().window().maximize();

        String detailsPage = "Stagebeheer - Bedrijf - Detail";
        //String loggedInTitle = "http://localhost:4200/#/coordinator/start";

        driver.findElement(By.id("email")).clear();
        driver.findElement(By.id("email")).sendKeys("SBP.PXL.STAGEBEDRIJF@GMAIL.COM");

        driver.findElement(By.id("password")).clear();
        driver.findElement(By.id("password")).sendKeys("Test123*");

        driver.findElement(By.id("btnSignIn")).click();

        TimeUnit.SECONDS.sleep(2);

        driver.findElement(By.linkText("AANVRAAG INDIENEN")).click();

        TimeUnit.SECONDS.sleep(2);


        //BEDRIJF
        driver.findElement(By.id("companyStreet")).sendKeys("Engelbamp");

        driver.findElement(By.id("companyHouseNumber")).sendKeys("58");

        driver.findElement(By.id("companyMailboxNumber")).sendKeys("1A");

        driver.findElement(By.id("companyZipCode")).sendKeys("3800");

        driver.findElement(By.id("companyCity")).sendKeys("Sint-Truiden");

        driver.findElement(By.id("companyCountry")).sendKeys("Belgium");




        //CONTACTPERSOON

        driver.findElement(By.id("contactCombobox")).sendKeys("Bill Gates");


        //BEDRIJFSPROMOTOR

        driver.findElement(By.id("promotorCombobox")).sendKeys("Bill Gates");



        //STAGEOPDRACHT

        driver.findElement(By.id("topicTitle")).sendKeys("Tester");

        driver.findElement(By.xpath("/html/body/app-root/div/app-company/div/nav/app-application-form/div/form/div[4]/div[2]/div[2]/div/label/input")).click();

        driver.findElement(By.id("topicAssignment")).sendKeys("Project lead / tester");


        //driver.findElement(By.id("environment0")).click();

        driver.findElement(By.xpath("/html/body/app-root/div/app-company/div/nav/app-application-form/div/form/div[4]/div[3]/div[1]/div/label")).click();
        /*WebElement element = driver.findElement(By.id("environment0"));
        JavascriptExecutor executor = (JavascriptExecutor)driver;
        executor.executeScript("arguments[0].click()", element);
        /*
        WebElement element = driver.findElement(By.id("environment0"));
        Actions actions = new Actions(driver);
        actions.moveToElement(element).click().build().perform();
        */
        //driver.findElement(By.id("topicEnvironmentOther")).sendKeys("Vrij");

        driver.findElement(By.id("topicDetails")).sendKeys("bla bla");

        driver.findElement(By.id("topicConditions")).sendKeys("bla bla");

        driver.findElement(By.id("topicResearchTheme")).sendKeys("test techniek");


        //OVERIG

        //driver.findElement(By.id("expecation2")).click();
        driver.findElement(By.xpath("        /html/body/app-root/div/app-company/div/nav/app-application-form/div/form/div[5]/div[1]/div[3]/div/label/input")).click();


        //WebElement radioBtn1 = driver.findElement(By.id("otherCountStudent1"));
        //((JavascriptExecutor) driver).executeScript("arguments[0].checked = true;", radioBtn1);
        WebElement element12 = driver.findElement(By.id("otherCountStudent1"));
        JavascriptExecutor executor12 = (JavascriptExecutor)driver;
        executor12.executeScript("arguments[0].click()", element12);

        driver.findElement(By.id("otherStudents")).sendKeys("Chirstopher pradhan");
        driver.findElement(By.id("otherRemarks")).sendKeys("bla bla");
        driver.findElement(By.xpath("/html/body/app-root/div/app-company/div/nav/app-application-form/div/form/div[5]/div[3]/div[3]/div/label/input")).click();


        driver.findElement(By.id("btnSend")).click();

        TimeUnit.SECONDS.sleep(1);

        String pageTitle = driver.getTitle();
        //String pageTitle = driver.getCurrentUrl();

        assertEquals(detailsPage,pageTitle);
    }

}