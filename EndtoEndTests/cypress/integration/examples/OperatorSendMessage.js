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

context("ارسال پیغام توسط ادمین", () => {
  beforeEach(() => {
    cy.window().then((win) => {
      win.onbeforeunload = null;
    });

    cy.on("uncaught:exception", (err, runnable) => {
      return false;
    });

    url= loginOperator(cy);
  });

  it("ورود اپراتور", () => {



    cy.get("#answered").click();
  });
});
