export const myhost = "http://gapchat.ashpazerooz.ir/";

export const baseUrl =
  `${myhost}/Customer/Panel/Index?websiteToken=N09XVk1peG5Gc2FtQWhLSHk4MjIrclFubVNZa0VEWDBGdTZrR3NtNDhXbUhQY3EzUWR5bTJzc2d5UjYwelYxeHBGRWxiZ1dtZ0p0aHVrNGlXWmk0eWc9PQ==`;
export const loginPage = `${myhost}/Security/Account/Login`;

export function login(cy) {
  cy.get("#username").type("admin");

  cy.get("#password").type("admin");

  cy.get("#login").click();
}

export function Adminlogin(cy) {
  cy.visit(loginPage)

  cy.on("uncaught:exception", (err, runnable) => {
  return false;
});

cy.get("#Username").type("admin@admin.com");

cy.get("#Password").type("admin@admin.com");

cy.get("#login").click();

cy.url().should("eq", `${myhost}Customer/Dashboard`); // => true
}



export function _MyLogout(cy) {

      
cy.get('#profilelink')
.click();

cy.get('#logout')
.click();


cy.url().should('eq', `${myhost}`)
}

export function SuperAdminlogin(cy) {
    cy.visit(myhost)
    cy.get("#sysAdminLogin").click();

  cy.get("#Username").type("superAdmin");

  cy.get("#Password").type("$2Mv55s@a");

  cy.get("#login").click();

  cy.url().should("eq", `${myhost}Admin/AdminDashboard`); // => true
}
