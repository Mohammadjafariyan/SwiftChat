import React from "react";
import { ListGroup } from "react-bootstrap";

const SettingMenu = (props) => {
  return (
    <>
      <ListGroup defaultActiveKey="#link1">
        {[
            { name: "تنظیمات ساعت کاری", id: "workingHourSetting",variant:'primary' },
            { name: "صفحات فعال", id: "ActivePages",variant:'success' },
            { name: "صفحات غیر فعال", id: "InActivePages",variant:'danger' },
            { name: "تنظیمات امینیتی", id: "BoolSettings",variant:'warning' },
            { name: "تنظیمات هشدار", id: "AlarmSetting",variant:'warning' },

            
            ].map(
          (row, i, arr) => {
            return (
              <ListGroup.Item
              
              id={row.id}
               key={i}
               variant={props.parent.state.activeMenu && props.parent.state.activeMenu.id==row.id ? 'info':null}
               className={' border-'+row.variant}
                action
                onClick={() => {
                  props.parent.setState({ activeMenu: row });
                }}
              >
                {row.name}
              </ListGroup.Item>
            );
          }
        )}
      </ListGroup>
    </>
  );
};

export default SettingMenu;
