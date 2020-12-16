import React from "react";
import { useEffect } from "react";
import { useState } from "react";
import FormSelect from "./FormSelect";
import { Editor } from 'primereact/editor';
import { DataHolder } from './../../../../Help/DataHolder';

const WorkingHourSetting_sentForm = (props) => {
  const [text, setText] = useState(DataHolder.Setting.workingHourSetting_sentFormTopText);

  const [form, setForm] = useState(DataHolder.Setting.workingHourSetting_sentFormSelect);

  useEffect(() => {
    setText(DataHolder.Setting.workingHourSetting_sentFormTopText);
  }, [DataHolder.Setting.workingHourSetting_sentFormTopText]);

  useEffect(() => {
    setForm(DataHolder.Setting.workingHourSetting_sentFormSelect);
  }, [DataHolder.Setting.workingHourSetting_sentFormSelect]);

  return (
    <>
      <label>متن بالای فرم تماس</label>
      <br />
      <Editor style={{height:'400px'}} id='workingHourSetting_sentFormTopText' value={text}
       onTextChange={(e) =>{
           
            setText(e.htmlValue);
            
           DataHolder.Setting.workingHourSetting_sentFormTopText=e.htmlValue;
            
            }} />

      <hr />

      <FormSelect 
      id='workingHourSetting_sentFormSelect'
        preValue={form ? form.Id : null}
        onChange={(val) => {
          setForm(val);

          DataHolder.Setting.workingHourSetting_sentFormSelect=val;

        }}
      />
    </>
  );
};

export default WorkingHourSetting_sentForm;
