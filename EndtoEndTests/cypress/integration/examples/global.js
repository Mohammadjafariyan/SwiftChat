
export const baseUrl="http://localhost:60518/Customer/Panel/Index?websiteToken=N09XVk1peG5Gc2FtQWhLSHk4MjIrclFubVNZa0VEWDBGdTZrR3NtNDhXbUhQY3EzUWR5bTJzc2d5UjYwelYxeHBGRWxiZ1dtZ0p0aHVrNGlXWmk0eWc9PQ==";


export function login(cy){
    cy.get('#username')
        .type('admin');

    cy.get('#password')
        .type('admin');

    cy.get('#login')
        .click();
}