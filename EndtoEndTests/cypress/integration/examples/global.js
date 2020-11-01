
export const baseUrl="http://localhost:60518/Customer/Panel/Index?websiteToken=N09XVk1peG5Gc2FtQWhLSHk4MjIrbFpXMHRTL3M4T0xyWi9IU3UzS25peFJucy9ZRGYxS0VzR05aT2ViendXMTN4eXFLQUFZVDljQ0lVa0QzRUJXZkE9PQ==#";


export function login(cy){
    cy.get('#username')
        .type('admin');

    cy.get('#password')
        .type('admin');

    cy.get('#login')
        .click();
}