package info.itest.www.pages;

import org.openqa.selenium.WebDriver;

public  class BasePage {
	protected WebDriver dr;
	protected String url;
	public BasePage(WebDriver driver){
		this.dr= driver;		
	}
	public void goTo(){
		this.dr.get(this.url);
	}
	
	public String currentUrl(){
		return this.dr.getCurrentUrl();
	}
	
}
