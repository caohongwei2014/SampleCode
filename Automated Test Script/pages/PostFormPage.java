package info.itest.www.pages;

import org.openqa.selenium.By;
import org.openqa.selenium.JavascriptExecutor;
import org.openqa.selenium.WebDriver;
import org.openqa.selenium.WebElement;

public class PostFormPage extends BasePage {
	
	final String create_post_url = "http://localhost/wordpress/wp-admin/post-new.php";
	
	public PostFormPage(WebDriver driver){
		super(driver);		
	}
	
	By titleLocator =By.id("title");
	By publicBtnLocator =By.id("publish");
	By permalinLocator = By.id("sample-permalink");
	
	public WebElement getTitleTextField(){
		return this.dr.findElement(titleLocator);
	}
	
	public WebElement getPublishBtn(){
		return this.dr.findElement(publicBtnLocator);
	}
	
	public WebElement getPermalink(){
		return this.dr.findElement(permalinLocator);
	}
	
	public String createPost(String title, String content){
		this.dr.get(this.create_post_url);
		this.getTitleTextField().sendKeys(title);
		this.setContent(content);
		this.getPublishBtn().click();
		return this.fetch_post_id();
	}
	
	public void setContent(String content){
		String js = "document.getElementById('content_ifr').contentWindow.document.body.innerHTML='"+content +"'";
		((JavascriptExecutor)dr).executeScript(js);
	}
	
	public String fetch_post_id(){
		String postUrl=this.getPermalink().getText();
		String[] arr = postUrl.split("=");		
		return arr[1];
	}
	
	
	public String createPost(){
		this.dr.get(this.create_post_url);
		String title = "Title"+System.currentTimeMillis();
		String content = "Test content" + System.currentTimeMillis();
		return this.createPost(title, content);	
	}
	
	public String createPost(String title)
	{
		String content = "Test content" + System.currentTimeMillis();
		return this.createPost(title, content);				
	}
	
	
}
