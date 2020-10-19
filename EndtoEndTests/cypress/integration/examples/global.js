
export const baseUrl="http://localhost:60518/Customer/Panel/Index?websiteToken=N09XVk1peG5Gc2FtQWhLSHk4MjIrcEtJd0VZOXhaSFRzeldaMEh0a1FzOHd5WjBBZno5cWZHRk9FSm1uUnBSR05ZUVRjSmpwNUZqMnpKbGIxR2ZHaEE9PQ==";


export function login(cy){
    cy.get('#username')
        .type('admin');

    cy.get('#password')
        .type('admin');

    cy.get('#login')
        .click();
}