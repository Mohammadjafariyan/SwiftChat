import { SampleHtml,Adminlogin, loginPage, myhost ,_MyLogout,SuperAdminlogin} from "./global";
//


context(' قالب ایمیل', () => {
    beforeEach(() => {
        SuperAdminlogin(cy);

      cy.on('uncaught:exception', (err, runnable) => {
        return false;
      });

      
    })
    

    it('ثبت قالب ایمیل', () => {

  cy.get('#EmailTemplates').click(); // => true
  cy.get('#newEmailTemplate').click(); // => true
    
        cy.get('input[name*="Title"]').type("فراموشی رمز عبور");
 //  cy.get('textarea[name*="Html"]').paste(SampleHtml,{ parseSpecialCharSequences: false });
        cy.get('textarea[name*="Html"]').invoke('val', SampleHtml).trigger('change')
            
        cy.get('#submit').click();
    });    

});