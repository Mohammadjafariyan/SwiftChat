/// <reference types="cypress" />

import {baseUrl, login} from "./global";

context('Actions', () => {
  beforeEach(() => {
    cy.visit(baseUrl)
  })
  

  // https://on.cypress.io/interacting-with-elements

  it('حذف زبان', () => {

    login(cy);

    cy.wait(3000)

    cy.get('#HelpDeskArticles').click();



    cy.get('#removeLanguage').click();



    cy.get('#removeLanguageConfirm').click();


  })

})
