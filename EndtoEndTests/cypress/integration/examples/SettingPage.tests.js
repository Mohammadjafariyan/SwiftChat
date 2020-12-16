/// <reference types="cypress" />

import {baseUrl, login} from "./global";


context('Actions', () => {
  beforeEach(() => {
    cy.visit(baseUrl)
  })
  

  // https://on.cypress.io/interacting-with-elements

  it('حذف زبان', () => {

    login(cy);


    cy.location('pathname', {timeout: 500})
  .should('include', '/Customer/Panel/');

    cy.get('#settingpage').click();

    cy.location('pathname', {timeout: 500})
  .should('include', '/Customer/Panel/');


    cy.get('#workingHourSetting').click();

    // radio box 
    cy.get('#workingHourSetting_show').click();
    
    // radio box 
    cy.get('#workingHourSetting_hide').click();
    
    // radio box 
    cy.get('#workingHourSetting_sentMessage').click();
   
    cy.get('#workingHourSetting_sentMessageText').type("ساعت کاری ما به پایان رسیده است ، لطفا پیام خود را ارسال کنید تا در بازدید بعدی ، پاسخگوی شما باشیم");

    
    // radio box 
    cy.get('#workingHourSetting_sentForm').click();

    cy.get('#workingHourSetting_sentFormTopText').type("کاربر گرامی ؛ بخش پشتیبانی در حال حاضر امکان پاسخگویی به شما را ندارد. لطفا مشخصات خود را ارسال نمایید.");



    // another setting page
    cy.get('#activeAndInactivePages').click();


    


  })

})
