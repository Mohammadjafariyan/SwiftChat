import React from "react";
import { Editor } from "primereact/editor";
import { useEffect } from "react";
import { useState } from "react";
import { DataHolder } from './../../../../Help/DataHolder';

const WorkingHourSetting_sentMessage = (props) => {
  const [text, setText] = useState(DataHolder.Setting.workingHourSetting_sentMessageText);

  useEffect(() => {
    setText(DataHolder.Setting.workingHourSetting_sentMessageText);
  },[ DataHolder.Setting.workingHourSetting_sentMessageText]);

  return (
    <>
      <label>پیام ارسالی</label>
      <br />
      <Editor  style={{height:'400px'}} id={'workingHourSetting_sentMessageText'} value={text} 
      onTextChange={(e) =>{
          
          
           setText(e.htmlValue);
           
           DataHolder.Setting.workingHourSetting_sentMessageText=e.htmlValue;
           
           }
      
      
      } />
    </>
  );
};

export default WorkingHourSetting_sentMessage;
