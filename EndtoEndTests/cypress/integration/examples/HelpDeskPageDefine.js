import {
  SampleHtml,
  Adminlogin,
  loginPage,
  myhost,
  _MyLogout,
  SuperAdminlogin,
  TimeToOpenChatBox,
  customerSendTextMessage,
  loginOperator,
} from "./global";

import "cypress-file-upload";

//
let url;
context("ارسال پیغام توسط کاربر", () => {
  beforeEach(() => {
    cy.on("uncaught:exception", (err, runnable) => {
      return false;
    });
    url = loginOperator(cy);
    cy.get("#HelpDeskPage").click();
  });


  it("تعریف help desk", () => {

    cy.wait(1000);

    cy.get(".p-dropdown").click();

    cy.get(".p-dropdown-item").eq(0).click();
  

    cy.wait(1000);
    cy.get('#removeLanguage').click();
    cy.get('#removeLanguageConfirm').click();
    
    cy.wait(1000);

    
  });

  it("تعریف و افزودن مقادیر help desk", () => {

    cy.wait(1000);

    cy.get(".p-dropdown").click();

    cy.get(".p-dropdown-item").eq(0).click();
  

    cy.wait(1000);
 
    addContent(cy);

    
  });

  
 

 /*  it("چند help desk", () => {
    cy.get(".p-dropdown").click();

    cy.get(".p-dropdown-item").eq(0).click();
  
    cy.get('#removeLanguage').click();


    AddNewHelpDesk(cy,5);
    AddNewHelpDesk(cy,1);
    AddNewHelpDesk(cy,2);
    AddNewHelpDesk(cy,3);
    AddNewHelpDesk(cy,6);


    SelectLanguage(cy,0,false);
    addContent(cy);

    
    SelectLanguage(cy,1,false);
    addContent(cy);


    SelectLanguage(cy,2,false);
    addMultipleContent(cy);

    SelectLanguage(cy,0,false);


  }); */
});

export function addMultipleContent(cy){

 // تنظیمات
 SaveSetting(cy,1);
 // END

  // تعریف دسته بندی
  DefineCategory(cy);
  DefineCategory(cy);
  DefineCategory(cy);
  DefineCategory(cy);
  DefineCategory(cy);
  DefineCategory(cy);
  // END

  
    // ویرایش دسته بندی
    EditCategory(cy,0)
    // END

    // حذف دسته بندی
    DeleteCategory(cy,0);
    // END

       // تعریف مقاله
       DefineArticle(cy,1);
       DefineArticle(cy,2);
       DefineArticle(cy,3);
       DefineArticle(cy,4);
       DefineArticle(cy,5);
       DefineArticle(cy,6);
       DefineArticle(cy,7);
       // END
   
       
   
   
       // ویرایش مقاله
       EditArticle(cy,0);
       EditArticle(cy,1);
       // END
   
       
       // حذف مقاله
       DeleteArticle(cy,0);
       DeleteArticle(cy,1);
       // END
}

export function addContent(cy){
  // ------------------------ تعریف help desk 1-------------------
    // تنظیمات
    SaveSetting(cy,1);
   // END

    // تعریف دسته بندی
    DefineCategory(cy);
    // END

    // ویرایش دسته بندی
    EditCategory(cy,0)
    // END

    // حذف دسته بندی
    DeleteCategory(cy,0);
    // END

    // تعریف مقاله
    DefineArticle(cy,1);
    DefineArticle(cy,2);
    DefineArticle(cy,3);
    DefineArticle(cy,4);
    DefineArticle(cy,5);
    // END

    


    // ویرایش مقاله
    EditArticle(cy,0);
    // END

    
    // حذف مقاله
    DeleteArticle(cy,0);
    // END
    // ------------------------ END-------------------
}






export function AddNewHelpDesk(cy,i){
    cy.get("#addLanguage").click();

    cy.get(".p-dropdown").eq(1).click();
    cy.get(".p-dropdown-item").eq(i).click();
    cy.get('.p-button.p-component').eq(0).click();

}
export function SelectLanguage(cy,i){
    cy.get(".p-dropdown").eq(0).click();

    cy.get(".p-dropdown-item").eq(i).click();
    

}




export function SaveSetting(cy,i){
    // تنظیمات
    cy.get(".p-tabview-nav-link").eq(2).click();
    cy.get("#helpDeskSiteName").eq(0).type(" مرکز پشتیبانی " + i);
    cy.get(".p-button.p-component.p-button-primary").click();
    // END
}



export function DefineCategory(cy){
    // تعریف دسته بندی
    cy.get(".p-tabview-nav-link").eq(0).click();
    cy.get(".p-button.p-component.p-button-primary").click(); // رکورد جدید
    cy.get("#categoryTitle").eq(0).type("دسته بندی 1");
    cy.get("#categoryDesc").eq(0).type("categoryDesc");
    
 cy.get("#Category_Save").click(); // ذخیره
    // END
}


export function EditCategory(cy,i){
   // ویرایش دسته بندی
   cy.get(".p-button.p-component.p-button-primary.p-button-icon-only")
   .eq(i)
   .click();
 cy.get("#categoryTitle").eq(0).type("ویرایش شده");
 cy.get("#categoryDesc").eq(0).type("ویرایش شده");
 cy.get("#Category_Save").click(); // ذخیره
 // END
}
export function DeleteCategory(cy,i){

// حذف دسته بندی
cy.get(".p-button.p-component.p-button-danger.p-button-icon-only")
  .eq(0)
  .click();

// END
 }
 export function DefineArticle(cy,i){
      // تعریف مقاله
    cy.get(".p-tabview-nav-link").eq(1).click();
    cy.get(".p-button.p-component.p-button-primary").click(); // رکورد جدید
    cy.get("#articleTitle").eq(0).type("مقاله 1" + i);
    
    // select
    cy.get(".p-dropdown.p-component.p-inputwrapper").eq(0).click(); // select
    cy.get(".p-dropdown-item").eq(0).click();

    // select 2
    cy.get(".p-dropdown.p-component.p-inputwrapper").eq(1).click(); // select
    cy.get(".p-dropdown-item").eq(0).click();


    cy.get(".ql-editor").type('tesssssssssssssssssssssssssssssst')
    
    cy.get("#saveArticleOk").click(); // ذخیره
    // END

    cy.wait(1000);
}
export function EditArticle(cy,i){

    // ویرایش مقاله
    cy.get(".p-button.p-component.p-button-success").eq(i).click();

    cy.wait(2000);
    cy.get(".ql-editor").type('tesssssssssssssssssssssssssssssst +- edit')
    cy.get("#saveArticleOk").click(); // ذخیره
    // END
    cy.wait(1000);


}
export function DeleteArticle(cy,i){
     // حذف مقاله
     cy.get(".p-button.p-component.p-button-danger").eq(i).click();
     cy.get(".p-button.p-component.p-button-success").contains('بله').click();
     // END
}
