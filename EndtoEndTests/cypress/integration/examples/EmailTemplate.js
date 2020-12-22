import {
  SampleHtml,
  Adminlogin,
  loginPage,
  myhost,
  _MyLogout,
  SuperAdminlogin,
} from "./global";
//

context(" قالب ایمیل", () => {
  beforeEach(() => {
    SuperAdminlogin(cy);

    cy.on("uncaught:exception", (err, runnable) => {
      return false;
    });
  });

  it("ثبت قالب ایمیل", () => {
    cy.get("#EmailTemplates").click(); // => true
    cy.get("#newEmailTemplate").click(); // => true

    cy.get('input[name*="Title"]').type("فراموشی رمز عبور");
    //  cy.get('textarea[name*="Html"]').paste(SampleHtml,{ parseSpecialCharSequences: false });
    cy.get('textarea[name*="Html"]')
      .invoke("val", SampleHtml)
      .trigger("change");

    cy.get("#submit").click();
  });

  it("ارسال ایمیل", () => {
    cy.get("#sendEmailByTemplate").click(); // => true

    cy.get('[type="checkbox"]').check();
    cy.get("#sendEmailbyTemplateSubmit").click();

    cy.url().should("include", `${myhost}Email/SendEmail/Success`); // => true

    cy.get("#showSents").click();
  });

  it("نمایش ارسال ها", () => {
    cy.get("#sentEmailsList").click({ force: true }); // => true

    cy.get(".SentEmailList").first().click();
  });

  it("حذف قالب  ها", () => {
    cy.get("#EmailTemplates").click(); // => true

    cy.get(".deleteTemplate").each(($el, index, $list) => {
      $el.click();
    });

    cy.get('#title').should(($lis) => {
      expect($lis.eq(0)).to.contain('ایمیل')
    })

    for (let i = 0; i < 10; i++) {
      cy.get(".deleteTemplate").each(($el, index, $list) => {
        $el.click();
      });
      
    }


  });


  it("نمایش ارسال ها", () => {
    cy.get("#sentEmailsList").click({ force: true }); // => true

  
    cy.get(".SentEmailList").eq(0).click(); // => true
    cy.get("#backToList").click(); // => true
    
    
  });
});
