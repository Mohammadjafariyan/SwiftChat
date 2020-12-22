import {
  SampleHtml,
  Adminlogin,
  loginPage,
  myhost,
  _MyLogout,
  SuperAdminlogin,
  TimeToOpenChatBox,
  customerSendTextMessage,
  loginOperator
} from "./global";

import 'cypress-file-upload';

//

context("ارسال پیغام توسط کاربر", () => {
  beforeEach(() => {
    cy.on("uncaught:exception", (err, runnable) => {
      return false;
    });
  });

  it("ارسال متنی", () => {
    customerSendTextMessage(cy);
  });


  it("ورود اپراتور", () => {
    loginOperator(cy);
  });
});
