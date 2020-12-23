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
  
  import "cypress-file-upload";
  
  //
  let url;
  
  context("EventTrigger ها", () => {
    beforeEach(() => {
      cy.window().then((win) => {
        win.onbeforeunload = null;
      });
  
      cy.on("uncaught:exception", (err, runnable) => {
        return false;
      });
  
      cy.visit(myhost);
    });
   
    it("تست اجرا", () => {

        
    
        cy.wait(3000);
        cy.document().trigger('mouseleave');



    });


});