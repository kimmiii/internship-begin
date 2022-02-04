package Login;


import org.junit.jupiter.api.Test;
import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.chrome.ChromeDriver;

import java.util.concurrent.TimeUnit;

import static org.junit.jupiter.api.Assertions.assertEquals;


public class UCT1_1 {


    @Test
    public void UCT1punt1() throws InterruptedException {
        System.setProperty("webdriver.chrome.driver","C:\\Drivers\\chromedriver.exe");
        WebDriver driver = new ChromeDriver();
        driver.get("http://localhost:4200/login");
        driver.manage().window().maximize();


        String loggedInTitle = "Stagebeheer - Coördinator - Start";
        //String loggedInTitle = "http://localhost:4200/#/coordinator/start";

        driver.findElement(By.id("email")).clear();
        driver.findElement(By.id("email")).sendKeys("STAGECOORDINATOR@outlook.com");

        driver.findElement(By.id("password")).clear();
        driver.findElement(By.id("password")).sendKeys("Test123*");

        driver.findElement(By.id("btnSignIn")).click();

        TimeUnit.SECONDS.sleep(3);

        String pageTitle = driver.getTitle();
        //String pageTitle = driver.getCurrentUrl();

        assertEquals(loggedInTitle,pageTitle);
    }

}