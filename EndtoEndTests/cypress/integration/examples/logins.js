import { loginPage, myhost } from "./global";


context('Actions', () => {
    beforeEach(() => {
      cy.visit(loginPage)

      cy.on('uncaught:exception', (err, runnable) => {
        return false;
      });
    })
    
  
    // https://on.cypress.io/interacting-with-elements
  
    it('ورود ادمین', () => {
  
      cy.get('#Username')
      .type('admin@admin.com');

  cy.get('#Password')
      .type('admin@admin.com');

  cy.get('#login')
      .click();  

      cy.url().should('eq', `${myhost}Customer/Dashboard`) // => true


      
      cy.get('#profilelink')
      .click();

      cy.get('#logout')
      .click();


      cy.url().should('eq', `${myhost}`)



      cy.visit(`${myhost}Customer/Dashboard`)



      cy.url().should('include', `Login`)

    })



    it('ورود  ادمین سیستم', () => {
  
      cy.get('#sysAdminLogin')
      .click();


      cy.get('#Username')
      .type('superAdmin');

  cy.get('#Password')
      .type('$2Mv55s@a');

      cy.get('#login')
      .click();  

      cy.url().should('eq', `${myhost}Admin/AdminDashboard`) // => true

    })
  
  })
  