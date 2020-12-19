import { Adminlogin, loginPage, myhost ,_MyLogout,SuperAdminlogin} from "./global";


context('تیکت ها', () => {
    beforeEach(() => {
      cy.visit(loginPage)

      cy.on('uncaught:exception', (err, runnable) => {
        return false;
      });
    })
    
    
  it('ارسال تیکت اپراتور', () => {

    Adminlogin(cy);


    cy.get('#sendTicketPage').click();


    addNewTicket(cy);
        
    
    _MyLogout(cy);


    // -------------------- super admin ticket
    SuperAdminlogin(cy);

    cy.get('#sendTicketPage').click();
   
    cy.get('.visitTicket').first().click();

        addNewTicket(cy);
            
        _MyLogout(cy);


    // -------------------- admin login again
    Adminlogin(cy);

    cy.get('#sendTicketPage').click();
    

  addNewTicket(cy);
        
    _MyLogout(cy);

  })

});


export function addNewTicket(cy){

    cy.get('input[name*="Title"]').type("sample Title");
    cy.get('textarea[name*="Body"]').type("sample Title");
    cy.get('button[type*="submit"]').click();
}