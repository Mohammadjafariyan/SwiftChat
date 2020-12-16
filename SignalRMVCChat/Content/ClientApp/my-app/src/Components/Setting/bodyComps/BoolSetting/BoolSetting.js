import React, { Component } from "react";

import { useState } from "react";
import { useEffect } from "react";
import { DataHolder } from "./../../../../Help/DataHolder";
import { InputSwitch } from 'primereact/inputswitch';

const BoolSettings = () => {
  const [isLockToUrl, setIsLockToUrl] = useState(true);

  useEffect(() => {
    setIsLockToUrl(DataHolder.Setting.IsLockToUrl);
  }, [DataHolder.Setting.IsLockToUrl]);

  return (
    <>
      <label>
        آیا پلاگین تنها در آدرس دامنه (و زیر دامنه های) وب سایت شما قفل شود ( از
        دامنه های دیگر قابل استفاده نخواهد بود )
      </label>
      <InputSwitch
        checked={isLockToUrl}
        onChange={(e) => {
          setIsLockToUrl(e.value);
          DataHolder.Setting.IsLockToUrl = e.value;
        }}
      />
    </>
  );
};

export default BoolSettings;
