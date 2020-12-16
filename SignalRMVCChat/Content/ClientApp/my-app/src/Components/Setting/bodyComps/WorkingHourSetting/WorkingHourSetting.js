import React from "react";
import WorkingHourSetting_sentMessage from "./workingHourSetting_sentMessage";
import WorkingHourSetting_sentForm from "./workingHourSetting_sentForm";
import { Form } from "react-bootstrap";
import { useState } from "react";

import "./WorkingHourSetting.css";
import { DataHolder } from "./../../../../Help/DataHolder";
import { useEffect } from "react";

export const WorkingHourSetting = (props) => {
  const [activeBody, setActiveBody] = useState(0);

  useEffect(() => {
    var find = WorkingHourSettingMenu.find(
      (f) => f.id == DataHolder.Setting.WorkingHourSettingMenu
    );

    setActiveBody(find);
  }, [DataHolder.Setting.WorkingHourSettingMenu]);


  return (
    <div>
      {WorkingHourSettingMenu.map((row, i, arr) => {
        return (
          <>
            <div key={`default-${row.id}`} dir="rtl">
              <Form.Check
                dir="rtl"
                style={{ textAlign: "right" }}
                type={"radio"}
                id={row.id}
                label={row.text}
                name={"type"}

                checked={DataHolder.Setting.WorkingHourSettingMenu == row.id}
                onClick={() => {
                  setActiveBody(row);

                  DataHolder.Setting.WorkingHourSettingMenu = row.id;
                }}
              />
            </div>
          </>
        );
      })}

      {activeBody && activeBody.body && (
        <>
          <hr />

          {activeBody.body(props.preValue)}
        </>
      )}
    </div>
  );
};

export const WorkingHourSettingMenu = [
  {
    text:
      "زمانی که هیچ ادمینی آنلاین نیست  ، ابزارک گفتگو بطور معمول در سایت من نمایش داده شود",
    body: null,
    id: "workingHourSetting_show",
  },
  {
    text: "زمانی که هیچ ادمینی آنلاین نیست  ، ابزارک گفتگو در سایت من نمایش داده نشود",
    body: null,
    id: "workingHourSetting_hide",
  },
  {
    text: "زمانی که هیچ ادمینی آنلاین نیست  ، پیامی به کاربر نمایش داده شود",
    body: (preValue) => {
      return (
        <WorkingHourSetting_sentMessage
          text={preValue ? preValue.WorkingHourSetting_sentMessage : null}
        />
      );
    },
    id: "workingHourSetting_sentMessage",
  },
  {
    text:
      "زمانی که هیچ ادمینی آنلاین نیست  ، فرم تماس با مشخصات زیر ، برای کاربر نمایش داده شود ",
    body: (preValue) => {
      return (
        <WorkingHourSetting_sentForm
          text={preValue ? preValue.WorkingHourSetting_sentMessage : null}
          form={preValue ? preValue.WorkingHourSetting_sentForm : null}
        />
      );
    },
    id: "workingHourSetting_sentForm",
  },
];
