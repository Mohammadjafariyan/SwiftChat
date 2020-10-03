/// <reference types="cypress" />

context('Location', () => {
    beforeEach(() => {
      cy.visit('http://localhost:60518/Customer/Panel/Index?websiteToken=N09XVk1peG5Gc2FtQWhLSHk4MjIrcXpjQUJ3eWFYVWgyNGlCNjQ3dHg5RElybTY0M2NZUDFVNlYrSXFMdFhYVUxmU3RvbytHeWx3Nm9DL0tpMGpGS0E9PQ==')
    })
  


    it('cy.hash() - get the current URL hash', () => {
      // https://on.cypress.io/hash


        cy.get('#username').type('admin');
        cy.get('#password').type('admin');
        cy.get('#login').click();






        cy.get('#formCreatorButton').click();

      cy.get('#title').type('فرم ایمیل');
      cy.get('#beforeMsg').type('در صورتی که آفلاین بودیم');
      cy.get('#afterMsg').type('باتشکر ایمیل شما دریافت شد');




        cy.get('#addElementInput').click();



        cy.get('.editElement').eq(0).click();

        cy.get('#fieldTitle').type('ایمیل');
        cy.get('#fieldHelp').type('لطفا ایمیل خود را وارد نمایید');
        cy.get('#fieldName').type('Email');

        cy.get('#saveModal').click();


        /*text area*/
        cy.get('#addElementTextArea').click();
      
        cy.get('.editElement').eq(1).click();

        cy.get('#fieldTitle').type('توضیحات');
        cy.get('#fieldHelp').type('لطفا توضیحات خود را وارد نمایید');
        cy.get('#fieldName').type('Description');

        cy.get('#saveModal').click();



        /*checkbox*/

        cy.get('#addElementCheckBox').click();
        cy.get('.editElement').eq(2).click();


        cy.get('#fieldTitle').type('checkbox');
        cy.get('#fieldHelp').type('checkboxHelp');
        cy.get('#fieldName').type('checkbox');
        
        
            cy.get('#addSubElements').click();
            cy.get('#addSubElements').click();


        cy.get('.subelement').eq(0).type('گزینه 1');
        cy.get('.subelement').eq(1).type('گزینه 1');
       
        cy.get('.subelement').eq(2).type('گزینه 2');
        cy.get('.subelement').eq(3).type('گزینه 2');
       
        cy.get('#saveModal').click();



        /*radio*/
        cy.get('#addElementRadio').click();

        cy.get('.editElement').eq(3).click();


        cy.get('#fieldTitle').type('radio');
        cy.get('#fieldHelp').type('radioHelp');
        cy.get('#fieldName').type('radio');

            cy.get('#addSubElements').click();
            cy.get('#addSubElements').click();


        cy.get('.mymodal').find('.subelement').eq(0).type('گزینه 1');
        cy.get('.mymodal').find('.subelement').eq(1).type('گزینه 1');

        cy.get('.mymodal').find('.subelement').eq(2).type('گزینه 2');
        cy.get('.mymodal').find('.subelement').eq(3).type('گزینه 2');
        
        
        cy.get('#saveModal').click();

        cy.get('#save').click();



    })
  

  })
  