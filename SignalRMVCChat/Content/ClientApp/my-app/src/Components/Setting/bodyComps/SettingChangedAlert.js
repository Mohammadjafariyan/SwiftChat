import React from "react";
import { DataHolder } from "./../../../Help/DataHolder";
import { useState } from "react";
import { useEffect } from "react";
import { Button } from "primereact/button";

const SettingchangedAlert = (props) => {
  return (
    <>
      <div class="alert alert-warning alert-dismissible fade show" role="alert">
        <strong>تغییرات ذخیره نشده </strong>
        <hr/>
        توجه داشته باشید اطلاعات تغییر داده شده تنها با دکمه{" "}
        <Button
          className={"p-button-raised  p-button-text"}
          label="ذخیره"
          icon="pi pi-check"
        />{" "}
        ثبت خواهند شد
        <button
          type="button"
          class="close"
          data-dismiss="alert"
          aria-label="Close"
        >
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
    </>
  );
};

export default SettingchangedAlert;
