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
  
      url= loginOperator(cy);
    });

    it("در رویداد خاص شما", () => {

        cy.wait(3000);
        cy.get("#EventTrigger").click();
        cy.wait(1000);
      cy.get("#AddNewEventTrigger").click();
      cy.wait(1000);
      cy.get("#deleteEventTrigger").click();

    

      cy.get("#AddNewEventTrigger").click();


      
      cy.get("input[name='Name']").click().clear().type('در رویداد خاص شما');

      cy.get('.MySwitcher').within(($list) => {
        
        // --------------- افزودن متن به زبان -----------------
        cy.get(".p-inputswitch").eq(0).click();
        cy.get("#addMsg").click();

        cy.get(".p-dropdown").click();
        cy.get(".p-dropdown-filter.p-inputtext.p-component").type('iran');

        cy.get(".p-dropdown-item").click();
        cy.get(".p-inputtextarea").type('با رویداد خاص کاربر event trigger fired');
        
        cy.get("#removeLanguageConfirm").click();
        // --------------- end--- افزودن متن به زبان -----------------
        

        cy.get(".p-inputswitch").eq(7).click();

        cy.get("#addNew").click();

        cy.get("input[name='ModalName']").click().clear().type('onCustomEventFired');
        cy.get("#addRecordOk").click();
        cy.get("#removeLanguageConfirm").click();
        
        
        cy.get(".p-inputswitch").eq(8).click();
        cy.get(".p-inputswitch").eq(9).click();
        cy.get(".p-inputswitch").eq(10).click();
        cy.get(".p-inputswitch").eq(11).click();
        cy.get(".p-inputswitch").eq(12).click();

        cy.get('input[role="spinbutton"]').type(10);


               // --------------- افزودن کشور  -----------------
              // cy.get(".p-inputswitch").eq(0).click();
               cy.get("#addMsg").eq(1).click();

               
       
               cy.get(".p-dropdown").click();
               cy.get(".p-dropdown-filter.p-inputtext.p-component").type('iran');
               cy.get(".p-dropdown-item").click();
               cy.get(".p-inputtextarea ").type('متن تستی event trigger is fired ');
               
               cy.get("#removeLanguageConfirm").click();
               // --------------- end--- افزودن  کشور  -----------------


               
      })
      cy.get("#saveEventTrigger").click();
      
 
     cy.get('.text-black').each((m)=>{
        m.click();

     })

    });
    it("با پارامتر های خاص", () => {

        cy.wait(3000);
        cy.get("#EventTrigger").click();
        cy.wait(1000);
      cy.get("#AddNewEventTrigger").click();
      cy.wait(1000);
      cy.get("#deleteEventTrigger").click();

    

      cy.get("#AddNewEventTrigger").click();


      
      cy.get("input[name='Name']").click().clear().type('با پارامتر های خاص');

      cy.get('.MySwitcher').within(($list) => {
        
        // --------------- افزودن متن به زبان -----------------
        cy.get(".p-inputswitch").eq(0).click();
        cy.get("#addMsg").click();

        cy.get(".p-dropdown").click();
        cy.get(".p-dropdown-filter.p-inputtext.p-component").type('iran');

        cy.get(".p-dropdown-item").click();
        cy.get(".p-inputtextarea").type('با پارامتر های خاص event trigger fired');
        
        cy.get("#removeLanguageConfirm").click();
        // --------------- end--- افزودن متن به زبان -----------------
        

        cy.get(".p-inputswitch").eq(6).click();
        cy.get(".p-inputswitch").eq(7).click();

        cy.get("#addNew").click();

        cy.get("input[name='ModalName']").click().clear().type('lang');
        cy.get("input[name='Name']").click().clear().type('IR');
        cy.get("#addRecordOk").click();
        cy.get("#removeLanguageConfirm").click();
        
        
        cy.get(".p-inputswitch").eq(8).click();
        cy.get(".p-inputswitch").eq(9).click();
        cy.get(".p-inputswitch").eq(10).click();
        cy.get(".p-inputswitch").eq(11).click();
        cy.get(".p-inputswitch").eq(12).click();

        cy.get('input[role="spinbutton"]').type(10);


               // --------------- افزودن کشور  -----------------
              // cy.get(".p-inputswitch").eq(0).click();
               cy.get("#addMsg").eq(1).click();

               
       
               cy.get(".p-dropdown").click();
               cy.get(".p-dropdown-filter.p-inputtext.p-component").type('iran');
               cy.get(".p-dropdown-item").click();
               cy.get(".p-inputtextarea ").type('متن تستی event trigger is fired ');
               
               cy.get("#removeLanguageConfirm").click();
               // --------------- end--- افزودن  کشور  -----------------


               
      })
      cy.get("#saveEventTrigger").click();
      
 
     cy.get('.text-black').each((m)=>{
        m.click();

     })

    });

    it("تعریف کلیک روی لینک", () => {

        cy.wait(3000);
        cy.get("#EventTrigger").click();
        cy.wait(1000);
      cy.get("#AddNewEventTrigger").click();
      cy.wait(1000);
      cy.get("#deleteEventTrigger").click();

    

      cy.get("#AddNewEventTrigger").click();


      
      cy.get("input[name='Name']").click().clear().type('تست کلیک روی لینک');

      cy.get('.MySwitcher').within(($list) => {
        
        // --------------- افزودن متن به زبان -----------------
        cy.get(".p-inputswitch").eq(0).click();
        cy.get("#addMsg").click();

        cy.get(".p-dropdown").click();
        cy.get(".p-dropdown-filter.p-inputtext.p-component").type('iran');

        cy.get(".p-dropdown-item").click();
        cy.get(".p-inputtextarea").type('event trigger کلیک روی یک لینک اتفاق افتاد');
        
        cy.get("#removeLanguageConfirm").click();
        // --------------- end--- افزودن متن به زبان -----------------
        
        cy.get(".p-inputswitch").eq(1).click();
        cy.get(".p-inputswitch").eq(2).click();

        cy.get(".p-inputswitch").eq(3).click();
        cy.get(".p-inputswitch").eq(5).click();

        cy.get("#addNew").click();

        cy.get("input[name='ModalName']").type('#login');
        cy.get("#addRecordOk").click();
        cy.get("#removeLanguageConfirm").click();
        
        
        cy.get(".p-inputswitch").eq(6).click();
        cy.get(".p-inputswitch").eq(7).click();
        cy.get(".p-inputswitch").eq(8).click();
        cy.get(".p-inputswitch").eq(9).click();
        cy.get(".p-inputswitch").eq(10).click();
        cy.get(".p-inputswitch").eq(11).click();
        cy.get(".p-inputswitch").eq(12).click();

        cy.get('input[role="spinbutton"]').type(15);


               // --------------- افزودن کشور  -----------------
              // cy.get(".p-inputswitch").eq(0).click();
               cy.get("#addMsg").eq(1).click();

               
       
               cy.get(".p-dropdown").click();
               cy.get(".p-dropdown-filter.p-inputtext.p-component").type('iran');
               cy.get(".p-dropdown-item").click();
               cy.get(".p-inputtextarea ").type('متن تستی event trigger is fired ');
               
               cy.get("#removeLanguageConfirm").click();
               // --------------- end--- افزودن  کشور  -----------------


               
      })
      cy.get("#saveEventTrigger").click();
      
 
     cy.get('.text-black').each((m)=>{
        m.click();

     })

    });
  
    it("تعریف و حذف", () => {
  


        cy.wait(3000);
        cy.get("#EventTrigger").click();
        cy.wait(1000);
      cy.get("#AddNewEventTrigger").click();
      cy.wait(1000);
      cy.get("#deleteEventTrigger").click();

    

      cy.get("#AddNewEventTrigger").click();


      
      cy.get("input[name='Name']").click().clear().type('زمان ترک کردن و در صفحهات خاص بعد از 10 ثانیه با تمامی شروط و کشور ایران');

      cy.get('.MySwitcher').within(($list) => {
        
        // --------------- افزودن متن به زبان -----------------
        cy.get(".p-inputswitch").eq(0).click();
        cy.get("#addMsg").click();

        cy.get(".p-dropdown").click();
        cy.get(".p-dropdown-filter.p-inputtext.p-component").type('iran');

        cy.get(".p-dropdown-item").click();
        cy.get(".p-inputtextarea").type('متن تستی event trigger is fired ');
        
        cy.get("#removeLanguageConfirm").click();
        // --------------- end--- افزودن متن به زبان -----------------
        
        cy.get(".p-inputswitch").eq(1).click();
        cy.get(".p-inputswitch").eq(2).click();

        cy.get(".p-inputswitch").eq(3).click();
        cy.get(".p-inputswitch").eq(4).click();
        cy.get(".p-inputswitch").eq(5).click();

        cy.get("#addNew").click();

        cy.get("input[name='ModalName']").type('login');
        cy.get("#addRecordOk").click();
        cy.get("#removeLanguageConfirm").click();
        
        
        cy.get(".p-inputswitch").eq(6).click();
        cy.get(".p-inputswitch").eq(7).click();
        cy.get(".p-inputswitch").eq(8).click();
        cy.get(".p-inputswitch").eq(9).click();
        cy.get(".p-inputswitch").eq(10).click();
        cy.get(".p-inputswitch").eq(11).click();
        cy.get(".p-inputswitch").eq(12).click();

        cy.get('input[role="spinbutton"]').type(10);


               // --------------- افزودن کشور  -----------------
              // cy.get(".p-inputswitch").eq(0).click();
               cy.get("#addMsg").eq(1).click();

               
       
               cy.get(".p-dropdown").click();
               cy.get(".p-dropdown-filter.p-inputtext.p-component").type('iran');
               cy.get(".p-dropdown-item").click();
               cy.get(".p-inputtextarea ").type('متن تستی event trigger is fired ');
               
               cy.get("#removeLanguageConfirm").click();
               // --------------- end--- افزودن  کشور  -----------------


               
      })
      cy.get("#saveEventTrigger").click();
      
 
     cy.get('.text-black').each((m)=>{
        m.click();

     })

      

      

    });

    it("فعالسازی event trigger", () => {

        
        cy.wait(3000);
        cy.get("#EventTrigger").click();
        cy.wait(1000);


        cy.get(".p-inputswitch").eq(0).click();
        cy.get(".p-inputswitch").eq(1).click();
        cy.get(".p-inputswitch").eq(4).click();
        cy.get(".p-inputswitch").eq(3).click();


    });

   
    
  });
  