/// <reference types="cypress" />

import {baseUrl, login} from "./global";

context('Actions', () => {
    beforeEach(() => {
        cy.visit(baseUrl)
    })


    // https://on.cypress.io/interacting-with-elements

    it('تفکیک کاربران', () => {

        login(cy);


        cy.get('#usersSeparation').click();


        cy.get('#enable').click();


        cy.get('#url-pattern').type("http://localhost:60518/dashboard");


        cy.get('#rest-api-type').click();

        
        cy.get('#rest-api').type("http://localhost:60518/FakeUserInfo/Index");

        
        /*add param1*/

        cy.get('#addNewParam').click();
       // cy.get('.param-type-rest').eq(0).click();

        cy.get('.param-name').eq(0).type('Phone');
        cy.get('.param-text').eq(0).type('شماره تلفن');



        /*add param2*/
        cy.get('#addNewParam').click();
      //  cy.get('.param-type-css').eq(1).click();

        cy.get('.param-name').eq(1).type('Email');
        cy.get('.param-text').eq(1).type('ایمیل کاربر');



        cy.get('#save').click();


    })

})


