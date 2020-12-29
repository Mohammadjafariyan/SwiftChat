import {
    SampleHtml,
    Adminlogin,
    loginPage,
    myhost,
    _MyLogout,
    SuperAdminlogin,
    TimeToOpenChatBox,
    loginOperator,
  } from "./global";
  
  
  //
  let url;
  
  context("تفکیک کاربران", () => {
    beforeEach(() => {
      cy.window().then((win) => {
        win.onbeforeunload = null;
      });
  
      cy.on("uncaught:exception", (err, runnable) => {
        return false;
      });
  
      url= loginOperator(cy);
    });
  
    it("تعریف تنظیمات", () => {
  
        cy.get("#usersSeparation").click();
        cy.get(".p-radiobutton-box").eq(0).click();
        cy.get("#url-pattern").type('http://localhost:60518/Security/Account/Login');
        
        cy.get(".p-radiobutton-box").eq(3).click();
        cy.get(".p-radiobutton-box").eq(3).click();
  

        cy.get("#addNewParam").click();
      //  cy.get(".p-radiobutton-box").eq(4).click();
        

        cy.get("input[name='paramName']").eq(0).type('.navbar-brand');
        cy.get("input[name='paramText']").eq(0).type('نام سایت');
        
        cy.get("#save").click();
        
    });
  });
  